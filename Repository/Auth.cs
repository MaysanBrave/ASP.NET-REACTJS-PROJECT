using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SAMSUNG_4_YOU.Models;
using SAMSUNG_4_YOU.Repository.IRepository;
using SAMSUNG_4_YOU.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SAMSUNG_4_YOU.Repository
{
    public class AuthRepository : Repository, IAuthRepository
    {
        private Samsung4YouContext _db;
        private IConfiguration _config;
        private IHttpContextAccessor _httpContext;

        private string setUserLogin;
        private int setUserId;
        public string userRole { get => this.setUserLogin;}
        public int userId { get => this.setUserId; }

        public AuthRepository(Samsung4YouContext db, IConfiguration config, IHttpContextAccessor httpContext) : base(db)
        {
            _db = db;
            _config = config;
            _httpContext = httpContext;
        }

        public bool LoginUser(UserLogin user)
        {
            try
            {
                var adminUser = _db.Admins.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();
                if (adminUser != null)
                {
                    this.setUserLogin = adminUser.Role;
                    this.setUserId = adminUser.AdminId;
                    return true;
                }
                else
                {
                    var customerUser = _db.Customers.Where(x => x.CustomerEmail == user.Email && x.CustomerPassword == user.Password).FirstOrDefault();
                    if (customerUser != null)
                    {
                        this.setUserLogin = customerUser.Role;
                        this.setUserId = customerUser.CustomerId;
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GenerateToken(UserLogin user, string role, int userID)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("userId", userID.ToString()),
                new Claim("userEmail", user.Email),
                new Claim("userRole", role)
                }),
                Expires = DateTime.UtcNow.AddDays(100),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public string?[] validateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "userId").Value;
                var userEmail = jwtToken.Claims.First(x => x.Type == "userEmail").Value;
                var userRole = jwtToken.Claims.First(x => x.Type == "userRole").Value;
                string[] userDetails = { userId, userEmail, userRole };
                return userDetails;
            }
            catch
            {
                return null;
            }
        }
        public bool CustomerRegistration(CustomerRegistration customer)
        {
            try
            {
                var model = new Customer()
                {
                    CustomerName = customer.CustomerName,
                    CustomerEmail = customer.CustomerEmail,
                    CustomerPassword = customer.CustomerPassword,
                    CustomerPhone = customer.CustomerPhone,
                    CustomerAddress = customer.CustomerAddress,
                    Role="customer"
                };
                _db.Customers.Add(model);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
          
        }

        //public string getUserEmail()
        //{
        //    var identity = _httpContext.HttpContext.User.Identity as ClaimsIdentity;
        //    if (identity != null)
        //    {
        //        var userClaims = identity.Claims;
        //        var userEmail = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;
        //        if (userEmail != null)
        //        {
        //            return userEmail;
        //        }

        //    }
        //    return null;
        //}
        public int getUserId()
        {
            var token = _httpContext.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                string?[] result = validateToken(token);
                if (result != null)
                {
                    return Int32.Parse(result[0].ToString());
                }
                   
            }
            return 0;
        }


        public string getUserEmail()
        {
            var token = _httpContext.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                string?[] result = validateToken(token);
                if (result != null)
                {
                    return result[1].ToString();
                }

            }
            return null;
        }

        public IEnumerable<Customer> getAllUsers()
        {
            return _db.Customers.ToList();
        }

        public Customer getCustomerDetails(int customerId)
        {
            return _db.Customers.Find(customerId);
        }
    }
}
