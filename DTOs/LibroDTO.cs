namespace WebApiAutores.DTOs
{
    public class LibroDTO
    {
        public int Id { get; set; }
        //required
        public string Nombre { get; set; }
        public DateTime FechaPublicacion { get; set; }
       
        //public List<ComentarioDTO> Comentarios { get; set; }
    }
}
