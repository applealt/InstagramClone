using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using InstagramClone.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.IO;

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

        // GET: /Posts/GetPostsContent
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

        // GET: /Posts/SearchPost
        public string SearchPost(int id)
        {
            if (id == null)
            {
                return NotFound().ToString();
            }

            var post = _context.Posts
                .SingleOrDefault(m => m.Id == id);
            if (post == null)
            {
                return NotFound().ToString();
            }

            return JsonConvert.SerializeObject(post);
        }

        // GET: /Posts/UploadFile
        public void UploadFile(IFormFile file, int id, String status, string hashtag)
        {
            // Create file's path
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/instagram_img/", System.IO.Path.GetFileName(file.FileName));

            // Upload file
            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Add new post
            Posts post = new Posts();
            post.User = id;
            post.Image = System.IO.Path.GetFileName(file.FileName);
            post.Status = status;
            post.HashTag = hashtag;
            post.LikeCount = 0;
            post.FeedbackCount = 0;
            post.CreatedDate = DateTime.Now;

            _context.Add(post);
            _context.SaveChanges();

            //Update user
            var user = _context.Users
                .SingleOrDefault(m => m.Id == id);
            user.PostsCount++;
            _context.SaveChanges();
        }

        // GET: /Posts/IncreaseLike
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

        // GET: /Posts/IsLike
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

        // GET: /Posts/IncreaseFollower
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

        // GET: /Posts/IsFollow
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
    }
}
