using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestaurantManagement.Models;
using RestaurantManagement.Services;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICustomerService _customerService;

        public HomeController(ILogger<HomeController> logger,
                              ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/Login")]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var loginSucess = await _customerService.LoginAsync(loginViewModel.UserName, loginViewModel.Password);
            
            if (!loginSucess)
            {
                return View(loginViewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("/Register")]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost("/Register")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            var registerSucess = await _customerService.RegisterAsync(registerViewModel);

            if (!registerSucess) {
                return View(registerViewModel);
            }

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> TableOrderedHistory()
        {
            var TbHistory = await _customerService.GetTableHistoryAsync(User);
            return View(TbHistory);
        }
        public async Task<IActionResult> PaymentHistory()
        {
            var pmHistory = await _customerService.GetPaymentHistoryAsync(User);
            return View(pmHistory);
        }
        public async Task<IActionResult> PaymentDetailHistory(Guid id)
        {
            var paymentDetail = await _customerService.GetPaymentDetailAsync(id);
            return View(paymentDetail);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
