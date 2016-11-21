namespace QuizwanieServiceV3
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QuestionsSet")]
    public partial class QuestionsSet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QuestionsSet()
        {
            OngoingGameSet = new HashSet<OngoingGameSet>();
        }

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

        public int? OngoingGameId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OngoingGameSet> OngoingGameSet { get; set; }
    }
}
