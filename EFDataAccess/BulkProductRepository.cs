using Domain;
using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataAccess
{
    public interface IBulkProductRepository
    {
        Task<TimeSpan> DeleteBulkDataAsync(int Weight, int Length);
    }

    public class BulkProductRepository : IBulkProductRepository
    {
        private readonly ORMFundContext _context;

        public BulkProductRepository(ORMFundContext context)
        {
            _context = context;
        }
        
        public async Task<TimeSpan> DeleteBulkDataAsync(int Weight, int Length)
        {
            var products = new List<Product>();
            var start = DateTime.Now;
            products = _context.Products.Where(x => x.Weight == Weight && x.Length == Length).ToList();
            await _context.BulkDeleteAsync(products);
            var timeSpan = DateTime.Now - start;
            return timeSpan;
        }
    }
}
