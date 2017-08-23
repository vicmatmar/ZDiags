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

/****** Object:  Table [dbo].[StationSiteId]    Script Date: 8/23/2017 4:11:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StationSiteId](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StationMac] [char](12) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[StationSiteId]  WITH CHECK ADD  CONSTRAINT [FK_StationMac_ToTable] FOREIGN KEY([StationMac])
REFERENCES [dbo].[StationSite] ([StationMac])
GO

ALTER TABLE [dbo].[StationSiteId] CHECK CONSTRAINT [FK_StationMac_ToTable]
GO

