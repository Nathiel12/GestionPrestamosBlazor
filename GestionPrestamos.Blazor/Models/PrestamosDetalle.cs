using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPrestamos.Models
{
    public partial class PrestamosDetalle
    {
        [Key]
        public int Id { get; set; }

        public int CuotaNo { get; set; }

        public int PrestamoId { get; set; }

        public double Valor { get; set; }
        public double Balance { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
         
        [ForeignKey("PrestamoId")]
        [InverseProperty("PrestamosDetalle")]
        public virtual Prestamos Prestamo { get; set; } = null!;
    }
}