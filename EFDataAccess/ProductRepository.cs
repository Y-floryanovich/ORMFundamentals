using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataAccess
{
    public interface IProductRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetAll();
        Task<bool> InsertAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);

    }

    public class ProductRepository : IProductRepository<Product>
    {
        private readonly ORMFundContext _context;
        public ProductRepository(ORMFundContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteAsync(Product entity)
        {
            _context.Products.Remove(entity);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> InsertAsync(Product entity)
        {
            await _context.Products.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> UpdateAsync(Product entity)
        {
            var product = await _context.Products.FindAsync(entity.Id);
            if(product == null)
                return false;

            product.Name = entity.Name;
            product.Description = entity.Description;
            product.Weight = entity.Weight;
            product.Height = entity.Height;
            product.Length = entity.Length;
            product.Width = entity.Width;
            var result = _context.Entry(product).State == EntityState.Modified ? true : false;
            await _context.SaveChangesAsync();
            return result;
        }

        public IQueryable<Product> GetAll() => _context.Products;

        public async Task<Product> GetByIdAsync(int id) => await _context.Products.FindAsync(id);
    }
}
