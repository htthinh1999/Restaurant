using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using System.Threading.Tasks;
using System.Linq;
using RestaurantManagement.Models;

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
            //var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

            //if (!result.Succeeded) { 
            
            //}
            //return result.Succeeded;

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

            //// method 3: linq
            var customer = await (from c in _context.Customer
                                 where c.UserName == username && c.PasswordHash == password
                                  select c).FirstOrDefaultAsync();

            if (customer == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> RegisterAsync(RegisterViewModel registerViewModel)
        {
            if (registerViewModel.Password != registerViewModel.RePassword) return false;

            var customer = await (from c in _context.Customer
                                  where c.UserName == registerViewModel.UserName
                                  select c).FirstOrDefaultAsync();
            if (customer != null)
            {
                return false;
            }

            _context.Customer.Add(new Customer()
            {
                Id = System.Guid.NewGuid(),
                UserName = registerViewModel.UserName,
                PasswordHash = registerViewModel.Password,
                PhoneNumber = registerViewModel.PhoneNumber,
                FullName = registerViewModel.FullName,
                Gender = registerViewModel.Gender,
                Birthday = registerViewModel.Birthday,
                VIP = false,
            });
            int result = _context.SaveChanges();
            if (result > 0) return true;
            return false;
       }
    }
}
