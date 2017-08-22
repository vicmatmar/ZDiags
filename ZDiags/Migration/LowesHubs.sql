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

/****** Object:  Table [dbo].[LowesHubs]    Script Date: 8/22/2017 10:02:08 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LowesHubs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[mac_id] [int] NOT NULL,
	[smt_serial] [nvarchar](10) NOT NULL,
	[hw_ver] [nvarchar](1) NOT NULL,
	[customer_id] [int] NOT NULL,
	[date] [datetime] NOT NULL,
 CONSTRAINT [PK_LowesHubs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[LowesHubs] ADD  DEFAULT (getdate()) FOR [date]
GO

ALTER TABLE [dbo].[LowesHubs]  WITH CHECK ADD  CONSTRAINT [FK_LowesCustomersLowesHubs] FOREIGN KEY([customer_id])
REFERENCES [dbo].[LowesCustomers] ([Id])
GO

ALTER TABLE [dbo].[LowesHubs] CHECK CONSTRAINT [FK_LowesCustomersLowesHubs]
GO

ALTER TABLE [dbo].[LowesHubs]  WITH CHECK ADD  CONSTRAINT [FK_LowesHubs_MacAddress] FOREIGN KEY([mac_id])
REFERENCES [dbo].[MacAddress] ([Id])
GO

ALTER TABLE [dbo].[LowesHubs] CHECK CONSTRAINT [FK_LowesHubs_MacAddress]
GO

