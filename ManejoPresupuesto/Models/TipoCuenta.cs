using System.ComponentModel.DataAnnotations;
using ManejoPresupuesto.Validaciones;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Models
{
    public class TipoCuenta
    {

        public int Id { get; set; }

        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength(maximumLength:50,MinimumLength =3,ErrorMessage ="La longitud del campo {0} debe de estar entre {2} y {1}")]
        [Display( Name = "Nombre del tipo cuenta")]
        [PrimeraLetraMayuscula]
        [Remote(action:"VerificarExisteTipoCuenta", controller: "TiposCuentas")]
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }

       

        /*pruebas de otras validaciones
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electronico valido")]
        public string Email { get; set; }
        [Range(minimum:18,maximum: 130,ErrorMessage ="El valor debe estar entre {1} y {2}")]
        public int Edad { get; set; }
        public string URL { get; set; }

        [CreditCard(ErrorMessage ="La tarjeta de credito no es valida")]
        [Display(Name = "Tarjeta de credito")]
        public string TarjetaDeCredito { get; set; }*/
    }
}
