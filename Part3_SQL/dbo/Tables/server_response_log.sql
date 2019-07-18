CREATE TABLE [dbo].[server_response_log] (
    [server_response_log_Id] INT           IDENTITY (1, 1) NOT NULL,
    [start_time]             DATETIME2 (2) NOT NULL,
    [end_time]               DATETIME2 (2) NULL,
    [status_code]            SMALLINT      NULL,
    [response_text]          VARCHAR (MAX) NULL,
    [error_code]             SMALLINT      NOT NULL,
    CONSTRAINT [PK_server_response_log] PRIMARY KEY CLUSTERED ([server_response_log_Id] ASC)
);






GO
CREATE NONCLUSTERED INDEX [IX_ServerResponseLog_StartTime]
    ON [dbo].[server_response_log]([start_time] ASC)
    INCLUDE([response_text], [error_code]);

