using FiorelloRepeat.Controllers.DAL;
using FiorelloRepeat.Models;
using FiorelloRepeat.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
            HttpContext.Session.SetString("name", "Nahida");
            Response.Cookies.Append("surname", "Mamishova", new CookieOptions {MaxAge=TimeSpan.FromMinutes(25)}); 
            HomeVM homeVM = new HomeVM
            {
                //Products = _db.Products.Include(p=>p.Category).Where(p=>p.IsDeleted==false).ToList(),
                Categories = _db.Categories.Where(p => p.IsDeleted == false).ToList() 

            };

            return View(homeVM); 
        }


        public async Task<IActionResult> AddBasket(int id)
        {
            Product product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound(); 
         
            List<BasketVM> basket;
            if (Request.Cookies["basket"]!=null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            else
            {
                basket= new List<BasketVM>();
            }
            BasketVM isExist = basket.FirstOrDefault(p => p.Id == id);

            if (isExist == null)
            {
                basket.Add(new BasketVM
                {
                    Id = id,
                    Count = 1
                });
            }
            else
            {
                isExist.Count += 1; 
            } 
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
            return RedirectToAction(nameof(Index)); 
        }

        public async Task <IActionResult> Basket() 
        {
            List<BasketVM> dbBasket = new List<BasketVM>(); 
            //string session = HttpContext.Session.GetString("name");
            //string cookie = Request.Cookies["surname"];
            //return Content(session + " " + cookie); 
            List<BasketVM> basket = new List<BasketVM>();
            ViewBag.Total = 0;
            if (Request.Cookies["basket"]!=null)
            {
                List<BasketVM> secondbasket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
                foreach (BasketVM products in secondbasket)
                {
                    Product dbproduct = await _db.Products.FindAsync(products.Id);
                    products.Title = dbproduct.Title;
                    products.Price = dbproduct.Price* products.Count;
                    products.Image = dbproduct.Image; 
                    dbBasket.Add(products);
                    ViewBag.Total += products.Price; 
                }
            }
         
            return View(dbBasket);  
        }

        public IActionResult RemoveItem(int id) 
        {
            List<BasketVM> basket = new List<BasketVM>();
           
                basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
                BasketVM remove = basket.FirstOrDefault(p => p.Id == id);
                basket.Remove(remove);
                Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
            
              return RedirectToAction(nameof(Basket)); 
        }
    }
}
