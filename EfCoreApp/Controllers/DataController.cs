using EfCoreApp.Models;
using EfCoreApp.Servicies.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EfCoreApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IDataService _dataService;

        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        [Route("getData")]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _dataService.GetAsync());
        }
    }
}