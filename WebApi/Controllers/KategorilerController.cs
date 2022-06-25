using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
    using DataAccess.Contexts;
    using DataAccess.Entities;
using Business.Services.Bases;
using Business.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KategorilerController : ControllerBase
    {
        // Add service injections here
        private readonly IKategoriService _kategoriService;

        public KategorilerController(IKategoriService kategoriService)
        {
            _kategoriService = kategoriService;
        }

        // GET: api/Kategoriler
        [HttpGet]
        public IActionResult Get()
        {
            List<KategoriModel> kategoriList = _kategoriService.Query().ToList(); // TODO: Add get list service logic here
            return Ok(kategoriList);
        }

        // GET: api/Kategoriler/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Kategori kategori = null; // TODO: Add get item service logic here
			if (kategori == null)
            {
                return NotFound();
            }
			return Ok(kategori);
        }

		// POST: api/Kategoriler
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult Post(Kategori kategori)
        {
            // TODO: Add insert service logic here
			return CreatedAtAction("Get", new { id = kategori.Id }, kategori);
        }

        // PUT: api/Kategoriler
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public IActionResult Put(Kategori kategori)
        {
            // TODO: Add update service logic here
            return NoContent();
        }

        // DELETE: api/Kategoriler/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // TODO: Add delete service logic here
            return NoContent();
        }
	}
}
