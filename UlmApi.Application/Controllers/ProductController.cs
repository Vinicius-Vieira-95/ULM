using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UlmApi.Application.Models;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Interfaces;
using UlmApi.Infra.CrossCutting.Logger;

namespace UlmApi.Application.Controllers
{
    [Authorize]
    [Route("api/v1/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IBaseService<Product, int> _baseProductService;
        private readonly ILoggerManager _logger;

        public ProductController(IBaseService<Product, int> baseProductService, ILoggerManager logger)
        {
            _baseProductService = baseProductService;
            _logger = logger;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> Get()
        {
            var products = await _baseProductService.Get<ProductModel>();
            _logger.LogInfo("Get Products");
            return Ok(products);
        }

    }
}
