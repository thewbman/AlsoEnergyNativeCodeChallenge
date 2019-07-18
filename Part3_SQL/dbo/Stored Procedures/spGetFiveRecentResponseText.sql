

CREATE PROCEDURE [dbo].[spGetFiveRecentResponseText] 
	@start_time_range [datetime2](2),
	@end_time_tange [datetime2](2) 
AS
BEGIN

	/*
		Created		20190717
		By			Wes Brown
		Description	Stored procedure to get 5 most recent rows of [server_response_log] based on supplied start and end times

		Modified
		Date		By		Description
		
	*/

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	SELECT TOP (5) 
		  [start_time]
		  ,[response_text]
		  ,[error_code]
	FROM [dbo].[server_response_log]
	WHERE start_time >= @start_time_range
	and start_time <= @end_time_tange
	order by start_time DESC

END