using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace IMS.BE.Models.Masters
{
    public class CreateBarang
    {
        [Required]
        [Display(Name = "SkuId", Prompt = "Sku Code harus Unik")]
        public string SkuId { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Name", Prompt = "Nama")]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Kategori { get; set; } = string.Empty;
    }
}
