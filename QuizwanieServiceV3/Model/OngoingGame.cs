using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace QuizwanieServiceV3.Model
{
    [DataContract]
    public class OngoingGame
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int OpponentUserId { get; set; }

        [DataMember]
        public int CallerUserCorrectAnswers { get; set; }

        [DataMember]
        public int OpponentCorrectAnswers { get; set; }

        [DataMember]
        public int CallerUserAnswerCount { get; set; }

        [DataMember]
        public int OpponentAnswerCount { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public bool IsGameOver { get; set; }

        [DataMember]
        public List<Question> QuestionsSet { get; set; }

        public OngoingGame(OngoingGameSet game)
        {
            QuestionsSet = new List<Question>();

            this.Id = game.Id;
            this.UserId = game.User_Id;
            this.CallerUserAnswerCount = game.CallerUserAnswerCount;
            this.CallerUserCorrectAnswers = game.CallerUserCorrectAnswers;

            this.OpponentAnswerCount = game.OpponentAnswerCount;
            this.OpponentCorrectAnswers = game.OpponentCorrectAnswers;
            this.OpponentUserId = game.OpponentUserId;

            this.IsGameOver = game.IsGameOver;

            foreach (QuestionsSet question in game.QuestionsSet)
            {
                this.QuestionsSet.Add(new Question(question));
            }
        }
    }
}