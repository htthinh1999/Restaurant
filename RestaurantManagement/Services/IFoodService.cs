using RestaurantManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagement.Services
{
    public interface IFoodService
    {
        Task<List<FoodViewModel>> GetAllFoodAsync();
        Task<FoodViewModel> GetFoodByIdAsync(int id);
    }
}
