using System;
using System.ComponentModel.DataAnnotations;

namespace PSMDataManager.Models
{
    public class AddServiceBindingModel
    {
        [Required]
        public string NamaPelanggan { get; set; }

        public string NoHp { get; set; }

        [Required]
        public string TipeHp { get; set; }

        public string Imei { get; set; }

        [Required]
        public string Kerusakan { get; set; }

        public string KondisiHp { get; set; }

        public string YangBelumDicek { get; set; }

        public string Kelengkapan { get; set; }

        public string Warna { get; set; }

        public string KataSandiPola { get; set; }

        [Required]
        public int TechnicianId { get; set; }

        [Required]
        public int SalesId { get; set; }

        [Required]
        public string StatusServisan { get; set; }

        public DateTime TanggalKonfirmasi { get; set; }

        public string IsiKonfirmasi { get; set; }

        public decimal Biaya { get; set; }

        public int Discount { get; set; }

        public decimal Dp { get; set; }

        public decimal TambahanBiaya { get; set; }

        public decimal HargaSparepart { get; set; }

        public DateTime TanggalPengambilan { get; set; }
    }

    public class EditServiceBindingModel
    {
        [Required]
        public int NomorNota { get; set; }

        [Required]
        public string NamaPelanggan { get; set; }

        public string NoHp { get; set; }

        [Required]
        public string TipeHp { get; set; }

        public string Imei { get; set; }

        [Required]
        public string Kerusakan { get; set; }

        public string KondisiHp { get; set; }

        public string YangBelumDicek { get; set; }

        public string Kelengkapan { get; set; }

        public string Warna { get; set; }

        public string KataSandiPola { get; set; }

        [Required]
        public int TechnicianId { get; set; }

        [Required]
        public int SalesId { get; set; }

        [Required]
        public string StatusServisan { get; set; }

        public DateTime TanggalKonfirmasi { get; set; }

        public string IsiKonfirmasi { get; set; }

        public decimal Biaya { get; set; }

        public int Discount { get; set; }

        public decimal Dp { get; set; }

        public decimal TambahanBiaya { get; set; }

        public decimal HargaSparepart { get; set; }

        public DateTime TanggalPengambilan { get; set; }
    }
}