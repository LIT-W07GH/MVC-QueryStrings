using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using WebApplication13.Models;

namespace WebApplication13.Controllers
{
    public class NorthwindController : Controller
    {
        private string _connectionString =
            @"Data Source=.\sqlexpress;Initial Catalog=Northwnd;Integrated Security=True;";
        public IActionResult Orders()
        {
            NorthwindDb db = new NorthwindDb(_connectionString);
            OrdersViewModel vm = new OrdersViewModel();
            vm.Orders = db.GetOrders();
            vm.CurrentDate = DateTime.Now;
            return View(vm);
        }

        public IActionResult OrderDetails(int year, string country)
        {
            NorthwindDb db = new NorthwindDb(_connectionString);
            List<Order> orders = db.GetOrdersForYear(year, country);
            return View(orders);
        }

        public IActionResult Categories()
        {
            NorthwindDb db = new NorthwindDb(_connectionString);
            return View(db.GetCategories());
        }

        public IActionResult Products(int catId)
        {
            NorthwindDb db = new NorthwindDb(_connectionString);
            ProductsViewModel vm = new ProductsViewModel();
            vm.Products = db.GetProductsForCategory(catId);
            vm.CategoryName = db.GetCategoryName(catId);
            return View(vm);
        }

        public IActionResult SearchProducts()
        {
            return View();
        }

        public IActionResult ShowSearchResults(string searchText)
        {
            NorthwindDb db = new NorthwindDb(_connectionString);
            SearchResultsViewModel vm = new SearchResultsViewModel();
            vm.Products = db.Search(searchText);
            vm.SearchText = searchText;
            return View(vm);
        }
    }
}

//Create an application that has two pages:
// /northwind/categories
// /northwind/products

//On the categories page, display a list of all categories in the northwind database
//(id, name, description). The name of the category should be a link, that when clicked
//takes the user to the products page. On the products page, only the products
//for the category that was clicked on should be displayed. Additionally, on top of
//the products page, have an H1 that says "Products for Category {CategoryName}"
