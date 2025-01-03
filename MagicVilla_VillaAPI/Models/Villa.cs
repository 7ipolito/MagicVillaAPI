using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaApi.Models
{
    public class Villa
    {

        public Villa(){
            CreateDate = DateTime.UtcNow;

        }

        [Key] // Define como chave primária
        public int Id { get; set; }

        [Required] // Campo obrigatório
        [MaxLength(100)] // Limite de caracteres
        public string Name { get; set; }

        [Range(1, int.MaxValue)] // Valor mínimo de 1
        public int Sqft { get; set; }

        [Range(1, 20)] // Ocupação mínima e máxima (exemplo)
        public int Occupancy { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
