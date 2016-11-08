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
    public class ExercisApiController : ApiController
    {
        private SpotrContext db = new SpotrContext();

        // GET: api/ExercisApi
        public IQueryable<Exercis> GetExercises()
        {
            return db.Exercises;
        }

        // GET: api/ExercisApi/5
        [ResponseType(typeof(Exercis))]
        public IHttpActionResult GetExercis(int id)
        {
            Exercis exercis = db.Exercises.Find(id);
            if (exercis == null)
            {
                return NotFound();
            }

            return Ok(exercis);
        }

        // PUT: api/ExercisApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutExercis(int id, Exercis exercis)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != exercis.Id)
            {
                return BadRequest();
            }

            db.Entry(exercis).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExercisExists(id))
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

        // POST: api/ExercisApi
        [ResponseType(typeof(Exercis))]
        public IHttpActionResult PostExercis(Exercis exercis)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Exercises.Add(exercis);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = exercis.Id }, exercis);
        }

        // DELETE: api/ExercisApi/5
        [ResponseType(typeof(Exercis))]
        public IHttpActionResult DeleteExercis(int id)
        {
            Exercis exercis = db.Exercises.Find(id);
            if (exercis == null)
            {
                return NotFound();
            }

            db.Exercises.Remove(exercis);
            db.SaveChanges();

            return Ok(exercis);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExercisExists(int id)
        {
            return db.Exercises.Count(e => e.Id == id) > 0;
        }
    }
}