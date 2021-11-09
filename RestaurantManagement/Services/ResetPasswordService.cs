using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestaurantManagement.Data;
using RestaurantManagement.Utils;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using RestaurantManagement.Data.Entities;
using System;
using RestaurantManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace RestaurantManagement.Services
{
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly RestaurantDbContext _context;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<Customer> _userManager;

        public ResetPasswordService(
            RestaurantDbContext context,
            IConfiguration config,
            IWebHostEnvironment environment,
            UserManager<Customer> userManager)
        {
            _context = context;
            _config = config;
            _environment = environment;
            _userManager = userManager;
        }

        public async Task<bool> SendMailResetPasswordAsync(string username)
        {
            //_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var customer = await _context.Customer.AsNoTracking()
                                        .Where(c => c.UserName == username)
                                        .Select(c => c)
                                        .FirstOrDefaultAsync();
            //(from c in _context.Customer.AsNoTracking()
            // where c.UserName == username
            // select c.Email).FirstOrDefault();

            if (string.IsNullOrEmpty(customer.Email))
            {
                return false;
            }

            await MailResetPasswordHandler(customer);
            return true;
        }

        private async Task MailResetPasswordHandler(Customer customer)
        {
            var emailFrom = _config.GetValue<string>("EmailInfo:Email");
            var emailPassword = _config.GetValue<string>("EmailInfo:Password");
            string title = "[RESTAURANT] Đổi mật khẩu tài khoản";

            string body = string.Empty;
            var filePath = Path.Combine(_environment.WebRootPath, "Template", "mail-reset-password.html");
            using (StreamReader reader = new StreamReader(filePath))
            {
                body = reader.ReadToEnd();
            }

            var resetPasswordLink = _config.GetValue<string>("WebURL") + $"/ResetPasswordHandle/{customer.Id}";
            body = body.Replace("[customer_full_name]", customer.FullName);
            body = body.Replace("[reset_password_link]", resetPasswordLink);
            body = body.Replace("[current_year]", DateTime.Now.Year.ToString());

            await MailUtils.SendMailGoogleSmtpAsync(emailFrom, customer.Email,
                                                    title, body,
                                                    emailFrom, emailPassword);
        }

        public async Task<bool> ResetPasswordAsync(Guid customerId, ResetPasswordViewModel resetPasswordViewModel)
        {
            if (resetPasswordViewModel.NewPassword != resetPasswordViewModel.ConfirmNewPassword)
                return false;

            var customer = await _context.Customer
                                        .Where(c => c.Id == customerId)
                                        .Select(c => c)
                                        .FirstOrDefaultAsync();

            var token = await _userManager.GeneratePasswordResetTokenAsync(customer);
            await _userManager.ResetPasswordAsync(customer, token, resetPasswordViewModel.NewPassword);
            _context.SaveChanges();
            return true;
        }
    }
}
