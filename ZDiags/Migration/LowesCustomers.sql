USE ManufacturingStore_RAD
GO

CREATE TABLE [dbo].[LowesCustomers] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_LowesCustomers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

