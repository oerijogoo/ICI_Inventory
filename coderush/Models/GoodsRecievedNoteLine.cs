using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    public class GoodsRecievedNoteLine
    {
        public int GoodsRecievedNoteLineId { get; set; }
        [Display(Name = "Goods Received Note")]
        public int GoodsReceivedNoteId { get; set; }
        [Display(Name = "Purchase Order")]
        public GoodsReceivedNote GoodsReceivedNote { get; set; }
        [Display(Name = "Product Item")]
        public int ProductId { get; set; }
        public string Description { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Amount { get; set; }
        [Display(Name = "Disc %")]
        public double DiscountPercentage { get; set; }
        public double DiscountAmount { get; set; }
        public double SubTotal { get; set; }
        [Display(Name = "Tax %")]
        public double TaxPercentage { get; set; }
        public double TaxAmount { get; set; }
        public double Total { get; set; }
        public string BatchID { get; set; }
        public DateTime ManufareDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
