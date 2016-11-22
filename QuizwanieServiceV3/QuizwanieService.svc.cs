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
                    var questions = Get5RandomQuestions();
                    context.OngoingGameSet.Add(new OngoingGameSet { UserSet = firstPendingGame.UserSet, OpponentUserId = userId, QuestionsSet = questions });
                    context.PendingGameSet.Remove(firstPendingGame);
                    context.SaveChanges();
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

        public List<User> getUsersRanking(string userName, string password)
        {
            List<User> users = new List<User>();
            var validatedUser = ValidateUser(userName, password);
            if (validatedUser != null)
            {
                var usersList = context.UserSet.OrderByDescending(user => user.Points).ToList();
                foreach (var user in usersList)
                {
                    if (!user.Role.Equals("Admin"))
                    {
                        users.Add(new User(user));
                    }
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
            questionSet.CreatorId = user.Id;
            context.UnauthorizedQuestionsSet.Add(questionSet);
            context.SaveChanges();

            return true;
        }

        public List<Question> GetUnauthorizedQuestions(string userName, string password)
        {
            List<Question> questions = new List<Question>();
            User user = ValidateUser(userName, password);
            if (user != null && user.Role.Equals("Admin"))
            {
                foreach (UnauthorizedQuestionsSet question in context.UnauthorizedQuestionsSet)
                {
                    questions.Add(new Question(question));
                }
            }
            return questions;
        }

        public void DeleteUnauthorizeQuestion(string userName, string password, int id)
        {
            User user = ValidateUser(userName, password);
            if (user != null && user.Role.Equals("Admin"))
            {
                UnauthorizedQuestionsSet question = context.UnauthorizedQuestionsSet.SingleOrDefault(x => x.Id == id);
                if (question != null)
                {
                    context.UnauthorizedQuestionsSet.Remove(question);
                    context.SaveChanges();
                }
            }
        }

        public bool AuthorizeQuestion(string userName, string password, int id)
        {
            User user = ValidateUser(userName, password);
            if (user != null)
            {
                var unauthorizedQuestion = context.UnauthorizedQuestionsSet.SingleOrDefault(x => x.Id == id);
                if (unauthorizedQuestion != null)
                {
                    context.QuestionsSet.Add(new QuestionsSet
                    {
                        Content = unauthorizedQuestion.Content,
                        CorrectAnswer = unauthorizedQuestion.CorrectAnswer,
                        WrongAnswer1 = unauthorizedQuestion.WrongAnswer1,
                        WrongAnswer2 = unauthorizedQuestion.WrongAnswer2,
                        WrongAnswer3 = unauthorizedQuestion.WrongAnswer3
                    });

                    var creator = context.UserSet.SingleOrDefault(x => x.Id == unauthorizedQuestion.CreatorId);
                    if (creator != null)
                    {
                        creator.Points += 5;
                        context.SaveChanges();
                    }

                    context.UnauthorizedQuestionsSet.Remove(unauthorizedQuestion);

                    context.SaveChanges();

                    return true;
                }
            }

            return false;
        }

        public List<OngoingGame> GetUserGames(string userName, string password, int userId)
        {
            List<OngoingGame> games = new List<OngoingGame>();
            var user = ValidateUser(userName, password);

            if (user != null && !user.Role.Equals("Admin"))
            {
                var selectedGames = context.OngoingGameSet.Where(x => (x.User_Id == userId || x.OpponentUserId == userId)).ToList();
                foreach (OngoingGameSet game in selectedGames)
                {
                    games.Add(new OngoingGame(game));
                }
            }

            return games;
        }

        public void CheckAnswer(int gameId, int questionId, int userId, string answer)
        {
            var game = context.OngoingGameSet.SingleOrDefault(x => x.Id == gameId);
            if (game != null && !game.IsGameOver)
            {
                var question = context.QuestionsSet.SingleOrDefault(x => x.Id == questionId);
                if (question != null)
                {
                    if (game.User_Id == userId)
                    {
                        if (question.CorrectAnswer.Equals(answer))
                        {
                            game.CallerUserCorrectAnswers++;
                        }
                        game.CallerUserAnswerCount++;
                    }
                    else
                    {
                        if (question.CorrectAnswer.Equals(answer))
                        {
                            game.OpponentCorrectAnswers++;
                        }
                        game.OpponentAnswerCount++;
                    }
                    if (game.OpponentAnswerCount == 5 && game.CallerUserAnswerCount == 5)
                    {
                        game.IsGameOver = true;
                        RewardWinner(game);
                    }
                    context.SaveChanges();
                }
            }
        }

        public Question GetNextQuestion(int gameId, int userId)
        {
            Question nextQuestion = null;
            var game = context.OngoingGameSet.SingleOrDefault(x => x.Id == gameId);
            if (game != null && !game.IsGameOver)
            {
                if (userId == game.User_Id && game.CallerUserAnswerCount < 5)
                {
                    nextQuestion = new Question(game.QuestionsSet.ElementAt(game.CallerUserAnswerCount));
                }
                else if (userId == game.OpponentUserId && game.OpponentAnswerCount < 5) 
                {
                    nextQuestion = new Question(game.QuestionsSet.ElementAt(game.OpponentAnswerCount));
                }
            }
            return nextQuestion;
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

        private List<QuestionsSet> Get5RandomQuestions()
        {
            Random random = new Random();
            return context.QuestionsSet.ToList().OrderBy(x => random.Next()).Take(5).ToList();
        }

        private void RewardWinner(OngoingGameSet game)
        {
            UserSet winner, loser;
            int winnerReward, loserPunishment;
            if (game.CallerUserCorrectAnswers > game.OpponentCorrectAnswers)
            {
                winner = context.UserSet.SingleOrDefault(x => x.Id == game.User_Id);
                loser = context.UserSet.SingleOrDefault(x => x.Id == game.OpponentUserId);

                winnerReward = 10 + 3 * game.CallerUserCorrectAnswers - 3 * (5 - game.CallerUserCorrectAnswers);
                loserPunishment = -10 + 3 * game.OpponentCorrectAnswers - 3 * (5 - game.OpponentCorrectAnswers);
            }
            else if (game.OpponentCorrectAnswers > game.CallerUserCorrectAnswers)
            {
                winner = context.UserSet.SingleOrDefault(x => x.Id == game.OpponentUserId);
                loser = context.UserSet.SingleOrDefault(x => x.Id == game.User_Id);

                winnerReward = 10 + 3 * game.OpponentCorrectAnswers - 3 * (5 - game.OpponentCorrectAnswers);
                loserPunishment = -10 + 3 * game.CallerUserCorrectAnswers - 3 * (5 - game.CallerUserCorrectAnswers);
            }
            else
            {
                winner = context.UserSet.SingleOrDefault(x => x.Id == game.User_Id);
                loser = context.UserSet.SingleOrDefault(x => x.Id == game.OpponentUserId);

                winnerReward = 10 + 3 * game.OpponentCorrectAnswers - 3 * (5 - game.OpponentCorrectAnswers);
                loserPunishment = -10 + 3 * game.CallerUserCorrectAnswers - 3 * (5 - game.CallerUserCorrectAnswers);
            }

            winner.Points += winnerReward;
            loser.Points += loserPunishment;

            context.SaveChanges();
        }
    }
}
