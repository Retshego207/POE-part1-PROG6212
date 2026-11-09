using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using RaceDayMVC.Models;

namespace RaceDayMVC.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.PostAsJsonAsync(
                "api/Users/register", model);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Registration successful. Please log in.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("",
                "Registration failed. Check your details or try again.");

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.PostAsJsonAsync(
                "api/Users/login", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("",
                    "Invalid email or password.");
                return View(model);
            }

            var user = await response.Content
                .ReadFromJsonAsync<LoginApiResponse>();

            if (user == null)
            {
                ModelState.AddModelError("",
                    "Unable to read the login response.");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserID", user.userID);
            HttpContext.Session.SetString("FirstName", user.firstName);
            HttpContext.Session.SetString("Role", user.role);
            HttpContext.Session.SetString("Email", user.email);

            if (user.role == "Organiser")
                return RedirectToAction("Dashboard", "Organiser");

            if (user.role == "Participant")
                return RedirectToAction("Dashboard", "Participant");

            HttpContext.Session.Clear();

            ModelState.AddModelError("",
                "Your account has an unrecognised role.");

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}