using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController] //validaciones automaticas
    [Route("api/autores")] //ruta principal
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context; //campo de la clase accesible desde toda la clase. para poder enviar, borrar, etc 
            this.mapper = mapper;
        }

        [HttpGet]
        
        public async Task<ActionResult<List<AutorDTO>>> Get()
        {
            var autores = await context.Autores.ToListAsync(); //trae una lista (.ToListAsync)
            return mapper.Map<List<AutorDTO>>(autores); // List: lo que muestro (el dto), como parametro la entidad
        }


        [HttpGet("{id:int}", Name = "obtenerAutor")]
        public async Task<ActionResult<AutorDTOconLibros>> Get(int id)
        {
            var autor = await context.Autores
                .Include(x => x.AutoresLibros)
                .ThenInclude(x => x.Libro)
                .FirstOrDefaultAsync(x => x.Id == id); //trae un solo autor (.FirstOrDefau.....) si no lo encuentra, el valor por defecto es null, por eso la validacion que sigue

            if (autor == null)
            {
                return NotFound();
            }

            return mapper.Map<AutorDTOconLibros>(autor);
        }

        //[HttpGet ("{nombre}")]
        //public async Task<ActionResult<Autor>> Get([FromRoute] string nombre)
        //{
        //    var autor = await context.Autores.Where(x => x.Nombre == nombre).ToAsyncList();

        //    if (autor == null)
        //    {
        //       return NotFound();
        //   }

        //    return autor;
        //}

        //[HttpPost]
        //public async Task<ActionResult> Post([FromBody] Autor autor)
        //{
        //    var existeAutorConElMismoNombre = await context.Autores.AnyAsync(x => x.Nombre == autor.Nombre);

        //   if (existeAutorConElMismoNombre)
        //   {
        //       return BadRequest($"Ya existe un autor con el nombre {autor.Nombre}");
        //   }

        //    context.Add(autor);
        //    await context.SaveChangesAsync();
        //    return Ok();
        //}

        [HttpPost]
        public async Task<ActionResult> Post(AutorCreacionDTO autorCreacionDTO)
         {
           var autor = mapper.Map<Autor>(autorCreacionDTO);

           context.Add(autor); // agrego el autor al contexto
           await context.SaveChangesAsync(); //guardo los cambios de manera asincrona y se crea el autor en la base de datos

            var autorDTO = mapper.Map<AutorDTO>(autor);

           return CreatedAtRoute("obtenerAutor", new { id = autor.Id}, autorDTO);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(AutorCreacionDTO autorCreacionDTO, int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);
            autor.Id = id;

            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
