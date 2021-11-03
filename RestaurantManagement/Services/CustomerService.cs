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

        public async Task<PaymentViewModel> GetBillToPayAsync(ClaimsPrincipal user)
        {
            var customer = await _userManager.GetUserAsync(user);
            var foodPayment = await (from b in _context.Bill
                                     where b.CustomerId == customer.Id && b.PaymentMethod == string.Empty
                                     select b).FirstOrDefaultAsync();

            if (foodPayment == null)
            {
                return null;
            }

            var billToPay = await (from bd in _context.BillDetail
                                 join f in _context.Food on bd.FoodId equals f.Id
                                 where bd.BillId == foodPayment.Id
                                 select new BillViewModel
                                 {
                                    FoodName = f.Name,
                                    UnitPrice = bd.UnitPrice,
                                    Quantity = bd.Quantity,
                                    Price = bd.Price
                                 }).ToListAsync();

            var payment = new PaymentViewModel
                            {
                                CreatedDate = foodPayment.CreatedDate,
                                BillId = foodPayment.Id,
                                BillToPay = billToPay,
                                Total = foodPayment.Total
                            };
            return payment;
        }

        public async Task UpdatePaymentMethodAsync(ClaimsPrincipal user, PaymentViewModel payment)
        {
            var customer = await _userManager.GetUserAsync(user);
            var billPayment = await (from b in _context.Bill
                                     where b.CustomerId == customer.Id
                                     select b).ToListAsync();

            // Update VIP
            if (billPayment.Count() > 10)
                customer.VIP = true;

            // Update payment method and total
            var update = (from u in billPayment
                          where u.PaymentMethod == string.Empty
                          select u).FirstOrDefault();
            update.PaymentMethod = payment.PaymentMethod;

            _context.SaveChanges();
        }

        public async Task<CartViewModel> ShowToCartAsync(ClaimsPrincipal user)
        {
            var customer = await _userManager.GetUserAsync(user);
            if (customer == null)
                return new CartViewModel();
            var cart = await (from f in _context.Bill
                              where f.CustomerId == customer.Id && f.PaymentMethod == string.Empty
                              select new CartViewModel
                              {
                                  Total=f.Total,
                                  ListFood = (from g in _context.BillDetail
                                              join h in _context.Food on g.FoodId equals h.Id
                                              where g.BillId == f.Id
                                              select new CartDetailViewModel
                                              {
                                              Id=f.Id,
                                              FoodId = g.FoodId,
                                              Name = h.Name,
                                              UnitPrice = g.UnitPrice,
                                              Quantity = g.Quantity,
                                              Price = g.Price,
                                              ImageURL = h.ImageURL,
                                              Type=""
                                              }).ToList(),
                                 }).FirstOrDefaultAsync();
            if (cart == null)
                return new CartViewModel();
            return cart;
        }
        public async Task<CartViewModel> ShowToCartAsync(ClaimsPrincipal user,CartDetailViewModel cartdetailvm)
        {
            var customer = await _userManager.GetUserAsync(user);
            if (customer == null)
                return new CartViewModel();
            if(cartdetailvm.Quantity==0)
            {
                var CartDetail = (from f in _context.BillDetail
                                  where f.BillId == cartdetailvm.Id && f.FoodId == cartdetailvm.FoodId
                                  select f).FirstOrDefault();
                var Cart = (from f in _context.Bill
                            where f.Id == cartdetailvm.Id
                            select f).FirstOrDefault();
                Cart.Total -= cartdetailvm.UnitPrice;
                _context.Remove(CartDetail);
            }
            else
            {
                var CartDt= (from f in _context.BillDetail
                             where f.BillId == cartdetailvm.Id && f.FoodId == cartdetailvm.FoodId
                             select f).FirstOrDefault();
                var Cart = (from f in _context.Bill
                            where f.Id == cartdetailvm.Id
                            select f).FirstOrDefault();
                if (cartdetailvm.Type=="-")
                {
                    CartDt.Quantity--;
                    CartDt.Price -= cartdetailvm.UnitPrice;
                    Cart.Total -= cartdetailvm.UnitPrice;
                }
                if(cartdetailvm.Type=="+")
                {
                    CartDt.Quantity++;
                    CartDt.Price += cartdetailvm.UnitPrice;
                    Cart.Total += cartdetailvm.UnitPrice;
                }    
            }
            _context.SaveChanges();
            var cartafterupdate = await (from f in _context.Bill
                              where f.CustomerId == customer.Id && (f.PaymentMethod == string.Empty || f.PaymentMethod == null)
                              select new CartViewModel
                              {
                                  Total = f.Total,
                                  ListFood = (from g in _context.BillDetail
                                              join h in _context.Food on g.FoodId equals h.Id
                                              where g.BillId == f.Id
                                              select new CartDetailViewModel
                                              {
                                                  Id = f.Id,
                                                  FoodId = g.FoodId,
                                                  Name = h.Name,
                                                  UnitPrice = g.UnitPrice,
                                                  Quantity = g.Quantity,
                                                  Price = g.Price,
                                                  ImageURL = h.ImageURL,
                                                  Type = ""
                                              }).ToList(),
                              }).FirstOrDefaultAsync();
            return cartafterupdate;
        }
    }
}
