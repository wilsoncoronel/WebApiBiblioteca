using HolaMundoWebAPI.DTOs;
using HolaMundoWebAPI.Entidades;

namespace HolaMundoWebAPI.Utilidad
{
    public interface IMapeos
    {
        AutorDTO MapeoAutorAAutorDto(Autor autor);
        IEnumerable<AutorDTO> MapeoArrayAutorAAutorDto(IEnumerable<Autor> autores);
        Autor MapeoAutorCreacionDtoAAutor(AutorCreacionDTO autorCreacionDto);
        LibroDTO MapeoLibroALibroDto(Libro libro);
        IEnumerable<LibroDTO> MapeoArrayLibroALibroDto(IEnumerable<Libro> libros);
        Libro MapeoLibroCreacionDtoALibro(LibroCreacionDTO libroCreacionDto);
        AutorConLibrosDTO MapeoAutorAAutorConLibrosDto(Autor autor);
        LibroConAutorDTO MapeoLibroALibroConAutorDTO(Libro libro);
        AutorPatchDTO MapeoAutorAAutorPatchDto(Autor autor);
        void MapeoReversoAutorPatchDtoAAutor(AutorPatchDTO autorDto, Autor entidad);
        LibroDTO MapeoLibroTbALibroDto(Libro libroTb);
        void MapLibroCreacionDtoToLibro(LibroCreacionDTO dto, Libro libro);
        /*---------------Comentario--------------*/
        Comentario MapeoComentarioDtoAComentario(ComentarioCreacionDTO comentarioDto);
        ComentarioPatchDTO MapeoComentarioAComentarioPatchDto(Comentario comentario);
        ///Comentario MapeoReversoComentarioPatchDtoAComentario(ComentarioPatchDTO comentarioPatchDto);
        List<ComentarioDTO> MapeoArrayComentarioDtoDesdeComentarios(IEnumerable<Comentario> comentarios);
        ComentarioDTO MapeoComentarioAComentarioDto(Comentario comentario);
        ComentarioCreacionDTO MapeoReversoComentarioAComentarioCreacionDto(Comentario comentario);
        Comentario MapeoComentarioCreacionDtoAComentario(ComentarioCreacionDTO comentarioCreacionDto);
        void MapeoReversoComentarioPatchDtoAComentario(ComentarioPatchDTO comentarioDto, Comentario entidad);

        /*---------------------AutorLibro-------------------*/
        AutorLibro MapeoAutorLibroDtoAAutorLibroTb(AutorLibroDTO autorLibroDto);

        /*--------------------------Mapeo ArraysAutores-------------------------*/
        IEnumerable<Autor> MapeoColeccionAuotrCreacioAColeccionAutores(IEnumerable<AutorCreacionDTO> autoresCreacionDto);
        List<AutorConLibrosDTO> MapeoColeccionAutorAAutoresColeccion(IEnumerable<Autor> autores);
    }
    public class Mapeos : IMapeos
    {

        public AutorLibro MapeoAutorLibroDtoAAutorLibroTb(AutorLibroDTO autorLibroDto) {
            return new AutorLibro {
                AutorId = autorLibroDto.AutorId,
                LibroId = autorLibroDto.LibroId,
            };
        }

        public List<AutorLibro> MapeoArrayAutorLibroDtoAAutorLibroTb(IEnumerable<AutorLibroDTO> autorLibroDto)
        {
            return autorLibroDto.Select(this.MapeoAutorLibroDtoAAutorLibroTb).ToList();
        }

        public void MapeoReversoComentarioPatchDtoAComentario(ComentarioPatchDTO comentarioDto, Comentario entidad)
        {
            entidad.Cuerpo = comentarioDto.Cuerpo;
        }
        public Comentario MapeoComentarioCreacionDtoAComentario(ComentarioCreacionDTO comentarioCreacionDto)
        {
            return new Comentario
            {
                Cuerpo = comentarioCreacionDto.Cuerpo,
                UsuarioId = "0"
            };
        }
        public List<ComentarioDTO> MapeoArrayComentarioDtoDesdeComentarios(IEnumerable<Comentario> comentarios) {
            return comentarios.Select(this.MapeoComentarioAComentarioDto).ToList();
        }

        public ComentarioDTO MapeoComentarioAComentarioDto(Comentario comentario)
        {
            return new ComentarioDTO
            {
                Id = comentario.Id,
                Cuerpo = comentario.Cuerpo,
                FechaPublicacion = comentario.FechaPublicacion,
                UsuarioEmail = comentario.Usuario.Email,
                UsuarioId = comentario.UsuarioId,
            };
        }
        public Comentario MapeoComentarioDtoAComentario(ComentarioCreacionDTO comentarioDto)
        {
            return new Comentario
            {
                Cuerpo = comentarioDto.Cuerpo,
                UsuarioId = comentarioDto.UsuarioId,

            };
        }
        public ComentarioPatchDTO MapeoComentarioAComentarioPatchDto(Comentario comentario) {
            return new ComentarioPatchDTO
            {
                Cuerpo = comentario.Cuerpo,
            };
        }
        public Comentario MapeoReversoComentarioPatchDtoAComentario(ComentarioPatchDTO comentarioPatchDto)
        {
            return new Comentario
            {
                Cuerpo = comentarioPatchDto.Cuerpo,
                UsuarioId = comentarioPatchDto.UsuarioId

            };
        }
        public ComentarioCreacionDTO MapeoReversoComentarioAComentarioCreacionDto(Comentario comentario) {
            return new ComentarioCreacionDTO
            {
                Cuerpo = comentario.Cuerpo,
            };
        }
        public AutorPatchDTO MapeoAutorAAutorPatchDto(Autor autor) {
            return new AutorPatchDTO
            {
                Apellidos = autor.Apellidos,
                Nombres = autor.Nombres,
                Identificacion = autor.Identificacion
            };
        }

