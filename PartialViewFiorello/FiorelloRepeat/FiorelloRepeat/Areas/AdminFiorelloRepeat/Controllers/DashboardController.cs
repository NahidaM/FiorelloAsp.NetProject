using FiorelloRepeat.Controllers.DAL;
using FiorelloRepeat.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloRepeat.Areas.AdminFiorelloRepeat.Controllers
{
    [Area("AdminFiorelloRepeat")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index() 
        {
            List<Category> cat = _context.Categories.ToList();
            return View(cat);
        }
    }
}
