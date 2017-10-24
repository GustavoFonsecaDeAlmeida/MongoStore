using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoStoreAPI.Models.Data;
using MongoStoreAPI.Models;
using MongoDB.Bson;

namespace MongoStoreAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Product")]
    public class ProductController : Controller
    {
        private readonly DataAccess ctx;

        public ProductController(DataAccess _ctx)
        {
            ctx = _ctx;
        }
        // GET: api/Product
        [HttpGet]
        
        public JsonResult  Get()
        {
            var x = ctx.GetProducts();
            return Json(Ok(x));
        }

        // GET: api/Product/5
        [HttpGet("{id}", Name = "Get")]
        public JsonResult Get(int id)
        {
            var x = ctx.GetProductsById(id);
            if (x != null)
            {
                return Json(Ok(x));
            }
            else
            {
                return Json(NotFound("Ops..Product not found"));
            }
            
        }

        [HttpGet("paged")]
        // GET: api/Product/paged?page=x&pagecount=y
        public JsonResult Get(int page , int pageCount)
        {
            int totalEntitys = 0 ;
            int totalPages = 0;
            int nextPage = 0;

            if (pageCount > 5)
            {
                return Json("A Pagecount cant exceed 5");
            }
            if (page <= 0 )
            {
                return Json("The Page number cant be 0 ");
            }
            var x = ctx.GetProductsPaged(page , pageCount , out totalEntitys, out totalPages, out nextPage);

            if (x != null)
            {
                Request.HttpContext.Response.Headers.Add("X-Total-Count", totalEntitys.ToString());
                Request.HttpContext.Response.Headers.Add("X-Total-Pages", totalPages.ToString());

                if (pageCount == 0 )
                {
                    Request.HttpContext.Response.Headers.Add("X-Total-Pages", "Last Page");
                }
                else
                {
                    Request.HttpContext.Response.Headers.Add("X-Total-Pages", nextPage.ToString());
                }
                return Json(Ok(x));
            }
            else
            {
                return Json(NotFound("Ops..Products not found"));
            }

        }

        // POST: api/Product
        [HttpPost]
        public JsonResult Post([FromBody]Products prod)
        {
           var x = ctx.AddProduct(prod);
            if (x != null)
            {
                return Json(Ok(x));

            }
            else
            {
                return Json("Ops.. Product cant be added");
            }
        }
        
        // PUT: api/Product/5
        [HttpPut("{id}")]
        public JsonResult Put(int id, [FromBody]Products prod)
        {
            var x = ctx.UpdateProduct(id, prod);

            if (x)
            {
                return Json(Ok("product update successful"));
            }
            else
            {
                return Json("Ops.. Product cant be updated");
            }
            
        }
        
        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            var x = ctx.DeleteProduct(id);
            if (x)
            {
                return Json(Ok("Product Deleted"));
            }
            else
            {
                return Json("Product can be deleted");
            }
        }
    }
}
