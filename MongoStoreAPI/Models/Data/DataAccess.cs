﻿using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MongoStoreAPI.Models.Data
{
    public class DataAccess
    {
        private readonly IMongoDatabase _mongoDatabase;
        
        public DataAccess()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            _mongoDatabase = mongoClient.GetDatabase("ProductsDb");
        }

        //Create
        public Products AddProduct(Products entity)
        {
            _mongoDatabase.GetCollection<Products>("Products").InsertOne(entity);
            return entity;
        }

        //Read
        public IEnumerable<Products> GetProducts()
        {
            return _mongoDatabase.GetCollection<Products>("Products").Find(FilterDefinition<Products>.Empty).ToList();
        }

        public List<Products>  GetProductsPaged(int page , int pageCount , out int totalEntitys ,out int totalPages, out int nextPage)
        {
            var query = _mongoDatabase.GetCollection<Products>("Products").Find(FilterDefinition<Products>.Empty).ToList();
            totalEntitys = query.Count();
            totalPages = totalEntitys / pageCount;
            nextPage = page < totalPages ?  page + 1 : 0;  

            return query.Skip(page * pageCount).SkipLast(pageCount).ToList();

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
