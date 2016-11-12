using SpotrBeta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpotrBeta.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private SpotrContext db = new SpotrContext();

        public ActionResult Index()
        {
            User currentUser = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            ViewBag.UserFollowed = db.Followers.Where(x => x.FollowerId == currentUser.Id);
            ViewBag.AllUsers = db.Users.ToList();

            ViewBag.AllWorkouts = db.Workouts.ToList();
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}