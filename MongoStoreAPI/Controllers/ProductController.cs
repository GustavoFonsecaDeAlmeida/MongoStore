using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoStoreAPI.Models.Data;
using MongoStoreAPI.Models;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace MongoStoreAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Product")]
    public class ProductController : Controller
    {
        private readonly DataAccess _ctx;

        public ProductController(DataAccess ctx)
        {
            this._ctx = ctx;
        }

        // GET: api/Product
        [HttpGet]

        public async Task<JsonResult> Get()
        {
            var x = await _ctx.GetProducts();
            return Json(Ok(x));
        }



        // GET: api/Product/5
        [HttpGet("{id}", Name = "Get")]
        public JsonResult Get(int id)
        {
            var x = _ctx.GetProductsById(id);
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
        public JsonResult Get(int page, int pageCount)
        {


            if (pageCount > 5)
            {
                return Json("A Pagecount cant exceed 5");
            }
            if (page <= 0)
            {
                return Json("The Page number cant be 0 ");
            }
            var x = _ctx.GetProductsPaged(page, pageCount, out var totalEntitys, out var totalPages, out var nextPage);

            if (x != null)
            {
                Request.HttpContext.Response.Headers.Add("X-Total-Count", totalEntitys.ToString());
                Request.HttpContext.Response.Headers.Add("X-Total-Pages", totalPages.ToString());

                if (nextPage == 0)
                {
                    Request.HttpContext.Response.Headers.Add("X-Next-Page", "Last Page");
                }
                else
                {
                    Request.HttpContext.Response.Headers.Add("X-Next-Page", nextPage.ToString());
                }
                return Json(Ok(x));
            }
            else
            {
                return Json(NotFound("Ops..Products not found"));
            }

        }

        [HttpGet("pagedAndFilterByBrand")]
        // GET: api/Product/pagedAndFilterByBrand?page=x&pageobjectcount=y&brand=z
        public JsonResult Get(int page, int pageobjectcount, string brand)
        {
            if (pageobjectcount > 5)
            {
                return Json("A Pagecount cant exceed 5");
            }
            if (page <= 0)
            {
                return Json("The Page number cant be 0 ");
            }
            var x = _ctx.GetProductsPagedFilteredByBrand(page, pageobjectcount, brand, out var totalEntitys, out var totalPages, out var nextPage);

            if (x != null)
            {
                Request.HttpContext.Response.Headers.Add("X-Total-Count", totalEntitys.ToString());
                Request.HttpContext.Response.Headers.Add("X-Total-Pages", totalPages.ToString());

                if (nextPage == 0)
                {
                    Request.HttpContext.Response.Headers.Add("X-Next-Pages", "Last Page");
                }
                else
                {
                    Request.HttpContext.Response.Headers.Add("X-Next-Pages", nextPage.ToString());
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
            var find = _ctx.GetProductsById(prod.ProductId);

            if (find != null)
            {
                var x = _ctx.AddProduct(prod);
                if (x != null)
                {
                    return Json(Ok(x));

                }
                else
                {
                    return Json("Ops.. Product cant be added");
                }
            }
            else
            {
                return Json(StatusCode(409, "Product already exists in the database "));
            }
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public JsonResult Put(int id, [FromBody]Products prod)
        {
            var x = _ctx.UpdateProduct(id, prod);

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
            var x = _ctx.DeleteProduct(id);
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
