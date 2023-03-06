using API.Data.Model;

namespace API.Interfaces
{
    public interface IStocksService
    {
        Task<IEnumerable<Stock>> GetStocks();
    }
}
