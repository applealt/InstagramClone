using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InstagramClone.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace InstagramClone.Controllers
{
    public class UsersController : Controller
    {
        private readonly InstagramDBContext _context;
        string SessionKeyName = "sessionEmail";

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
            var user = _context.Users
                  .Where(m => m.Email == email)
                  .ToList<Users>();

            if (user.Count == 1)
            {
                if (user[0].Password == password)
                {
                    // Save session
                    HttpContext.Session.SetString(SessionKeyName, email);
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
                newUser.Password = password;
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

    }
}
