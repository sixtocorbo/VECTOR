# Relevamiento: lugares donde hoy sí conviene aplicar UnitOfWork

Este documento identifica puntos del código donde **UnitOfWork** aporta valor inmediato (consistencia transaccional, misma unidad de contexto y menor riesgo de escritura parcial).

## 1) `frmPase`: pase masivo de expediente (alta prioridad)

- Archivo: `VECTOR/frmPase.vb`.
- Situación actual:
  - El formulario usa un `SecretariaDBEntities` directo a nivel clase (`Private db As New SecretariaDBEntities()`).
  - En `btnConfirmar_Click` se actualiza un conjunto de documentos (`Mae_Documento`) y además se agregan movimientos (`Tra_Movimiento`) en un loop, cerrando con `db.SaveChanges()`.
- Por qué aquí sí se necesita UnitOfWork:
  - Es un caso de **múltiples entidades relacionadas** que deben persistir juntas como una sola operación de negocio (pase de paquete).
  - Un `UnitOfWork` explícito deja más claro el límite transaccional y alinea este flujo con el resto de pantallas que ya usan repositorios.

## 2) `frmMesaEntrada`: guardar documento + adjuntos + auditoría (alta prioridad)

- Archivo: `VECTOR/frmMesaEntrada.vb`.
- Situación actual:
  - El alta/edición principal ya usa `_unitOfWork.Commit()`.
  - Después del commit se dispara `GuardarAdjuntos(...)` (almacenamiento físico + metadata en otra conexión/transacción interna) y luego `AuditoriaSistema.RegistrarEvento(...)` (crea otro `UnitOfWork` propio).
- Por qué aquí sí se necesita fortalecer UnitOfWork (o una estrategia transaccional de aplicación):
  - La operación real del usuario es “guardar documento completo”. Hoy se puede confirmar el documento aunque fallen adjuntos/auditoría.
  - Si el negocio exige atomicidad fuerte, habría que unificar persistencia de documento + metadata de adjuntos + auditoría en la misma unidad lógica (o implementar compensaciones explícitas).

## 3) `AuditoriaSistema.RegistrarEvento`: acoplamiento con operaciones críticas (prioridad media)

- Archivo: `VECTOR/AuditoriaSistema.vb`.
- Situación actual:
  - Siempre abre un `New UnitOfWork()` interno y captura errores silenciosamente.
- Por qué es un punto donde UnitOfWork compartido puede ser necesario:
  - En procesos críticos (pase, recepción, edición sensible) puede interesar que el log de auditoría quede dentro de la misma unidad de trabajo del caso de uso.
  - Recomendación: permitir una sobrecarga que acepte `IUnitOfWork` externo para reutilizar contexto cuando se necesite consistencia fuerte.

## 4) Contrato de repositorio: `SaveChanges` en `IRepository` (prioridad media)

- Archivos: `VECTOR/IRepository.vb`, `VECTOR/Repository.vb`.
- Situación actual:
  - El repositorio expone `SaveChanges()`/`SaveChangesAsync()` además de existir `Commit` en `UnitOfWork`.
- Por qué aquí sí importa UnitOfWork:
  - Tener commit en dos capas facilita romper el patrón y hacer persistencias parciales fuera de la unidad de trabajo.
  - Recomendación: centralizar commits en `UnitOfWork` y dejar repositorio solo para acceso/manipulación de entidades.

## No prioritario / opcional por ahora

- `frmAuditoria` usa contextos directos para consultas de lectura y filtros. En lectura pura, `UnitOfWork` no es estrictamente necesario salvo por estandarización arquitectónica.
