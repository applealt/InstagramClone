using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InstagramClone.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace InstagramClone.Controllers
{
    public class UsersController : Controller
    {
        private readonly InstagramDBContext _context;
        private string SessionKey = session.SessionKey;

        public UsersController(InstagramDBContext context)
        {
            _context = context;
        }

        // Users/SearchProfile
        public async Task<string> SearchProfile(int id)
        {
            if (id == null)
            {
                return NotFound().ToString();
            }

            var user = await _context.Users
                .SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound().ToString();
            }

            return JsonConvert.SerializeObject(user);
        }

        // Users/SearchProfile2
        [HttpPost]
        public string  SearchProfile2(string user)
        {

            var result = _context.Users
                .SingleOrDefault(m => m.DisplayName == user || m.UserName == user || m.Email == user);
            return JsonConvert.SerializeObject(result);
        }

        // Users/GetUserProfile
        public string GetUserProfile()
        {
            int id = Convert.ToInt32(JsonConvert.DeserializeObject(GetLoggedInUserId()));

            if (id == null)
            {
                return NotFound().ToString();
            }

            var user = _context.Users
                .SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound().ToString();
            }

            return JsonConvert.SerializeObject(user);
        }

        // Users/GetTopProfiles
        public string GetTopProfiles()
        {
            List<Users> users = _context.Users.Take(3).ToList<Users>();
            if (users == null)
            {
                return NotFound().ToString();
            }

            return JsonConvert.SerializeObject(users);
        }

        // Users/GetLoggedInUserId
        public string GetLoggedInUserId()
        {
            string email = HttpContext.Session.GetString(SessionKey);
            var user = _context.Users
                .SingleOrDefault(m => m.Email == email);
            if (user == null)
            {
                return NotFound().ToString();
            }

            return JsonConvert.SerializeObject(user.Id);
        }

        // Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .SingleOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(JsonConvert.SerializeObject(users));
        }

        // Users/Login
        [HttpPost]
        public async Task<bool> Login(string email, string password)
        {
            var users = _context.Users
                  .Where(m => m.Email == email)
                  .ToList<Users>();

            if (users.Count == 1)
            {
                bool checkPassword = common.CheckPass(users[0].Password, password);
                if (checkPassword)
                {
                    // Save session
                    HttpContext.Session.SetString(SessionKey, email);
                    // create claims
                    List<Claim> claims = new List<Claim>

                    {
                        new Claim(ClaimTypes.Name, "Sean Connery"),
                        new Claim(ClaimTypes.Email, email)
                    };

                    // create identity
                    ClaimsIdentity identity = new ClaimsIdentity(claims, "cookie");

                    // create principal
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    // sign-in
                    try
                    {
                        await HttpContext.SignInAsync(
                                scheme: "FiverSecurityScheme",
                                principal: principal);
                      
                    }catch (Exception ex)
                    {
                        string error = ex.Message;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // Users/Register
        public bool Register(string firstName, string lastName, string userName, string email, string password)
        {
            string displayName = firstName + lastName;

            var users = _context.Users
                 .Where(m => m.Email == email)
                 .ToList<Users>();

            if (users.Count == 0)
            {
                var newUser = new Users();
                
                newUser.DisplayName = displayName;
                newUser.FirstName = firstName;
                newUser.LastName = lastName;
                newUser.UserName = userName;
                newUser.Email = email;
                newUser.Password = common.HashPassword(password);
                newUser.ImageProfile = "instagram.jpg";
              
                _context.Add(newUser);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        // Users/ForgetPassword
        public bool ForgetPassword(string email, string newPassword)
        {
            // check account whether it is existed 
            var users = _context.Users
                 .Where(m => m.Email == email)
                 .ToList<Users>();

            if(users.Count != 0)
            {
                // insert records into PasswordRecoveries
                var newPasswordRecord = new PasswordRecoveries();
                newPasswordRecord.Password = common.HashPassword(newPassword);
                newPasswordRecord.User = users[0].Id;
                newPasswordRecord.VerifyId = "default";
                newPasswordRecord.ExpirationDate = DateTime.Now.AddDays(1);
                _context.Add(newPasswordRecord);
                _context.SaveChanges();

                // select verification
                var verificationRecord = _context.PasswordRecoveries
               .Where(m => m.User == users[0].Id)
               .ToList<PasswordRecoveries>();

                // Update verify ID
                verificationRecord[0].VerifyId = verificationRecord[0].Id + common.HashPassword(newPassword);
                _context.Update(verificationRecord[0]);
                _context.SaveChanges();

                /*
                 * send email
                 * references sources: stackoverflow.com/questions/32260/sending-email-in-net-through-gmail  
                 */
                string SendersAddress = "recievewebdesign@gmail.com";            
                string ReceiversAddress = "hieutrantvvn2006@gmail.com";              
                const string SendersPassword = "recievewebdesign123@";    
                string subject = "Password Recovery";
                string host = HttpContext.Request.Host.ToString();
                string body = "Dear " + users[0].FirstName + " " + users[0].LastName + "," + "\n \n";
                body = body + "We have received a request to reset the password for this email. \n";
                body = body + "Please click on the link below to confirm your new password. \n \n";
                body = body + "http://" + host + "/Users/VerifyResetPassword?id=" + verificationRecord[0].VerifyId;
                body = body + "\n \n";
                body = body + "Thank You! \n";
                body = body + "Instagram Customer Service";
                try
                {
                    SmtpClient smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Credentials = new NetworkCredential(SendersAddress, SendersPassword)                     
                    };
                    MailMessage message = new MailMessage(SendersAddress, ReceiversAddress, subject, body);
                    smtp.Send(message);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        // Users/VerifyResetPassword
        public void VerifyResetPassword(string id)
        {
            // get verifycation record by id
            var verifications = _context.PasswordRecoveries
                 .Where(m => m.VerifyId == id)
                 .ToList<PasswordRecoveries>();
            // Check record existed
            if (verifications.Count > 0)
            {
                // Check expired date
                if (verifications[0].ExpirationDate >= DateTime.Now)
                {
                    // check account
                    var users = _context.Users
                         .Where(m => m.Id == verifications[0].User)
                         .ToList<Users>();

                    if (users.Count > 0)
                    {
                        users[0].Password = verifications[0].Password;
                        // Update password of User
                        _context.Update(users[0]);
                        // delete verification record 
                        _context.Remove(verifications[0]);
                        // commit change
                        _context.SaveChanges();
                    }
                    Response.Redirect("../Home/LoginAndRegistration");
                }
                else
                {
                    Response.Redirect("../Home/VerificationError?e=passwordExpired");
                }
            }
            else
            {
                Response.Redirect("../Home/VerificationError?e=invalidVerification");
            }
        }

        // Users/Logout
        public bool Logout()
        {
            HttpContext.Session.SetString(SessionKey, "");
            return true;
        }
    }
}
