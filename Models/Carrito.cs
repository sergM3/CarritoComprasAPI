namespace CarritoComprasAPI.Models;
using CarritoComprasAPI.Models;

public class Carrito
{
    public int Id { get; set; }
    public string Usuario { get; set; } = string.Empty;
    
    public List<CarritoProducto> ProductosAgregados { get; set; } = new();
    
    public decimal Total => ProductosAgregados.Sum(cp => cp.Producto.Precio * cp.Cantidad);
}

