
CREATE PROCEDURE [dbo].[spInsert_server_response_log] 
	@start_time [datetime2](2),
	@end_time [datetime2](2) = null,
	@status_code [smallint] = null,
	@response_text [varchar](max) = null,
	@error_code [smallint] = -1		--optional, -1 is an invalid code
AS
BEGIN

	/*
		Created		20190716
		By			Wes Brown
		Description	Stored procedure to insert rows into server_response_log table.  
					Processes should not insert into table directly - they should use this procedure

		Modified
		Date		By		Description
		20190717	Wes		Treat status_code of 0 same as null
		
	*/

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	IF(@error_code = -1)
	BEGIN
		IF(isnull(@status_code,0) is null)
			set @error_code = -999		--timeout
		ELSE IF(@status_code = 200)
			set @error_code = 1			--ok code
		ELSE 
			set @error_code = 2			--anything else
	END


	insert into server_response_log ([start_time]
      ,[end_time]
      ,[status_code]
      ,[response_text]
      ,[error_code]
	)
	VALUES (
		@start_time
		  ,@end_time
		  ,@status_code
		  ,@response_text
		  ,@error_code
	)

END