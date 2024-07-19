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