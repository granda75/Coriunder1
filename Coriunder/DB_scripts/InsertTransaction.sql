/****** Object:  StoredProcedure [dbo].[InsertTransaction]    Script Date: 2/12/2021 3:24:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alexandra Grenader
-- Create date: 12/02/2020
-- Description:	Inserts transaction to transactions table
-- =============================================
ALTER PROCEDURE [dbo].[InsertTransaction]
	-- Add the parameters for the stored procedure here
	@p_cardHolderName  nvarchar(50),
	@p_email nvarchar(50),
	@p_phone nvarchar(50),
	@p_cardNumber int,
	@p_expDate char(4),
	@p_Cvv int,
	@p_address nvarchar(250),
	@p_city nvarchar(50),
	@p_zipCode varchar(10),
	@p_country char(2)
AS
BEGIN

	INSERT INTO [dbo].[Transactions] (CardHolderName, Email, Phone, CardNumber, ExpDate, Cvv, [Address], City, ZipCode, Country)
    VALUES (@p_cardHolderName, @p_email, @p_phone, @p_cardNumber, @p_expDate, @p_Cvv, @p_address, @p_city, @p_zipCode, @p_country);

END
