using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrunlerController : ControllerBase
    {
        private readonly IUrunService _urunService;

        public UrunlerController(IUrunService urunService)
        {
            _urunService = urunService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var urunler = _urunService.Query().ToList();
            return Ok(urunler);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id) //~/api/Urunler/1
        {
            var urun = _urunService.Query().SingleOrDefault(u => u.Id == id);
            if (urun == null)
                return NotFound(); //404
            return Ok(urun);
        }
        [HttpPost]
        public IActionResult Post(UrunModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _urunService.Add(model);
                if (result.IsSuccessful)
                {
                    return Ok(model);
                }
                ModelState.AddModelError("", result.Message);
            }
            return BadRequest(ModelState);
            //return StatusCode(500, ModelState); Internal Server Error
        }
        [HttpPut]
        public IActionResult Put(UrunModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _urunService.Update(model);
                if (result.IsSuccessful)
                {
                    return Ok(model);
                }
                ModelState.AddModelError("", result.Message);
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _urunService.Delete(id);
            return NoContent();

        }
    }
}
