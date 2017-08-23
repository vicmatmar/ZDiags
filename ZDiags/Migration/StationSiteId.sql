USE ManufacturingStore_v2
GO

CREATE TABLE [dbo].[StationSiteId] (
    [Id]         INT       IDENTITY (1, 1) NOT NULL,
    [StationMac] CHAR (12) COLLATE Latin1_General_CI_AI NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StationMac_ToTable] FOREIGN KEY ([StationMac]) REFERENCES [dbo].[StationSite] ([StationMac])
);
