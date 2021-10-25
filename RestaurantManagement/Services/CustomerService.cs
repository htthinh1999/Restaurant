using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantManagement.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly SignInManager<Customer> _signInManager;
        private readonly UserManager<Customer> _userManager;
        private readonly RestaurantDbContext _context;

        public CustomerService(
            SignInManager<Customer> signInManager,
            UserManager<Customer> userManager,
            RestaurantDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            // Method 1: Use .NET Core
            var customer = await _userManager.FindByNameAsync(username);
            if(customer == null)
            {
                return false;
            }
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
            //                     where c.UserName == username && c.PasswordHash == password
            //                      select c).FirstOrDefaultAsync();

            //if (customer == null)
            //{
            //    return false;
            //}
            //return true;
        }

        public async Task<bool> RegisterAsync(RegisterViewModel registerViewModel)
        {
            if (registerViewModel.Password != registerViewModel.RePassword) return false;

            var customer = await _userManager.FindByNameAsync(registerViewModel.UserName);
            if (customer != null)
            {
                return false;
            }

            var newCustomer = new Customer()
            {
                Id = System.Guid.NewGuid(),
                UserName = registerViewModel.UserName,
                PhoneNumber = registerViewModel.PhoneNumber,
                FullName = registerViewModel.FullName,
                Gender = registerViewModel.Gender,
                Birthday = registerViewModel.Birthday,
                VIP = false,
            };

            var result = await _userManager.CreateAsync(newCustomer, registerViewModel.Password);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<List<TableHistoryViewModels>> GetTableHistoryAsync(ClaimsPrincipal user)
        {
            var customer = await _userManager.GetUserAsync(user);
            if (customer == null)
                return new List<TableHistoryViewModels>();
            var tableOrderHistory = await (from f in _context.OderTable
                                           join g in _context.Table on f.TableId equals g.Id
                                           where f.CustomerId == customer.Id
                                           select new TableHistoryViewModels
                                           {
                                               Id = f.Id,
                                               From = f.From,
                                               To = f.To,
                                               TableName = g.Name,
                                               PeopleCount = g.PeopleCount,
                                           }).ToListAsync();
            return tableOrderHistory;
        }
        public async Task<List<PaymentHistoryViewModel>> GetPaymentHistoryAsync(ClaimsPrincipal user)
        {
            var customer = await _userManager.GetUserAsync(user);
            if (customer == null)
                return new List<PaymentHistoryViewModel>();
            var paymentHistory = await (from b in _context.Bill
                                           where b.CustomerId == customer.Id && b.PaymentMethod != null && b.PaymentMethod !=string.Empty
                                           select new PaymentHistoryViewModel
                                           {
                                               Id = b.Id,
                                               Total = b.Total,
                                               PaymentMethod = b.PaymentMethod,
                                               CreatedDate = b.CreatedDate,
                                           }).ToListAsync();
            return paymentHistory;
        }
        public async Task<List<PaymentDetailViewModel>> GetPaymentDetailAsync(Guid billId)
        {
            var paymentDetail = await (from b in _context.BillDetail
                                        join g in _context.Food on b.FoodId equals g.Id
                                        where b.BillId == billId
                                        select new PaymentDetailViewModel
                                        {
                                            BillId = b.BillId,
                                            FoodName = g.Name,
                                            UnitPrice = b.UnitPrice,
                                            Quantity = b.Quantity,
                                            Price = b.Price
                                        }).ToListAsync();
            return paymentDetail;
        }
    }
}
