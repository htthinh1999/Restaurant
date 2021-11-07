using RestaurantManagement.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantManagement.Services
{
    public interface IFoodService
    {
        Task<List<FoodViewModel>> GetAllFoodAsync(string[] listCategory);
        Task<FoodViewModel> GetFoodByIdAsync(int id);
        Task InsertFoodAsync(ClaimsPrincipal user, FoodViewModel food);
    }
}
