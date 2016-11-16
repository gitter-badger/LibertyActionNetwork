


-- =============================================
-- Author:		Aaron Hoffman
-- Create date: 2013-02-20
-- Description:	Insert Log and Return Primary Key
-- =============================================
CREATE PROCEDURE [dbo].[InsertLog]
(
	 @LogGuid uniqueidentifier
	,@StoredOnUtc datetime = null
	,@CreatedOnUtc datetime = null
	,@ApplicationVersion char(14) = null
	,@UserId int = null
	,@SessionId uniqueidentifier = null
	,@ActionId uniqueidentifier = null
	,@Tag nvarchar(200) = null
	,@Category nvarchar(200) = null
	,@SubCategory nvarchar(200) = null
	,@Message nvarchar(4000) = null
	,@LogType tinyint = null
	,@UserHostAddress varchar(50) = null
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	insert into dbo.[Log]
	(
		 LogGuid
		,InsertedOnUtc
		,StoredOnUtc
		,CreatedOnUtc
		,ApplicationVersion
		,UserId
		,SessionId
		,ActionId
		,Tag
		,Category
		,SubCategory
		,[Message]
		,LogType
		,UserHostAddress
	)
	values
	(
		 @LogGuid
		,getutcdate()
		,@StoredOnUtc
		,@CreatedOnUtc
		,@ApplicationVersion
		,@UserId
		,@SessionId
		,@ActionId
		,@Tag
		,@Category
		,@SubCategory
		,@Message
		,@LogType
		,@UserHostAddress
	)
    
	return SCOPE_IDENTITY()
END


