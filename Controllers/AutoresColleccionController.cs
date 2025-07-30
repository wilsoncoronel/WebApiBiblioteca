using HolaMundoWebAPI.Datos;
using HolaMundoWebAPI.DTOs;
using HolaMundoWebAPI.Utilidad;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HolaMundoWebAPI.Controllers
{
    [ApiController]
    [Route("api/autores-coleccion")]
    public class AutoresColleccionController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapeos mapeos;

        public AutoresColleccionController(ApplicationDbContext dbContext, IMapeos mapeos)
        {
            this.dbContext = dbContext;
            this.mapeos = mapeos;
        }

        [HttpGet("{ids}", Name = "ObtenerAutoresPorIds")]
        public async Task<ActionResult<List<AutorConLibrosDTO>>> Get(string ids)
        {
            var idsColeccion = new List<int>();
            foreach (var id in ids.Split(","))
            {
                if (int.TryParse(id, out int idInt)) {
                    idsColeccion.Add(idInt);
                }
            }

            if (!idsColeccion.Any())
            {
                ModelState.AddModelError(nameof(ids), "Ningún Id fue encontrado!!");
                return ValidationProblem();
            }
            var autores = await dbContext.Autores.Include(x => x.Libros)
                .ThenInclude(x => x.Libro)
                .Where(x => idsColeccion.Contains(x.Id)).ToListAsync();
            if(autores.Count != idsColeccion.Count)
            {
                return NotFound();
            }

            var autoresDto = mapeos.MapeoColeccionAutorAAutoresColeccion(autores);
            return autoresDto;
        }

        [HttpPost]
        public async Task<ActionResult> Post(IEnumerable<AutorCreacionDTO> autoresCreacionDTO)
        {
            var autores = mapeos.MapeoColeccionAuotrCreacioAColeccionAutores(autoresCreacionDTO);
            dbContext.AddRange(autores);//Permite insertar una collecion de autores
            await dbContext.SaveChangesAsync();
            var autoresDto = mapeos.MapeoArrayAutorAAutorDto(autores);

            var ids = autores.Select(x => x.Id);
            var idsString = string.Join(",", ids);

            return CreatedAtRoute("ObtenerAutoresPorIds", new { ids = idsString}, autoresDto);
        }
    }
}
