/*
    Configuración general del sistema para valores administrables desde UI.
    Incluye el parámetro de días de alerta para frmRenovacionesArt120.
*/

IF OBJECT_ID('dbo.Cfg_SistemaParametros', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Cfg_SistemaParametros
    (
        IdParametro INT IDENTITY(1,1) NOT NULL,
        Clave NVARCHAR(150) NOT NULL,
        Valor NVARCHAR(500) NOT NULL,
        Descripcion NVARCHAR(500) NULL,
        UsuarioActualizacion NVARCHAR(100) NULL,
        FechaActualizacion DATETIME2(0) NOT NULL CONSTRAINT DF_Cfg_SistemaParametros_FechaActualizacion DEFAULT (SYSDATETIME()),
        CONSTRAINT PK_Cfg_SistemaParametros PRIMARY KEY CLUSTERED (IdParametro),
        CONSTRAINT UQ_Cfg_SistemaParametros_Clave UNIQUE (Clave)
    );
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM dbo.Cfg_SistemaParametros
    WHERE Clave = 'RenovacionesArt120.DiasAlerta'
)
BEGIN
    INSERT INTO dbo.Cfg_SistemaParametros (Clave, Valor, Descripcion, UsuarioActualizacion)
    VALUES
    (
        'RenovacionesArt120.DiasAlerta',
        '30',
        'Cantidad de días previos al vencimiento para marcar ALERTA en frmRenovacionesArt120.',
        SUSER_SNAME()
    );
END;
GO

/*
-- Ejemplo para cambiar el valor manualmente por SQL:
UPDATE dbo.Cfg_SistemaParametros
SET Valor = '45',
    UsuarioActualizacion = SUSER_SNAME(),
    FechaActualizacion = SYSDATETIME()
WHERE Clave = 'RenovacionesArt120.DiasAlerta';
*/
