
CREATE VIEW [dbo].[vResponseLogHourlySummary]
as
	/*
		Created		20190717
		By			Wes Brown
		Description	View to group row counts by hour and error_code

		Modified
		Date		By		Description
		
	*/

SELECT CAST(CONVERT(varchar(13),[start_time],120)+':00' as smalldatetime) as DateTimeHour
      ,[error_code]
	  ,COUNT(*) as RowCnt
  FROM [dbo].[server_response_log]
  group by CAST(CONVERT(varchar(13),[start_time],120)+':00' as smalldatetime)
      ,[error_code]