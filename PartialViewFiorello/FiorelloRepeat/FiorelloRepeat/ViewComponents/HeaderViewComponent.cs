using FiorelloRepeat.Controllers.DAL;
using FiorelloRepeat.Models;
using FiorelloRepeat.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 

namespace FiorelloRepeat.ViewComponents
{
    public class HeaderViewComponent: ViewComponent 
    {
        private readonly AppDbContext _context; 
        public HeaderViewComponent(AppDbContext context)
        {
            _context = context;
        } 

        public async Task<IViewComponentResult> InvokeAsync() 
        {
            ViewBag.BasketCount = 0;
            if (Request.Cookies["basket"]!=null)
            {
                List<BasketVM> baskets = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
                //ViewBag.BasketCount = baskets.Count(); 
                ViewBag.BasketCount = baskets.Sum(p => p.Count); 
            }
            Bio model = _context.Bios.FirstOrDefault(); 
            return View(await Task.FromResult(model)); 
        } 
    }
}
