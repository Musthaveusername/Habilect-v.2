using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Habilect;
using Microsoft.AspNet.Identity;
using Habilect.Models;
using Microsoft.Owin.Security;
using Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Habilect.Controllers
{
    public class DoctorsController : Controller
    {
        private localHabEntities db = new localHabEntities();

        private ApplicationUserManager _userManager;

        // GET: Doctors
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            string user_id = User.Identity.GetUserId();
            Admins admin = db.Admins.Where(b => b.AspNetUserId == user_id).First();

            var doctors = db.Doctors.Where(a => a.AdminId == admin.Id);

            return View(doctors);
        }

        // GET: Doctors/Details/5
        [Authorize(Roles = "Doctor,Administrator")]
        public ActionResult Patients(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctors doctors = db.Doctors.Find(id);
            if (doctors == null)
            {
                return HttpNotFound();
            }
            return View(doctors);
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Doctors/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create([Bind(Include = "Id,FirstName,SecondName,Login,Password,AdminId,AspNetUserId")] Doctors doctors)
        {
            if (ModelState.IsValid)
            {                
                var user = new ApplicationUser() { UserName = doctors.Login, Email = doctors.Login };
                IdentityResult result = UserManager.Create(user, doctors.Password);
                UserManager.AddToRoleAsync(user.Id, "Doctor");
                string user_id = User.Identity.GetUserId();
                doctors.AdminId = db.Admins.Where(a => a.AspNetUserId == user_id).First().Id;
                doctors.AspNetUserId = user.Id;
                db.Doctors.Add(doctors);
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: Doctors/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctors doctors = db.Doctors.Find(id);
            if (doctors == null)
            {
                return HttpNotFound();
            }
            return View(doctors);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit([Bind(Include = "Id,FirstName,SecondName,Login,Password,AdminId,AspNetUserId")] Doctors doctors)
        {
            if (ModelState.IsValid)
            {
                UserManager.RemovePassword(doctors.AspNetUserId);
                UserManager.AddPassword(doctors.AspNetUserId, doctors.Password);
                UserManager.SetEmail(doctors.AspNetUserId, doctors.Login);
                db.Entry(doctors).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(doctors);
        }

        // GET: Doctors/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctors doctors = db.Doctors.Find(id);
            if (doctors == null)
            {
                return HttpNotFound();
            }
            return View(doctors);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Doctors doctors = db.Doctors.Find(id);
            //ApplicationUser user = 
            //UserManager.DeleteAsync(user);
            db.Doctors.Remove(doctors);            
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
