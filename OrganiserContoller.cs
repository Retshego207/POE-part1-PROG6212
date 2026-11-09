using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using RaceDayMVC.Models;
using System.Linq;

namespace RaceDayMVC.Controllers
{
    public class OrganiserContoller : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            return View();
        }

        public async Task<IActionResult> Event()
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var events = await client.GetFromJsonAsync<List<EventViewModel>>(
                    "api/Events");

                return View(events ?? new List<EventViewModel>());
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to retrieve events.";
                return View(new List<EventViewModel>());
            }
        }

        [HttpGet]
        public IActionResult CreateEvent()
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            return View(new EventViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(EventViewModel model)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            int? organiserId = HttpContext.Session.GetInt32("UserID");

            if (organiserId == null)
                return RedirectToAction("Login", "Account");

            model.organiserID = organiserId.Value;

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var response = await client.PostAsJsonAsync("api/Events", model);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Event created successfully.";
                    return RedirectToAction("Events");
                }

                ViewBag.Error = "Unable to create event.";
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to connect to RaceDayAPI.";
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditEvent(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var eventDetails = await client.GetFromJsonAsync<EventViewModel>(
                    $"api/Events/{id}");

                if (eventDetails == null)
                    return NotFound();

                return View(eventDetails);
            }
            catch (HttpRequestException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEvent(int id, EventViewModel model)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            int? organiserId = HttpContext.Session.GetInt32("UserID");

            if (organiserId == null)
                return RedirectToAction("Login", "Account");

            model.eventID = id;
            model.organiserID = organiserId.Value;

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var response = await client.PutAsJsonAsync(
                    $"api/Events/{id}", model);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Event updated successfully.";
                    return RedirectToAction("Events");
                }

                ViewBag.Error = "Unable to update event.";
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to connect to RaceDayAPI.";
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var response = await client.DeleteAsync($"api/Events/{id}");

                TempData[response.IsSuccessStatusCode ? "Success" : "Error"] =
                    response.IsSuccessStatusCode
                        ? "Event deleted successfully."
                        : "Unable to delete event.";
            }
            catch (HttpRequestException)
            {
                TempData["Error"] = "Unable to connect to RaceDayAPI.";
            }

            return RedirectToAction("Event");
        }

        public async Task<IActionResult> Categories()
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var categories = await client.GetFromJsonAsync<List<CategoryViewModel>>(
                    "api/Categories");

                return View(categories ?? new List<CategoryViewModel>());
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to retrieve categories.";
                return View(new List<CategoryViewModel>());
            }
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            return View(new CategoryViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CategoryViewModel model)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var response = await client.PostAsJsonAsync("api/Categories", model);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Category created successfully.";
                    return RedirectToAction("Categories");
                }

                ViewBag.Error = "Unable to create category.";
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to connect to RaceDayAPI.";
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var category = await client.GetFromJsonAsync<CategoryViewModel>(
                    $"api/Categories/{id}");

                if (category == null)
                    return NotFound();

                return View(category);
            }
            catch (HttpRequestException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, CategoryViewModel model)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            model.categoryID = id;

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var response = await client.PutAsJsonAsync(
                    $"api/Categories/{id}", model);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Category updated successfully.";
                    return RedirectToAction("Categories");
                }

                ViewBag.Error = "Unable to update category.";
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to connect to RaceDayAPI.";
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var response = await client.DeleteAsync($"api/Categories/{id}");

                TempData[response.IsSuccessStatusCode ? "Success" : "Error"] =
                    response.IsSuccessStatusCode
                        ? "Category deleted successfully."
                        : "Unable to delete category.";
            }
            catch (HttpRequestException)
            {
                TempData["Error"] = "Unable to connect to RaceDayAPI.";
            }

            return RedirectToAction("Categories");
        }

        public async Task<IActionResult> Enrollment()
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var enrollment =
                    await client.GetFromJsonAsync<List<EnrollmentViewModel>>(
                        "api/EventEnrollments");

                var events =
                    await client.GetFromJsonAsync<List<EventViewModel>>(
                        "api/Events");

                foreach (var enrolment in enrollment ?? new List<EnrollmentViewModel>())
                {
                    var eventDetails = events?.FirstOrDefault(
                        e => e.eventID == enrolment.eventID);

                    if (eventDetails != null)
                    {
                        enrolment.eventName = eventDetails.eventName;
                        enrolment.location = eventDetails.location;
                        enrolment.eventDate = eventDetails.eventDate;
                    }
                }

                return View(enrollment ?? new List<EnrollmentViewModel>());
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to retrieve enrolments.";

                return View(new List<EnrollmentViewModel>());
            }
        }

        [HttpGet]
        public IActionResult EnterResult(int enrollmentId)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var model = new ResultViewModel
            {
                enrollmentID = enrollmentId,
                resultStatus = "Completed"
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnterResult(ResultViewModel model)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var response = await client.PostAsJsonAsync("api/Results", model);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Race result recorded successfully.";
                    return RedirectToAction("Enrolments");
                }

                ViewBag.Error = "Unable to save the race result.";
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to connect to RaceDayAPI.";
            }

            return View(model);
        }
    }
}
