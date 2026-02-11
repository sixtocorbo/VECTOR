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
