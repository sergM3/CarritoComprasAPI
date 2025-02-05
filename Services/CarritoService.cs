using CarritoComprasAPI.Models;
using CarritoComprasAPI.Data;
using Microsoft.EntityFrameworkCore;

public class CarritoService : ICarritoService
{
    private readonly CarritoContext _context;

    public CarritoService(CarritoContext context) { _context = context; }

    public async Task<List<Carrito>> GetCarritos() =>
        await _context.Carritos.Include(c => c.ProductosAgregados)
                               .ThenInclude(cp => cp.Producto)
                               .ToListAsync();

    public async Task<Carrito?> GetCarrito(int id)
    {
        try
        {
            Console.WriteLine($"Buscando carrito con ID: {id}");
            var carrito = await _context.Carritos
                .Include(c => c.ProductosAgregados)
                .ThenInclude(cp => cp.Producto)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (carrito == null)
            {
                Console.WriteLine($"Carrito con ID {id} no encontrado.");
                return null;
            }

            Console.WriteLine($"Carrito encontrado: {carrito.Id} con {carrito.ProductosAgregados.Count} productos.");
            return carrito;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en GetCarrito: {ex.Message}");
            throw new Exception("Error al obtener el carrito");
        }
    }

    public async Task<Carrito> AgregarProducto(int carritoId, int productoId, int cantidad)
    {
        var carrito = await _context.Carritos.Include(c => c.ProductosAgregados)
                                             .FirstOrDefaultAsync(c => c.Id == carritoId);
        if (carrito == null) throw new Exception("Carrito no encontrado");

        var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == productoId);
        if (producto == null) throw new Exception("Producto no encontrado");

        var carritoProducto = await _context.CarritoProductos
            .FirstOrDefaultAsync(cp => cp.CarritoId == carritoId && cp.ProductoId == productoId);

        if (carritoProducto != null)
        {
            carritoProducto.Cantidad += cantidad;
        }
        else
        {
            carritoProducto = new CarritoProducto
            {
                CarritoId = carritoId,
                ProductoId = productoId,
                Cantidad = cantidad
            };
            _context.CarritoProductos.Add(carritoProducto);
        }

        await _context.SaveChangesAsync();
        return carrito;
    }


    public async Task<Carrito> ModificarCantidad(int carritoId, int productoId, int cantidad)
    {
        var carrito = await _context.Carritos.Include(c => c.ProductosAgregados)
                                              .ThenInclude(cp => cp.Producto)
                                              .FirstOrDefaultAsync(c => c.Id == carritoId);
        if (carrito == null) throw new Exception("Carrito no encontrado");

        var carritoProducto = carrito.ProductosAgregados.FirstOrDefault(cp => cp.ProductoId == productoId);
        if (carritoProducto == null) throw new Exception("Producto no encontrado en el carrito");

        if (cantidad <= 0)
        {
            carrito.ProductosAgregados.Remove(carritoProducto);
        }
        else
        {
            carritoProducto.Cantidad = cantidad;
        }

        await _context.SaveChangesAsync();
        return carrito;
    }

    public async Task<Carrito> EliminarProducto(int carritoId, int productoId)
    {
        var carrito = await _context.Carritos.Include(c => c.ProductosAgregados)
                                              .FirstOrDefaultAsync(c => c.Id == carritoId);
        if (carrito == null) throw new Exception("Carrito no encontrado");

        var carritoProducto = carrito.ProductosAgregados.FirstOrDefault(cp => cp.ProductoId == productoId);
        if (carritoProducto == null) throw new Exception("Producto no encontrado en el carrito");

        carrito.ProductosAgregados.Remove(carritoProducto);

        await _context.SaveChangesAsync();
        return carrito;
    }

    public async Task<bool> VaciarCarrito(int carritoId)
    {
        var carrito = await _context.Carritos.Include(c => c.ProductosAgregados)
                                              .FirstOrDefaultAsync(c => c.Id == carritoId);
        if (carrito == null) throw new Exception("Carrito no encontrado");

        carrito.ProductosAgregados.Clear();
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Carrito> CrearCarrito(Carrito carrito)
    {
        _context.Carritos.Add(carrito);
        await _context.SaveChangesAsync();
        return carrito;
    }
}
