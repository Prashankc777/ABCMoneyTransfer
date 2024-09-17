using ABCMoneyTransfer.Data.Entities;
using ABCMoneyTransfer.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABCMoneyTransfer.App.Controllers
{
    [Route("api/general")]
    [ApiController]
    public class GeneralController(IGeneralRepository generalRepository) : ControllerBase
    {
        [HttpGet , Route("countries")]
        public async Task<IActionResult> GetCountries()
        {
            return Ok(await generalRepository.GetAllAsync<Country>());
        }
    }
}
