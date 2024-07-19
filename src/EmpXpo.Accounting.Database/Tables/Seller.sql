CREATE TABLE [dbo].[Seller]
(
   [Id]                    INT           PRIMARY KEY  IDENTITY   NOT NULL,	
   [Name]                  VARCHAR(80)                           NOT NULL,
   [Email]                 VARCHAR(50)                           NOT NULL,   
   [CreatedOn]             SMALLDATETIME                         NOT NULL,
)