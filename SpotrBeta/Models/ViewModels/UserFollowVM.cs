using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpotrBeta.Models.ViewModels
{
    public class UserFollowVM
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
       
        public string SkillLevel { get; set; }
                
        public string Specialty { get; set; }

        public short Rating { get; set; }

        public virtual ICollection<Workout> Workouts { get; set; }

        public List<int> TrainersFollowed { get; set; }
    }
}