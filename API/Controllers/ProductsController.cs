using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Helpers;

namespace API.Controllers
{
    public class ProductsController : BaseApiController 
    {
        private readonly IGenericRepository<Product> _productsRepo;

        private readonly IGenericRepository<ProductType> _productTypeReop;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        public IMapper _mapper { get; }

        public ProductsController(IGenericRepository<Product> productsRepo,
        IGenericRepository<ProductBrand>productBrandRepo,
        IGenericRepository<ProductType> productTypeReop,IMapper mapper)
        {
            _mapper = mapper;
            _productTypeReop = productTypeReop;
            _productBrandRepo = productBrandRepo;
            _productsRepo = productsRepo;
        }


    
        [HttpGet]    
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery]ProductSpecParams productParams)
            {

            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);

            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _productsRepo.CountAsync(countSpec);

            var products = await _productsRepo.ListAsync(spec);
            
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);


            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize,totalItems,data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id){
           
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product =  await _productsRepo.GetEntityWithSpec(spec);
            if (product == null) return NotFound(new ApiResponse(404));
            return _mapper.Map<Product , ProductToReturnDto>(product);
        }


        [HttpGet("brands")] 

        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepo.ListAllAsync());

        }
         
        [HttpGet("types")] 

        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeReop.ListAllAsync()); 
        }
            
    }
}