namespace QuizwanieServiceV3
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OngoingGameSet")]
    public partial class OngoingGameSet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OngoingGameSet()
        {
            QuestionsSet = new HashSet<QuestionsSet>();
            CallerUserAnswerCount = 0;
            CallerUserCorrectAnswers = 0;
            OpponentAnswerCount = 0;
            OpponentCorrectAnswers = 0;
        }

        public int Id { get; set; }

        public int OpponentUserId { get; set; }

        public int CallerUserCorrectAnswers { get; set; }

        public int OpponentCorrectAnswers { get; set; }

        public int CallerUserAnswerCount { get; set; }

        public int OpponentAnswerCount { get; set; }

        public int User_Id { get; set; }

        public virtual UserSet UserSet { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionsSet> QuestionsSet { get; set; }
    }
}
