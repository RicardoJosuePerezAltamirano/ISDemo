using Client.Models;
using Client.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITokenService tokenService;

        public HomeController(ILogger<HomeController> logger
            ,  ITokenService tokenService)
        {
            _logger = logger;
            this.tokenService = tokenService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #region paso 4
        [Authorize]
        #endregion
        public async Task<IActionResult> Weather()
        {
            using var client = new HttpClient();

            // muestra de primer paso, autenticacion entre aplicaciones 
           // var token =await tokenService.GetToken("weatherapi.read");--------paso 3 , se comenta en paso 4


            //muestra de segundo paso autenticacion por usuatio
            var token = await HttpContext.GetTokenAsync("access_token");//---------- paso 4

            // provar con usuario alice/alice




            client.SetBearerToken(/*token.AccessToken*/ /*<- paso 1*/token /*<- paso 4*/);

            var result = await client.GetAsync("https://localhost:7204/weatherforecast");
            if (result.IsSuccessStatusCode)
            {
                var data = await result.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<List<weatherData>>(data);
                return Ok(response);
            }
            throw new Exception("error");
        }
    }
}