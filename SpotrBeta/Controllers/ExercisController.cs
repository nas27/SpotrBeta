using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SpotrBeta.Models;

namespace SpotrBeta.Controllers
{
    [Authorize]
    public class ExercisController : Controller
    {
        private SpotrContext db = new SpotrContext();

        // GET: Exercis
        public ActionResult Index()
        {
            var exercises = db.Exercises.Where(x => x.Workout.User.Email == User.Identity.Name).Include(e => e.Workout);
            return View(exercises.ToList());
        }

        // GET: Exercis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exercis exercis = db.Exercises.Find(id);
            if (exercis == null)
            {
                return HttpNotFound();
            }
            return View(exercis);
        }

        // GET: Exercis/Create
        public ActionResult Create()
        {
            SelectList list = new SelectList(db.Workouts.Where(x => x.User.Email == User.Identity.Name), "Id", "Name");

            ViewBag.Workout_Id = list.OrderByDescending(x => x.Value);
            return View();
        }

        // POST: Exercis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,NumReps,RestTime,Weight,Workout_Id")] Exercis exercis)
        {
            if (ModelState.IsValid)
            {
                db.Exercises.Add(exercis);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Workout_Id = new SelectList(db.Workouts, "Id", "Name", exercis.Workout_Id);
            return View(exercis);
        }

        // GET: Exercis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exercis exercis = db.Exercises.Find(id);
            if (exercis == null)
            {
                return HttpNotFound();
            }
            ViewBag.Workout_Id = new SelectList(db.Workouts, "Id", "Name", exercis.Workout_Id);
            return View(exercis);
        }

        // POST: Exercis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,NumReps,RestTime,Weight,Workout_Id")] Exercis exercis)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exercis).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Workout_Id = new SelectList(db.Workouts, "Id", "Name", exercis.Workout_Id);
            return View(exercis);
        }

        // GET: Exercis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exercis exercis = db.Exercises.Find(id);
            if (exercis == null)
            {
                return HttpNotFound();
            }
            return View(exercis);
        }

        // POST: Exercis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Exercis exercis = db.Exercises.Find(id);
            db.Exercises.Remove(exercis);
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
