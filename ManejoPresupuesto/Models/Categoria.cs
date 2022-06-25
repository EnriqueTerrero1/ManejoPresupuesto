using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class Categoria
    {

        public int Id { get; set; }
        [Required(ErrorMessage="El campo {0} es requerido")]
        public string Nombre { get; set; }

        [Display(Name="Operacion")]
        public TipoOperacion TipoOperacionId { get; set; }
        public int UsuarioId { get; set; }
    }
}
