using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Website.Extensions;

namespace Website.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;
        private readonly IDistributedCache _cache;

        public CatalogController(
            IProductRepository repository,
            ILogger<CatalogController> logger,
            IDistributedCache cache)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cache = cache;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                var products = await _repository.GetAll();

                if (products == null)
                {
                    return NotFound(nameof(products));
                }

                return Ok(products);
            }
            catch (Exception e)
            {
                _logger.LogError($"[CatalogController] GetProducts: {e}");
                //return Internal Server Error
                return StatusCode(500);
            }
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            try
            {
                var cachedProduct = await _cache.GetAsync<Product>(id);
                _logger.LogInformation($"GetProduct from cache: {cachedProduct?.Id ?? "None"}");

                var product =  cachedProduct ?? await _repository.Get(id);

                if (product == null)
                {
                    _logger.LogError($"Product with id: {id}, not found.");
                    return NotFound();
                }

                await _cache.SetAsync(id, product, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(7));
                return Ok(product);
            }
            catch (Exception e)
            {
                _logger.LogError($"[CatalogController] GetProductById: {e}");
                return StatusCode(500);
            }
        }

        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            try
            {
                var products =  string.IsNullOrEmpty(category) 
                ? await _repository.GetAll()
                : await _repository.GetProductByCategory(category);
                return Ok(products);
            }
            catch (Exception e)
            {
                _logger.LogError($"[CatalogController] GetProductById: {e}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            if (product == null) return BadRequest();

            try
            {
                await _repository.Create(product);
                await _cache.SetAsync(product.Id, product, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(7));
                _logger.LogInformation($"[CatalogController] Created Product with key {product.Id}");
            }
            catch (Exception e)
            {
                _logger.LogError($"[CatalogController] CreateProduct: {e}");
                return StatusCode(500);
            }

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            if (product == null) return BadRequest();

            return Ok(await _repository.Update(product));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            if (id == null) return BadRequest();
            await _cache.RemoveAsync(id);

            return Ok(await _repository.Delete(id));
        }
    }
}