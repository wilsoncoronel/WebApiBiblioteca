using HolaMundoWebAPI.Datos;
using HolaMundoWebAPI.DTOs;
using HolaMundoWebAPI.Utilidad;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HolaMundoWebAPI.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    public class ComentariosController : Controller
    {
        private readonly IMapeos _mapper;
        private readonly ApplicationDbContext _context;

        public ComentariosController(ApplicationDbContext context, IMapeos mapper)
        {
            this._mapper = mapper;
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int libroId)
        {
            var existeLibro = await this._context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro)
            {
                return NotFound();
            }

            var comentarios = await this._context.Comentarios
                .Where(x => x.LibroId == libroId)
                .OrderByDescending(x => x.FechaPublicacion)
                .ToListAsync();
            return _mapper.MapeoArrayComentarioDtoDesdeComentarios(comentarios);
        }

        [HttpGet("{id}", Name = "ObtenerComentario")]
        public async Task<ActionResult<ComentarioDTO>> Get(Guid id)
        {
            var comentario = await this._context.Comentarios.FirstOrDefaultAsync(x => x.Id == id);
            if (comentario is null) {
                return NotFound();
            }
            return this._mapper.MapeoComentarioAComentarioDto(comentario);
        }

        /*[HttpPost]
        public async Task<IActionResult> Post(int libroId, ComentarioCreacionDTO comentarioCreacionDTO) {
            var existeLibro = await this._context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro)
            {
                return NotFound();
            }
            var comentario = _mapper.MapeoComentarioCreacionDtoAComentario(comentarioCreacionDTO);

            comentario.LibroId = libroId;
            comentario.FechaPublicacion = DateTime.UtcNow;
            _context.Add(comentario);
            await _context.SaveChangesAsync();
            var comentarioDTO = _mapper.MapeoComentarioAComentarioDto(comentario);
            return CreatedAtRoute("ObtenerComentario", new { id=comentario.Id, libroId}, comentarioDTO);
        }*/

       /* [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(Guid id,int libroId, JsonPatchDocument<ComentarioPatchDTO> patchDoc)
        {
            if (patchDoc is null)
            {
                return BadRequest();
            }
            var existeLibro = await this._context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro)
            {
                return NotFound();
            }
            var comentarioDb = await _context.Comentarios.FirstOrDefaultAsync(x => x.Id == id);
            if (comentarioDb is null)
            {
                return NotFound();
            }

            var comentarioPatchDto = this._mapper.MapeoComentarioAComentarioPatchDto(comentarioDb);
            patchDoc.ApplyTo(comentarioPatchDto, ModelState);

            var esValido = TryValidateModel(comentarioPatchDto);
            if (!esValido)
            {
                return ValidationProblem();
            }
            this._mapper.MapeoReversoComentarioPatchDtoAComentario(comentarioPatchDto, comentarioDb);
            await _context.SaveChangesAsync();
            return NoContent();
        }*/

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id, int libroId) {
            var existeLibro = await _context.Libros.AnyAsync(x => x.Id==libroId);
            if (!existeLibro)
            {
                return NotFound();
            }
            var registrosBorrados = await _context.Comentarios.Where(x=> x.Id == id).ExecuteDeleteAsync();
            if(registrosBorrados == 0)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
