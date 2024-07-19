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
CREATE PROCEDURE [dbo].[CashFlowReport](   
   @StartDate     SMALLDATETIME,
   @EndDate       SMALLDATETIME
)
AS
BEGIN 
      SET NOCOUNT ON;

      /* Ex.:
            Procedure que retorna o valor de debito e credito de acordo com ranger das datas especificadas

            EXEC [dbo].[CashFlowReport] @StartDate='20240719', @EndDate = '20240720';
            EXEC [dbo].[CashFlowReport] @StartDate='20240701', @EndDate = '20240801';
      */      
      
      WITH CashFlow_cte([Type], [Amount])
      AS
      (
        SELECT 
               CASE WHEN cf.[Type] = 0 THEN 'Debit' ELSE 'Credit' END 'Type',
               cf.[Amount]           
         FROM [dbo].[CashFlow]  cf
         WITH (NOLOCK)
        WHERE cf.[CreatedOn] BETWEEN @StartDate AND @EndDate
      )
      SELECT  
              
              ISNULL([Debit], 0)                       AS [Debit], 
              ISNULL([Credit], 0)                      AS [Credit],
              ISNULL([Debit], 0) + ISNULL([Credit],0)  AS [DailyBalance],            
              GETDATE()                                AS [ProcessingDate]
        FROM CashFlow_cte
       PIVOT (
               SUM([Amount])
               FOR [Type] IN([Debit], [Credit])
             ) AS pvt
       

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
         WITH (NOLOCK)
     GROUP BY CreatedOn

     SET NOCOUNT OFF
END