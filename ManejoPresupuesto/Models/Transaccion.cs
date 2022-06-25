using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class Transaccion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }

        [Display(Name = "Fecha Transaccion")]
        [DataType(DataType.Date)]
        public DateTime FechaTransaccion { get; set; } = DateTime.Today;

        public decimal Monto { get; set; }

        [Display(Name = "Categoria")]
        [Range(0, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una categoria")]
        public int CategoriaId { get; set; }

        [StringLength(maximumLength: 1000, ErrorMessage = "La nota de no puede pasar de {1} caracteres")]
        public string Nota { get; set; }
        [Range(0, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una cuenta")]

        [Display(Name = "Cuenta")]
        public int CuentaId { get; set; }
        [Display(Name = "Tipo Operacion")]

        public TipoOperacion TipoOperacionId {get;set; }= TipoOperacion.Ingreso;

    }
}
