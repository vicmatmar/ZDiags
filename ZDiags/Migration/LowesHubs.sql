USE ManufacturingStore_v2
GO

CREATE TABLE [dbo].[LowesHubs] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [mac_id]      INT           NOT NULL,
    [smt_serial]  NVARCHAR (10) NOT NULL,
    [hw_ver]      INT           NOT NULL,
    [customer_id] INT           NOT NULL,
    [date]        DATETIME      DEFAULT (getdate()) NOT NULL,
    [lowes_serial] BIGINT NOT NULL, 
    CONSTRAINT [PK_LowesHubs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LowesCustomersLowesHubs] FOREIGN KEY ([customer_id]) REFERENCES [dbo].[LowesCustomers] ([Id]),
    CONSTRAINT [FK_LowesHubs_MacAddress] FOREIGN KEY ([mac_id]) REFERENCES [dbo].[MacAddress] ([Id])
);

