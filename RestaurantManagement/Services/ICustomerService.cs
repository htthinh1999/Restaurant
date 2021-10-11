using RestaurantManagement.Models;
using System.Threading.Tasks;

namespace RestaurantManagement.Services
{
    public interface ICustomerService
    {
        Task<bool> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(RegisterViewModel registerViewModel);
    }
}
