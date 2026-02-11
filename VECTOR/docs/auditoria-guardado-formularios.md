# Auditoría de cierre durante guardado (post-fix `frmMesaEntrada`)

## Objetivo
Verificar si el problema reportado en `frmMesaEntrada` (intentar cerrar con el flag de guardado aún activo) se repite en otros formularios.

## Alcance revisado
Formularios con estas dos características en conjunto:
1. una bandera de operación en curso (`_guardando` / `_operacionEnCurso`), y
2. bloqueo de cierre en `FormClosing` cuando esa bandera está activa.

## Resultado
No se detectó repetición del mismo patrón en otros formularios.

El caso de `frmMesaEntrada` ya quedó corregido porque, en el flujo exitoso, limpia `_guardando = False` antes de `Me.Close()`. En los demás formularios auditados:
- o bien no cierran automáticamente tras guardar,
- o bien no combinan “cierre automático + cancelación por guardado en progreso” en la misma secuencia problemática.

## Matriz actualizada

| Formulario | Flag de operación | `FormClosing` bloquea cierre | ¿Cierra al guardar exitoso? | Estado frente al bug de `frmMesaEntrada` |
|---|---|---|---|---|
| `frmMesaEntrada` | `_guardando` | Sí | Sí | ✅ Corregido (resetea flag antes de `Close`) |
| `frmGestionTiempos` | `_guardando` | Sí | No | ✅ No aplica el bug |
| `frmRenovacionesArt120` | `_guardando` | Sí | No | ✅ No aplica el bug |
| `frmGestionTiposDocumento` | `_guardando` | Sí | No | ✅ No aplica el bug |
| `frmUsuarios` | `_operacionEnCurso` | Sí | No | ✅ No aplica el bug |
| `frmGestionRangos` | `_operacionEnCurso` | Sí | No | ✅ No aplica el bug |

## Nota complementaria
`frmConfiguracionSistema` sí cierra al guardar exitosamente, pero no implementa cancelación de cierre en `FormClosing` basada en una bandera de guardado; por eso no presenta el mismo comportamiento específico.

## Comandos usados para la revisión
- `rg "_guardando|_operacionEnCurso|FormClosing|Close\(" -n /workspace/VECTOR/VECTOR`
- `for f in $(rg "Handles MyBase.FormClosing" -l /workspace/VECTOR/VECTOR/*.vb); do ...; done`
- inspección puntual de:
  - `frmMesaEntrada.vb`
  - `frmGestionTiempos.vb`
  - `frmRenovacionesArt120.vb`
  - `frmGestionTiposDocumento.vb`
  - `frmUsuarios.vb`
  - `frmGestionRangos.vb`
  - `frmConfiguracionSistema.vb`
