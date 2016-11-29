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
                //fb case
                var tmp = User.Identity.Name.Split('_')[0];
                User currentFBUser = db.Users.Where(x => x.FirstName == tmp).FirstOrDefault();

                if (currentUser != null)
                {

                    ViewBag.UserFollowed = db.Followers.Where(x => x.FollowerId == currentUser.Id);
                    ViewBag.AllUsers = db.Users.ToList();
                    ViewBag.AllExercises = db.Exercises.ToList();
                    ViewBag.AllWorkouts = db.Workouts.ToList();
                    ViewBag.userRating = currentUser.Rating;

                }
                else if (currentFBUser != null)
                {
                    ViewBag.UserFollowed = db.Followers.Where(x => x.FollowerId == currentFBUser.Id);
                    ViewBag.AllUsers = db.Users.ToList();
                    ViewBag.AllExercises = db.Exercises.ToList();
                    ViewBag.AllWorkouts = db.Workouts.ToList();
                    ViewBag.userRating = currentFBUser.Rating;
                }
                else
                {
                    return View();
                }
                
                
            }   
            catch(Exception ex)
            {
                ex.ToString();
            }

            
            return View();
        }

        [HttpPost]
        public ActionResult UpVote(int id)
        {
            var trainer = db.Users.Find(id);

            trainer.Rating++;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
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