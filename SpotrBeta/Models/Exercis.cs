namespace SpotrBeta.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Exercises")]
    public partial class Exercis
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public int NumReps { get; set; }

        public int RestTime { get; set; }

        public int Weight { get; set; }

        public int Workout_Id { get; set; }

        public virtual Workout Workout { get; set; }
    }
}
