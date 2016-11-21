namespace QuizwanieServiceV3
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PendingGameSet")]
    public partial class PendingGameSet
    {
        public int Id { get; set; }

        public int User_Id { get; set; }

        public virtual UserSet UserSet { get; set; }
    }
}
