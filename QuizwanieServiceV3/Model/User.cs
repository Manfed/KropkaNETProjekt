using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace QuizwanieServiceV3.Model
{
    [DataContract]
    public class User
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Points { get; set; }

        [DataMember]
        public string Role { get; set; }

        public User(UserSet user)
        {
            this.Id = user.Id;
            this.Name = user.Name;
            this.Points = user.Points;
            this.Role = user.Role;
        }
    }
}