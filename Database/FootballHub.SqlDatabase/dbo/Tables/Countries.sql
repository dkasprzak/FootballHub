CREATE TABLE [dbo].[Countries]
(
	[Id] INT IDENTITY(1, 1) NOT NULL,
	[Name] NVARCHAR(100) NOT NULL,
	[ShortName] NVARCHAR(3) NOT NULL,
	CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO 

CREATE UNIQUE INDEX [UQ_Countries_Name] on [dbo].[Countries] ([Name]);

GO 

CREATE UNIQUE INDEX [UQ_Coubtries_ShortName] on [dbo].[Countries] ([ShortName]);
