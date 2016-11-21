namespace QuizwanieServiceV3
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UnauthorizedQuestionsSet")]
    public partial class UnauthorizedQuestionsSet
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string CorrectAnswer { get; set; }

        [Required]
        public string WrongAnswer1 { get; set; }

        [Required]
        public string WrongAnswer2 { get; set; }

        [Required]
        public string WrongAnswer3 { get; set; }

        public int CreatorId { get; set; }
    }
}
