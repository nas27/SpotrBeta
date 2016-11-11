using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpotrBeta.Models.ViewModels
{
    public class ExerciseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Workout_Id { get; set; }
        public bool selected { get; set; }
    }
}