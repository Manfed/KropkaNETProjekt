using QuizwanieServiceV3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace QuizwanieServiceV3
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IQuizwanieService" in both code and config file together.
    [ServiceContract]
    public interface IQuizwanieService
    {

        [OperationContract]
        User GetUser(int id);

        [OperationContract]
        User CreateUser(string name, string password);

        [OperationContract]
        User ValidateUser(string name, string password);

        [OperationContract]
        string hashSha512(string str);

        [OperationContract]
        bool isGameInProgress(int userId);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void startLookingForGame(int userId);

        [OperationContract]
        List<User> getUsersRanking(string userName, string password);

        [OperationContract]
        bool isGameSearching(int userId);

        [OperationContract]
        bool AddQuestion(string name, string password, string content, string correctAnswer, string wrongAnswer1, string wrongAnswer2, string wrongAnswer3);

        [OperationContract]
        bool AuthorizeQuestion(string userName, string password, int id);

        [OperationContract]
        List<Question> GetUnauthorizedQuestions(string userName, string password);

        [OperationContract]
        void DeleteUnauthorizeQuestion(string userName, string password, int id);

        [OperationContract]
        List<OngoingGame> GetUserGames(string userName, string password, int userId);

        [OperationContract]
        void CheckAnswer(int gameId, int questionId, int userId, string answer);

        [OperationContract]
        Question GetNextQuestion(int gameId, int userId);

        [OperationContract]
        void fill();
    }
}