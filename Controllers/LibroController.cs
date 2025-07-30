using HolaMundoWebAPI.Datos;
using HolaMundoWebAPI.DTOs;
using HolaMundoWebAPI.Entidades;
using HolaMundoWebAPI.Utilidad;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HolaMundoWebAPI.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibroController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapeos mapper;

        public LibroController(ApplicationDbContext context, IMapeos mapper )
        {
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IEnumerable<LibroDTO>> Get()
        {
            var libros = await context.Libros.ToListAsync();
            return this.mapper.MapeoArrayLibroALibroDto(libros);
        }

        [HttpGet("{id:int}", Name = "ObtenerLibro")]
        public async Task<ActionResult<LibroConAutorDTO>> Get(int id)
        {
            var libroEncontrado = await context.Libros
                .Include(x => x.Autores)
                .ThenInclude(a => a.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (libroEncontrado is null)
            {
                return NotFound("No existe un libro con ese id");
            }

            return this.mapper.MapeoLibroALibroConAutorDTO(libroEncontrado);

        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroDto)
        {
            if(libroDto.AutoresIds is null || libroDto.AutoresIds.Count == 0)
            {
                ModelState.AddModelError(nameof(LibroCreacionDTO.AutoresIds), "No se puede crear un Libro sin autores");
                return ValidationProblem();
            }
            var autoresIdsExisten = await context.Autores.Where(x => libroDto.AutoresIds.Contains(x.Id))
                .Select(x=> x.Id).ToListAsync();

            if(autoresIdsExisten.Count != libroDto.AutoresIds.Count)
            {
                var autoresNoExisten = libroDto.AutoresIds.Except(autoresIdsExisten);
                var autoresNoEsistenString = string.Join(",", autoresIdsExisten);
                var mensajeDeError = $"Los siguientes autores no existen: {autoresNoEsistenString}";
                ModelState.AddModelError(nameof(libroDto.AutoresIds), mensajeDeError);
                return ValidationProblem();
            }

            var libro = this.mapper.MapeoLibroCreacionDtoALibro(libroDto);
            AsignarOrdenAutores(libro);
            context.Libros.Add(libro);
            await context.SaveChangesAsync();
            var libroDtoReturn = this.mapper.MapeoLibroTbALibroDto(libro);
            return CreatedAtRoute("ObtenerLibro", new { id = libro.Id }, libroDto);
        }

        private void AsignarOrdenAutores(Libro libro) {
            if(libro.Autores is not null)
            {
                for (int i = 0; i < libro.Autores.Count; i++)
                {
                    libro.Autores[i].Orden = i;
                }
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, LibroCreacionDTO libroDto)
        {
            if (libroDto.AutoresIds is null || libroDto.AutoresIds.Count == 0)
            {
                ModelState.AddModelError(nameof(LibroCreacionDTO.AutoresIds), "No se puede crear un Libro sin autores");
                return ValidationProblem();
            }

            var autoresIdsExisten = await context.Autores.Where(x => libroDto.AutoresIds.Contains(x.Id))
                .Select(x => x.Id).ToListAsync();

            if (autoresIdsExisten.Count != libroDto.AutoresIds.Count)
            {
                var autoresNoExisten = libroDto.AutoresIds.Except(autoresIdsExisten);
                var autoresNoEsistenString = string.Join(",", autoresIdsExisten);
                var mensajeDeError = $"Los siguientes autores no existen: {autoresNoEsistenString}";
                ModelState.AddModelError(nameof(libroDto.AutoresIds), mensajeDeError);
                return ValidationProblem();
            }

            // var libro = this.mapper.MapeoLibroCreacionDtoALibro(libroDto);
            var libroDB = await this.context.Libros
                .Include(x => x.Autores)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (libroDB is null)
            {
                return NotFound();
            }
            this.mapper.MapLibroCreacionDtoToLibro(libroDto, libroDB);
            AsignarOrdenAutores(libroDB);
            context.Libros.Update(libroDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var registroBorrados = await context.Libros.Where(x => x.Id == id).ExecuteDeleteAsync();
            if(registroBorrados == 0)
            {
                return NotFound();
            }
            return NoContent();
        }
        
    }
}
