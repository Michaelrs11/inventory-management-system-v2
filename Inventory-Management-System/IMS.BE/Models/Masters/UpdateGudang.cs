using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace IMS.BE.Models.Masters
{
    public class UpdateGudang
    {
        [Required]
        [Display(Name = "Name", Prompt = "Nama")]
        public string Name { get; set; } = string.Empty;
        public string GudangCode { get; set; } = string.Empty;
        public string Outlet { get; set; } = string.Empty;
    }
}
