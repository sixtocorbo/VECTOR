/*
  Propuestas de índices críticos para SecretariaDB
  Basadas en patrones de consulta del cliente WinForms (VECTOR).
  Ejecutar en una ventana de mantenimiento y validar con Query Store/planes reales.
*/

USE [SecretariaDB];
GO

/* 1) Bandeja global (chkVerDerivados = True):
   filtro por IdEstadoActual <> 5 + ORDER BY FechaCreacion DESC.
   Evita recorrer todas las oficinas cuando no aplica IdOficinaActual. */
IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_Mae_Documento_Activos_Fecha_Global'
      AND object_id = OBJECT_ID('dbo.Mae_Documento')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Mae_Documento_Activos_Fecha_Global]
    ON [dbo].[Mae_Documento] ([FechaCreacion] DESC)
    INCLUDE ([IdDocumento],[IdTipo],[NumeroOficial],[Asunto],[Descripcion],[FechaRecepcion],[IdEstadoActual],[IdOficinaActual],[IdDocumentoPadre],[IdHiloConversacion],[FechaVencimiento])
    WHERE [IdEstadoActual] <> 5;
END
GO

/* 2) Salidas laborales por recluso:
   consultas por IdRecluso, estado activo y fechas + proyección de IdDocumentoRespaldo. */
IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_Tra_SalidasLaborales_Recluso_Activo_Vencimiento'
      AND object_id = OBJECT_ID('dbo.Tra_SalidasLaborales')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Tra_SalidasLaborales_Recluso_Activo_Vencimiento]
    ON [dbo].[Tra_SalidasLaborales] ([IdRecluso] ASC, [Activo] ASC, [FechaVencimiento] ASC)
    INCLUDE ([IdSalida],[IdDocumentoRespaldo],[FechaInicio]);
END
GO

/* 3) Grilla de cupos:
   ORDER BY Fecha DESC, IdCupo DESC + joins por IdTipo/IdUsuario. */
IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_Mae_CuposSecretaria_Fecha_IdCupo'
      AND object_id = OBJECT_ID('dbo.Mae_CuposSecretaria')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Mae_CuposSecretaria_Fecha_IdCupo]
    ON [dbo].[Mae_CuposSecretaria] ([Fecha] DESC, [IdCupo] DESC)
    INCLUDE ([IdTipo],[IdUsuario],[Cantidad]);
END
GO

/* 4) Auditoría transaccional:
   filtro frecuente por rango de fecha y, opcionalmente, usuario/origen/destino. */
IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_Tra_Movimiento_Auditoria'
      AND object_id = OBJECT_ID('dbo.Tra_Movimiento')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Tra_Movimiento_Auditoria]
    ON [dbo].[Tra_Movimiento] ([FechaMovimiento] DESC, [IdUsuarioResponsable] ASC, [IdOficinaOrigen] ASC, [IdOficinaDestino] ASC)
    INCLUDE ([IdDocumento],[IdEstadoEnEseMomento],[ObservacionPase]);
END
GO

/* 5) Búsqueda textual (opcional, alta prioridad funcional):
   Las expresiones Contains(...) generan LIKE '%texto%'.
   Índices B-Tree no escalan bien con ese patrón.
   Recomendación: habilitar Full-Text Search sobre Asunto/Descripcion/ObservacionPase. */
-- CREATE FULLTEXT CATALOG ftSecretaria AS DEFAULT;
-- CREATE FULLTEXT INDEX ON dbo.Mae_Documento(Asunto LANGUAGE 3082, Descripcion LANGUAGE 3082)
-- KEY INDEX PK__Mae_Docu__...;
-- CREATE FULLTEXT INDEX ON dbo.Tra_Movimiento(ObservacionPase LANGUAGE 3082)
-- KEY INDEX PK__Tra_Movi__...;
GO
