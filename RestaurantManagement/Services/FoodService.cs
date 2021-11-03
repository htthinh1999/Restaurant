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
    public class FoodService : IFoodService
    {
        private readonly RestaurantDbContext _context;
        private readonly UserManager<Customer> _userManager;
        public FoodService(RestaurantDbContext context, UserManager<Customer> userManager)
        {
            _context = context;
            _userManager = userManager;
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
                                  Id = f.Id,
                                  Category = f.Category,
                                  Name = f.Name,
                                  UnitPrice = f.UnitPrice,
                                  ImageURL = f.ImageURL,
                                  Description = f.Description
                                  
                              }).FirstOrDefaultAsync();
            return food;
        }
        public async Task InsertFoodAsync(ClaimsPrincipal user, FoodViewModel food)
        {
            var customer = await _userManager.GetUserAsync(user);
            var currentCart = await (from b in _context.Bill
                                     where b.CustomerId == customer.Id && b.PaymentMethod == string.Empty
                                     select b).FirstOrDefaultAsync();
            var billId = System.Guid.NewGuid();
            if (currentCart != null)
            {
                billId = currentCart.Id;
                var currentFoodInCart = await (from bd in _context.BillDetail
                                               where bd.BillId == billId && bd.FoodId == food.InsertFoodViewModel.FoodId
                                               select bd).FirstOrDefaultAsync();
                if (currentFoodInCart != null)
                {
                    currentFoodInCart.Quantity += food.InsertFoodViewModel.Quantity;
                    currentFoodInCart.Price = currentFoodInCart.Quantity * currentFoodInCart.UnitPrice;
                    _context.SaveChanges();
                    currentCart.Total = (from bd in _context.BillDetail
                                         where bd.BillId == billId
                                         select bd).Sum(x => x.Price);
                }
                else
                {
                    var newBillDetail = new BillDetail()
                    {
                        BillId = billId,
                        FoodId = food.InsertFoodViewModel.FoodId,
                        UnitPrice = food.InsertFoodViewModel.UnitPrice,
                        Quantity = food.InsertFoodViewModel.Quantity,
                        Price = food.InsertFoodViewModel.UnitPrice * food.InsertFoodViewModel.Quantity
                    };
                    currentCart.Total += food.InsertFoodViewModel.UnitPrice * food.InsertFoodViewModel.Quantity;
                    _context.BillDetail.Add(newBillDetail);
                }
                _context.SaveChanges();
                return;
            }
            else
            {
                var newBill = new Bill()
                {
                    Id = billId,
                    CustomerId = customer.Id,
                    Total = food.InsertFoodViewModel.Quantity * food.InsertFoodViewModel.UnitPrice,
                    PaymentMethod = string.Empty,
                    CreatedDate = DateTime.Now
                };
                _context.Bill.Add(newBill);
                var newBillDetail = new BillDetail()
                {
                    BillId = billId,
                    FoodId = food.InsertFoodViewModel.FoodId,
                    UnitPrice = food.InsertFoodViewModel.UnitPrice,
                    Quantity = food.InsertFoodViewModel.Quantity,
                    Price = food.InsertFoodViewModel.UnitPrice * food.InsertFoodViewModel.Quantity
                };
                _context.BillDetail.Add(newBillDetail);
                _context.SaveChanges();
                var newBillId = await (from b in _context.Bill
                                       where b.Id == billId
                                       select b).FirstOrDefaultAsync();
                newBillId.Total = (from bd in _context.BillDetail
                                   where bd.BillId == billId
                                   select bd).Sum(x => x.Price); 
                _context.SaveChanges();
            }
        }
    }
}