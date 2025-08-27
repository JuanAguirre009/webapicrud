using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiProducto.Data;
using WebApiProducto.DTOs;
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
        public async Task<ActionResult<Producto>> post(ProdructoForCreateDTO productoCreateDto)
{
        // Mapear DTO → Entidad
        var producto = new Producto
        {
            Nombre = productoCreateDto.Nombre,
            Descripcion = productoCreateDto.Descripcion,
            Precio = productoCreateDto.Precio,
            FechaDeAlta = DateTime.Now,
            Activo = true
        };

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        // Mapear Entidad → DTO de respuesta
        var productoDto = new Producto
        {
        Id = producto.Id,
        Nombre = producto.Nombre,
        Precio = producto.Precio,
        Activo = producto.Activo
        };

    return new CreatedAtRouteResult("GetProducto", new { id = producto.Id }, productoDto);
}
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, ProductoUpdateDto productoUpdateDto)
{
    var producto = await _context.Productos.FindAsync(id);

        if (producto == null)
    {
        return NotFound();
    }

        // Mapear DTO → Entidad (actualizar solo lo permitido)
        producto.Nombre = productoUpdateDto.Nombre;
        producto.Descripcion = productoUpdateDto.Descripcion;
        producto.Precio = productoUpdateDto.Precio;
        producto.Activo = productoUpdateDto.Activo;

        await _context.SaveChangesAsync();

        return NoContent();
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