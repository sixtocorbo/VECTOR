# Auditoría rápida: riesgo de operaciones de guardado interrumpidas por UI

## Pregunta
¿Existe la posibilidad de que, mientras una operación de guardado/actualización/delete está en curso, el usuario cierre el formulario o dispare otra acción y deje el proceso en un estado inconsistente?

## Resultado
Sí, pero **acotado**: hoy hay varios formularios con protección robusta, y el riesgo principal quedó concentrado en `frmGestionTiempos` y `frmRenovacionesArt120`.

## Matriz de revisión (formularios con `btnGuardar_Click`)

| Formulario | ¿Reentrada bloqueada? | ¿Cierre bloqueado durante guardado? | ¿Restauración de UI en `Finally`? | Estado |
|---|---|---|---|---|
| `frmMesaEntrada` | Sí (`_guardando`) | Sí (`FormClosing` cancela) | Sí | ✅ Robusto |
| `frmConfiguracionSistema` | Parcial (`btnGuardar.Enabled = False`) | No | Sí (`btnGuardar.Enabled = True`) | ⚠️ Mejorable |
| `frmUsuarios` | Sí (`_operacionEnCurso`) | Sí (`FormClosing` cancela) | Sí (`Try/Catch/Finally`) | ✅ Robusto |
| `frmGestionRangos` | Sí (`_operacionEnCurso` + `CambiarEstadoOperacion`) | Sí (`FormClosing` cancela) | Sí (`CambiarEstadoOperacion(False)` en `Finally`) | ✅ Robusto |
| `frmGestionTiposDocumento` | Sí (`_guardando`) | Sí (`FormClosing` cancela) | Sí (`SetGuardadoUIState(False)` en `Finally`) | ✅ Robusto |
| `frmGestionTiempos` | No | No | No | ⚠️ Riesgo |
| `frmRenovacionesArt120` | No | No | No | ⚠️ Riesgo |

## Recomendación de estándar
Aplicar el mismo patrón base en formularios con operaciones de persistencia:

1. `Private _guardando As Boolean`
2. Al inicio de `btnGuardar_Click`:
   - `If _guardando Then Return`
   - `_guardando = True`
   - deshabilitar botones relevantes
   - `UseWaitCursor = True`
3. Encapsular persistencia en `Try ... Catch ... Finally`
4. En `Finally`:
   - `_guardando = False`
   - re-habilitar controles
   - `UseWaitCursor = False`
5. En `FormClosing`:
   - si `_guardando`, `e.Cancel = True` y notificar "guardado en progreso"

## Priorización sugerida
1. `frmGestionTiempos`
2. `frmRenovacionesArt120`
3. `frmConfiguracionSistema` (cierre durante guardado)

## Próximo paso propuesto
Empezar por `frmGestionTiempos`, porque tiene un `btnGuardar_Click` asíncrono sin flag de operación ni bloqueo de cierre, y su flujo es corto para estandarizar rápido el patrón.

## Evidencia puntual por formulario crítico

### `frmGestionTiempos`
- `btnGuardar_Click` es `Async`, pero no valida estado de operación en curso (`If _guardando Then Return`) antes de iniciar persistencia.
- No deshabilita acciones de UI al comenzar (`btnGuardar`, `btnCancelar`, grilla), por lo que el usuario puede reintentar guardado.
- No hay `FormClosing` con cancelación condicional durante commit.
- El bloque tiene `Try/Catch`, pero no `Finally` para restauración determinística de estado visual.

**Impacto:** doble disparo del botón o cierre de ventana en medio de `CommitAsync` puede producir experiencia inconsistente (mensaje de éxito/error sin correlación clara con el último click del usuario).

### `frmRenovacionesArt120`
- `btnGuardar_Click` no usa flag de exclusión mutua local (`_guardando`/`_operacionEnCurso`).
- Durante guardado, botones secundarios (`Desactivar`, `Reactivar`, `Buscar`) siguen disponibles.
- No se observó bloqueo de `FormClosing` mientras hay operación asíncrona activa.
- No se centraliza un método de estado de UI para operación en curso.

**Impacto:** operaciones superpuestas sobre la misma entidad (`Tra_SalidasLaborales`) y potenciales mensajes cruzados de notificación.

### `frmConfiguracionSistema` (mejora menor)
- Tiene mitigación de reentrada parcial (`btnGuardar.Enabled = False`).
- Cierra el formulario al éxito (`Close()`), pero no bloquea explícitamente cierre manual mientras los `await` siguen en curso.
- Sí restaura botón en `Finally`.

**Impacto:** bajo; el principal ajuste es consistencia de patrón con el resto del sistema.

## Patrón mínimo (plantilla reutilizable)

```vb
Private _guardando As Boolean

Private Sub Form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
    If _guardando Then
        e.Cancel = True
        Notifier.Info(Me, "Hay un guardado en progreso. Espere a que finalice.")
    End If
End Sub

Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
    If _guardando Then Return

    _guardando = True
    CambiarEstadoGuardado(True)

    Try
        ' Persistencia
        Await GuardarAsync()
        Notifier.Success(Me, "Guardado correcto.")
    Catch ex As Exception
        Notifier.Error(Me, "No se pudo guardar: " & ex.Message)
    Finally
        _guardando = False
        CambiarEstadoGuardado(False)
    End Try
End Sub

Private Sub CambiarEstadoGuardado(guardando As Boolean)
    btnGuardar.Enabled = Not guardando
    btnCancelar.Enabled = Not guardando
    UseWaitCursor = guardando
End Sub
```

## Criterios de aceptación sugeridos

1. Si el usuario hace doble click en **Guardar**, solo se ejecuta una persistencia.
2. Si el usuario intenta cerrar el formulario durante `CommitAsync`, el cierre se cancela y se muestra aviso.
3. Al finalizar (éxito o error), la UI vuelve siempre a su estado habilitado.
4. No quedan cursores de espera activos al salir por excepción.
5. El flujo de notificaciones muestra un único resultado por intento real de guardado.

## Plan de implementación incremental

### Fase 1 (rápida)
- Aplicar patrón completo en `frmGestionTiempos`.
- Verificar manualmente doble click + cierre durante guardado.

### Fase 2
- Repetir en `frmRenovacionesArt120` incluyendo botones auxiliares de edición/estado.

### Fase 3
- Homologar `frmConfiguracionSistema` (bloqueo de cierre en operación) para consistencia transversal.

## Nota de alcance
Esta auditoría está enfocada en **robustez de interacción UI durante guardado** (reentrada/cierre), no en concurrencia multiusuario a nivel base de datos.
