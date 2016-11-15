using SpotrBeta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpotrBeta.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private SpotrContext db = new SpotrContext();

        public ActionResult Index()
        {
         try
            {
                User currentUser = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                //avoid nullobjectreference exception
                if (currentUser != null)
                {
                    ViewBag.UserFollowed = db.Followers.Where(x => x.FollowerId == currentUser.Id);
                    ViewBag.AllUsers = db.Users.ToList();
                    ViewBag.AllExercises = db.Exercises.ToList();
                    ViewBag.AllWorkouts = db.Workouts.ToList();
                    ViewBag.Weight = currentUser.Weight;
                    ViewBag.Height = currentUser.Height;
                }
                


            }   
            catch(InvalidOperationException ex)
            {
                ex.ToString();
            }
                
         
            
            
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "What is Spotr";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}