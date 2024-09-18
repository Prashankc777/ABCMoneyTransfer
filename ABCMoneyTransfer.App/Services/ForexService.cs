using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using ABCMoneyTransfer.App.Models;

namespace ABCMoneyTransfer.App.Services
{
    public interface IForexService
    {
        Task<Root?> GetDate(DateOnly startDate , DateOnly endDate);
    }

    public class ForexService(HttpClient httpClient) : IForexService
    {
        public async Task<Root?> GetDate(DateOnly startDate, DateOnly endDate)
        {
            if (endDate < startDate)
            {
                throw new ArgumentException("End date cannot be earlier than start date.");
            }

            string startDateString = startDate.ToString("yyyy-MM-dd");
            string endDateString = endDate.ToString("yyyy-MM-dd");

            string apiUrl = $"https://www.nrb.org.np/api/forex/v1/rates?page=1&per_page=5&from={startDateString}&to={endDateString}";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Root? result = JsonSerializer.Deserialize<Root>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase, 
                    PropertyNameCaseInsensitive = true
                });

                return result;
            }
            catch (HttpRequestException ex)
            {
                
                Console.WriteLine($"Request error: {ex.Message}");
                throw new ApplicationException("Error while fetching data from the API.", ex);
            }
            catch (JsonException ex)
            {
                
                Console.WriteLine($"Serialization error: {ex.Message}");
                throw new ApplicationException("Error while parsing the API response.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }
    }
}
