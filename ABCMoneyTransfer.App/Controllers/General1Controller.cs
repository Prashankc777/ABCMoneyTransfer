using ABCMoneyTransfer.App.Services;
using ABCMoneyTransfer.Data.Entities;
using ABCMoneyTransfer.Data.Exceptions;
using ABCMoneyTransfer.Data.Repositories;
using ABCMoneyTransfer.Data.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace ABCMoneyTransfer.App.Controllers
{
    public class General1Controller(IGeneralRepository generalRepository, IForexService forexService) : Controller
    {
       

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet, Route("api/general/countries")]
        public async Task<IActionResult> GetCountries()
        {
            return Ok(await generalRepository.GetAllAsync<Country>());
        }

        [HttpGet, Route("api/general/forex")]
        public async Task<IActionResult> GetForexData(string? startDate = null, string? endDate = null)
        {
            
            var nepaliToday = GeneralUtility.GetCurrentNepaliDateTime();
            startDate = startDate ?? nepaliToday.ToString("yyyy-MM-dd");
            endDate = endDate ?? nepaliToday.ToString("yyyy-MM-dd");
            try
            {
            
                if (!DateOnly.TryParseExact(startDate, "yyyy-MM-dd", out var parsedStartDate))
                {
                    return BadRequest("Invalid start date format. Please use 'yyyy-MM-dd'.");
                }
                if (!DateOnly.TryParseExact(endDate, "yyyy-MM-dd", out var parsedEndDate))
                {
                    return BadRequest("Invalid end date format. Please use 'yyyy-MM-dd'.");
                }
                var data = await forexService.GetDate(parsedStartDate, parsedEndDate);
                if (data == null)
                {
                    return NotFound("Forex data not found for the specified dates.");
                }
                return Ok(data.Data.Payload.SelectMany(x=>x.Rates));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



    }
}
