using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SpotrBeta.Models;

namespace SpotrBeta.Controllers
{
    public class WorkoutsApiController : ApiController
    {
        private SpotrContext db = new SpotrContext();

        // GET: api/WorkoutsApi
        public IQueryable<Workout> GetWorkouts()
        {
            return db.Workouts;
        }

        // GET: api/WorkoutsApi/5
        [ResponseType(typeof(Workout))]
        public IHttpActionResult GetWorkout(int id)
        {
            Workout workout = db.Workouts.Find(id);
            if (workout == null)
            {
                return NotFound();
            }

            return Ok(workout);
        }

        // PUT: api/WorkoutsApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutWorkout(int id, Workout workout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != workout.Id)
            {
                return BadRequest();
            }

            db.Entry(workout).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkoutExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/WorkoutsApi
        [ResponseType(typeof(Workout))]
        public IHttpActionResult PostWorkout(Workout workout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Workouts.Add(workout);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = workout.Id }, workout);
        }

        // DELETE: api/WorkoutsApi/5
        [ResponseType(typeof(Workout))]
        public IHttpActionResult DeleteWorkout(int id)
        {
            Workout workout = db.Workouts.Find(id);
            if (workout == null)
            {
                return NotFound();
            }

            db.Workouts.Remove(workout);
            db.SaveChanges();

            return Ok(workout);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WorkoutExists(int id)
        {
            return db.Workouts.Count(e => e.Id == id) > 0;
        }
    }
}