using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockControlProject.Entities.Entities;
using StockControlProject.Service.Abstract;

namespace StockControlProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericService<Product> _service;

        public ProductController(IGenericService<Product> service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult TumUrunleriGetir()
        {
            return Ok(_service.GetAll(x => x.Category, y => y.Supplier));
        }

        [HttpGet]
        public IActionResult AktifUrunleriGetir()
        {
            var activeProducts = _service.GetActive(x => x.Supplier, y => y.Category);
            return Ok(activeProducts);
        }

        [HttpGet("{id}")]
        public IActionResult IdyeGoreUrunleriGetir(int id)
        {
            return Ok(_service.GetById(id));
        }

        [HttpPost]
        public IActionResult UrunEkle(Product product)
        {
            _service.Add(product);
            return CreatedAtAction("IdyeGoreUrunleriGetir", new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult UrunGuncelle(int id, Product product)
        {
            if (id != product.Id) return BadRequest();
            try
            {
                if (_service.Update(product))
                {
                    return Ok(product);
                }
                else return NotFound();
            }
            catch (Exception)
            {
                if (!ProductExist(id))
                {
                    return NotFound();
                }
            }
            return NoContent();
        }

        private bool ProductExist(int id)
        {
            return _service.Any(x => x.Id == id);
        }


        [HttpDelete("{id}")]
        public IActionResult UrunSil(int id)
        {
            Product p=_service.GetById(id);
            if (p is null) return NotFound();
            try
            {
                _service.Remove(p);
                return Ok("Ürün Silindi");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult UrunAktiflestir(int id)
        {
            Product p=_service.GetById(id); 
            if (p is null) return NotFound();
            try
            {
                _service.Activate(id);
                return Ok(p);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
