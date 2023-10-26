using AutoMapper;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AutorCreacionDTO, Autor>(); //POST ENVIAR UN DATO A LA BD, fuente-destino
            CreateMap<Autor, AutorDTO>(); //GET MOSTRAR UN DATO DE LA BD, fuente-destino
            CreateMap<Autor, AutorDTOconLibros>()
                 .ForMember(x => x.Libros, opciones => opciones.MapFrom(MapAutorDTOLibros));
            CreateMap<LibroCreacionDTO, Libro>()
                .ForMember(x => x.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));
            //PARA UN ENDPOINT POST
            CreateMap<Libro, LibroDTO>();
            CreateMap<Libro, LibroDTOconAutores>()
                .ForMember(x => x.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores));
            //PARA UN ENDPOINT GET. Aca estamos generando el mapeo para meter datos en Autores, pasando por la entidad intermedia "AutorLibro", que esta dentro de "Libro".
            CreateMap<ComentarioCreacionDTO, Comentario>(); //post
            CreateMap<Comentario, ComentarioDTO>();
        }

        private List<LibroDTO> MapAutorDTOLibros(Autor autor, AutorDTO autorDTO)
        {
            var resultado = new List<LibroDTO>();

            if (autor.AutoresLibros == null)
            {
                return resultado;
            }

            foreach (var autorlibro in autor.AutoresLibros)
            {
                resultado.Add(new LibroDTO()
                {
                    Id = autorlibro.LibroId,
                    Nombre = autorlibro.Libro.Nombre
                });
            }

            return resultado;
        }

        private List<AutorDTO> MapLibroDTOAutores (Libro libro, LibroDTO libroDTO)
        {
            var resultado = new List<AutorDTO>();

            if (libro.AutoresLibros == null)
            {
                return resultado;
            }

            foreach (var autorlibro in libro.AutoresLibros)
            {
                resultado.Add(new AutorDTO()
                {
                    Id = autorlibro.AutorId,
                    Nombre = autorlibro.Autor.Nombre
                });
            }

            return resultado;
        }

        private List<AutorLibro> MapAutoresLibros(LibroCreacionDTO libroCreacionDTO, Libro libro)
        {
            var resultado = new List<AutorLibro>();

            if (libroCreacionDTO.AutoresIds == null)
            {
                return resultado;
            }

            foreach (var autorId in libroCreacionDTO.AutoresIds)
            {
                resultado.Add(new AutorLibro() { AutorId = autorId });
            }

            return resultado;
        }
    }
}
