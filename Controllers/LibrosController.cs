using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;
using WebApiAutores.Migrations;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}", Name = "ObtenerLibro")]
        public async Task<ActionResult<LibroDTOconAutores>> Get(int id)
        {
            var libro = await context.Libros
                .Include(x => x.Comentarios)
                .Include(x => x.AutoresLibros)
                .ThenInclude(x => x.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<LibroDTOconAutores>(libro); 
            //mapeo con los valores de "libro" (guardados en la variable "libro"), y lo almaceno en "LibroDTOconAutores" que es lo que se muestra al cliente.
            //en los GET siempre se retorna el mapeo
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacionDTO)
        {
            if (libroCreacionDTO.AutoresIds == null)
            {
                return BadRequest("No se puede crear un libro sin autores");
            }

            var autoresIds = await context.Autores
                .Where(x => libroCreacionDTO.AutoresIds.Contains(x.Id)).Select(x => x.Id).ToListAsync(); 
            //esta linea es para chequear que existan los autores con ese id en la base de datos.

            if (libroCreacionDTO.AutoresIds.Count != autoresIds.Count)
            {
                return BadRequest("No existe uno de los autores enviados");
            }
            //con esta linea chequea que la cantidad de autores que se envian por el cliente coincidan con la cantidad de autores que tiene la base de datos. 

            var libro = mapper.Map<Libro>(libroCreacionDTO);
            AsignarOrdenAutores(libro);

            context.Add(libro);
            await context.SaveChangesAsync();

            var libroDTO = mapper.Map<LibroDTO>(libro);

            return CreatedAtRoute("ObtenerLibro", new { id = libro.Id }, libroDTO);

            //"LibroCreacionDTO" mapea datos hacia "Libros" (entidad). "Libro" genera datos para la tabla "AutorLibro", que a su vez espera recibir un id del libro y del autor.
            //por ende, tengo que hacer un mapeo intermedio para AutorLibro 
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put (int id, LibroCreacionDTO libroCreacionDTO)
        {
            var libroDB = await context.Libros
                .Include(x => x.AutoresLibros)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (libroDB == null)
            {
                return NotFound();
            }

            libroDB = mapper.Map(libroCreacionDTO, libroDB);
            AsignarOrdenAutores(libroDB);

            await context.SaveChangesAsync();
            return NoContent();
        }

        private void AsignarOrdenAutores (Libro libro)
        {
            if (libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Order = i;
                }
            }
        }

        //[HttpPatch("{id:int}")]
        //public async Task<ActionResult> Patch (int id, JsonPatchDocument<LibroPatchDTO> patchDocument)
        //{ 
        //    if (patchDocument == null)
        //    {
        //        return BadRequest();
        //    }

        //    var libroDB = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);

        //    if (libroDB == null)
        //    {
        //        return NotFound();
        //    }

        //    var libroDTO = mapper.Map<LibroPatchDTO>(libroDB);

        //    patchDocument.ApplyTo(libroDTO, ModelState);

        //    var esValido = TryValidateModel(libroDTO);

        //    if (!esValido)
        //    {
        //        return BadRequest();
        //    }

        //    mapper.Map(libroDTO, libroDB);
        //    return NoContent();
        //}

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Libros.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Libro() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
