USE [ManufacturingStore_v2]
GO

/****** Object:  Table [dbo].[MacAddress]    Script Date: 8/22/2017 10:02:27 AM ******/
SET ANSI_NULLS ON
GO

CREATE TABLE [dbo].[MacAddress] (
    [Id]   INT      IDENTITY (1, 1) NOT NULL,
    [MAC]  BIGINT   NOT NULL,
    [Date] DATETIME CONSTRAINT [DF_MacAddress_Date] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_MacAddresses] PRIMARY KEY CLUSTERED ([Id] ASC)
);

