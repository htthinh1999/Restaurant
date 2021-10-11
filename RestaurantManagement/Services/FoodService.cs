using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.Services
{
    public class FoodService : IFoodService
    {
        private readonly RestaurantDbContext _context;
        public FoodService(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<List<FoodViewModel>> GetAllFoodAsync()
        {
            var foods = await (from f in _context.Food
                               select new FoodViewModel
                               {
                                   Id = f.Id,
                                   Category = f.Category,
                                   Name = f.Name,
                                   UnitPrice = f.UnitPrice,
                                   ImageURL = f.ImageURL
                               }).ToListAsync();
            return foods;
        }
        public async Task<FoodViewModel> GetFoodByIdAsync(int id)
        {
            var food = await (from f in _context.Food
                              where f.Id == id
                              select new FoodViewModel
                              {
                                  Category = f.Category,
                                  Name = f.Name,
                                  UnitPrice = f.UnitPrice,
                                  ImageURL = f.ImageURL,
                                  Description = f.Description
                                  
                              }).FirstOrDefaultAsync();
            return food;
        }
    }
}
