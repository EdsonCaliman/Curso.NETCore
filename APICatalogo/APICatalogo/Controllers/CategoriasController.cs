using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {

        private readonly IUnitOfWork _uof;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CategoriasController(IUnitOfWork uof, ILogger<CategoriasController> logger, IMapper mapper)
        {
            _uof = uof;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAll()
        {
            var categorias = await _uof.CategoriaRepository.Get().ToListAsync();
            var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);

            return categoriasDTO;
        }

        [HttpGet("produtos")]
        [HttpGet("/produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
        {
            _logger.LogInformation("============GET api/categorias/produtos==================");

            var categorias = await _uof.CategoriaRepository.GetCategoriasProdutos();
            var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);

            return categoriasDTO;
        }

        /// <summary>
        /// Obtem uma Categoria pelo seu Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retorna uma categoria</returns>
        [ProducesResponseType(typeof(CategoriaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id:min(1)}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> GetId(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetById(x => x.CategoriaId == id);

            if (categoria == default)
            {
                return NotFound();
            }

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return categoriaDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoriaDTO categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);

            _uof.CategoriaRepository.Add(categoria);
            await _uof.Commit();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDTO);
        }

        [HttpPut("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Put))]
        public async Task<ActionResult> Put(int id, [FromBody] CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                return BadRequest();
            }

            var categoria = _mapper.Map<Categoria>(categoriaDto);

            _uof.CategoriaRepository.Update(categoria);
            await _uof.Commit();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetById(cat => cat.CategoriaId == id);

            if (categoria == default)
            {
                return NotFound();
            }

            _uof.CategoriaRepository.Delete(categoria);
            await _uof.Commit();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return categoriaDTO;
        }

    }
}
