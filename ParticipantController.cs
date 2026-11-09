using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using RaceDayMVC.Models;
using System.Linq.Expressions;
using System.Linq;

namespace RaceDayMVC.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ParticipantController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Participant Dashboard
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // Browse Events
        public async Task<IActionResult> Events()
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var events = await client.GetFromJsonAsync<List<EventViewModel>>(
                    "api/Events");

                return View(events ?? new List<EventViewModel>());
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to connect to RaceDayAPI. Please try again later.";

                return View(new List<EventViewModel>());
            }
        }

        public async Task<ActionResult> EventDetails(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var eventDetails = await client.GetFromJsonAsync<EventViewModel>(
                    $"api/Event/{id}");
            

            if (EventDetails == null)
            {
                return NotFound();
            }

            return View(eventDetails);
            }
            catch (HttpRequestException)
            {
                return View("Error");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enrol(int eventId)
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var enrollment = new EnrollmentViewModel
            {
                eventID = eventId,
                userID = userId.Value,
                enrollmentDate = DateTime.Now,
                status = "Pending",
                raceNumber = null
            };

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var response = await client.PostAsJsonAsync(
                    "api/EventEnrollments", enrollment);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Your event enrolment was submitted!";
                }
                else
                {
                    TempData["Error"] = "Unable to submit your enrolment.";
                }
            }
            catch (HttpRequestException)
            {
                TempData["Error"] = "Unable to connect to RaceDayAPI.";
            }

            return RedirectToAction("Event");
        }

        public async Task<IActionResult> Enrollment()
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var allEnrolments =
                    await client.GetFromJsonAsync<List<EnrollmentViewModel>>(
                        "api/EventEnrollments");

                var myEnrolments = allEnrolments?
                    .Where(e => e.userID == userId.Value)
                    .ToList() ?? new List<EnrollmentViewModel>();

                foreach (var enrolment in myEnrolments)
                {
                    var eventDetails =
                        await client.GetFromJsonAsync<EventViewModel>(
                            $"api/Events/{enrolment.eventID}");

                    if (eventDetails != null)
                    {
                        enrolment.eventName = eventDetails.eventName;
                        enrolment.location = eventDetails.location;
                        enrolment.eventDate = eventDetails.eventDate;
                    }
                }

                return View(myEnrolments);
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to retrieve your enrolments.";
                return View(new List<EnrollmentViewModel>());
            }
        }

        public async Task<IActionResult> Profile()
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
                return RedirectToAction("Login", "Account");

            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var profiles = await client.GetFromJsonAsync<List<ParticipantProfileViewModel>>(
                    "api/ParticipantProfiles");

                var profile = profiles?.FirstOrDefault(p => p.userID == userId.Value);

                if (profile == null)
                {
                    ViewBag.Error = "No participant profile was found.";
                    return View(new ParticipantProfileViewModel { userID = userId.Value });
                }

                return View(profile);
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to retrieve your profile.";
                return View(new ParticipantProfileViewModel { userID = userId.Value });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ParticipantProfileViewModel model)
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
                return RedirectToAction("Login", "Account");

            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            model.userID = userId.Value;

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var response = await client.PutAsJsonAsync(
                    $"api/ParticipantProfiles/{model.profileID}", model);

                if (response.IsSuccessStatusCode)
                    TempData["Success"] = "Your profile was updated successfully.";
                else
                    TempData["Error"] = "Unable to update your profile.";
            }
            catch (HttpRequestException)
            {
                TempData["Error"] = "Unable to connect to RaceDayAPI.";
            }

            return RedirectToAction("Profile");
        }

        public async Task<IActionResult> Result()
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
                return RedirectToAction("Login", "Account");

            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var allResults = await client.GetFromJsonAsync<List<ResultViewModel>>(
                    "api/Results");

                var allEnrolments = await client.GetFromJsonAsync<List<EnrollmentViewModel>>(
                    "api/EventEnrollments");

                var myEnrolments = allEnrolments?
                    .Where(e => e.userID == userId.Value)
                    .ToList() ?? new List<EnrollmentViewModel>();

                var myResults = new List<ResultViewModel>();

                foreach (var result in allResults ?? new List<ResultViewModel>())
                {
                    var enrolment = myEnrolments.FirstOrDefault(
                        e => e.enrollmentID == result.enrollmentID);

                    if (enrolment != null)
                    {
                        var eventDetails = await client.GetFromJsonAsync<EventViewModel>(
                            $"api/Events/{enrolment.eventID}");

                        result.eventName = eventDetails?.eventName ?? "Race Event";
                        result.eventDate = eventDetails?.eventDate ?? DateTime.MinValue;

                        myResults.Add(result);
                    }
                }

                return View(Result);
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to retrieve your results.";
                return View(new List<ResultViewModel>());
            }
        }
    }
}