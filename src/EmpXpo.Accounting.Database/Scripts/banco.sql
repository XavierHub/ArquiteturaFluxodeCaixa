-- BEGIN Criação do Banco
USE [master]
GO

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'DbAccounting')
  BEGIN
    CREATE DATABASE [DbAccounting]
  END
GO
   USE [DbAccounting]
GO
-- END Criação do Banco


CREATE TABLE [dbo].[Seller]
(
   [Id]                    INT           PRIMARY KEY  IDENTITY   NOT NULL,	
   [Name]                  VARCHAR(80)                           NOT NULL,
   [Email]                 VARCHAR(50)                           NOT NULL,   
   [CreatedOn]             SMALLDATETIME                         NOT NULL,
)
GO

CREATE TABLE [dbo].[CashFlow]
(
   [Id]                    INT           PRIMARY KEY  IDENTITY   NOT NULL,	
   [SellerId]              INT                                   NOT NULL,
   [Type]                  INT                                   NOT NULL,
   [Amount]                DECIMAL(10, 2)                        NOT NULL,
   [Description]           VARCHAR(100)                          NOT NULL,
   [CreatedOn]             SMALLDATETIME                         NOT NULL,    
   CONSTRAINT FK_CashFlowSeller FOREIGN KEY ([SellerId]) REFERENCES [dbo].[Seller](Id)
)
GO
CREATE INDEX [IX_CashFlow_CreatedOn] ON [dbo].[CashFlow] ([CreatedOn])
GO

CREATE TABLE [dbo].[Report]
(
   [Id]                    INT           PRIMARY KEY  IDENTITY   NOT NULL,
   [SellerId]              INT                                   NOT NULL,
   [Debit]                 DECIMAL(10, 2)                        NOT NULL DEFAULT(0),
   [Credit]                DECIMAL(10, 2)                        NOT NULL DEFAULT(0),
   [DailyBalance]          DECIMAL(18, 2)                        NOT NULL DEFAULT(0),
   [Reference]             SMALLDATETIME                         NOT NULL,
   [ProcessingDate]        SMALLDATETIME                         NOT NULL,
   CONSTRAINT FK_ReportSeller FOREIGN KEY([SellerId]) REFERENCES [dbo].[Seller](Id)
)
GO
CREATE INDEX [IX_Report_Reference] ON [dbo].[Report]([Reference])
GO


DROP PROCEDURE IF EXISTS [dbo].[CashFlowReport];
GO
CREATE PROCEDURE [dbo].[CashFlowReport]
(
    @StartDate SMALLDATETIME,
    @EndDate   SMALLDATETIME
)
AS
BEGIN 
    SET NOCOUNT ON;

    /* Ex.:
            Procedure que retorna o valor de debito e credito de acordo com ranger das datas especificadas

            EXEC [dbo].[CashFlowReport] @StartDate='20240719', @EndDate = '20240720';
            EXEC [dbo].[CashFlowReport] @StartDate='20240701', @EndDate = '20240801';
    */      

    SELECT 
           ISNULL(SUM(CASE WHEN cf.[Type] = 0 THEN cf.[Amount] ELSE 0 END), 0) AS [Debit],
           ISNULL(SUM(CASE WHEN cf.[Type] = 1 THEN cf.[Amount] ELSE 0 END), 0) AS [Credit],
           ISNULL(SUM(CASE WHEN cf.[Type] = 0 THEN cf.[Amount] ELSE 0 END), 0) +
           ISNULL(SUM(CASE WHEN cf.[Type] = 1 THEN cf.[Amount] ELSE 0 END), 0) AS [DailyBalance],           
           GETDATE()                                                           AS [ProcessingDate]
      FROM [dbo].[CashFlow] cf
     WHERE cf.[CreatedOn] BETWEEN @StartDate AND @EndDate;

    SET NOCOUNT OFF;
END


GO

DROP PROCEDURE IF EXISTS [dbo].[CashFlowReportDate];
GO
CREATE PROCEDURE [dbo].[CashFlowReportDate]
AS
BEGIN
      SET NOCOUNT ON;
      /* Ex.:
            Procedure que retorna as datas disponíveis para geração do relatório

            EXEC [dbo].[CashFlowReportDate]            
      */      
     
       SELECT DISTINCT
              FORMAT(cf.CreatedOn, 'yyyy-MM-dd 00:00:00') CreatedOn
         FROM [dbo].[CashFlow] cf         
     GROUP BY CreatedOn

     SET NOCOUNT OFF;
END