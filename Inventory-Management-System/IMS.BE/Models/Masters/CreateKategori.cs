using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace IMS.BE.Models.Masters
{
    public class CreateKategori
    {
        [Required]
        [Display(Name = "CategoryId", Prompt = "Kategori Code harus Unik")]
        public string CategoryId { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Name", Prompt = "Nama")]
        public string Name { get; set; } = string.Empty;
    }
}
