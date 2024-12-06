
namespace APP.Utils
{
    public class Paginador<T>
    {
        public List<T> Paginar(List<T> datos, int pageNumber, int itemsPerPage)
        {
            return datos.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToList();
        }
    }
}
