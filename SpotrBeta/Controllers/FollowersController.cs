using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SpotrBeta.Models;
using System.Threading.Tasks;

namespace SpotrBeta.Controllers
{
    [Authorize]
    public class FollowersController : Controller
    {
        private SpotrContext db = new SpotrContext();

        // GET: Followers
        public ActionResult Index()
        {
            return View(db.Followers.ToList());
        }

        

        // GET: Followers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Follower follower = db.Followers.Find(id);
            if (follower == null)
            {
                return HttpNotFound();
            }
            return View(follower);
        }

        // GET: Followers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Followers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserId,FollowerId")] Follower follower)
        {
            if (ModelState.IsValid)
            {
                db.Followers.Add(follower);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(follower);
        }

        // GET: Followers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Follower follower = db.Followers.Find(id);
            if (follower == null)
            {
                return HttpNotFound();
            }
            return View(follower);
        }

        // POST: Followers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserId,FollowerId")] Follower follower)
        {
            if (ModelState.IsValid)
            {
                db.Entry(follower).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(follower);
        }

        // GET: Followers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Follower follower = db.Followers.Find(id);
            if (follower == null)
            {
                return HttpNotFound();
            }
            return View(follower);
        }

        // POST: Followers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Follower follower = db.Followers.Find(id);
            db.Followers.Remove(follower);
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
