using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using products_categories.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace products_categories.Controllers
{
    public class HomeController : Controller
    {
        // NOTE DB function
        private products_categoriesContext db;
        public HomeController(products_categoriesContext context)
        {
            db = context;
        }



        // NOTE routes
        [HttpGet("")]
        public IActionResult Index()
        {
            return RedirectToAction("Products");
        }


        [HttpGet("/products")]
        public IActionResult Products()
        {
            List<Product> AllProducts = db.Products.OrderByDescending (d => d.CreatedAt).ToList();
            return View("Products", AllProducts);
        }

        [HttpPost("/products/create")]
        public IActionResult CreateProduct(Product createProduct)
        {
            if (createProduct is null)
            {
                throw new ArgumentNullException(nameof(createProduct));
            }

            if (ModelState.IsValid)
            {
                db.Products.Add(createProduct);
                db.SaveChanges();
                return RedirectToAction("Products");
            }
            return View("Products");
        }


        [HttpGet("/categories")]
        public IActionResult Categories()
        {
            List<Category> AllCategories = db.Categories.OrderByDescending (d => d.CreatedAt).ToList();
            return View("Categories", AllCategories);
        }

        [HttpPost("/categories/create")]
        public IActionResult CreateCategory(Category createCategory)
        {
            if (createCategory is null)
            {
                throw new ArgumentNullException(nameof(createCategory));
            }

            if (ModelState.IsValid)
            {
                db.Categories.Add(createCategory);
                db.SaveChanges();
                return RedirectToAction("Categories");
            }
            List<Category> AllCategories = db.Categories.OrderByDescending (d => d.CreatedAt).ToList();
            return View("Categories", AllCategories);
        }


        // many to many categories 
        [HttpGet("categories/{categId}")]
        public IActionResult GetCategory(int categId)
        {
            ViewBag.getProducts = db.Products.Include(p => p.CategoryProducts)
                .ThenInclude(pc => pc.category)
                .Where(p => p.CategoryProducts.Any(cp => cp.CategoryId == categId)== false)
                .ToList();
            ViewBag.categId = categId;

            Category pageCategory = db.Categories
                .Where(c => c.CategoryId == categId)
                .Include(pc => pc.CategoryProducts)
                .ThenInclude(prod => prod.product)
                .FirstOrDefault();

            return View("Category", pageCategory);
        }

        [HttpPost("AddProdToCat")]
        public IActionResult AddProdToCat(int CategoryId, int ProductId)
        {
            Console.WriteLine(CategoryId);
            Console.WriteLine(ProductId);
            ProdCatMany prod = new ProdCatMany();
            prod.CategoryId = CategoryId;
            prod.ProductId = ProductId;

            db.Add(prod);
            db.SaveChanges();
            return Redirect("Categories/" + CategoryId);
        }






        // many to many products 
        [HttpGet("products/{prodId}")]
        public IActionResult GetProduct(int prodId)
        {
            ViewBag.getCategories = db.Categories.Include(p => p.CategoryProducts)
                .ThenInclude(cp => cp.product)
                .Where(c => c.CategoryProducts.Any(pc => pc.ProductId == prodId)== false)
                .ToList();
            ViewBag.prodId = prodId;

            Product pageProduct = db.Products
                .Where(p => p.ProductId == prodId)
                .Include(cp => cp.CategoryProducts)
                .ThenInclude(categ => categ.category)
                .FirstOrDefault();

            return View("Product", pageProduct);
        }

        [HttpPost("AddCatToProd")]
        public IActionResult AddCatToProd(int ProductId, int CategoryId)
        {
            Console.WriteLine(CategoryId);
            Console.WriteLine(ProductId);
            ProdCatMany categ = new ProdCatMany();
            categ.ProductId = ProductId;
            categ.CategoryId = CategoryId;

            db.Add(categ);
            db.SaveChanges();
            return Redirect("Products/" + ProductId);
        }
















        // NOTE pre installed things
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
