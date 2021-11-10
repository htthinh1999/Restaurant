using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using RestaurantManagement.Services;
using System;
using RestaurantManagement.Models;

namespace RestaurantManagement.Controllers
{
    public class ResetPasswordController : Controller
    {
        private readonly IResetPasswordService _resetPaswordService;

        public ResetPasswordController(IResetPasswordService resetPasswordService)
        {
            _resetPaswordService = resetPasswordService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm]string username)
        {
            var success = await _resetPaswordService.SendMailResetPasswordAsync(username);
            return RedirectToAction(nameof(SendMailResetPasswordSuccess));
        }

        [HttpGet("/ResetPasswordHandle/{customerId}")]
        public IActionResult ResetPasswordHandle([FromRoute]Guid customerId)
        {
            return View(customerId);
        }

        [HttpPost("/ResetPasswordHandle/{customerId}")]
        public async Task<IActionResult> ResetPasswordHandle([FromRoute] Guid customerId, [FromForm] ResetPasswordViewModel resetPasswordViewModel)
        {
            var success = await _resetPaswordService.ResetPasswordAsync(customerId, resetPasswordViewModel);
            if (success)
            {
                return RedirectToAction(nameof(ResetPasswordSuccess));
            }
            return View(customerId);
        }

        public IActionResult ResetPasswordSuccess()
        {
            return View();
        }

        public IActionResult SendMailResetPasswordSuccess()
        {
            return View();
        }
    }
}
