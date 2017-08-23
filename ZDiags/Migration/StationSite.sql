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

/****** Object:  Table [dbo].[StationSite]    Script Date: 8/23/2017 4:11:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StationSite](
	[StationMac] [char](12) NOT NULL,
	[ProductionSiteId] [int] NOT NULL,
 CONSTRAINT [PK_StationSite] PRIMARY KEY CLUSTERED 
(
	[StationMac] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[StationSite]  WITH CHECK ADD  CONSTRAINT [FK_StationSite_ProductionSite] FOREIGN KEY([ProductionSiteId])
REFERENCES [dbo].[ProductionSite] ([Id])
GO

ALTER TABLE [dbo].[StationSite] CHECK CONSTRAINT [FK_StationSite_ProductionSite]
GO

