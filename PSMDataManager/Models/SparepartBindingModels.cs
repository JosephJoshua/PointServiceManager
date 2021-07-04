using System;
using System.ComponentModel.DataAnnotations;

namespace PSMDataManager.Models
{
    public class AddSparepartBindingModel
    {
        [Required]
        public int NomorNota { get; set; }

        [Required]
        public string Nama { get; set; }

        [Required]
        public int Harga { get; set; }

        [Required]
        public DateTime TanggalPengambilan { get; set; }
    }
}