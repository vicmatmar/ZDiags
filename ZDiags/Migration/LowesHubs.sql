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

/****** Object:  Table [dbo].[LowesHubs]    Script Date: 9/20/2017 3:26:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LowesHubs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[mac_id] [int] NOT NULL,
	[smt_serial] [nvarchar](10) NOT NULL,
	[hw_ver] [int] NOT NULL,
	[customer_id] [int] NOT NULL,
	[date] [datetime] NOT NULL,
	[lowes_serial] [bigint] NOT NULL,
	[hub_id] [char](8) NOT NULL,
	[operator_id] [int] NULL,
	[test_station_id] [int] NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[LowesHubs] ADD  CONSTRAINT [DF_LowesHubs_date]  DEFAULT (getdate()) FOR [date]
GO

