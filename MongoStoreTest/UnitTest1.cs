
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
        private readonly ProductController prd;
        private readonly DataAccess ctx;

        public UnitTest1()
        {
            ctx = new DataAccess();
            prd = new ProductController(ctx);
        }


        [Theory]
        [InlineData(2)]
        public void GetProductId2AndReturn2(int value)
        {
            var obj = ctx.GetProductsById(value);
            Assert.Equal(value, obj.ProductId);
        }

        [Theory]
        [InlineData(4)]
        public void GetAllProductsAndReturn4Objs(int value)
        {
            var obj = ctx.GetProducts();
            var count = value;
            Assert.Equal(value, obj.Count);
        }

        [Theory]
        [InlineData(2)]
        public void UpdateProductBrandToAppleWhereProductId2(int value)
        {
            var obj = ctx.GetProductsById(value);
            obj.Brand = "Apple";
            ctx.UpdateProduct(value, obj);
            var objAfterUpdate = ctx.GetProductsById(value);
            Assert.Equal("Apple", objAfterUpdate.Brand);

        }




    }
}
