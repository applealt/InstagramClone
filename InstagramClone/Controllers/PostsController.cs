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
    public class PostsController : Controller
    {
        private readonly InstagramDBContext _context;
        private string SessionKey = session.SessionKey;

        public PostsController(InstagramDBContext context)
        {
            _context = context;
        }

        // GET: Posts/GetPostsContent
        public string GetPostsContent(int id, int max)
        {
            if (id == null || max == null)
            {
                return NotFound().ToString();
            }

            List<Posts> posts = _context.Posts
               .Where(m => m.User == id)
               .Take(max).ToList<Posts>();

            if (posts == null)
            {
                return NotFound().ToString();
            }

            return JsonConvert.SerializeObject(posts);
        }

        public string IncreaseLike(int postID)
        {
            string userEmail = HttpContext.Session.GetString(SessionKey);

            var users = _context.Users
               .Where(m => m.Email == userEmail)
               .ToList<Users>();

            int userID = users[0].Id;
            // Check Like exists
            var likes = _context.Likes
               .Where(m => m.User == userID && m.Post == postID)
               .ToList<Likes>();

            // Increase likes
            if(likes.Count == 0)
            {
                // Update post table by increase by 1
                var posts = _context.Posts
                .Where(m => m.Id == postID)
                .ToList<Posts>();

                int countLike = posts[0].LikeCount + 1;
                posts[0].LikeCount = countLike;
                _context.Update(posts[0]);

                // Insert a record into Like table
                var like = new Likes();
                like.User = userID;
                like.Post = postID;
                _context.Add(like);
                _context.SaveChanges();

                return JsonConvert.SerializeObject(new {result = "true", count = countLike, id = postID });
            }
            // Decrease likes
            else
            {
                // Update post table by decrease by 1
                var posts = _context.Posts
                .Where(m => m.Id == postID)
                .ToList<Posts>();

                int countLike = posts[0].LikeCount - 1;
                posts[0].LikeCount = countLike;
                _context.Update(posts[0]);

                // delete like record
                _context.Remove(likes[0]);
                _context.SaveChanges();
                return JsonConvert.SerializeObject(new { result = "false", count = countLike, id = postID });
            }
        }

        public bool IsLike(int PostID)
        {
            string userEmail = HttpContext.Session.GetString(SessionKey);

            var users = _context.Users
               .Where(m => m.Email == userEmail)
               .ToList<Users>();

            int userID = users[0].Id;

            var likes = _context.Likes
                .Where(m => m.User == userID && m.Post == PostID)
                .ToList<Likes>();

            if (likes.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string IncreaseFollower(int followerID)
        {
            // Check Follow exists
            string userEmail = HttpContext.Session.GetString(SessionKey);

            var users = _context.Users
               .Where(m => m.Email == userEmail)
               .ToList<Users>();

            int userID = users[0].Id;

            var follows = _context.Follows
                .Where(m => m.FollowingUser == userID && m.FollowedUser == followerID)
                .ToList<Follows>();
            
            // Increase Follower and Following 
            if (follows.Count == 0)
            {
                // Update following
                int followingCount = (int)users[0].FollowingCount + 1 ;
                users[0].FollowingCount = followingCount;
                _context.Update(users[0]);

                //Update follower
               var followedUsers = _context.Users
               .Where(m => m.Id == followerID)
               .ToList<Users>();

                followedUsers[0].FollowersCount = followedUsers[0].FollowersCount + 1;
                _context.Update(followedUsers[0]);

                //Insert record into Follows table
                var followRecord = new Follows();
                followRecord.FollowingUser = userID;
                followRecord.FollowedUser = followerID;
                _context.Add(followRecord);

                // commit changes
                _context.SaveChanges();
               // array('result' => true, 'id' => $FollowID, 'followingCount' => $CurrentFollowingCount);
                return JsonConvert.SerializeObject(new { result = "true", FollowingCount = followingCount });
                
                }
            // Decrease Follower and Following 
            else
            {
                // Update following
                int followingCount = (int)users[0].FollowingCount - 1;
                users[0].FollowingCount = followingCount;
                _context.Update(users[0]);

                //Update follower
                var followedUsers = _context.Users
                .Where(m => m.Id == followerID)
                .ToList<Users>();

                followedUsers[0].FollowersCount = followedUsers[0].FollowersCount - 1;
                _context.Update(followedUsers[0]);

                //Insert record into Follows table
                _context.Remove(follows[0]);

                // commit changes
                _context.SaveChanges();

                return JsonConvert.SerializeObject(new { result = "false", FollowingCount = followingCount });
            }
        }

        public bool IsFollow(int followid)
        {
            string userEmail = HttpContext.Session.GetString(SessionKey);

            var users = _context.Users
               .Where(m => m.Email == userEmail)
               .ToList<Users>();

            int userID = users[0].Id;

            var follow = _context.Follows
                .Where(m => m.FollowedUser == followid && m.FollowingUser == userID)
                .ToList<Follows>();

            if (follow.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //// GET: Posts
        //public async Task<IActionResult> Index()
        //{
        //    var instagramDBContext = _context.Posts.Include(p => p.UserNavigation);
        //    return View(await instagramDBContext.ToListAsync());
        //}




        //// GET: Posts/Create
        //public IActionResult Create()
        //{
        //    ViewData["User"] = new SelectList(_context.Users, "Id", "DisplayName");
        //    return View();
        //}

        //// POST: Posts/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,User,Image,Status,HashTag,LikeCount,FeedbackCount,CreatedDate")] Posts posts)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(posts);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["User"] = new SelectList(_context.Users, "Id", "DisplayName", posts.User);
        //    return View(posts);
        //}

        //// GET: Posts/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var posts = await _context.Posts.SingleOrDefaultAsync(m => m.Id == id);
        //    if (posts == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["User"] = new SelectList(_context.Users, "Id", "DisplayName", posts.User);
        //    return View(posts);
        //}

        //// POST: Posts/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,User,Image,Status,HashTag,LikeCount,FeedbackCount,CreatedDate")] Posts posts)
        //{
        //    if (id != posts.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(posts);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PostsExists(posts.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["User"] = new SelectList(_context.Users, "Id", "DisplayName", posts.User);
        //    return View(posts);
        //}

        //// GET: Posts/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var posts = await _context.Posts
        //        .Include(p => p.UserNavigation)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (posts == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(posts);
        //}

        //// POST: Posts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var posts = await _context.Posts.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.Posts.Remove(posts);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool PostsExists(int id)
        //{
        //    return _context.Posts.Any(e => e.Id == id);
        //}
    }
}
