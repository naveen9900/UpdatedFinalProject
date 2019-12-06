using LoginTestDemo.Models;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginTestDemo.Controllers
{
    public class ResetPasswordController : Controller
    {
        // GET: ResetPassword
        LoginEntities entities = new LoginEntities();

        public void SendEmail(string email)
        {
            //string message = "This email address does not match our records.";
            tblLogin user;

            user = entities.tblLogins.Where(x => x.eEmail == email).FirstOrDefault();

            if (user != null)
            {
                string username = user.eEmail;
                string password = user.ePassword;

                // To send OTP to email 
                string strNewPassword = GeneratePassword().ToString();
                user.OTP = strNewPassword;
                entities.SaveChanges();
                if (!string.IsNullOrEmpty(password))
                {
                    MimeMessage mm = new MimeMessage();
                    MailboxAddress from = new MailboxAddress("ADMIN", "fundintest1997@gmail.com");
                    mm.From.Add(from);

                    MailboxAddress to = new MailboxAddress(username, email);
                    mm.To.Add(to);

                    mm.Subject = "OTP For ResetPassword";
                    var body = "Hi, <br/>You recently requested to reset your password for your account.<br/><div style='background-color: white; display: block; font-family: sans-serif; text-align: center'> " +
                        "<div style='background-color:#F7F6A2; width: 80%; color: white; text-align: center; font-size: 2rem; padding: 2rem;'> Your OTP for Resetting Password:<h2 style='color:black'>{0}</h2></div><div></div>" +
                        "<br/>If you did not request a password reset, please ignore this email or reply to let us know.<br/><br/> Thank you" +
                        "</div>";
                    BodyBuilder bodyBuilder = new BodyBuilder();
                    bodyBuilder.HtmlBody = string.Format(body, user.OTP);
                    bodyBuilder.TextBody = string.Format(body, user.OTP);
                    mm.Body = bodyBuilder.ToMessageBody();


                    using (var client = new SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 465, true);
                        client.Authenticate("fundintest1997@gmail.com", "dxcproject");
                        client.Send(mm);
                        client.Disconnect(true);
                        client.Dispose();
                        ViewBag.Message = string.Format("OTP Generated Successfully.");
                    }

                }
            }
        }
        public string GeneratePassword()
        {
            string PasswordLength = "6";
            string OTP = "";

            string allowedChars = "";
            allowedChars = "1,2,3,4,5,6,7,8,9,0";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            allowedChars += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";

            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string IDString = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i < Convert.ToInt32(PasswordLength); i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                IDString += temp;
                OTP = IDString;
            }
            return OTP;
        }

        public ActionResult ForgotPassword()
        {
            return View();

        }

        [HttpPost]
        public ActionResult ForgotPassword(System.Web.Mvc.FormCollection form)
        {
            if (ModelState.IsValid)
            {
                String enteredEmail = form["email"];
                

                var obj = entities.tblLogins.Where(x => x.eEmail == enteredEmail).FirstOrDefault();
                if (obj != null)
                {
                    SendEmail(form["email"]);
                    return RedirectToAction("ResetPassword");
                }
                else
                {
                    ModelState.AddModelError("", "Email Address not Registered");
                    return View("ForgotPassword");
                }
            }
            return View();
        }

        public ActionResult ResetPassword()
        {
            if (Session["Rem_Time"] == null)
            {
                Session["Rem_Time"] = DateTime.Now.AddMinutes(3).ToString("dd-MM-yyyy h:mm:ss tt");
            }
            ViewBag.Rem_Time = Session["Rem_Time"];
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(System.Web.Mvc.FormCollection form, ResetPasswordModel obj)
        {
            tblLogin details;
            
            string OTP = form["OTPInput"];
            details = entities.tblLogins.Where(x => x.OTP == OTP).FirstOrDefault();
            if (details != null)
            {
                details.ePassword = obj.NewPassword;
                details.eConfirmPassword = obj.NewPassword;
                entities.SaveChanges();
            }
            return RedirectToAction("Login","Home");
        }
    }
}

