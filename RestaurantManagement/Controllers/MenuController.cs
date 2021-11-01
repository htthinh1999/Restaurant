using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Models;
using RestaurantManagement.Services;

namespace RestaurantManagement.Controllers
{
    public class MenuController : Controller
    {
        private readonly IFoodService _foodService;
        public MenuController(IFoodService foodService)
        {
            _foodService = foodService;
        }
        // GET: MenuController
        public async Task<IActionResult> MenuFood()
        {
            var foods = await _foodService.GetAllFoodAsync();
            return View(foods);
        }

        // GET: MenuController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var food = await _foodService.GetFoodByIdAsync(id);
            return View(food);
        }
        [HttpPost]
        public IActionResult InsertFoodToCart(FoodViewModel food)
        {
            _foodService.InsertFoodAsync(User, food);
            return RedirectToAction("MenuFood");
        }
    }
}
