using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InstagramClone.Models;
using Newtonsoft.Json;

namespace InstagramClone.Controllers
{
    public class PostsController : Controller
    {
        private readonly InstagramDBContext _context;

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
