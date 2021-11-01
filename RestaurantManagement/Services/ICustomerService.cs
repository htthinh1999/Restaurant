using RestaurantManagement.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantManagement.Services
{
    public interface ICustomerService
    {
        Task<bool> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(RegisterViewModel registerViewModel);
        Task<List<TableHistoryViewModels>> GetTableHistoryAsync(ClaimsPrincipal user);
        Task<List<PaymentHistoryViewModel>> GetPaymentHistoryAsync(ClaimsPrincipal user);
        Task<List<PaymentDetailViewModel>> GetPaymentDetailAsync(Guid billId);
        Task<PaymentViewModel> GetBillToPayAsync(ClaimsPrincipal user);
        Task UpdatePaymentMethodAsync(ClaimsPrincipal user, PaymentViewModel billPaymentVM);
    }
}
