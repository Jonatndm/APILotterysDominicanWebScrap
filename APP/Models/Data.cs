namespace APP.Models
{
    public class Data
    {
        public int Id { get; set; }
        public string Sorteo { get; set; } = null!;
        public List<string> Numeros { get; set; } = null!;
        public string Fecha { get; set; } = null!;

    }
}
