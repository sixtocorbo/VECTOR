# SecretariaDB: índices críticos sugeridos y mejoras de funcionamiento

## 1) Qué observé en la base y en la app

### Cobertura actual (positiva)
- `Mae_Documento` ya tiene índices para bandeja por oficina (`IX_Bandeja_Rapida_Optimizado`), hilo (`IX_Mae_Documento_Hilo_Covering`) y padre/estado (`IX_Documento_Padre_Estado`).
- `Tra_Movimiento` ya tiene índices para documento, fecha, oficina origen/destino y último movimiento.
- `Tra_AdjuntoDocumento` y `Tra_SalidasLaboralesDocumentoRespaldo` tienen índices correctos para sus accesos principales.

### Huecos de rendimiento detectados
1. **Bandeja sin filtro por oficina (ver derivados)**
   - La consulta filtra solo `IdEstadoActual <> 5` y ordena por `FechaCreacion DESC`.
   - El índice actual de bandeja empieza por `IdOficinaActual`, por lo que este escenario puede degradarse.

2. **Módulo Art.120 / salidas laborales**
   - Hay consultas frecuentes por `IdRecluso`, estado `Activo`, fechas e `IdDocumentoRespaldo`.
   - `Tra_SalidasLaborales` no tiene índice no cluster específico para este patrón.

3. **Grilla de cupos de secretaría**
   - Consulta con `ORDER BY Fecha DESC, IdCupo DESC` + joins por `IdTipo` e `IdUsuario`.
   - `Mae_CuposSecretaria` no tiene índice orientado a ese orden.

4. **Auditoría de transacciones**
   - Filtro por rango de `FechaMovimiento` con selectivos opcionales por usuario/origen/destino.
   - Existen índices parciales, pero no uno compuesto pensado para ese “shape” de consulta.

5. **Búsquedas por texto (`Contains`)**
   - En EF suelen traducirse a `LIKE '%texto%'`, poco aprovechable por índices B-Tree tradicionales.

---

## 2) Índices críticos propuestos (prioridad)

### Alta prioridad (impacto directo)
1. `IX_Mae_Documento_Activos_Fecha_Global` (filtrado)
   - Tabla: `Mae_Documento`
   - Clave: `FechaCreacion DESC`
   - Filtro: `WHERE IdEstadoActual <> 5`
   - Objetivo: acelerar bandeja global y ordenamiento sin depender de `IdOficinaActual`.

2. `IX_Tra_SalidasLaborales_Recluso_Activo_Vencimiento`
   - Tabla: `Tra_SalidasLaborales`
   - Clave: `(IdRecluso, Activo, FechaVencimiento)`
   - Include: `(IdSalida, IdDocumentoRespaldo, FechaInicio)`
   - Objetivo: acelerar carga y consolidación de salidas por recluso.

3. `IX_Mae_CuposSecretaria_Fecha_IdCupo`
   - Tabla: `Mae_CuposSecretaria`
   - Clave: `(Fecha DESC, IdCupo DESC)`
   - Include: `(IdTipo, IdUsuario, Cantidad)`
   - Objetivo: evitar sort costoso en la grilla y mejorar la latencia al abrir/recargar.

### Prioridad media
4. `IX_Tra_Movimiento_Auditoria`
   - Tabla: `Tra_Movimiento`
   - Clave: `(FechaMovimiento DESC, IdUsuarioResponsable, IdOficinaOrigen, IdOficinaDestino)`
   - Include: `(IdDocumento, IdEstadoEnEseMomento, ObservacionPase)`
   - Objetivo: mejorar filtros combinados de auditoría transaccional.

### Prioridad funcional (no solo índice)
5. **Full-Text Search** sobre:
   - `Mae_Documento(Asunto, Descripcion)`
   - `Tra_Movimiento(ObservacionPase)`
   - Objetivo: resolver de forma escalable búsquedas de texto libre tipo “contiene palabras”.

---

## 3) Impacto esperado

- Menor tiempo de carga en **Bandeja** cuando se visualizan documentos de múltiples oficinas.
- Menor latencia en pantalla de **Renovaciones Art.120** al buscar soporte documental por recluso.
- Mejor rendimiento en **Cupos Secretaría** al ordenar y renderizar la grilla completa.
- Mejor respuesta en **Auditoría** para filtros combinados por fecha/usuario/oficina.

---

## 4) Recomendaciones de funcionamiento del sistema (no DB)

1. **Paginar en Bandeja/Auditoría**
   - Hoy se hace `ToListAsync()` de volúmenes potencialmente grandes y luego filtro en memoria.
   - Recomendado: paginación (TOP + OFFSET/FETCH) y filtro en SQL.

2. **Debounce de búsqueda ya implementado, pero mover “palabras” al SQL si se desea escalar**
   - En varias pantallas la búsqueda final se realiza en memoria sobre listas ya cargadas.

3. **Encender Query Store en producción**
   - En script base figura `QUERY_STORE = OFF`; encenderlo permite detectar regresiones por plan.

4. **Evaluar `READ_COMMITTED_SNAPSHOT ON`**
   - Reduce bloqueos de lectura en escenarios con mucha concurrencia de consultas.

---

## 5) Script listo para aplicar

Se dejó script ejecutable con validación `IF NOT EXISTS` en:

- `docs/indices_criticos_propuestos.sql`

Aplicar por etapas (1 índice por ventana), medir con tiempos reales y revisar uso en `sys.dm_db_index_usage_stats` a la semana.
