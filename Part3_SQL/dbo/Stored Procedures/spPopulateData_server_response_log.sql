

CREATE PROCEDURE [dbo].[spPopulateData_server_response_log] 
	@RowsToInsert int = 1000
AS
BEGIN

	/*
		Created		20190717
		By			Wes Brown
		Description	Seed rows into server_response_log for testing purposes

		Modified
		Date		By		Description
		
	*/

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	DECLARE @i int = 0;


	CREATE TABLE #tmp_server_response_log(
		[start_time] [datetime2](2) NOT NULL,
		[end_time] [datetime2](2) NULL,
		[status_code] [smallint] NULL,
		[response_text] [varchar](max) NULL,
		[error_code] [smallint] NOT NULL
	)

	DECLARE @start_time datetime2(2)
	DECLARE @end_time datetime2(2)
	DECLARE @status_code smallint
	DECLARE @response_text varchar(max)
	DECLARE @error_code smallint

	WHILE(@i < @RowsToInsert)
	BEGIN

		SET @start_time = DATEADD(minute,0-(RAND() * 1440),GETUTCDATE())	--random time in past 24 hours

		IF(SELECT @i % 3) = 0
		BEGIN
			--OK
			SET @end_time = DATEADD(second,1,@start_time)
			SET @status_code = 200
			SET @response_text = 'FAKE DATA: OK response'
			SET @error_code = 1
		END
		ELSE IF(SELECT @i % 3) = 1
		BEGIN
			--Error
			SET @end_time = null
			SET @status_code = 500
			SET @response_text = 'FAKE DATA: Error response'
			SET @error_code = 2
		END
		ELSE 
		BEGIN
			--Timeout
			SET @end_time = null
			SET @status_code = null
			SET @response_text = 'FAKE DATA: Timeout'
			SET @error_code = -999
		END

		EXEC [dbo].[spInsert_server_response_log] 
			   @start_time
			  ,@end_time
			  ,@status_code
			  ,@response_text
			  ,@error_code

		SET @i = @i + 1;
	END

END