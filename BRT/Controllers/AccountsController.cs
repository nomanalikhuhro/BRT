using BRT.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BRT.Controllers
{
    public class AccountsController : Controller
    {
        private readonly BRTDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public AccountsController(BRTDBContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration, HttpClient httpClient)
        {
            _context = context;
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient("ContainerApi");

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {

            var loginRequest = new
            {
                Username = username,
                Password = password
            };

            var jsonContent = JsonConvert.SerializeObject(loginRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/Account/Login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<ResponseModel>(responseContent);
                HttpContext.Session.SetString("Userid", responseObject.User.UserId.ToString());
                HttpContext.Session.SetString("UserName", responseObject.User.FirstName + " " + responseObject.User.LastName);
                string message = "Login Successful \n Welcome: "+ responseObject.User.FirstName + " " + responseObject.User.LastName;
                TempData["message"] = message;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Handle login failure
                ViewBag.Message = "Login Failed";
                return View();
            }
        }

        public IActionResult Logout()
        {
            // Clear the user session
            HttpContext.Session.Clear();


            // Redirect to login page after logout
            return View(nameof(Login));
        }

    }
}
