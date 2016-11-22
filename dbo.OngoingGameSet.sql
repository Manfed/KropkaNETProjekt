CREATE TABLE [dbo].[OngoingGameSet] (
    [Id]                       INT IDENTITY (1, 1) NOT NULL,
    [OpponentUserId]           INT NOT NULL,
    [CallerUserCorrectAnswers] INT NOT NULL,
    [OpponentCorrectAnswers]   INT NOT NULL,
    [CallerUserAnswerCount]    INT NOT NULL,
    [OpponentAnswerCount]      INT NOT NULL,
    [User_Id]                  INT NOT NULL,
    [IsGameOver] BIT NOT NULL, 
    CONSTRAINT [PK_OngoingGameSet] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserOngoingGame] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[UserSet] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_UserOngoingGame]
    ON [dbo].[OngoingGameSet]([User_Id] ASC);

