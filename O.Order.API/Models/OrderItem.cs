using System.ComponentModel.DataAnnotations.Schema;

namespace O.Order.API.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        [Column(TypeName ="decimal(18,2)")] // toplam 16 basamak virgülden önce 2 basamak sonra
        public decimal Price { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int Count { get; set; }
    }
}
