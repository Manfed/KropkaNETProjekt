namespace QuizwanieServiceV3
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataModel : DbContext
    {
        public DataModel()
            : base("name=DataModel")
        {
        }

        public virtual DbSet<OngoingGameSet> OngoingGameSet { get; set; }
        public virtual DbSet<PendingGameSet> PendingGameSet { get; set; }
        public virtual DbSet<QuestionsSet> QuestionsSet { get; set; }
        public virtual DbSet<UnauthorizedQuestionsSet> UnauthorizedQuestionsSet { get; set; }
        public virtual DbSet<UserSet> UserSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OngoingGameSet>()
                .HasMany(e => e.QuestionsSet)
                .WithMany(e => e.OngoingGameSet)
                .Map(m => m.ToTable("OngoingGameQuestions").MapLeftKey("OngoingGame_Id").MapRightKey("Questions_Id"));

            modelBuilder.Entity<UserSet>()
                .HasMany(e => e.OngoingGameSet)
                .WithRequired(e => e.UserSet)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserSet>()
                .HasMany(e => e.PendingGameSet)
                .WithRequired(e => e.UserSet)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);
        }
    }
}
