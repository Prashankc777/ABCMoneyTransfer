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
        public async Task<IActionResult> GetForexData(string? startDate = null, string? endDate = null, string? iso3 = null)
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

                var rates = data.Data.Payload.SelectMany(x => x.Rates).ToList();

                if (string.IsNullOrEmpty(iso3))
                {
                    return Ok(rates);
                }
                else
                {
                  //  var filteredRates = rates.Where(x => string.Equals(x.Currency.Iso3, iso3, StringComparison.InvariantCultureIgnoreCase)).ToList();
                  var filteredRates = rates[14];

                    //if (filteredRates.Count == 0)
                    //{
                    //    return NotFound($"No forex data found for the currency code: {iso3}.");
                    //}

                    return Ok(filteredRates.Buy);
                }
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
