using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CS2TotalCasePriceCalc
{
    public class SteamCaseValueCalculator
    {
        private readonly HttpClient _http = new HttpClient();

        public async Task<decimal> CalculateTotalAsync(Dictionary<string, int> caseInventory)
        {
            decimal total = 0m;

            foreach (var kvp in caseInventory)
            {
                var caseName = kvp.Key;
                var quantity = kvp.Value;

                var price = await GetMarketPrice(caseName);
                total += price * quantity;
            }

            return total;
        }

        private async Task<decimal> GetMarketPrice(string caseName)
        {
            var encoded = Uri.EscapeDataString(caseName);
            var url = $"https://steamcommunity.com/market/priceoverview/?appid=730&currency=3&market_hash_name={encoded}";

            var response = await _http.GetStringAsync(url);
            var json = JObject.Parse(response);

            var priceText = json["lowest_price"]?.ToString();
            if (string.IsNullOrWhiteSpace(priceText))
                return 0m;

            priceText = priceText.Replace("$", "")
                                 .Replace("USD", "")
                                 .Replace("€", "")
                                 .Replace("pуб.", "")
                                 .Replace("£", "")
                                 .Trim();

            int commaCount = priceText.Count(c => c == ',');
            int dotCount = priceText.Count(c => c == '.');

            if (commaCount > 0 && dotCount == 0)
                priceText = priceText.Replace(',', '.');

            priceText = new string(priceText.Where(c => char.IsDigit(c) || c == '.').ToArray());

            if (decimal.TryParse(priceText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var price))
                return price;

            return 0m;
        }
    }
}
