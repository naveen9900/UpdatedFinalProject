using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoginTestDemo.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace LoginTestDemo.Controllers
{
    public class Manager_DashboardController : Controller
    {
        LoginEntities entity = new LoginEntities();
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult All()
        {
            List<tblSkill> allskill = entity.tblSkills.ToList();
            return PartialView("Manager_view", allskill);
        }
        public PartialViewResult AllApps()
        {
            string chapter1 = "Apps";
            List<tblSkill> allskill1 = entity.tblSkills.Where(x => x.skDomain == chapter1).ToList();
            return PartialView("Manager_view", allskill1);
        }
        public PartialViewResult AllCloud()
        {
            string chapter2 = "Cloud";
            List<tblSkill> allskill2 = entity.tblSkills.Where(x => x.skDomain == chapter2).ToList();
            return PartialView("Manager_view", allskill2);
        }
        public PartialViewResult AllIoT()
        {
            string chapter3 = "IoT";
            List<tblSkill> allskill3 = entity.tblSkills.Where(x => x.skDomain == chapter3).ToList();
            return PartialView("Manager_view", allskill3);
        }
        public PartialViewResult AllSecurity()
        {
            string chapter4 = "Security";
            List<tblSkill> allskill4 = entity.tblSkills.Where(x => x.skDomain == chapter4).ToList();
            return PartialView("Manager_view", allskill4);
        }
        public PartialViewResult AllAnalytics()
        {
            string chapter5 = "Analytics";
            List<tblSkill> allskill5 = entity.tblSkills.Where(x => x.skDomain == chapter5).ToList();
            return PartialView("Manager_view", allskill5);
        }
        public ActionResult Approve(int id)
        {
            int ID = id;
            tblSkill idea = entity.tblSkills.FirstOrDefault(g => g.skId == id);
            idea.skStatus = "Approved";
            entity.Entry(idea).State = System.Data.Entity.EntityState.Modified;
            entity.SaveChanges();
            return RedirectToAction("ConfirmEmail", new { id = ID });
        }
        public ActionResult ConfirmEmail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ConfirmEmail(System.Web.Mvc.FormCollection form, int id)
        {
            string cnfemail = form["confirmemail"];
            tblSkill idea = entity.tblSkills.FirstOrDefault(g => g.skId == id);
            tblLogin user;
            tblSkill skill;
            user = entity.tblLogins.Where(x => x.eUsername == idea.skUsername).FirstOrDefault();
            skill = entity.tblSkills.Where(x => x.skId == id).FirstOrDefault();
            string email = user.eEmail;
            if (user != null)
            {
                string username = user.eUsername;
                entity.SaveChanges();
                if (!string.IsNullOrEmpty(email))
                {
                    MimeMessage mm = new MimeMessage();
                    MailboxAddress from = new MailboxAddress("ADMIN", "fundintest1997@gmail.com");
                    mm.From.Add(from);
                    MailboxAddress to = new MailboxAddress(username, email);
                    mm.To.Add(to);
                    mm.Subject = "Idea Approval";
                    var body = "Hi, <br/>" + user.eFirstName + "." + user.eLastName + "Your idea of " + skill.skCurrentProject + "of" + skill.skDomain + "<br/>" +
                        "<div style='background-color: white; display: block; font-family: sans-serif; text-align: center'> " +
                        "<div style='background-color:#1ED5A2; width: 80%; color: white; text-align: center; font-size: 2rem; padding: 1rem;'> " +
                        "<h2 style='color:black'>" + skill.skStatus + "</h2>Your idea has been Accepted, and Contact him: " + cnfemail + "</div><div></div>" +
                        "<br/> Thank you</div>";
                    BodyBuilder bodyBuilder = new BodyBuilder();
                    bodyBuilder.HtmlBody = string.Format(body, skill.skStatus, skill.skCurrentProject, user.eFirstName, user.eLastName, skill.skDomain);
                    bodyBuilder.TextBody = string.Format(body, skill.skStatus, skill.skCurrentProject, user.eFirstName, user.eLastName, skill.skDomain);
                    mm.Body = bodyBuilder.ToMessageBody();
                    using (var client = new SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 465, true);
                        client.Authenticate("fundintest1997@gmail.com", "dxcproject");
                        client.Send(mm);
                        client.Disconnect(true);
                        client.Dispose();
                    }

                }
            }
            return RedirectToAction("Index");
        }
    }
}
