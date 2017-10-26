
using MongoDB.Bson;
using MongoStoreAPI.Controllers;
using MongoStoreAPI.Helper;
using MongoStoreAPI.Models;
using MongoStoreAPI.Models.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace MongoStoreTest
{
    public class UnitTest1
    {
        private readonly ProductController _prd;
        private readonly DataAccess _ctx;

        public UnitTest1()
        {
            _ctx = new DataAccess();
            _prd = new ProductController(_ctx);
        }


        [Theory]
        [InlineData(2)]
        public void GetProductId2AndReturn2(int value)
        {
            var obj = _ctx.GetProductsById(value);
            Assert.Equal(value, obj.ProductId);
        }

        [Theory]
        [InlineData(4)]
        public async void GetAllProductsAndReturn4Objs(int value)
        {
            var obj = await _ctx.GetProducts();
            var count = value;
            Assert.Equal(value, obj.Count);
        }

        [Theory]
        [InlineData(2)]
        public void UpdateProductBrandToAppleWhereProductId2(int value)
        {
            var obj = _ctx.GetProductsById(value);
            obj.Brand = "Apple";
            _ctx.UpdateProduct(value, obj);
            var objAfterUpdate = _ctx.GetProductsById(value);
            Assert.Equal("Apple", objAfterUpdate.Brand);

        }

        [Fact]
        public void AddProductAndReturnItCoparingNameAndUniqId()
        {
            var prd = new Products();

            prd.Brand = "Multilaser";
            prd.Category = "Router";
            prd.Description = "BF Router";
            prd.ImageUrl = "ABC.COM/1223.PNG";
            prd.ProductName = "MultilazerBfG";
            prd.Id = ObjectId.GenerateNewId();
            prd.ProductId = new Random().Next();
            var CreateProductReturn = _ctx.AddProduct(prd);
            
            Assert.Equal(prd.ProductName, CreateProductReturn.ProductName);
            Assert.Equal(prd.Id, CreateProductReturn.Id);

        }




    }
}
