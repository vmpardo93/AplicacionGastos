IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] varchar(150) NOT NULL,
        [ProductVersion] varchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AspNetRoles] (
    [Id] varchar(450) NOT NULL,
    [Name] varchar(256) NULL,
    [NormalizedName] varchar(256) NULL,
    [ConcurrencyStamp] text NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] varchar(450) NOT NULL,
    [UserName] varchar(256) NULL,
    [NormalizedUserName] varchar(256) NULL,
    [Email] varchar(256) NULL,
    [NormalizedEmail] varchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] text NULL,
    [SecurityStamp] text NULL,
    [ConcurrencyStamp] text NULL,
    [PhoneNumber] text NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] DateTime NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] varchar(450) NOT NULL,
    [ClaimType] text NULL,
    [ClaimValue] text NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] varchar(450) NOT NULL,
    [ClaimType] text NULL,
    [ClaimValue] text NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] varchar(128) NOT NULL,
    [ProviderKey] varchar(128) NOT NULL,
    [ProviderDisplayName] text NULL,
    [UserId] varchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] varchar(450) NOT NULL,
    [RoleId] varchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] varchar(450) NOT NULL,
    [LoginProvider] varchar(128) NOT NULL,
    [Name] varchar(128) NOT NULL,
    [Value] text NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'00000000000000_CreateIdentitySchema', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [FondosMonetarios] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] varchar(100) NOT NULL,
    [Tipo] varchar(50) NOT NULL,
    CONSTRAINT [PK_FondosMonetarios] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [TiposGasto] (
    [Id] int NOT NULL IDENTITY,
    [Codigo] varchar(10) NOT NULL,
    [Nombre] varchar(100) NOT NULL,
    [Descripcion] varchar(250) NOT NULL,
    CONSTRAINT [PK_TiposGasto] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Depositos] (
    [Id] int NOT NULL IDENTITY,
    [Fecha] DateTime2 NOT NULL,
    [FondoId] int NOT NULL,
    [Monto] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Depositos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Depositos_FondosMonetarios_FondoId] FOREIGN KEY ([FondoId]) REFERENCES [FondosMonetarios] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Gastos] (
    [Id] int NOT NULL IDENTITY,
    [Fecha] DateTime2 NOT NULL,
    [FondoId] int NOT NULL,
    [Observaciones] varchar(500) NOT NULL,
    [Comercio] varchar(200) NOT NULL,
    [TipoDoc] varchar(50) NOT NULL,
    CONSTRAINT [PK_Gastos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Gastos_FondosMonetarios_FondoId] FOREIGN KEY ([FondoId]) REFERENCES [FondosMonetarios] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Presupuestos] (
    [Id] int NOT NULL IDENTITY,
    [UsuarioId] varchar(450) NOT NULL,
    [TipoGastoId] int NOT NULL,
    [Mes] int NOT NULL,
    [Monto] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Presupuestos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Presupuestos_AspNetUsers_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Presupuestos_TiposGasto_TipoGastoId] FOREIGN KEY ([TipoGastoId]) REFERENCES [TiposGasto] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [GastosDetalle] (
    [Id] int NOT NULL IDENTITY,
    [GastoId] int NOT NULL,
    [TipoGastoId] int NOT NULL,
    [Monto] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_GastosDetalle] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GastosDetalle_Gastos_GastoId] FOREIGN KEY ([GastoId]) REFERENCES [Gastos] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_GastosDetalle_TiposGasto_TipoGastoId] FOREIGN KEY ([TipoGastoId]) REFERENCES [TiposGasto] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Depositos_FondoId] ON [Depositos] ([FondoId]);
GO

CREATE INDEX [IX_Gastos_FondoId] ON [Gastos] ([FondoId]);
GO

CREATE INDEX [IX_GastosDetalle_GastoId] ON [GastosDetalle] ([GastoId]);
GO

CREATE INDEX [IX_GastosDetalle_TipoGastoId] ON [GastosDetalle] ([TipoGastoId]);
GO

CREATE INDEX [IX_Presupuestos_TipoGastoId] ON [Presupuestos] ([TipoGastoId]);
GO

CREATE UNIQUE INDEX [IX_Presupuestos_UsuarioId_TipoGastoId_Mes] ON [Presupuestos] ([UsuarioId], [TipoGastoId], [Mes]);
GO

CREATE UNIQUE INDEX [IX_TiposGasto_Codigo] ON [TiposGasto] ([Codigo]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250529140609_MigracionInicial', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Depositos] DROP CONSTRAINT [FK_Depositos_FondosMonetarios_FondoId];
GO

ALTER TABLE [Gastos] DROP CONSTRAINT [FK_Gastos_FondosMonetarios_FondoId];
GO

ALTER TABLE [GastosDetalle] DROP CONSTRAINT [FK_GastosDetalle_TiposGasto_TipoGastoId];
GO

EXEC sp_rename N'[Depositos].[FondoId]', N'FondoMonetarioId', N'COLUMN';
GO

EXEC sp_rename N'[Depositos].[IX_Depositos_FondoId]', N'IX_Depositos_FondoMonetarioId', N'INDEX';
GO

ALTER TABLE [GastosDetalle] ADD [Descripcion] varchar(200) NULL;
GO

ALTER TABLE [GastosDetalle] ADD [TipoGastoId1] int NULL;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Gastos]') AND [c].[name] = N'Observaciones');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Gastos] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Gastos] ALTER COLUMN [Observaciones] varchar(500) NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Gastos]') AND [c].[name] = N'FondoId');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Gastos] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Gastos] ALTER COLUMN [FondoId] int NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Gastos]') AND [c].[name] = N'Comercio');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Gastos] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Gastos] ALTER COLUMN [Comercio] varchar(100) NOT NULL;
GO

