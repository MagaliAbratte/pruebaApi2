namespace WebApiAutores.DTOs
{
    public class LibroCreacionDTO
    {
        public string Nombre { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public List<int> AutoresIds { get; set; }
    }
}
