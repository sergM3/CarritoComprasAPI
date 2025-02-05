using CarritoComprasAPI.Models;

public interface ICarritoService
{
    Task<List<Carrito>> GetCarritos();
    Task<Carrito?> GetCarrito(int id);
    Task<Carrito> AgregarProducto(int carritoId, int productoId, int cantidad);
    Task<Carrito> ModificarCantidad(int carritoId, int productoId, int cantidad);
    Task<Carrito> EliminarProducto(int carritoId, int productoId);
    Task<bool> VaciarCarrito(int carritoId);
    Task<Carrito> CrearCarrito(Carrito carrito);

}

