using Newsletter_AppMVC.Models;
using Newsletter_AppMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Newsletter_AppMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly string connectionString = @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = Newsletter; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(string firstName, string lastName, string emailAddress)
        {
            //Check for validation
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress))
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                using (NewsletterEntities db = new NewsletterEntities())
                {
                    var signup = new SignUp();
                    signup.FirstName = firstName;
                    signup.LastName = lastName;
                    signup.EmailAddress = emailAddress;

                    db.SignUps.Add(signup);
                    db.SaveChanges();

                }
                return View("Success");
            }
        }

        public ActionResult Admin()
        {
            //Best practice is to wrap Entity statements in using statements, so they close when done
            using (NewsletterEntities db = new NewsletterEntities())
            {
                var signups = db.SignUps;
                var signupVms = new List<SignUpVm>();
                foreach (var signup in signups)
                {
                    var signupVm = new SignUpVm();
                    signupVm.FirstName = signup.FirstName;
                    signupVm.LastName = signup.LastName;
                    signupVm.EmailAddress = signup.EmailAddress;
                    signupVms.Add(signupVm);
                }
                return View(signupVms);
            }
        }
    }
}