-- Tabla para adjuntos digitales almacenados en base de datos.
-- Ejecutar en la base SecretariaDB.
CREATE TABLE dbo.Tra_AdjuntoDocumento (
    IdAdjunto BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdDocumento BIGINT NOT NULL,
    StoredName NVARCHAR(200) NOT NULL,
    DisplayName NVARCHAR(255) NOT NULL,
    AddedAt DATETIME NOT NULL,
    Content VARBINARY(MAX) NOT NULL,
    CONSTRAINT FK_Tra_AdjuntoDocumento_Mae_Documento
        FOREIGN KEY (IdDocumento) REFERENCES dbo.Mae_Documento(IdDocumento)
        ON DELETE CASCADE
);

CREATE INDEX IX_Tra_AdjuntoDocumento_Documento ON dbo.Tra_AdjuntoDocumento(IdDocumento);
CREATE UNIQUE INDEX UX_Tra_AdjuntoDocumento_Documento_StoredName ON dbo.Tra_AdjuntoDocumento(IdDocumento, StoredName);
