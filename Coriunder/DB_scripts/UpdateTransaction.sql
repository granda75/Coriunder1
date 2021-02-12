/****** Object:  StoredProcedure [dbo].[UpdateTransactionData]    Script Date: 2/12/2021 3:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alexandra Grenader
-- Create date: 12/02/2020
-- Description:	UpdateTransactionData
-- =============================================
ALTER PROCEDURE [dbo].[UpdateTransactionData]
	-- Add the parameters for the stored procedure here
	@p_transId int,
	@p_replyCode char(3),
	@p_replyDesc nvarchar(250)
AS
BEGIN
	
    -- Insert statements for procedure here
	UPDATE [dbo].[Transactions]
	SET ReplyCode = @p_replyCode,
		ReplyDescription = @p_replyDesc
	WHERE Id = @p_transId;

END