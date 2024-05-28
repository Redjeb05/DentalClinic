using DentalClinic.Data;
using DentalClinic.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DentalClinic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {
            int dentists = _context.Dentist
                .Count();
            int patients = _context.Patient
                .Count();
            int examinations = _context.Examination
                .Count();
            ;

            ViewData["Dentists"] = dentists;
            ViewData["Patients"] = patients;
            ViewData["Examinations"] = examinations;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}