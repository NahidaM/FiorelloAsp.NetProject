using FiorelloRepeat.Controllers.DAL;
using FiorelloRepeat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloRepeat.Areas.AdminFiorelloRepeat.Controllers
{ [Area("AdminFiorelloRepeat")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Categories.Where(c=>c.IsDeleted==false).ToList());
        }

        public IActionResult Create()
        {
            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return NotFound();

            bool isExist = _context.Categories.Where(c => c.IsDeleted == false)
                .Any(c=>c.Name.ToLower()== category.Name.ToLower());
            if (isExist)
            {
                ModelState.AddModelError("Name", "Category name movcuddur");
                return View(); 
            }

            await _context.Categories.AddAsync(category); 
            category.IsDeleted = false;
            await _context.SaveChangesAsync();  
            return RedirectToAction(nameof(Index)); 
        }

        public IActionResult Detail(int? id)  
        {
            if (id==null) return NotFound();
            Category category = _context.Categories.Where(c => c.IsDeleted == false).FirstOrDefault(c => c.Id == id);
            if (category==null) return NotFound();
            return View(category);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            Category category = _context.Categories.Where(c => c.IsDeleted == false).FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound(); 
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null) return NotFound();
            Category category =  _context.Categories.Where(c=>c.IsDeleted==false).Include(c=>c.Products).FirstOrDefault(c=>c.Id==id);
            if (category == null) return NotFound();
            //_context.Categories.Remove(category);
            //await _context.SaveChangesAsync(); 
            category.IsDeleted = true;
            category.DeletedTime = DateTime.Now;
            foreach (Product product in category.Products)
            {
                product.IsDeleted = true;
                product.DeletedTime = DateTime.Now; 
            }
            await _context.SaveChangesAsync(); 
            return RedirectToAction(nameof(Index));
        }
       
        public async Task<IActionResult> Update(int? id) 
        {
            if (id == null) return NotFound();
            Category category =  _context.Categories.Where(c => c.IsDeleted == false).FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Category category)
        {
            if (!ModelState.IsValid) return View();
            if (id == null) return NotFound();
            Category categorydb = await _context.Categories.FindAsync(id);
            if (categorydb==null) return NotFound();

            Category repeatCategory= _context.Categories.Where(c => c.IsDeleted == false).FirstOrDefault(c => c.Name.ToLower() == category.Name.ToLower());
           if (repeatCategory!=null)
            {
                ModelState.AddModelError("Name", "Artiq bu adda category movcuddur");
                return View();
            }
            categorydb.Name = category.Name;
            categorydb.Description = category.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }

}
