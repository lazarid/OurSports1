﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OurSports1.Data;
using OurSports1.Models;
using Microsoft.AspNetCore.Authorization;
 
namespace OurSports1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static int ArticleID;
        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index(String Year, String YearSelect, String CommentWriter, String CommentWriterInput, String Writer, String WriterSelect, string Category, String CategorySelect)
        {
            ViewData["Status"] = "true";

            ViewData["Result"] = "f";


            var yeras = new List<String>();
            for (int year = 2017; year <= DateTime.Now.Year; year++)
            {
                yeras.Add(year.ToString());
            }
            ViewData["Years"] = new SelectList(yeras);
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "Title");
            var Comments = _context.Comment.Include(a => a.Article).Select(a => a);



            if (Year == "true")
            {
                Comments = Comments.Where(a => a.Article.TimeCreate.Year.ToString() == YearSelect);
            }
            if (CommentWriter == "true")
            {
                if(CommentWriterInput!=null)
                Comments = Comments.Where(a => a.WriterName.ToString().Contains(CommentWriterInput));
            }
            if (Category == "true")
            {
                Comments = Comments.Where(a => a.Article.CategoryID.ToString() == CategorySelect);
            }
            if (Comments == _context.Comment.Include(a => a.Article).Select(a => a))
            {
                ViewData["Result"] = "empty";
            }


            if (Comments.Count() == 0)
            {
                ViewData["Status"] = "false";
                ViewData["Result"] = "There is no such Comment...";
                Comments = _context.Comment.Include(a => a.Article).Select(a => a);
            }
            else if (ViewData["Result"].ToString() != "empty")
            {
                ViewData["Result"] = "We Found " + Comments.Count() + " Comments For You!";
            }

            var webSportContext = Comments.OrderByDescending<Comment, DateTime>(a => a.Article.TimeCreate);

            return View(await webSportContext.ToListAsync());
        }
        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Article)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        [AllowAnonymous]
        // GET: Comments/Create
        public IActionResult Create(int? num)
        {
            if(num!=null)
            {
 ArticleID = (int)num;
            }
           
             ViewData["ArticleID"] = num;
            return View();
        }
        [AllowAnonymous]
        public IActionResult Thanks()
        {

            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,WriterName,Title,Content,ArticleID")] Comment comment)
        {
            comment.ArticleID = ArticleID;
            
            if (ModelState.IsValid)
            {
                
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Thanks));
            }

            return Create(ArticleID);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.SingleOrDefaultAsync(m => m.ID == id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["ArticleID"] = new SelectList(_context.Article, "ID", "ID", comment.ArticleID);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,WriterName,Title,Content,ArticleID")] Comment comment)
        {
            if (id != comment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.ID))
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
            ViewData["ArticleID"] = new SelectList(_context.Article, "ID", "ID", comment.ArticleID);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Article)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comment.SingleOrDefaultAsync(m => m.ID == id);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.ID == id);
        }
    }
}
