using AutoMapper;
using E_Commerce_Backend.Model;
using E_Commerce_Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Backend.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductController(
            IProductRepository productRepository,
            IMapper mapper)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var productEntities = await _productRepository.GetProductsAsync();

            return Ok(_mapper.Map<IEnumerable<ProductDto>>(productEntities));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var productEntity = await _productRepository.GetProductAsync(id);

            if (productEntity == null)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<Model.ProductDto>(productEntity));
        }
        
    }
}
