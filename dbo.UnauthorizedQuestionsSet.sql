CREATE TABLE [dbo].[UnauthorizedQuestionsSet] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Content]       NVARCHAR (MAX) NOT NULL,
    [CorrectAnswer] NVARCHAR (MAX) NOT NULL,
    [WrongAnswer1]  NVARCHAR (MAX) NOT NULL,
    [WrongAnswer2]  NVARCHAR (MAX) NOT NULL,
    [WrongAnswer3]  NVARCHAR (MAX) NOT NULL,
    [CreatorId] INT NULL, 
    CONSTRAINT [PK_UnauthorizedQuestionsSet] PRIMARY KEY CLUSTERED ([Id] ASC)
);

