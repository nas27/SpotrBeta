using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpotrBeta.Models.ViewModels
{
    public class WorkoutViewModel
    {
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }
        public string Name { get; set; }

        public int User_Id { get; set; }
        public virtual ICollection<ExerciseViewModel> ExerciseList { get; set; }

        public virtual User User { get; set; }
    }
}