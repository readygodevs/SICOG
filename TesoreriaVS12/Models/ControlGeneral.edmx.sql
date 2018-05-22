
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 11/05/2014 16:18:36
-- Generated from EDMX file: C:\Users\Julio\Desktop\Project\tesorrerivs12\TesoreriaVS12\Models\ControlGeneral.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DB_ControlGral];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'DE_Permisos'
CREATE TABLE [dbo].[DE_Permisos] (
    [IdPermiso] int IDENTITY(1,1) NOT NULL,
    [IdOpcion] smallint  NOT NULL,
    [IdPerfil] tinyint  NOT NULL,
    [Activo] bit  NOT NULL
);
GO

-- Creating table 'CA_Perfiles'
CREATE TABLE [dbo].[CA_Perfiles] (
    [IdPerfil] tinyint IDENTITY(1,1) NOT NULL,
    [Descripcion] varchar(50)  NOT NULL,
    [DefaultPage] varchar(100)  NULL,
    [usuAct] int  NULL,
    [fAct] datetime  NULL
);
GO

-- Creating table 'CA_Usuarios'
CREATE TABLE [dbo].[CA_Usuarios] (
    [IdUsuario] int IDENTITY(1,1) NOT NULL,
    [Nombre] varchar(120)  NOT NULL,
    [ApellidoPaterno] varchar(60)  NOT NULL,
    [ApellidoMaterno] varchar(60)  NULL,
    [Usuario] varchar(50)  NOT NULL,
    [Contrasenia] char(64)  NOT NULL,
    [email] varchar(100)  NULL,
    [IdPerfil] tinyint  NOT NULL,
    [Activo] bit  NULL,
    [CambiaContrasenia] bit  NULL,
    [FechaRegistro] datetime  NULL,
    [Intentos] tinyint  NOT NULL,
    [GeneradoAutomatico] bit  NULL,
    [Titulo] varchar(150)  NULL,
    [Cargo] varchar(150)  NULL,
    [usuAct] int  NULL,
    [fAct] datetime  NULL
);
GO

-- Creating table 'CA_Opciones'
CREATE TABLE [dbo].[CA_Opciones] (
    [IdOpcion] smallint IDENTITY(1,1) NOT NULL,
    [IdOpcionP] smallint  NULL,
    [Controlador] varchar(50)  NOT NULL,
    [Accion] varchar(50)  NULL,
    [Sistema] varchar(50)  NULL,
    [Descripcion] varchar(150)  NULL,
    [Barra] bit  NULL,
    [BarraPadre] smallint  NULL,
    [Menu] bit  NULL,
    [MenuPadre] smallint  NULL,
    [usuAct] int  NULL,
    [fAct] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IdPermiso] in table 'DE_Permisos'
ALTER TABLE [dbo].[DE_Permisos]
ADD CONSTRAINT [PK_DE_Permisos]
    PRIMARY KEY CLUSTERED ([IdPermiso] ASC);
GO

-- Creating primary key on [IdPerfil] in table 'CA_Perfiles'
ALTER TABLE [dbo].[CA_Perfiles]
ADD CONSTRAINT [PK_CA_Perfiles]
    PRIMARY KEY CLUSTERED ([IdPerfil] ASC);
GO

-- Creating primary key on [IdUsuario] in table 'CA_Usuarios'
ALTER TABLE [dbo].[CA_Usuarios]
ADD CONSTRAINT [PK_CA_Usuarios]
    PRIMARY KEY CLUSTERED ([IdUsuario] ASC);
GO

-- Creating primary key on [IdOpcion] in table 'CA_Opciones'
ALTER TABLE [dbo].[CA_Opciones]
ADD CONSTRAINT [PK_CA_Opciones]
    PRIMARY KEY CLUSTERED ([IdOpcion] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [IdOpcion] in table 'DE_Permisos'
ALTER TABLE [dbo].[DE_Permisos]
ADD CONSTRAINT [FK_DE_Permisos_CA_Opciones]
    FOREIGN KEY ([IdOpcion])
    REFERENCES [dbo].[CA_Opciones]
        ([IdOpcion])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DE_Permisos_CA_Opciones'
CREATE INDEX [IX_FK_DE_Permisos_CA_Opciones]
ON [dbo].[DE_Permisos]
    ([IdOpcion]);
GO

-- Creating foreign key on [IdPerfil] in table 'DE_Permisos'
ALTER TABLE [dbo].[DE_Permisos]
ADD CONSTRAINT [FK_DE_Permisos_CA_Perfiles]
    FOREIGN KEY ([IdPerfil])
    REFERENCES [dbo].[CA_Perfiles]
        ([IdPerfil])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DE_Permisos_CA_Perfiles'
CREATE INDEX [IX_FK_DE_Permisos_CA_Perfiles]
ON [dbo].[DE_Permisos]
    ([IdPerfil]);
GO

-- Creating foreign key on [IdPerfil] in table 'CA_Usuarios'
ALTER TABLE [dbo].[CA_Usuarios]
ADD CONSTRAINT [FK_CA_Usuarios_CA_Perfiles]
    FOREIGN KEY ([IdPerfil])
    REFERENCES [dbo].[CA_Perfiles]
        ([IdPerfil])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CA_Usuarios_CA_Perfiles'
CREATE INDEX [IX_FK_CA_Usuarios_CA_Perfiles]
ON [dbo].[CA_Usuarios]
    ([IdPerfil]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------