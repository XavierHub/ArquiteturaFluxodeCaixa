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
           CAST(cf.[CreatedOn] AS DATE)                                        AS [ReferenceDate],
           GETDATE()                                                           AS [ProcessingDate]
      FROM [dbo].[CashFlow] cf
     WHERE cf.[CreatedOn] BETWEEN @StartDate AND @EndDate;

    SET NOCOUNT OFF;
END
