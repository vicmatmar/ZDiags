USE [ManufacturingStore_v2]
GO

/****** Object: Table [dbo].[Operators] Script Date: 9/20/2017 2:54:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Operators] (
    [Id]   INT         IDENTITY (1, 1) NOT NULL,
    [Name] NCHAR (250) NOT NULL,
    [Pin]  INT         NULL
);


