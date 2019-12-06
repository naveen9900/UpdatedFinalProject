using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LoginTestDemo.Models;

namespace LoginTestDemo.Controllers
{
    public class Home2Controller : Controller
    {
        private LoginEntities db = new LoginEntities();

        // GET: Home2
        //public ActionResult Index()
        //{
        //    return View(db.tblSkills.ToList());
        //}
       static string loggedinUser = "";
        public ActionResult Dashboard_Employee(string uname)
        {
            loggedinUser = uname;
            
            List<tblSkill> userdata = db.tblSkills.Where(x => x.skUsername == uname).ToList();
            
            return View(userdata);
        }
        
        [HttpPost]
        public ActionResult Dashboard_Employee(tblSkill data)
        {
            Session["skusername"] = Session["eUsername"];
            ViewBag.Name = Session["eFirstName"].ToString();
          
            List<tblSkill> userdata = db.tblSkills.Where(x => x.skUsername == data.skUsername).ToList();
            loggedinUser = data.skUsername;
            return View(userdata);
        }

        // GET: Home2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSkill tblSkill = db.tblSkills.Find(id);
            if (tblSkill == null)
            {
                return HttpNotFound();
            }
            return View(tblSkill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "skId,skName,skExperience,skPrevProjects,skCurrentProject,skDomain,skUsername")] tblSkill tblSkill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblSkill).State = EntityState.Modified;
                db.SaveChanges();
                string un = loggedinUser;
                return RedirectToAction("Dashboard_Employee",new { uname = loggedinUser});
            }
            return View(tblSkill);
        }

        // GET: Home2/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSkill tblSkill = db.tblSkills.Find(id);
            if (tblSkill == null)
            {
                return HttpNotFound();
            }
            return View(tblSkill);
        }

        // POST: Home2/Delete/5askdna
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSkill tblSkill = db.tblSkills.Find(id);
            db.tblSkills.Remove(tblSkill);
            db.SaveChanges();
            Response.Write("<script>alert(" + loggedinUser + ")</script>");
            string un = loggedinUser;
            return RedirectToAction("Dashboard_Employee",new { uname = loggedinUser });
        }

    }
}
