using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using System.Threading.Tasks;
using System.Linq;

namespace RestaurantManagement.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly SignInManager<Customer> _signInManager;
        private readonly RestaurantDbContext _context;

        public CustomerService(SignInManager<Customer> signInManager,
                               RestaurantDbContext context)
        {
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            // Method 1: Use .NET Core
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);
            return result.Succeeded;

            //// Method 2: SQL Script
            //string sql = @$"SELECT * FROM CUSTOMER
            //                WHERE username = '{username}'
            //                AND password = '{password}'";
            //var customer = await _context.Customer.FromSqlRaw(sql).FirstOrDefaultAsync();
            //if(customer == null)
            //{
            //    return false;
            //}
            //return true;

            //// Method 3: LINQ
            //var customer = await (from c in _context.Customer
            //                      where c.UserName == username && c.PasswordHash == password
            //                      select c).FirstOrDefaultAsync();
            //if (customer == null)
            //{
            //    return false;
            //}
            //return true;
        }
    }
}
