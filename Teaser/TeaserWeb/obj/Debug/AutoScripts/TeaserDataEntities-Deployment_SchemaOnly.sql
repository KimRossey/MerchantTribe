CREATE TABLE [dbo].[TeaserEmails](
	[Id] [bigint] IDENTITY(4,1) NOT NULL,
	[TimeStampUtc] [datetime] NOT NULL,
	[Email] [nvarchar](1024) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)

GO
ALTER TABLE [dbo].[TeaserEmails] ADD  CONSTRAINT [PK_TeaserEmails] PRIMARY KEY 
(
	[Id]
)
GO
ALTER TABLE [dbo].[TeaserEmails] ADD  CONSTRAINT [UQ__TeaserEmails__000000000000000C] UNIQUE 
(
	[Id]
)
GO
