using SAMSUNG_4_YOU.Models;
using SAMSUNG_4_YOU.ViewModels;
namespace SAMSUNG_4_YOU.Repository.IRepository
{
    public interface IAuthRepository
    {
        string userRole { get; }
        int userId { get; }
        bool LoginUser(UserLogin user);
        string GenerateToken(UserLogin user, string role,int userId);
        string?[] validateToken(string token);
        bool CustomerRegistration(CustomerRegistration customer);
        int getUserId();
        string? getUserEmail();
        IEnumerable<Customer> getAllUsers();
        Customer getCustomerDetails(int customerId);

    }
}
