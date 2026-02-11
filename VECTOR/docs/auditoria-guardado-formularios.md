# Auditoría rápida: riesgo de operaciones de guardado interrumpidas por UI

## Pregunta
¿Existe la posibilidad de que, mientras una operación de guardado/actualización/delete está en curso, el usuario cierre el formulario o dispare otra acción y deje el proceso en un estado inconsistente?

## Resultado
Sí: **el riesgo no está eliminado en todos los formularios**. Actualmente la protección robusta (flag de reentrada + bloqueo de cierre + restauración de UI en `Finally`) está implementada en `frmMesaEntrada`, pero no de forma homogénea en otros formularios.

## Matriz de revisión (formularios con `btnGuardar_Click`)

| Formulario | ¿Reentrada bloqueada? | ¿Cierre bloqueado durante guardado? | ¿Restauración de UI en Finally? | Estado |
|---|---|---|---|---|
| `frmMesaEntrada` | Sí (`_guardando`) | Sí (`FormClosing` cancela) | Sí | ✅ Robusto |
| `frmConfiguracionSistema` | Parcial (`btnGuardar.Enabled=False`) | No | Sí (`btnGuardar.Enabled=True`) | ⚠️ Mejorable |
| `frmUsuarios` | No | No | No | ⚠️ Riesgo |
| `frmGestionRangos` | Parcial (`SetBusy`) | No | Sí (libera `SetBusy`) | ⚠️ Mejorable |
| `frmGestionTiposDocumento` | No | No | No | ⚠️ Riesgo |
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
1. `frmUsuarios`
2. `frmGestionTiposDocumento`
3. `frmGestionTiempos`
4. `frmRenovacionesArt120`
5. `frmConfiguracionSistema` y `frmGestionRangos` (para cerrar brechas de cierre durante guardado)

