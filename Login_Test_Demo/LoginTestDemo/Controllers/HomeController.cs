using LoginTestDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LoginTestDemo.Controllers
{
    public class HomeController : Controller
    {
        LoginEntities entity = new LoginEntities();

        public ActionResult HomePage()
        {
            return View();
        }
        // GET: Home
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(tblLogin log)
        {
                  var obj = entity.tblLogins.Where(a => a.eUsername.Equals(log.eUsername) && a.ePassword.Equals(log.ePassword)).FirstOrDefault();
                if (obj != null)
                {
                    Session["eLoginId"] = obj.eLoginId.ToString();
                    Session["eUsername"] = obj.eUsername.ToString();
                    Session["eFirstName"] = obj.eFirstName.ToString();
                ViewBag.Name = Session["eFirstName"];
                    FormsAuthentication.SetAuthCookie(log.eUsername, false);
                    if (obj.Role == "Manager")
                    {
                    //return RedirectToAction("Dashboard_Manager", data);
                    return RedirectToAction("Index","Manager_Dashboard");
                }
                    else if (obj.Role == "Employee")
                    {
                        return RedirectToAction("Dashboard_Employee","Home2",new { @uname = obj.eUsername });
                    }

                }
                ModelState.AddModelError("", "Invalid Login Details!");
                return View("Login");
            
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(tblLogin data)
        {
            if (ModelState.IsValid)
            {
                tblLogin obj = entity.tblLogins.Where(x => x.eEmail == data.eEmail).FirstOrDefault();
                if(obj == null)
                {
                    entity.tblLogins.Add(data);
                    entity.SaveChanges();
                    
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Email ID already registered.");
                    return View("Register");
                }
                

            }
            return View(data);
        }

        public ActionResult User_Data_Entry()
        {

            return View();
        }

        [HttpPost]
        public ActionResult User_Data_Entry(tblSkill data)
        {
            if(ModelState.IsValid)
            {

                if (Session["eLoginId"] != null)
                {

                    entity.tblSkills.Add(data);
                    data.skUsername = Session["eUsername"].ToString();
                    data.skStatus = "Pending";
                    entity.SaveChanges();

                    return RedirectToAction("Dashboard_Employee", "Home2", new { @uname = data.skUsername });
                }

            }            
            return View();
        }


        //public ActionResult Dashboard_Employee(tblSkill data)
        //{
        //    Session["skusername"] = Session["eUsername"];
        //    ViewBag.Name = Session["eFirstName"].ToString();
            
        //    List<tblSkill> userdata = entity.tblSkills.Where(x => x.skUsername == data.skUsername).ToList();
        //    return View(userdata);
        //}

        public ActionResult Dashboard_Manager()
        {
            ViewBag.Display = "Manager Dashboard";
            return View();
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}