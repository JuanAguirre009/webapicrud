using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiProducto.Data;
using WebApiProducto.Models;

namespace WebApiProducto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class productosController : ControllerBase
    {
        private readonly ILogger<productosController> _logger;
        private readonly DataContext _context;

        public productosController(ILogger<productosController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet(Name = "GetProductos")]
        public async Task<ActionResult<List<Producto>>> Get()
        {
            return Ok(await _context.Productos.ToListAsync());
        }
        [HttpGet("{id}", Name = "GetProducto")]
        public async Task<ActionResult<Producto>> Get(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound("El producto no fue encontrado");
            }
            return producto;
        }
        [HttpPost]
        public async Task<ActionResult<Producto>> post(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return new CreatedAtRouteResult("GetProducto", new { id = producto.Id }, producto);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> put(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest("El id del producto no coincide");
            }
            _context.Entry(producto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Producto>> delete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound("El producto no fue encontrado");
            }
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return producto;
        }
        
    }
}