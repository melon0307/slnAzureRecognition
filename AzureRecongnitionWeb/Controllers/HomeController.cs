using AzureRecongnitionWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AzureRecongnitionWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FaceAnalysis()
        {
            return View();
        }

        public IActionResult VisionAnalysis()
        {
            return View();
        }

        public IActionResult VisionAnalysisBinaryData()
        {
            return View();
        }

        public IActionResult ActionSync()
        {
            return View();
        }

        public async Task<IActionResult> ActionAsync() // 同步
        {
            return View();
        }

        public IActionResult CustomVision()
        {
            return View();
        }

        public IActionResult LUIS()
        {
            return View();
        }

        //public IActionResult BOT()
        //{
        //    return View();
        //}

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
