namespace ManejoPresupuesto.Models
{
    public class PaginacionRespuesta
    {

        public int Pagina { get; set; } = 1;
        public int RecordPorPagina { get; set; } = 10;
        public int CantidadTotalRecords { get; set; }

        public int CantidadTotalDePaginas=>(int)Math.Ceiling((double)CantidadTotalRecords/RecordPorPagina);

        public string BaseURL { get; set; }

    }

    public class PaginacionRespuesta<T>:PaginacionRespuesta
    {
        public IEnumerable<T> Elementos { get; set; }
    }
}
