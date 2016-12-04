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
                var tmp = User.Identity.Name.Split('_')[0];
                User currentFBUser = db.Users.Where(x => x.FirstName == tmp).FirstOrDefault();
                //CDC problem with this code - we need first and last name..... ex. for "chris" we have multiple matches
                //Fix this soon, find out if you can do 2 linq expressions for FName and LName and put them together with "&&"
                if (currentUser != null)
                {
                    ViewBag.CurrentUserId = currentUser.Id;
                    ViewBag.Weight = currentUser.Weight;
                    ViewBag.Height = currentUser.Height;
                    ViewBag.Rating = currentUser.Rating;
                    return View(currentUser);
                }
                else if (currentFBUser != null)
                {
                    //case for fb users

                    ViewBag.CurrentUserId = currentFBUser.Id;
                    ViewBag.Weight = currentFBUser.Weight;
                    ViewBag.Height = currentFBUser.Height;
                    return View(currentFBUser);
                }
                else
                {
                    return View();
                }
               
                

            }   
            catch(InvalidOperationException ex)
            {
                ex.ToString();
            }

           
            return HttpNotFound();


        }
        
        public ActionResult Follow(string id, string SpecialtyID)
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
                    List<User> lt = ViewBag.AllTrainers;
                    

                    try
                    {
                        //add two lists together, search for rating
                        short num = System.Convert.ToInt16(id);
                        ViewBag.AllTrainers.AddRange(db.Users.Where(x => x.Rating >= num).ToList());
                    }
                    //make sure it is a number not string
                    catch (FormatException error)
                    {
                        ViewBag.AllTrainers = db.Users.Where(x => x.FirstName.Contains(id)).ToList();
                        ViewBag.AllTrainers.AddRange(db.Users.Where(x => x.Specialty.Trim().ToUpper().Contains(id.Trim().ToUpper())).OrderByDescending(x => x.Rating).ToList());

                    }

                    if (id.Contains("Top 3"))
                    {
                        ViewBag.AllTrainers = db.Users.OrderByDescending(x => x.Rating).Take(3).ToList();

                    }
                    //if (SpecialtyID == "SpecialtySearch")
                    //{
                    //    ViewBag.AllTrainers = db.Users.Where(x => x.Specialty.Trim().ToUpper().Contains(id.Trim().ToUpper())).ToList();
                    //}
                }

                


                User currentUser = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                var tmp = User.Identity.Name.Split('_')[0];
                User currentFBUser = db.Users.Where(x => x.FirstName == tmp).FirstOrDefault();
                if (currentUser != null)
                {
                    ViewBag.CurrentTrainers = db.Followers.Where(x => x.FollowerId == currentUser.Id).ToList();
                }
                else if(currentFBUser != null)
                {
                    ViewBag.CurrentTrainers = db.Followers.Where(x => x.FollowerId == currentFBUser.Id).ToList();
                }
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
            User currentUser = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            var tmp = User.Identity.Name.Split('_')[0];
            User currentFBUser = db.Users.Where(x => x.FirstName == tmp).FirstOrDefault();

            if (currentUser == null)
            {
                if (followed != null)
                {

                    Follower temp = new Follower();


                    //see if this id exists in the database - if yes, we must select a new one 
                    do
                    {
                        temp.ID = temp.ID + 1;
                    } while (db.Followers.Find(temp.ID) != null);

                    temp.FollowerId = currentFBUser.Id;
                    temp.UserId = trainerNum;

                    //Determine if the trainer they're trying to follow is already on the list 
                    ViewBag.isFollowed = db.Followers.Where(x => x.FollowerId == currentFBUser.Id);
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
                }
                else
                {
                    var tempName = User.Identity.Name.Split(' ')[0];
                    currentFBUser = db.Users.Where(x => x.FirstName == tempName).FirstOrDefault();
                    Follower foll = db.Followers.Where(x => x.UserId == trainerNum).Where(n => n.FollowerId == currentFBUser.Id).FirstOrDefault();

                    db.Followers.Remove(foll);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }

            else
            {
                if (followed != null)
                {

                    Follower temp = new Follower();
    

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
                }
                else
                {
                    currentUser = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                    Follower foll = db.Followers.Where(x => x.UserId == trainerNum).Where(n => n.FollowerId == currentUser.Id).FirstOrDefault();

                    db.Followers.Remove(foll);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
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