ALTER TABLE [Gastos] ADD [FondoMonetarioId] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [Gastos] ADD [FondoMonetarioId1] int NULL;
GO

ALTER TABLE [Gastos] ADD [NumeroDoc] varchar(50) NOT NULL DEFAULT N'';
GO

CREATE TABLE [Fondos] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] varchar(100) NOT NULL,
    [SaldoInicial] decimal(18,2) NOT NULL,
    [Descripcion] varchar(500) NOT NULL,
    CONSTRAINT [PK_Fondos] PRIMARY KEY ([Id])
);
GO

CREATE INDEX [IX_GastosDetalle_TipoGastoId1] ON [GastosDetalle] ([TipoGastoId1]);
GO

CREATE INDEX [IX_Gastos_FondoMonetarioId] ON [Gastos] ([FondoMonetarioId]);
GO

CREATE INDEX [IX_Gastos_FondoMonetarioId1] ON [Gastos] ([FondoMonetarioId1]);
GO

ALTER TABLE [Depositos] ADD CONSTRAINT [FK_Depositos_FondosMonetarios_FondoMonetarioId] FOREIGN KEY ([FondoMonetarioId]) REFERENCES [FondosMonetarios] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [Gastos] ADD CONSTRAINT [FK_Gastos_FondosMonetarios_FondoMonetarioId] FOREIGN KEY ([FondoMonetarioId]) REFERENCES [FondosMonetarios] ([Id]) ON DELETE NO ACTION;
GO

ALTER TABLE [Gastos] ADD CONSTRAINT [FK_Gastos_FondosMonetarios_FondoMonetarioId1] FOREIGN KEY ([FondoMonetarioId1]) REFERENCES [FondosMonetarios] ([Id]);
GO

ALTER TABLE [Gastos] ADD CONSTRAINT [FK_Gastos_Fondos_FondoId] FOREIGN KEY ([FondoId]) REFERENCES [Fondos] ([Id]);
GO

ALTER TABLE [GastosDetalle] ADD CONSTRAINT [FK_GastosDetalle_TiposGasto_TipoGastoId] FOREIGN KEY ([TipoGastoId]) REFERENCES [TiposGasto] ([Id]) ON DELETE NO ACTION;
GO

ALTER TABLE [GastosDetalle] ADD CONSTRAINT [FK_GastosDetalle_TiposGasto_TipoGastoId1] FOREIGN KEY ([TipoGastoId1]) REFERENCES [TiposGasto] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250529161316_CorreccionFondoMonetarioId', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250529163859_CorreccionRelacionesFondoMonetario', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Depositos] DROP CONSTRAINT [FK_Depositos_FondosMonetarios_FondoMonetarioId];
GO

ALTER TABLE [GastosDetalle] DROP CONSTRAINT [FK_GastosDetalle_TiposGasto_TipoGastoId1];
GO

DROP INDEX [IX_GastosDetalle_TipoGastoId1] ON [GastosDetalle];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GastosDetalle]') AND [c].[name] = N'TipoGastoId1');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [GastosDetalle] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [GastosDetalle] DROP COLUMN [TipoGastoId1];
GO

ALTER TABLE [Depositos] ADD [FondoMonetarioId1] int NULL;
GO

CREATE INDEX [IX_Depositos_FondoMonetarioId1] ON [Depositos] ([FondoMonetarioId1]);
GO

ALTER TABLE [Depositos] ADD CONSTRAINT [FK_Depositos_FondosMonetarios_FondoMonetarioId] FOREIGN KEY ([FondoMonetarioId]) REFERENCES [FondosMonetarios] ([Id]) ON DELETE NO ACTION;
GO

ALTER TABLE [Depositos] ADD CONSTRAINT [FK_Depositos_FondosMonetarios_FondoMonetarioId1] FOREIGN KEY ([FondoMonetarioId1]) REFERENCES [FondosMonetarios] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250529164339_CorreccionRelacionesEntidades', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Depositos] DROP CONSTRAINT [FK_Depositos_FondosMonetarios_FondoMonetarioId1];
GO

ALTER TABLE [Gastos] DROP CONSTRAINT [FK_Gastos_FondosMonetarios_FondoMonetarioId1];
GO

DROP INDEX [IX_Gastos_FondoMonetarioId1] ON [Gastos];
GO

DROP INDEX [IX_Depositos_FondoMonetarioId1] ON [Depositos];
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Gastos]') AND [c].[name] = N'FondoMonetarioId1');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Gastos] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Gastos] DROP COLUMN [FondoMonetarioId1];
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Depositos]') AND [c].[name] = N'FondoMonetarioId1');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Depositos] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Depositos] DROP COLUMN [FondoMonetarioId1];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250529164646_CorreccionNavegacionFondoMonetario', N'8.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250529171634_SeedFondosMonetarios', N'8.0.16');
GO

COMMIT;
GO

