using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using MVC.Models;
using System.Text.Json;
using System.Collections.Generic;
using DTO;

namespace MVC.Controllers
{
    public class RolController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RolController> _logger;

        public RolController(HttpClient httpClient, ILogger<RolController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IActionResult> UsuarioRole()
        { 

            return View("~/Views/Home/UsuarioRole.cshtml");
        }

    }
}
