using FiorelloRepeat.Controllers.DAL;
using FiorelloRepeat.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloRepeat.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db; 
        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Products = _db.Products.Include(p=>p.Category).Where(p=>p.IsDeleted==false).ToList(),
                Categories = _db.Categories.Where(p => p.IsDeleted == false).ToList() 
            }; 
           
            return View(homeVM); 
        }
    }
}
