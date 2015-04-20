using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Habilect;

namespace Habilect.Controllers
{
    public class PatientsController : Controller
    {
        private localHabEntities db = new localHabEntities();

        // GET: Patients
        [Authorize(Roles = "Doctor,Administrator")]
        public ActionResult Index()
        {
            var patients = db.Patients.Include(p => p.Doctors);
            return View(patients.ToList());
        }

        // GET: Patients/Details/5
        [Authorize(Roles = "Doctor,Administrator")]
        public ActionResult Statistics(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //var attempts = db.Attempts.Where(a => a.Sessions.PatientSchedule.PatientId == id);
            var patients = db.PatientCourses.Where(a => a.PatientId == id);
            ViewBag.Patient = db.Patients.Find(id).Name;
            return View(patients);
        }

        /*[Authorize(Roles = "Doctor,Administrator")]
         * public ActionResult CourseStatistics(int? patientid, int? courseid)
        {
            if (patientid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (courseid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //через PatientsSchedule плохо, потому что он затирается !
            //var attempts = db.Attempts.Where(a => a.Sessions.PatientSchedule.PatientId == patientid && a => a.Sessions.PatientSchedule.Patients.PatientCourses);
            var attempts = db.Attempts.Where(a => a.Motions.CourseMotions.CourseId == courseid).Where(a => a.Motions.CourseMotions.Courses.PatientCourses.PatientId == patientid);
            var patients = db.PatientCourses.Where(a => a.PatientId == patientid);
            ViewBag.Patient = db.Patients.Find(patientid).Name;
            return View(patients);
        }*/

        [Authorize(Roles = "Doctor,Administrator")]
        public ActionResult Schedule(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var patients = db.PatientSchedule.Where(a => a.PatientId == id && a.CourseOrder == 1);

            if (patients == null)
            {
                return HttpNotFound();
            }
            return View(patients);
        }

        [Authorize(Roles = "Doctor,Administrator")]
        public ActionResult Courses(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var patients = db.PatientSchedule.Where(a => a.PatientId == id);

            if (patients == null)
            {
                return HttpNotFound();
            }
            return View(patients);
        }


        [Authorize(Roles = "Doctor,Administrator")]
        public ActionResult DateSchedule(DateTime? plandate, int? patientid)
        {
            if (plandate == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = db.PatientSchedule.Where(a => a.PlanDate == plandate && a.PatientId == patientid);

            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        [Authorize(Roles = "Doctor,Administrator")]
        public ActionResult DisplayCourse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = db.CourseMotions.Where(a => a.CourseId == id);

            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // GET: Patients/Create
        [Authorize(Roles = "Doctor,Administrator")]
        public ActionResult Create()
        {
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "SecondName");
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Doctor,Administrator")]
        public ActionResult Create([Bind(Include = "Id,Name,DoctorId")] Patients patients)
        {
            if (ModelState.IsValid)
            {
                db.Patients.Add(patients);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "SecondName", patients.DoctorId);
            return View(patients);
        }

        // GET: Doctors/Create
        [Authorize(Roles = "Doctor,Administrator")]
        public ActionResult CreateCourse()
        {return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Doctor,Administrator")]
        public ActionResult CreateCourse([Bind(Include = "Id,Name")] Courses course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Courses");
            }

            return View(course);
        }


        // GET: Patients/Edit/5
        [Authorize(Roles = "Doctor,Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patients patients = db.Patients.Find(id);
            if (patients == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "SecondName", patients.DoctorId);
            return View(patients);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Doctor,Administrator")]
        public ActionResult Edit([Bind(Include = "Id,Name,DoctorId")] Patients patients)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patients).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "SecondName", patients.DoctorId);
            return View(patients);
        }

        // GET: Patients/Delete/5
        [Authorize(Roles = "Doctor,Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patients patients = db.Patients.Find(id);
            if (patients == null)
            {
                return HttpNotFound();
            }
            return View(patients);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Doctor,Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Patients patients = db.Patients.Find(id);
            db.Patients.Remove(patients);
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
