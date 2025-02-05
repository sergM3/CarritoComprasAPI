namespace CarritoComprasAPI.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarritoComprasAPI.Models;

public class CarritoProducto
{
    public int CarritoId { get; set; }
    public Carrito Carrito { get; set; } = null!;

    public int ProductoId { get; set; }
    public Producto Producto { get; set; } = null!;

    public int Cantidad { get; set; }
}
