namespace SpotrBeta.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SpotrContext : DbContext
    {
        public SpotrContext()
            : base("name=SpotrContext")
        {
        }

        public virtual DbSet<Exercis> Exercises { get; set; }
        public virtual DbSet<Follower> Followers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Workout> Workouts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.Workouts)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Workout>()
                .HasMany(e => e.Exercises)
                .WithRequired(e => e.Workout)
                .HasForeignKey(e => e.Workout_Id)
                .WillCascadeOnDelete(false);
        }
    }
}
