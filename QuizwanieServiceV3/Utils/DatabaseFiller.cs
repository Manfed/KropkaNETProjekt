using QuizwanieServiceV3;
using QuizwanieServiceV3.Utils;
using System.Collections.Generic;

namespace QuizwanieV3
{
    public class DatabaseFiller : System.Data.Entity.DropCreateDatabaseIfModelChanges<DataModel>
    {

        public DatabaseFiller()
        {
            var context = new DataModel();
            foreach (var entity in context.OngoingGameSet)
            {
                context.OngoingGameSet.Remove(entity);
            }
            foreach (var entity in context.PendingGameSet)
            {
                context.PendingGameSet.Remove(entity);
            }
            foreach (var entity in context.QuestionsSet)
            {
                context.QuestionsSet.Remove(entity);
            }
            foreach (var entity in context.UnauthorizedQuestionsSet)
            {
                context.UnauthorizedQuestionsSet.Remove(entity);
            }
            foreach (var entity in context.UserSet)
            {
                context.UserSet.Remove(entity);
            }
            context.SaveChanges();
            Seed(context);
        }

        protected override void Seed(DataModel context)
        {
            context.UserSet.Add(new UserSet { Name = "admin", Password = SHA.GenerateSHA512String("admin1"), Role = "Admin" });

            context.SaveChanges();

            var questions = new List<QuestionsSet> {
                new QuestionsSet
                {
                    Content = "Z jakiego kraju pochodzi firma Lenovo?",
                    CorrectAnswer = "Chiny",
                    WrongAnswer1 = "Korea Południowa",
                    WrongAnswer2 = "USA",
                    WrongAnswer3 = "Japonia"
                },

                new QuestionsSet
                {
                    Content = "Zjawisko piezoelektryczności zostało odkryte przez:",
                    CorrectAnswer = "Braci Curie",
                    WrongAnswer1 = "Woldemar Voigt",
                    WrongAnswer2 = "Andre Ampere",
                    WrongAnswer3 = "Allesandro Volt"
                },

                new QuestionsSet
                {
                    Content = "W którym roku uruchomiono wyszukiwarkę Google?",
                    CorrectAnswer = "1998",
                    WrongAnswer1 = "1995",
                    WrongAnswer2 = "2000",
                    WrongAnswer3 = "2001"
                },

                new QuestionsSet
                {
                    Content = "Który producent telewizorów pochodzi z Niemiec?",
                    CorrectAnswer = "Grundig",
                    WrongAnswer1 = "Panasonic",
                    WrongAnswer2 = "Samsung",
                    WrongAnswer3 = "Philips"
                },

                new QuestionsSet
                {
                    Content = "Jaki kolor ma zazwyczaj czarna skrzynka w samolocie?",
                    CorrectAnswer = "Pomarańczowy",
                    WrongAnswer1 = "Czarny",
                    WrongAnswer2 = "Biały",
                    WrongAnswer3 = "Różowy"
                },

                new QuestionsSet
                {
                    Content = "Jak nazywani są chińscy astronauci?",
                    CorrectAnswer = "Tajkonauci",
                    WrongAnswer1 = "Kosmonauci",
                    WrongAnswer2 = "Spationauci",
                    WrongAnswer3 = "Astronauci"
                },

                new QuestionsSet
                {
                    Content = "Jaki kolor ma ametyst?",
                    CorrectAnswer = "Fioletowy",
                    WrongAnswer1 = "Zielony",
                    WrongAnswer2 = "Czerwony",
                    WrongAnswer3 = "Pomarańczowy"
                },

                new QuestionsSet
                {
                    Content = "EEG to badanie",
                    CorrectAnswer = "Pracy mózgu",
                    WrongAnswer1 = "Pracy serca",
                    WrongAnswer2 = "Endoskopia guzów",
                    WrongAnswer3 = "Endoskopia głowy"
                },

                new QuestionsSet
                {
                    Content = "Co mają wspólnego Jowisz, Saturn, Uran i Neptun?",
                    CorrectAnswer = "To gazowe giganty",
                    WrongAnswer1 = "Znaleziono tam ślady życia",
                    WrongAnswer2 = "Są większe niż Słońce",
                    WrongAnswer3 = "Są najbliżej Słońca"
                },

                new QuestionsSet
                {
                    Content = "Planetą gazową nie jest",
                    CorrectAnswer = "Merkury",
                    WrongAnswer1 = "Saturn",
                    WrongAnswer2 = "Uran",
                    WrongAnswer3 = "Jowisz"
                }
            };

            questions.ForEach(s => context.QuestionsSet.Add(s));
            context.SaveChanges();
        }
    }
}