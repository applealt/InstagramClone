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

        // GET: Users/SearchProfile
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

        // GET: Users/SearchProfile2
        public async Task<string> SearchProfile2(string searchText)
        {
            if (searchText == null)
            {
                return NotFound().ToString();
            }

            //var user = await _context.Users
            //    .SingleOrDefaultAsync(m => m.Id == id);
            //if (user == null)
            //{
            //    return NotFound().ToString();
            //}

            //return JsonConvert.SerializeObject(user);
            return "";
        }

        // GET: Users/GetUserProfile
        public string GetUserProfile()
        {
            int id = 1;

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

        // GET: Users/GetTopProfiles
        public string GetTopProfiles()
        {
            List<Users> users = _context.Users.Take(3).ToList<Users>();
            if (users == null)
            {
                return NotFound().ToString();
            }

            return JsonConvert.SerializeObject(users);
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
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



        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DisplayName,UserName,Email,Password,ImageProfile,FirstName,LastName,PostsCount,FollowersCount,FollowingCount,CreatedDate")] Users users)
        {
            if (ModelState.IsValid)
            {
                _context.Add(users);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DisplayName,UserName,Email,Password,ImageProfile,FirstName,LastName,PostsCount,FollowersCount,FollowingCount,CreatedDate")] Users users)
        {
            if (id != users.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var users = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            _context.Users.Remove(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }


        public bool Login(string email, string password)
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
    }
}
