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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Habilect.Controllers
{
    public class AdminsController : Controller
    {
        private localHabEntities db = new localHabEntities();

        private ApplicationUserManager _userManager;

        // GET: Admins

        [Authorize(Roles="SuperAdministrator")]
        public ActionResult Index()
        { 
            return View(db.Admins.ToList());
        }

        // GET: Admins/Details/5
        [Authorize(Roles = "SuperAdministrator")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admins admins = db.Admins.Find(id);
            if (admins == null)
            {
                return HttpNotFound();
            }
            return View(admins);
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

        // GET: Admins/Create
        [Authorize(Roles = "SuperAdministrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdministrator")]
        public ActionResult Create([Bind(Include = "Id,ClinicName,Password,Login,AspNetUserId")] Admins admins)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = admins.Login, Email = admins.Login };
                IdentityResult result = UserManager.Create(user, admins.Password);
                UserManager.AddToRoleAsync(user.Id, "Administrator");
                admins.AspNetUserId = user.Id; 
                db.Admins.Add(admins);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index"); 
        }

        // GET: Admins/Edit/5
        [Authorize(Roles = "SuperAdministrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admins admins = db.Admins.Find(id);
            if (admins == null)
            {
                return HttpNotFound();
            }
            return View(admins);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdministrator")]
        public ActionResult Edit([Bind(Include = "Id,ClinicName,AspNetUserId,Password,Login")] Admins admins)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admins).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admins);
        }

        // GET: Admins/Delete/5
        [Authorize(Roles = "SuperAdministrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admins admins = db.Admins.Find(id);
            if (admins == null)
            {
                return HttpNotFound();
            }
            return View(admins);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdministrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Admins admins = db.Admins.Find(id);
            db.Admins.Remove(admins);
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
