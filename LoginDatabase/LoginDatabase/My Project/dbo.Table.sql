CREATE TABLE [dbo].[Table]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Title] VARCHAR(50) NOT NULL, 
    [Author] VARCHAR(50) NOT NULL, 
    [Genre] VARCHAR(50) NOT NULL, 
    [Price] MONEY NOT NULL, 
    [PublicationDate] DATE NOT NULL
)
