using System.ComponentModel.DataAnnotations.Schema;

namespace MisterS.Services.OrderAPI.Models
{
    public class OrderDetails
    {
        public int OrderDetailsId { get; set; }

        public int OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public virtual OrderHeader? OrderHeader { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Count { get; set; }
    }
}
