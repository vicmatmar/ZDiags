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

/****** Object:  StoredProcedure [dbo].[GetNextMac]    Script Date: 8/21/2017 9:42:55 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetNextMac]
	@StartBlock BIGINT,
	@EndBlock BIGINT,
	@NewMac BIGINT OUTPUT
AS
BEGIN
	BEGIN TRANSACTION
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Mac BIGINT;
	SET @Mac = 0;

	IF @StartBlock > @EndBlock
		-- Blocks are out of order
		SET @NewMac = 0;
	ELSE
		BEGIN
			SELECT TOP 1 @Mac = [Mac]
			FROM [MacAddress]
			WITH (TABLOCK, HOLDLOCK)
			WHERE MAC >= @StartBlock AND MAC <= @EndBlock
			ORDER BY MAC DESC;

			IF @Mac = @EndBlock
				-- All MACs have been assigned in block range
				SET @NewMac = 0;
			ELSE IF @Mac = 0
				-- No MACs have been assigned in block range
				-- Use start of block range
				SET @NewMac = @StartBlock;
			ELSE
				-- Get next MAC in block range
				SET @NewMac = @Mac + 2;

			IF @NewMac <> 0
				INSERT INTO MacAddress (MAC) VALUES (@NewMac);
		END

	COMMIT TRANSACTION
END
GO

