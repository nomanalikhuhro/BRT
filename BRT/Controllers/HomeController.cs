using BRT.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace BRT.Controllers
{
    public class HomeController : Controller
    {
        private readonly BRTDBContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly PasswordHasher<object> _passwordHasher;

        private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly ILogger<LoginModel> _logger;

        public HomeController(SignInManager<IdentityUser> signInManager, ILogger<HomeController> logger, BRTDBContext context, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _signInManager = signInManager;
            _passwordHasher = new PasswordHasher<object>();

            _httpClient = httpClientFactory.CreateClient("ContainerApi");
        }
        public async Task<IActionResult> PlaceContainer(string ContainerNo,int Size ,string[] locationNames)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    foreach (var locationName in locationNames)
                    {
                        using (SqlCommand command = new SqlCommand("usp_AddUpdateContainerLocation", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            // Add parameters
                            command.Parameters.AddWithValue("@ContainerNo", ContainerNo);
                            command.Parameters.AddWithValue("@LocationNames", locationName);
                            command.Parameters.AddWithValue("@CreatedBy", HttpContext.Session.GetString("UserName")); // Adjust as necessary for the actual user
                            command.Parameters.AddWithValue("@Size", Size);

                            // Execute the command
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                }

                // Return success message with the new container ID
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log the error for debugging purposes
                _logger.LogError(ex, "Error placing container.");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetContainerNos()
        {

            List<string> containerNos = new List<string>();

            // Call the API endpoint to get container numbers
            var response = await _httpClient.GetAsync("/api/container/GetContainerNos");

            if (response.IsSuccessStatusCode)
            {
                containerNos = await response.Content.ReadFromJsonAsync<List<string>>();
            }
            return Json(containerNos);
        }




        [HttpGet]
        public async Task<IActionResult> GetContainerSize(string containerNo)
        {
            int containerSize = 0;

            // Call the API endpoint to get the container size
            var response = await _httpClient.GetAsync($"/api/container/GetContainerSize?containerNo={containerNo}");

            if (response.IsSuccessStatusCode)
            {
                containerSize = await response.Content.ReadFromJsonAsync<int>();
            }

            return Json(new { size = containerSize });
        }


        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Userid") == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (TempData["message"] != null)
            {

                ViewBag.Message = TempData["message"];
            }
            return View();
        }
        public ActionResult GetContainerLocationHistory()
        {
            if (HttpContext.Session.GetString("Userid") == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            return View();
        }
        public ActionResult GetContainerLocation(string containerNo)
        {
            // containerNo = "ACLU6491409";
            if (containerNo != null)
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetContainerHistory", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ContainerNo", containerNo);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dt);
                    }
                }

                ViewBag.Data = dt;
            }
            return View(nameof(GetContainerLocationHistory));
        }
        public IActionResult AllLocations()
        {
            if (HttpContext.Session.GetString("Userid") == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
