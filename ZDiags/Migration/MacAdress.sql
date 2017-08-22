/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2014 (12.0.2000)
    Source Database Engine Edition : Microsoft SQL Server Standard Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2014
    Target Database Engine Edition : Microsoft SQL Server Standard Edition
    Target Database Engine Type : Standalone SQL Server
*/

USE [ManufacturingStore_v2]
GO

/****** Object:  Table [dbo].[MacAddress]    Script Date: 8/22/2017 10:02:27 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MacAddress](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MAC] [bigint] NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_MacAddresses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MacAddress] ADD  CONSTRAINT [DF_MacAddress_Date]  DEFAULT (getdate()) FOR [Date]
GO

