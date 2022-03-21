using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using coderush.Data;
using coderush.Models;
using coderush.Models.SyncfusionViewModels;
using Microsoft.AspNetCore.Authorization;

namespace coderush.Controllers.Api
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/PurchaseOrderLine")]
    public class PurchaseOrderLineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrderLineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PurchaseOrderLine
        [HttpGet]
        public async Task<IActionResult> GetPurchaseOrderLine()
        {
            var headers = Request.Headers["PurchaseOrderId"];
            int purchaseOrderId = Convert.ToInt32(headers);
            List<PurchaseOrderLine> Items = await _context.PurchaseOrderLine
                .Where(x => x.PurchaseOrderId.Equals(purchaseOrderId))
                .ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        private PurchaseOrderLine Recalculate(PurchaseOrderLine purchaseOrderLine)
        {
            try
            {
                purchaseOrderLine.Amount = /*purchaseOrderLine.Quantity * */ purchaseOrderLine.Price;
                purchaseOrderLine.DiscountAmount = (purchaseOrderLine.DiscountPercentage * purchaseOrderLine.Amount) / 100.0;
                purchaseOrderLine.SubTotal = purchaseOrderLine.Amount - purchaseOrderLine.DiscountAmount;
                purchaseOrderLine.TaxAmount = (purchaseOrderLine.TaxPercentage * purchaseOrderLine.SubTotal) / 100.0;
                purchaseOrderLine.Total = purchaseOrderLine.SubTotal + purchaseOrderLine.TaxAmount;

            }
            catch (Exception)
            {

                throw;
            }

            return purchaseOrderLine;
        }

        private void UpdateStock(int productId)
        {
            try
            {
                Stock stock = new Stock();
                stock = _context.Stock
                    .Where(x => x.ProductId.Equals(productId))
                    .FirstOrDefault();

                if (stock != null)
                {
                    List<PurchaseOrderLine> line = new List<PurchaseOrderLine>();
                    line = _context.PurchaseOrderLine.Where(x => x.ProductId.Equals(productId)).ToList();

                    //update master data by its lines

                    stock.TotalSales = line.Sum(x => x.Quantity);

                    if (stock.TotalSales > stock.TotalReceived)
                    {
                        stock.StockDevicit = stock.TotalSales - stock.TotalReceived;
                        stock.InStock = 0;
                    }
                    else
                    {
                        stock.InStock = stock.TotalReceived - stock.TotalSales;
                        stock.StockDevicit = 0;
                    }
                    _context.Update(stock);

                    _context.SaveChanges();
                }


            }
            catch (Exception)
            {

                throw;
            }
        }


        private void UpdatePurchaseOrder(int purchaseOrderId)
        {
            try
            {
                PurchaseOrder purchaseOrder = new PurchaseOrder();
                purchaseOrder = _context.PurchaseOrder
                    .Where(x => x.PurchaseOrderId.Equals(purchaseOrderId))
                    .FirstOrDefault();

                if (purchaseOrder != null)
                {
                    List<PurchaseOrderLine> lines = new List<PurchaseOrderLine>();
                    lines = _context.PurchaseOrderLine.Where(x => x.PurchaseOrderId.Equals(purchaseOrderId)).ToList();

                    //update master data by its lines
                    purchaseOrder.Amount = lines.Sum(x => x.Amount);
                    purchaseOrder.SubTotal = lines.Sum(x => x.SubTotal);
                    
                    purchaseOrder.Discount = lines.Sum(x => x.DiscountAmount);
                    purchaseOrder.Tax = lines.Sum(x => x.TaxAmount);

                    purchaseOrder.Total = purchaseOrder.Freight + lines.Sum(x => x.Total);

                    _context.Update(purchaseOrder);

                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<PurchaseOrderLine> payload)
        {
            PurchaseOrderLine purchaseOrderLine = payload.value;
            purchaseOrderLine = this.Recalculate(purchaseOrderLine);
            _context.PurchaseOrderLine.Add(purchaseOrderLine);
            _context.SaveChanges();
            this.UpdatePurchaseOrder(purchaseOrderLine.PurchaseOrderId);
            this.UpdateStock(purchaseOrderLine.ProductId);
            return Ok(purchaseOrderLine);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<PurchaseOrderLine> payload)
        {
            PurchaseOrderLine purchaseOrderLine = payload.value;
            purchaseOrderLine = this.Recalculate(purchaseOrderLine);
            _context.PurchaseOrderLine.Update(purchaseOrderLine);
            _context.SaveChanges();
            this.UpdatePurchaseOrder(purchaseOrderLine.PurchaseOrderId);
            this.UpdateStock(purchaseOrderLine.ProductId);
            return Ok(purchaseOrderLine);
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<PurchaseOrderLine> payload)
        {
            PurchaseOrderLine purchaseOrderLine = _context.PurchaseOrderLine
                .Where(x => x.PurchaseOrderLineId == (int)payload.key)
                .FirstOrDefault();
            _context.PurchaseOrderLine.Remove(purchaseOrderLine);
            _context.SaveChanges();
            this.UpdatePurchaseOrder(purchaseOrderLine.PurchaseOrderId);
            this.UpdateStock(purchaseOrderLine.ProductId);
            return Ok(purchaseOrderLine);

        }
    }
}