using System.ComponentModel.DataAnnotations;

namespace PSMDataManager.Models
{
    public class AddTechnicianBindingModel
    {
        [Required]
        public string Nama { get; set; }
    }
}