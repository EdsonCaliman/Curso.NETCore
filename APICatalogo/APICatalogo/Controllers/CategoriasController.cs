using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace APICatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {

        private readonly IUnitOfWork _uof;
        private readonly ILogger _logger;

        public CategoriasController(IUnitOfWork uof, ILogger<CategoriasController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> GetAll()
        {
            return _uof.CategoriaRepository.Get().ToList();
        }

        [HttpGet("produtos")]
        [HttpGet("/produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            _logger.LogInformation("============GET api/categorias/produtos==================");

            return _uof.CategoriaRepository.GetCategoriasProdutos().ToList();
        }

        [HttpGet("{id:min(1)}", Name = "ObterCategoria")]
        public ActionResult<Categoria> GetId(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(x => x.CategoriaId == id);

            if (categoria == default)
            {
                return NotFound();
            }
            return categoria;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            _uof.CategoriaRepository.Add(categoria);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();
            return Ok();

        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(cat => cat.CategoriaId == id);

            if (categoria == default)
            {
                return NotFound();
            }

            _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();
            return categoria;
        }

    }
}
