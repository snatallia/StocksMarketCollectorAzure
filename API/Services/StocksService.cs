using API.Data.Model;
using API.DTO;
using API.Interfaces;
using System.Text.Json;

namespace API.Services
{
    public class StocksService : IStocksService
    {
        private readonly IHttpClientFactory _clientFactory;
        public string Error { get; set; }
        public StocksService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<Stock>> GetStocks()
        {
            IEnumerable<StockDto> stocks = null;

            var request = new HttpRequestMessage(HttpMethod.Get,
                "https://finnhub.io/api/v1/stock/symbol?exchange=US&currency=USD&token=c6am8ciad3id24fn4v2g");

            var client = _clientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();
                try
                {
                    stocks = await JsonSerializer.DeserializeAsync<IEnumerable<StockDto>>(contentStream);
                }
                catch (Exception ex) 
                {
                    Error = $" finnhub json deserialize error: {ex.Message}";
                }
               }
            else
            { 
                Error = $" finnhub response error: {response.ReasonPhrase}";
            }

            return from stock in stocks
                   where stock.symbol.EndsWith('F') == false && stock.type == "Common Stock" &&
                         stock.currency == "USD"
                   select new Stock
                   {
                       Symbol = stock.symbol,
                       DisplaySymbol = stock.displaySymbol,
                       Currency = stock.currency,
                       Description = stock.description,
                       Mic = stock.mic,
                       Type = stock.type
                   };
        }
    }
}
