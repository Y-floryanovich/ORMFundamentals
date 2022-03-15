using Domain;
using EFDataAccess;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    class SprintRepositoryTest
    {
        private ORMFundContext _context;
        private IProductRepository<Product> _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ORMFundContext>().UseInMemoryDatabase(databaseName: "ProductDatabase").Options;
            _context = new ORMFundContext(options);
            _repository = new ProductRepository(_context);
        }

        private IQueryable<Product> GetFakeDb()
        {
            var data = new List<Product>
            {
                new Product { Id = 1, Name = "First", Description = "First", Height = 10, Length = 10, Weight = 10, Width = 10 },
                new Product { Id = 2, Name = "Second", Description = "Second", Height = 20, Length = 20, Weight = 20, Width = 20 },
                new Product { Id = 3, Name = "Third", Description = "Third", Height = 30, Length = 30, Weight = 30, Width = 30 },
            }.AsQueryable();
            return data;
        }

        [Test]
        public async Task GetAll_ProductRepositoryReturnsList_ListCount3()
        {
            //Arrange 
            var count = 3;
            _context.Products.AddRange(GetFakeDb());
            _context.SaveChanges();

            //Act 
            var result = await _repository.GetAll().ToListAsync();

            //Assert 
            Assert.AreEqual(result.Count(), count);
        }

        [Test]
        public async Task GetByIdAsync_WhenProductExist_ReturnProductWithCorrectId()
        {
            //Arrange 
            var id = 3;

            //Act 
            var result = await _repository.GetByIdAsync(id);

            //Assert 
            Assert.AreEqual(id, result.Id);
        }

        [Test]
        public async Task GetByIdAsync_WhenProductDoesNotExist_ReturnNull()
        {
            //Arrange 
            var id = 30;

            //Act 
            var result = await _repository.GetByIdAsync(id);

            //Assert 
            Assert.IsNull(result);
        }

        [Test]
        public async Task InsertAsync_WhenProductWasInserted_ReturnTrue()
        {
            //Arrange 
            var product = new Product { Id = 4, Name = "Fourth", Description = "Fourth", Height = 40, Length = 40, Weight = 40, Width = 40 };

            //Act 
            var result = await _repository.InsertAsync(product);

            //Assert 
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteAsync_WhenProductWasDeleted_ReturnTrue()
        {
            //Arrange 
            var product = new Product { Id = 4, Name = "Fourth", Description = "Fourth", Height = 40, Length = 40, Weight = 40, Width = 40 };
            _context.Products.Add(product);
            _context.SaveChanges();

            //Act 
            var result = await _repository.DeleteAsync(product);

            //Assert 
            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateAsync_WhenProductWasUpdate_ReturnTrue()
        {
            //Arrange 
            var product = new Product { Id = 3, Name = "UpdatedThird", Description = "UpdatedThird", Height = 40, Length = 40, Weight = 40, Width = 40 };

            //Act 
            var result = await _repository.UpdateAsync(product);

            //Assert 
            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateAsync_WhenProductWasUpdated_ReturnFalse()
        {
            //Arrange 
            var product = new Product { Id = 4, Name = "Fourth", Description = "Fourth", Height = 40, Length = 40, Weight = 40, Width = 40 };

            //Act 
            var result = await _repository.UpdateAsync(product);

            //Assert 
            Assert.IsFalse(result);
        }
    }
}