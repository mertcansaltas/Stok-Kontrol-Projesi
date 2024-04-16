using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockControlProject.Entities.Entities;
using StockControlProject.Entities.Enums;
using StockControlProject.Service.Abstract;

namespace StockControlProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IGenericService<Order> orderservice;
        private readonly IGenericService<OrderDetail> odservice;
        private readonly IGenericService<Product> productservice;

        public OrderController(IGenericService<Order> orderservice, IGenericService<OrderDetail> orderdetailservice, IGenericService<Product> productservice)
        {
            this.orderservice = orderservice;
            this.odservice = orderdetailservice;
            this.productservice = productservice;
        }

        [HttpGet]
        public IActionResult TumSiparisleriGetir()
        {
            return Ok(orderservice.GetAll(x => x.OrderDetails, y => y.User));
        }

        [HttpGet]
        public IActionResult AktifSiparisleriGetir()
        {
            return Ok(orderservice.GetActive());
        }

        [HttpGet("{id}")]
        public IActionResult IdyeGoreSiparisleriGetir(int id)
        {
            return Ok(orderservice.GetById(id, x => x.OrderDetails, y => y.User));
        }

        [HttpGet("{id}")]
        public IActionResult IdyeGoreSiparisDetaylariniGetir(int id)
        {
            return Ok(odservice.GetAll(x => x.OrderId == id, y => y.Product));
        }

        [HttpGet]
        public IActionResult BekleyenSiparisleriGetir()
        {
            return Ok(orderservice.GetDefault(x => x.Status == Status.Pending));
        }

        [HttpGet]
        public IActionResult OnaylananSiparisleriGetir()
        {
            return Ok(orderservice.GetDefault(x => x.Status == Status.Confirmed));
        }

        [HttpGet]
        public IActionResult ReddedilenSiparisleriGetir()
        {
            return Ok(orderservice.GetDefault(x => x.Status == Status.Cancelled));
        }

        [HttpPost]
        public IActionResult SiparisEkle(int userId, [FromQuery] int[] productIDs, [FromQuery] short[] quantityies)
        {
            Order yeniSiparis = new Order();
            yeniSiparis.UserId = userId;
            yeniSiparis.Status = Status.Pending;
            yeniSiparis.isActive = true;
            orderservice.Add(yeniSiparis);
            for (int i = 0; i < productIDs.Length; i++)
            {
                OrderDetail yeniDetay = new OrderDetail();
                yeniDetay.OrderId = yeniSiparis.Id;
                yeniDetay.ProductId = productIDs[i];
                yeniDetay.Quantity = quantityies[i];
                yeniDetay.UnitPrice = productservice.GetById(productIDs[i]).UnitPrice * yeniDetay.Quantity;
                yeniDetay.isActive = true;
                odservice.Add(yeniDetay);
            }
            return Ok(yeniSiparis);
        }

        [HttpGet("{id}")]
        public IActionResult SiparisiOnayla(int id)
        {
            Order order = orderservice.GetById(id);
            if (order is null) return NotFound();
            else
            {
                List<OrderDetail> detaylar = odservice.GetDefault(x => x.OrderId == order.Id);
                foreach (OrderDetail item in detaylar)
                {
                    Product productInOrder = productservice.GetById(item.ProductId); //Kişi Hangi ürünü istemiş
                    if (productInOrder.Stock >= item.Quantity)
                    {
                        productInOrder.Stock -= item.Quantity;
                        productservice.Update(productInOrder);
                    }
                    else return BadRequest();
                }
                order.Status = Status.Confirmed;
                order.isActive = false;
                orderservice.Update(order);
                return Ok(order);
            }
        }

        [HttpGet("{id}")]
        public IActionResult SiparisiReddet(int id)
        {
            Order canceledOrder = orderservice.GetById(id);
            if (canceledOrder is null) return NotFound();
            else
            {
                canceledOrder.Status = Status.Cancelled;
                canceledOrder.isActive = false; 
                orderservice.Update(canceledOrder);
                return Ok(canceledOrder);
            }
        }



    }
}
