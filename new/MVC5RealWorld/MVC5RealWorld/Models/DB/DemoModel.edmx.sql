
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/21/2017 09:34:43
-- Generated from EDMX file: C:\Users\HP\Desktop\new\MVC5RealWorld\MVC5RealWorld\Models\DB\DemoModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DemoDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__SYSUserPr__SYSUs__239E4DCF]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SYSUserProfile] DROP CONSTRAINT [FK__SYSUserPr__SYSUs__239E4DCF];
GO
IF OBJECT_ID(N'[dbo].[FK__SYSUserRo__LOOKU__29572725]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SYSUserRole] DROP CONSTRAINT [FK__SYSUserRo__LOOKU__29572725];
GO
IF OBJECT_ID(N'[dbo].[FK__SYSUserRo__SYSUs__2A4B4B5E]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SYSUserRole] DROP CONSTRAINT [FK__SYSUserRo__SYSUs__2A4B4B5E];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[LOOKUPRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LOOKUPRole];
GO
IF OBJECT_ID(N'[dbo].[SYSUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SYSUser];
GO
IF OBJECT_ID(N'[dbo].[SYSUserProfile]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SYSUserProfile];
GO
IF OBJECT_ID(N'[dbo].[SYSUserRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SYSUserRole];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'LOOKUPRoles'
CREATE TABLE [dbo].[LOOKUPRoles] (
    [LOOKUPRoleID] int IDENTITY(1,1) NOT NULL,
    [RoleName] varchar(100)  NULL,
    [RoleDescription] varchar(500)  NULL,
    [RowCreatedSYSUserID] int  NOT NULL,
    [RowCreatedDateTime] datetime  NULL,
    [RowModifiedSYSUserID] int  NOT NULL,
    [RowModifiedDateTime] datetime  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [UserID] int IDENTITY(1,1) NOT NULL,
    [LoginName] varchar(50)  NOT NULL,
    [PasswordEncryptedText] varchar(200)  NOT NULL,
    [RowCreatedSYSUserID] int  NOT NULL,
    [RowCreatedDateTime] datetime  NULL,
    [RowModifiedSYSUserID] int  NOT NULL,
    [RowMOdifiedDateTime] datetime  NULL
);
GO

-- Creating table 'SYSUserProfiles'
CREATE TABLE [dbo].[SYSUserProfiles] (
    [SYSUserProfileID] int IDENTITY(1,1) NOT NULL,
    [SYSUserID] int  NOT NULL,
    [FirstName] varchar(50)  NOT NULL,
    [LastName] varchar(50)  NOT NULL,
    [Gender] char(1)  NOT NULL,
    [RowCreatedSYSUserID] int  NOT NULL,
    [RowCreatedDateTime] datetime  NULL,
    [RowModifiedSYSUserID] int  NOT NULL,
    [RowModifiedDateTime] datetime  NULL
);
GO

-- Creating table 'SYSUserRoles'
CREATE TABLE [dbo].[SYSUserRoles] (
    [SYSUserRoleID] int IDENTITY(1,1) NOT NULL,
    [SYSUserID] int  NOT NULL,
    [LOOKUPRoleID] int  NOT NULL,
    [IsActive] bit  NULL,
    [RowCreatedSYSUserID] int  NOT NULL,
    [RowCreatedDateTime] datetime  NULL,
    [RowModifiedSYSUserID] int  NOT NULL,
    [RowModifiedDateTime] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [LOOKUPRoleID] in table 'LOOKUPRoles'
ALTER TABLE [dbo].[LOOKUPRoles]
ADD CONSTRAINT [PK_LOOKUPRoles]
    PRIMARY KEY CLUSTERED ([LOOKUPRoleID] ASC);
GO

-- Creating primary key on [UserID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserID] ASC);
GO

-- Creating primary key on [SYSUserProfileID] in table 'SYSUserProfiles'
ALTER TABLE [dbo].[SYSUserProfiles]
ADD CONSTRAINT [PK_SYSUserProfiles]
    PRIMARY KEY CLUSTERED ([SYSUserProfileID] ASC);
GO

-- Creating primary key on [SYSUserRoleID] in table 'SYSUserRoles'
ALTER TABLE [dbo].[SYSUserRoles]
ADD CONSTRAINT [PK_SYSUserRoles]
    PRIMARY KEY CLUSTERED ([SYSUserRoleID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [LOOKUPRoleID] in table 'SYSUserRoles'
ALTER TABLE [dbo].[SYSUserRoles]
ADD CONSTRAINT [FK__SYSUserRo__LOOKU__29572725]
    FOREIGN KEY ([LOOKUPRoleID])
    REFERENCES [dbo].[LOOKUPRoles]
        ([LOOKUPRoleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__SYSUserRo__LOOKU__29572725'
CREATE INDEX [IX_FK__SYSUserRo__LOOKU__29572725]
ON [dbo].[SYSUserRoles]
    ([LOOKUPRoleID]);
GO

-- Creating foreign key on [SYSUserID] in table 'SYSUserProfiles'
ALTER TABLE [dbo].[SYSUserProfiles]
ADD CONSTRAINT [FK__SYSUserPr__SYSUs__239E4DCF]
    FOREIGN KEY ([SYSUserID])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__SYSUserPr__SYSUs__239E4DCF'
CREATE INDEX [IX_FK__SYSUserPr__SYSUs__239E4DCF]
ON [dbo].[SYSUserProfiles]
    ([SYSUserID]);
GO

-- Creating foreign key on [SYSUserID] in table 'SYSUserRoles'
ALTER TABLE [dbo].[SYSUserRoles]
ADD CONSTRAINT [FK__SYSUserRo__SYSUs__2A4B4B5E]
    FOREIGN KEY ([SYSUserID])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__SYSUserRo__SYSUs__2A4B4B5E'
CREATE INDEX [IX_FK__SYSUserRo__SYSUs__2A4B4B5E]
ON [dbo].[SYSUserRoles]
    ([SYSUserID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------