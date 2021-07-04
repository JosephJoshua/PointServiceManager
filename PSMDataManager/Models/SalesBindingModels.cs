using System.ComponentModel.DataAnnotations;

namespace PSMDataManager.Models
{
    public class AddSalesBindingModel
    {
        [Required]
        public string Nama { get; set; }
    }
}