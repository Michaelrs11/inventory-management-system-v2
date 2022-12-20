using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace IMS.BE.Models.Masters
{
    public class UpdateBarang
    {
        [Required]
        [Display(Name = "Name", Prompt = "Nama")]
        public string Name { get; set; } = string.Empty;
        public string SkuId { get; set; } = string.Empty;
        public string Kategori { get; set; } = string.Empty;
    }
}