        //Este método no retorna nada, porque modifica directamente el objeto entidad, el cual ya está siendo trackeado por Entity Framework Core.
        public void MapeoReversoAutorPatchDtoAAutor(AutorPatchDTO autorDto, Autor entidad)
        {

            entidad.Apellidos = autorDto.Apellidos;
            entidad.Nombres = autorDto.Nombres;
            entidad.Identificacion = autorDto.Identificacion;

        }

        public AutorConLibrosDTO MapeoAutorAAutorConLibrosDto(Autor autor)
        {
            return new AutorConLibrosDTO
            {
                Id = autor.Id,
                NombreCompleto = $"{autor.Nombres} {autor.Apellidos}",
                Libros = autor.Libros.Select(l => new LibroDTO
                {
                    Id = l.Libro.Id,
                    Titulo = l.Libro.Titulo
                }).ToList()
            };
        }
        public AutorDTO MapeoAutorAAutorDto(Autor autor) {

            return new AutorDTO {
                Id = autor.Id,
                NombreCompleto = $"{autor.Nombres} {autor.Apellidos}"
            };
        }
        public IEnumerable<AutorDTO> MapeoArrayAutorAAutorDto(IEnumerable<Autor> autores)
        {
            //Metodo largo
            /*var listaAutoresDto = new List<AutorDTO>();
            foreach (Autor autor in autores) { 
                var autordto = this.MapeoAutorAAutorDto(autor);
                listaAutoresDto.Add(autordto);
            }
            return listaAutoresDto;*/
            /*Método simple con link*/

            return autores.Select(this.MapeoAutorAAutorDto);
        }
        public Autor MapeoAutorCreacionDtoAAutor(AutorCreacionDTO autorCreacionDto)
        {
            return new Autor
            {
                Nombres = autorCreacionDto.Nombres,
                Apellidos = autorCreacionDto.Apellidos,
                Identificacion = autorCreacionDto.Identificacion,
                Libros = autorCreacionDto.Libros.Select(l => new AutorLibro
                {
                    Libro = new Libro { Titulo = l.Titulo }
                }).ToList()
            };
        }
        public IEnumerable<Autor> MapeoColeccionAuotrCreacioAColeccionAutores(IEnumerable<AutorCreacionDTO> autoresCreacionDto) {
            
            return autoresCreacionDto.Select(this.MapeoAutorCreacionDtoAAutor).ToList();
        }
       
        public List<AutorConLibrosDTO> MapeoColeccionAutorAAutoresColeccion(IEnumerable<Autor> autores)
        {
            return autores.Select(this.MapeoAutorAAutorConLibrosDto).ToList();
        }


        public LibroConAutorDTO MapeoLibroALibroConAutorDTO(Libro libro){
            return new LibroConAutorDTO
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                Autores = libro.Autores.Select(a => new AutorDTO { 
                    NombreCompleto = $"{a.Autor.Nombres} {a.Autor.Apellidos}",
                    Id = a.Autor.Id,
                }).ToList()
            };
        }
        public LibroDTO MapeoLibroALibroDto(Libro libro) 
        {
            return new LibroDTO
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
            };
        }
        public IEnumerable<LibroDTO> MapeoArrayLibroALibroDto(IEnumerable<Libro> libros) {
            return libros.Select(this.MapeoLibroALibroDto);
        }
        public Libro MapeoLibroCreacionDtoALibro(LibroCreacionDTO libroCreacionDto) {
            var libro = new Libro
            {
                Titulo = libroCreacionDto.Titulo,
                Autores = libroCreacionDto.AutoresIds.Select(id=> new AutorLibro { 
                    AutorId = id,
                }).ToList()
            };
            return libro;
        }
        public LibroDTO MapeoLibroTbALibroDto(Libro libroTb) {
            return new LibroDTO
            {
                Titulo = libroTb.Titulo,
                Id = libroTb.Id,
                AutorLibroDto = libroTb.Autores.Select(id => new AutorLibroDTO{ 
                    AutorId = id.AutorId,
                    LibroId = id.LibroId,
                    Orden = id.Orden
                }).ToList()
            };
        }

        public void MapLibroCreacionDtoToLibro(LibroCreacionDTO dto, Libro libro)
        {
            libro.Titulo = dto.Titulo;
            // Si quieres sobrescribir la lista de autores
            libro.Autores = dto.AutoresIds
                .Select(id => new AutorLibro { 
                    AutorId = id,
                })
                .ToList();
        }
    }
}
