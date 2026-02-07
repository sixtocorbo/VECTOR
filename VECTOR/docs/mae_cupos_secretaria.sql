-- Tabla para almacenar los cupos otorgados por Secretaría General
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Mae_CuposSecretaria' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[Mae_CuposSecretaria](
        [IdCupo] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Fecha] DATETIME NOT NULL,
        [IdUsuario] INT NULL,
        [IdTipo] INT NOT NULL,
        [Cantidad] INT NOT NULL,
        CONSTRAINT [FK_MaeCuposSecretaria_Tipo] FOREIGN KEY ([IdTipo]) REFERENCES [dbo].[Cat_TipoDocumento]([IdTipo]),
        CONSTRAINT [FK_MaeCuposSecretaria_Usuario] FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Cat_Usuario]([IdUsuario])
    );
END
GO
