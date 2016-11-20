using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SpotrBeta.Models;
using System.Windows.Forms;

namespace SpotrBeta.Controllers
{
    public class UsersController : Controller
    {
        private SpotrContext db = new SpotrContext();

        // GET: Users
        public ActionResult Index()
        {
            try
            {
                User currentUser = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                if (currentUser != null)
                {
                    ViewBag.CurrentUserId = currentUser.Id;
                    ViewBag.Weight = currentUser.Weight;
                    ViewBag.Height = currentUser.Height;
                }
                return View(currentUser);
                

            }   
            catch(InvalidOperationException ex)
            {
                ex.ToString();
            }

           
            return HttpNotFound();


        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Index([Bind(Include = "Id,FirstName,LastName,Email,Age,Weight,GoalWeight,Height,Country,SkillLevel,IsTrainer,Specialty,Rating")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(user).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(user);
        //}




        public ActionResult Follow(string id)
        {
            try
            {
                if (id == null)
                {
                    ViewBag.AllTrainers = db.Users.ToList();
                }
                else
                {
                    ViewBag.AllTrainers = db.Users.Where(x => x.FirstName.Contains(id)).ToList();
                }


                User currentUser = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                ViewBag.CurrentTrainers = db.Followers.Where(x => x.FollowerId == currentUser.Id).ToList();
                
                return View();
            }
            catch(System.Data.Entity.Core.EntityCommandExecutionException ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return View(db);
            }
                

        }

        [HttpPost]
        public ActionResult Follow(string followed, int trainerNum)
        {
            if (followed != null)
            {
                User currentUser = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                Follower temp = new Follower();

                //Random r = new Random();
                //int rInt = r.Next(0, 100);

                //temp.ID = temp.ID + r.Next(0, 9999);

                //see if this id exists in the database - if yes, we must select a new one 
                do
                {
                    temp.ID = temp.ID + 1;
                } while (db.Followers.Find(temp.ID) != null);

                temp.FollowerId = currentUser.Id;
                temp.UserId = trainerNum;

                //Determine if the trainer they're trying to follow is already on the list 
                ViewBag.isFollowed = db.Followers.Where(x => x.FollowerId == currentUser.Id);
                bool isDuplicate = false;
                foreach (var item in ViewBag.isFollowed)
                {
                    if (temp.UserId == item.UserId)
                    {
                        isDuplicate = true;
                    }
                }

                if (isDuplicate == false)
                {
                    //not already existing
                    db.Followers.Add(temp);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // user is already following this trainer so do not add to db again
                    //String ErrorMessage = "You are already following this User!";
                    //MessageBox.Show(ErrorMessage, "Could not add User:");
                    return RedirectToAction("Follow", "Users");
                }
            } else
            {
                User currentUser = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                Follower foll = db.Followers.Where(x => x.UserId == trainerNum).Where(n => n.FollowerId == currentUser.Id).FirstOrDefault();

                db.Followers.Remove(foll);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
        }


        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Email,Age,Weight,GoalWeight,Height,Country,SkillLevel,IsTrainer,Specialty,Rating")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,Age,Weight,GoalWeight,Height,Country,SkillLevel,IsTrainer,Specialty")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
    }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
