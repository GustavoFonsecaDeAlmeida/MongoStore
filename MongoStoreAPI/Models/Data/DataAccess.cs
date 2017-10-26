using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoStoreAPI.Helper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MongoStoreAPI.Models.Data
{
    public class DataAccess
    {

        private readonly IMongoDatabase _mongoDatabase;
        private IMongoCollection<Products> ProductsCollection;

        public DataAccess()
        {
            try
            {
                var mongoClient = new MongoClient("mongodb://gustavofalmeida:123456@ds231715.mlab.com:31715/productsdb");
                _mongoDatabase = mongoClient.GetDatabase("productsdb");
                ProductsCollection = _mongoDatabase.GetCollection<Products>("Products");
            }
            catch (Exception ex)
            {
                throw new Exception("Cant acess database", ex);
            }

        }

        //Create
        public Products AddProduct(Products entity)
        {
            _mongoDatabase.GetCollection<Products>("Products").InsertOne(entity);
            return entity;
        }

        //Read
        public async Task<List<Products>> GetProducts()
        {
            var x = await _mongoDatabase.GetCollection<Products>("Products").Find(FilterDefinition<Products>.Empty).ToListAsync();
            return x;
        }

        public List<Products> GetProductsPaged(int page, int pageCount, out int totalEntitys, out int totalPages, out int nextPage)
        {
            var query = _mongoDatabase.GetCollection<Products>("Products").Find(FilterDefinition<Products>.Empty).ToList();
            totalEntitys = query.Count();
            totalPages = totalEntitys / pageCount;
            nextPage = page < totalPages ? page + 1 : 0;
            return query.Skip((page - 1) * pageCount).Take(pageCount).ToList();

        }

        public List<Products> GetProductsPagedFilteredByBrand(int page, int pageobjectcount, string propValue, out int totalEntitys, out int totalPages, out int nextPage)
        {
            var filter = Builders<Products>.Filter.Eq(Products => Products.Brand, propValue);
            var query = _mongoDatabase.GetCollection<Products>("Products").Find(filter).ToList();
            totalEntitys = query.Count();
            totalPages = totalEntitys / pageobjectcount;
            nextPage = page < totalPages ? page + 1 : 0;

            return query.Skip((page - 1) * pageobjectcount).Take(pageobjectcount).ToList();

        }

        public Products GetProductsById(int prodId)
        {
            var filter = Builders<Products>.Filter.Eq(Products => Products.ProductId, prodId);
            return _mongoDatabase.GetCollection<Products>("Products").Find(filter).FirstOrDefault();
        }

        //Update
        public bool UpdateProduct(int prodId, Products entity)
        {
            var filter = Builders<Products>.Filter.Eq(Products => Products.ProductId, prodId);
            var update = Builders<Products>.Update
                .Set(x => x.ProductName, entity.ProductName)
                .Set(x => x.Brand, entity.Brand)
                .Set(x => x.Description, entity.Description)
                .Set(x => x.ImageUrl, entity.ImageUrl)
                .Set(x => x.Price, entity.Price)
                .Set(x => x.ProductId, entity.ProductId);

            var updateResult = _mongoDatabase.GetCollection<Products>("Products").UpdateOne(filter, update);

            return updateResult.IsAcknowledged;

            //Work with updateResult
        }

        //Delete
        public bool DeleteProduct(int prodId)
        {
            var filter = Builders<Products>.Filter.Eq(Products => Products.ProductId, prodId);
            var dbQuery = _mongoDatabase.GetCollection<Products>("Products").DeleteOne(filter);
            return dbQuery.IsAcknowledged;

        }
    }


}

