using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data.Contex;
using API.Data.Model;
using API.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly StockContext _context;
        private readonly IStocksService _stocksService;
        private readonly ILogger<StocksController> _logger;

        public StocksController(StockContext context, IStocksService stocksService, ILogger<StocksController> logger)
        {
            _context = context;
            _stocksService = stocksService;
            _logger = logger;
        }

        // GET: api/Stocks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stock>>> GetStocks()
        {
            return await _context.Stocks.ToListAsync();
        }

        // GET: api/Stocks/AAAA
        [HttpGet("{symbol}")]
        public async Task<ActionResult<Stock>> GetStock(string symbol)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);

            if (stock == null)
            {
                return NotFound();
            }

            return stock;
        }

       

        // POST: api/Stocks
        [HttpPost]
        public async Task<bool> PostStock(Stock stock)
        {
            if (stock == null || String.IsNullOrEmpty(stock.Symbol))
                return false;

            _logger.LogInformation("StockController PostStocks: {symbol}", stock.Symbol);
            var myEntity = await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == stock.Symbol);

            if (myEntity == null)
                _context.Stocks.Add(stock);

            await _context.SaveChangesAsync();
            return true;
        }
        


        // DELETE: api/Stocks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(Guid id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();

            return NoContent();
        }




        [HttpPost("loading")]
        public async Task<bool> LoadStocks(IEnumerable<Stock> stocks)
        { 
            if (stocks == null || !stocks.Any())
                return false;
            _logger.LogInformation("StockController started LoadStocks at: {time}", DateTimeOffset.Now);

            foreach (var stock in stocks)
            {
                var myEntity = await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == stock.Symbol);
                if (myEntity == null)
                    _context.Stocks.Add(stock);
            }
            await _context.SaveChangesAsync();
            _logger.LogInformation("StockController finished LoadStocks at: {time}", DateTimeOffset.Now);
            return true;
        }





        // PUT: api/Stocks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStock(Guid id, Stock stock)
        {
            if (id != stock.Id)
            {
                return BadRequest();
            }

            _context.Entry(stock).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        private bool StockExists(Guid id)
        {
            return _context.Stocks.Any(e => e.Id == id);
        }
    }
}
