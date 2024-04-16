using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockControlProject.Entities.Entities;
using StockControlProject.Service.Abstract;

namespace StockControlProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericService<Category> _service;

        public CategoryController(IGenericService<Category> service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult TumKategorileriGetir()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet]
        public IActionResult AktifKategorileriGetir()
        {
            return Ok(_service.GetActive());
        }

        [HttpGet("{id}")]
        public IActionResult IdyeGoreKategorileriListele(int id)
        {
            return Ok(_service.GetById(id));
        }

        [HttpPost]
        public IActionResult KategoriEkle(Category category)
        {
            _service.Add(category);
            return CreatedAtAction("IdyeGoreKategorileriListele", new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public IActionResult KategoriGuncelle(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }
            try
            {
                if (_service.Update(category))
                    return Ok(category);
                else
                    return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExist(id))
                    return NotFound();
            }
            return NoContent();
        }

        private bool CategoryExist(int id)
        {
            return _service.Any(x => x.Id == id);
        }

        [HttpDelete("{id}")]
        public IActionResult KategoriSil(int id)
        {
            var category = _service.GetById(id);
            if (category is null) return NotFound();
            try
            {
                _service.Remove(category);
                return Ok("Kategori Silindi");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpGet("{id}")]
        public IActionResult KategoriAktifleştir(int id)
        {
            var category=_service.GetById(id);  
            if (category is null) return NotFound();
            try
            {
                _service.Activate(id);
                return Ok(category);    
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
