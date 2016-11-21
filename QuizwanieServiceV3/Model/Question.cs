using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace QuizwanieServiceV3.Model
{
    [DataContract]
    public class Question
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public string CorrectAnswer { get; set; }

        [DataMember]
        public string WrongAnswer1 { get; set; }

        [DataMember]
        public string WrongAnswer2 { get; set; }

        [DataMember]
        public string WrongAnswer3 { get; set; }

        public Question(UnauthorizedQuestionsSet unauthorizedQuestion)
        {
            this.Id = unauthorizedQuestion.Id;
            this.Content = unauthorizedQuestion.Content;
            this.CorrectAnswer = unauthorizedQuestion.CorrectAnswer;
            this.WrongAnswer1 = unauthorizedQuestion.WrongAnswer1;
            this.WrongAnswer2 = unauthorizedQuestion.WrongAnswer2;
            this.WrongAnswer3 = unauthorizedQuestion.WrongAnswer3;
        }

        public Question(QuestionsSet question)
        {
            this.Id = question.Id;
            this.Content = question.Content;
            this.CorrectAnswer = question.CorrectAnswer;
            this.WrongAnswer1 = question.WrongAnswer1;
            this.WrongAnswer2 = question.WrongAnswer2;
            this.WrongAnswer3 = question.WrongAnswer3;
        }
    }
}