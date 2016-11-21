using QuizwanieServiceV3.Model;
using QuizwanieServiceV3.Utils;
using QuizwanieV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace QuizwanieServiceV3
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "QuizwanieService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select QuizwanieService.svc or QuizwanieService.svc.cs at the Solution Explorer and start debugging.
    public class QuizwanieService : IQuizwanieService
    {

        DataModel context = new DataModel();

        public User GetUser(int id)
        {
            var user = context.UserSet.SingleOrDefault(x => x.Id == id);
            if (user != null)
            {
                return new User(user);
            }
            return null;
        }

        public User CreateUser(string name, string password)
        {
            var validateUserName = context.UserSet.SingleOrDefault(user => user.Name == name);
            if (validateUserName == null)
            {
                context.UserSet.Add(new UserSet { Name = name, Password = password, Role = "User" });
                context.SaveChanges();
                var createdUser = context.UserSet.SingleOrDefault(user => user.Name == name);
                if (createdUser != null)
                {
                    return new User(createdUser);
                }
            }
            return null;
        }

        public string hashSha512(string str)
        {
            return SHA.GenerateSHA512String(str);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public void startLookingForGame(int userId)
        {
            var pendingGame = context.PendingGameSet.SingleOrDefault(x => x.UserSet.Id == userId);

            if (pendingGame == null)
            {
                if (!context.PendingGameSet.Any())
                {
                    context.PendingGameSet.Add(new PendingGameSet { UserSet = getUserSet(userId) });
                    context.SaveChanges();
                }
                else
                {
                    var firstPendingGame = context.PendingGameSet.First();
                    //Game start
                    context.OngoingGameSet.Add(new OngoingGameSet { UserSet = firstPendingGame.UserSet, OpponentUserId = userId });
                    context.PendingGameSet.Remove(firstPendingGame);
                }
            }
        }

        public bool isGameInProgress(int userId)
        {
            var game = context.OngoingGameSet.SingleOrDefault(x => x.UserSet.Id == userId);
            if (game != null)
            {
                return (game.OpponentAnswerCount < 5 || game.CallerUserAnswerCount < 5);
            }
            return false;
        }

        public List<User> getUsersRanking()
        {
            var usersList =  context.UserSet.OrderByDescending(user => user.Points).ToList();
            List<User> users = new List<User>();
            foreach (var user in usersList)
            {
                if (!user.Role.Equals("Admin"))
                {
                    users.Add(new User(user));
                }
            }
            return users;
        }

        public bool isGameSearching(int userId)
        {
            return context.PendingGameSet.SingleOrDefault(x => x.UserSet.Id == userId) != null;
        }

        public User ValidateUser(string name, string password)
        {
            var user = context.UserSet.SingleOrDefault(x => x.Name == name);
            if (user != null && user.Password.Equals(password))
            {
                User userToSend = new User(user);
                return userToSend;
            }
            return null;
        }

        public string GetUserRole(int id)
        {
            var user = context.UserSet.SingleOrDefault(x => x.Id == id);
            if (user != null)
            {
                return user.Role;
            }
            return null;
        }

        public bool AddQuestion(string name, string password, string content, string correctAnswer, string wrongAnswer1, string wrongAnswer2, string wrongAnswer3)
        {
            User user = ValidateUser(name, password);
            // nie pozwol niezalogowanym na wrzucanie pytan
            if (user == null)
                return false;
            UnauthorizedQuestionsSet questionSet = new UnauthorizedQuestionsSet();
            questionSet.Content = content;
            questionSet.CorrectAnswer = correctAnswer;
            questionSet.WrongAnswer1 = wrongAnswer1;
            questionSet.WrongAnswer2 = wrongAnswer2;
            questionSet.WrongAnswer3 = wrongAnswer3;
            context.UnauthorizedQuestionsSet.Add(questionSet);
            context.SaveChanges();
            return true;
        }

        public void authorizeQuestion(int id)
        {

        }

        [WebGet]
        public void fill()
        {
            var fill = new DatabaseFiller();
        }

        private UserSet getUserSet(int userId)
        {
            return context.UserSet.SingleOrDefault(x => x.Id == userId);
        }
    }
}
