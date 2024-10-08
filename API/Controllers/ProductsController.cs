using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        public StoreContext _context { get; }

        public ProductsController(StoreContext context) 
        {
            _context = context;
            
        }


    
        [HttpGet]    
        public async Task<ActionResult<List<Product>>> GetProducts(){

            var products = _context.Products.ToListAsync();


            return await products ;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id){
            return await _context.Products.FindAsync(id);
        }
            
    }
}