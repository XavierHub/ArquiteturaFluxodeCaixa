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