using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PromotionalWebsite.Models;

namespace PromotionalWebsite.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Contacts
        public ActionResult Index()
        {
            return View(db.Contacts.ToList());
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactDM contactDM = db.Contacts.Find(id);
            if (contactDM == null)
            {
                return HttpNotFound();
            }
            return View(contactDM);
        }
        [AllowAnonymous]
        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,FirstName,Email,Message,Date")] ContactDM contactDM)
        {
            var captcha = Request.Form["g-recaptcha-response"]; const string secret = "6Lei-9sUAAAAAJmy7TqUVDe4hf0JvF6yYRn0dBrg";

            var restUrl = string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, captcha);

            WebRequest req = WebRequest.Create(restUrl);
            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

            JsonSerializer serializer = new JsonSerializer();

            CaptchaResult result = null;

            using (var reader = new StreamReader(resp.GetResponseStream()))
            {
                string resultObject = reader.ReadToEnd();
                result = JsonConvert.DeserializeObject<CaptchaResult>(resultObject);
            }
            if (!result.Success)
            {
                ModelState.AddModelError("", "captcha messagge error");
                if (result.ErrorCodes != null)
                {
                    ModelState.AddModelError("", result.ErrorCodes[0]);
                }
            
            }
            else
            {
                if (ModelState.IsValid)
                {
                    contactDM.Date = DateTime.Now;
                    db.Contacts.Add(contactDM);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(contactDM);
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactDM contactDM = db.Contacts.Find(id);
            if (contactDM == null)
            {
                return HttpNotFound();
            }
            return View(contactDM);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,FirstName,Email,Message,Date")] ContactDM contactDM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contactDM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contactDM);
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactDM contactDM = db.Contacts.Find(id);
            if (contactDM == null)
            {
                return HttpNotFound();
            }
            return View(contactDM);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContactDM contactDM = db.Contacts.Find(id);
            db.Contacts.Remove(contactDM);
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
