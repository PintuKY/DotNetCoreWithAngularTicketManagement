SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;

IF OBJECT_ID(N'[dbo].[Users]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Users] (
        [Id] int IDENTITY(1,1) NOT NULL,
        [UserGuid] uniqueidentifier NULL,
        [FullName] nvarchar(max) NULL,
        [Email] nvarchar(450) NULL,
        [Mobile] nvarchar(max) NULL,
        [PasswordHash] nvarchar(max) NULL,
        [Role] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [CreatedOn] datetime2 NULL,
        [LastLoginDate] datetime2 NULL,
        [EmailVerified] bit NOT NULL,
        [UpdatedOn] datetime2 NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE [name] = N'IX_Users_Email'
      AND [object_id] = OBJECT_ID(N'[dbo].[Users]')
)
BEGIN
    CREATE UNIQUE INDEX [IX_Users_Email]
    ON [dbo].[Users] ([Email])
    WHERE [Email] IS NOT NULL;
END;

IF OBJECT_ID(N'[dbo].[__EFMigrationsHistory]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

IF NOT EXISTS (
       SELECT 1
       FROM [dbo].[__EFMigrationsHistory]
       WHERE [MigrationId] = N'20260627153000_AddUsersLoginRegistration'
   )
BEGIN
    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260627153000_AddUsersLoginRegistration', N'9.0.8');
END;
