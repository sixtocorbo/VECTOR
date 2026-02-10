-- Tabla puente para asociar múltiples documentos de bandeja de entrada
-- a una salida laboral (frmRenovacionesArt120).
IF OBJECT_ID('dbo.Tra_SalidasLaboralesDocumentoRespaldo', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Tra_SalidasLaboralesDocumentoRespaldo (
        IdSalidaDocumentoRespaldo INT IDENTITY(1,1) NOT NULL,
        IdSalida INT NOT NULL,
        IdDocumento BIGINT NOT NULL,
        FechaRegistro DATETIME NOT NULL CONSTRAINT DF_TraSalidaDocRespaldo_FechaRegistro DEFAULT (GETDATE()),
        CONSTRAINT PK_Tra_SalidasLaboralesDocumentoRespaldo PRIMARY KEY (IdSalidaDocumentoRespaldo),
        CONSTRAINT UQ_Tra_SalidasLaboralesDocumentoRespaldo UNIQUE (IdSalida, IdDocumento),
        CONSTRAINT FK_Tra_SalidasLaboralesDocumentoRespaldo_Tra_SalidasLaborales
            FOREIGN KEY (IdSalida) REFERENCES dbo.Tra_SalidasLaborales(IdSalida),
        CONSTRAINT FK_Tra_SalidasLaboralesDocumentoRespaldo_Mae_Documento
            FOREIGN KEY (IdDocumento) REFERENCES dbo.Mae_Documento(IdDocumento)
    );

    CREATE INDEX IX_Tra_SalidasLaboralesDocumentoRespaldo_IdSalida
        ON dbo.Tra_SalidasLaboralesDocumentoRespaldo(IdSalida);

    CREATE INDEX IX_Tra_SalidasLaboralesDocumentoRespaldo_IdDocumento
        ON dbo.Tra_SalidasLaboralesDocumentoRespaldo(IdDocumento);
END;
GO
