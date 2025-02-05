using Microsoft.AspNetCore.Mvc;
using CarritoComprasAPI.Models;

[ApiController]
[Route("api/carrito")]
public class CarritoController : ControllerBase
{
    private readonly ICarritoService _carritoService;
    public CarritoController(ICarritoService carritoService) { _carritoService = carritoService; }

    [HttpGet]
    public async Task<IActionResult> GetCarritos() => Ok(await _carritoService.GetCarritos());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCarrito(int id)
    {
        var carrito = await _carritoService.GetCarrito(id);
        if (carrito == null) return NotFound();
        return Ok(carrito);
    }

    [HttpPost("{carritoId}/agregar/{productoId}/{cantidad}")]
    public async Task<IActionResult> AgregarProducto(int carritoId, int productoId, int cantidad)
    {
        try
        {
            var carrito = await _carritoService.AgregarProducto(carritoId, productoId, cantidad);
            return Ok(carrito);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }



    [HttpPut("{carritoId}/modificar/{productoId}")]
    public async Task<IActionResult> ModificarCantidad(int carritoId, int productoId, [FromQuery] int cantidad)
    {
        try
        {
            var carrito = await _carritoService.ModificarCantidad(carritoId, productoId, cantidad);
            return Ok(carrito);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }



    [HttpDelete("{carritoId}/eliminar/{productoId}")]
    public async Task<IActionResult> EliminarProducto(int carritoId, int productoId)
    {
        var carrito = await _carritoService.EliminarProducto(carritoId, productoId);
        return Ok(carrito);
    }

    [HttpDelete("{carritoId}/vaciar")]
    public async Task<IActionResult> VaciarCarrito(int carritoId)
    {
        var resultado = await _carritoService.VaciarCarrito(carritoId);
        return resultado ? Ok() : BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> CrearCarrito([FromBody] Carrito nuevoCarrito)
    {
        var carrito = await _carritoService.CrearCarrito(nuevoCarrito);
        return CreatedAtAction(nameof(GetCarrito), new { id = carrito.Id }, carrito);
    }

}
