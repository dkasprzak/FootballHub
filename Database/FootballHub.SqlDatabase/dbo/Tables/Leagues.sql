﻿CREATE TABLE [dbo].[Leagues]
(
	[Id] INT IDENTITY(1, 1) NOT NULL,
	[LeagueName] NVARCHAR(200) NOT NULL,
	[FileName] NVARCHAR(200) NOT NULL,
    [FileType] NVARCHAR(100) NOT NULL,
    [ContentType] NVARCHAR(100) NOT NULL,
    [Data] VARBINARY(MAX) NOT NULL,
	[CountryId] INT NOT NULL,
	[CreateDate] DATETIMEOFFSET NOT NULL,
	CONSTRAINT [PK_Leagues] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Leagues_Countries] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Countries] ([Id]) ON DELETE CASCADE
);

GO
CREATE INDEX [IX_Leagues_Countries] on [dbo].[Leagues] ([CountryId]);
