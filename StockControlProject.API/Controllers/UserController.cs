using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockControlProject.Entities.Entities;
using StockControlProject.Repositories.Abstract;

namespace StockControlProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGenericRepository<User> _service;

        public UserController(IGenericRepository<User> service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Login(string email, string parola)
        {
            if (_service.Any(x => x.Email.Equals(email) && x.Password.Equals(parola)))
            {
                User user = _service.GetByDefault(x => x.Email.Equals(email) && x.Password == parola);
                return Ok(user);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult IdyeGoreKullaniciGetir(int id)
        {
            return Ok(_service.GetById(id));
        }


        [HttpPost]
        public IActionResult KullaniciEkle(User user)
        {
            if (_service.Add(user))
            {
                return CreatedAtAction("IdyeGoreKullaniciGetir", new { id = user.Id }, user);
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult TumKullanicilariGetir()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet]
        public IActionResult AktifKullanicilariGetir()
        {
            return Ok(_service.GetActive());
        }

        [HttpPut("{id}")]
        public IActionResult KullanicilariGuncelle(int id, User user)
        {
            if (id != user.Id) return NotFound();
            try
            {
                if (_service.Update(user)) return Ok(user);
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult KullaniciSil(int id)
        {
            User user = _service.GetById(id);
            if (user is null) return NotFound();
            if (_service.Remove(user))
                return Ok("Kayıt Silindi");
            return BadRequest();
        }

        [HttpGet("{id}")]
        public IActionResult KullaniciAktiflestir(int id)
        {
            User user = _service.GetById(id);
            if (user is null) return NotFound();
            _service.Activate(id);
            return Ok(user);
        }
    }
}
