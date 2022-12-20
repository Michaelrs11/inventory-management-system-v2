using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace IMS.BE.Models.Masters
{
    public class CreateGudang
    {
        [Required]
        [Display(Name = "GudangCode", Prompt = "Gudang Code harus Unik")]
        public string GudangCode { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Name", Prompt = "Nama")]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Outlet { get; set; } = string.Empty;
    }
}
