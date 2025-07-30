using Azure;
using HolaMundoWebAPI.Datos;
using HolaMundoWebAPI.DTOs;
using HolaMundoWebAPI.Entidades;
using HolaMundoWebAPI.Utilidad;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HolaMundoWebAPI.Controllers
{

    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapeos mapper;

        public AutoresController(ApplicationDbContext context, IMapeos mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IEnumerable<AutorDTO>> Get()
        {
            var autores =  await context.Autores.ToListAsync();
            var autoresDto = this.mapper.MapeoArrayAutorAAutorDto(autores);
            return autoresDto;
        }

        [HttpPost]
        public async Task<ActionResult> Post(AutorCreacionDTO autorCreacionDto)
        {

            var autor = this.mapper.MapeoAutorCreacionDtoAAutor(autorCreacionDto);
            context.Autores.Add(autor);
            await context.SaveChangesAsync();
            var autorDto = this.mapper.MapeoAutorAAutorDto(autor);
            return CreatedAtRoute("ObtenerAutor", new { id = autor.Id}, autorDto);
        }

        [HttpGet("{id:int}", Name = "ObtenerAutor")]//agrego un nombre
        public async Task<ActionResult<AutorConLibrosDTO>> Get(int id)
        {
            var autor = await context.Autores
                .Include(a => a.Libros)
                    .ThenInclude(x => x.Libro)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (autor is null)
            {
                return NotFound();
            }
            return this.mapper.MapeoAutorAAutorConLibrosDto(autor);
        }
        
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            //Manera 1 de eliminar
            //var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            //if ( autor == null)
            //{
            //    return BadRequest("No existe un usuario con este id!!");
            //}
            //context.Remove(autor);
            //await context.SaveChangesAsync();
            //

            var registroBorrado = await context.Autores.Where(x => x.Id == id).ExecuteDeleteAsync();
            //ExecuteDeleteAsync devuelve la cantidad de elementos borrados
            if(registroBorrado == 0)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<AutorPatchDTO> patchDoc)
        {
            if (patchDoc is null) {
                return BadRequest();
            }

            var autorDb = await context.Autores.FirstOrDefaultAsync(a => a.Id == id);
            if (autorDb is null) {
                return NotFound();
            }

            var autorPatchDto = this.mapper.MapeoAutorAAutorPatchDto(autorDb);
            patchDoc.ApplyTo(autorPatchDto, ModelState);

            var esValido = TryValidateModel(autorPatchDto);
            if (!esValido) {
                return ValidationProblem();
            }
            this.mapper.MapeoReversoAutorPatchDtoAAutor(autorPatchDto, autorDb);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, AutorCreacionDTO autorCreacionDto)
        {
            var autor = mapper.MapeoAutorCreacionDtoAAutor(autorCreacionDto);
            autor.Id = id;
            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
