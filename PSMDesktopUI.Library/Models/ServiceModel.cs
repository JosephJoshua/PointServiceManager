using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PSMDesktopUI.Library.Models
{
    public class ServiceModel : INotifyPropertyChanged, IEquatable<ServiceModel>
    {
        public int NomorNota { get; set; }

        public DateTime Tanggal { get; set; }

        public string NamaPelanggan { get; set; }

        public string NoHp { get; set; }

        public string TipeHp { get; set; }

        public string Imei { get; set; }

        public string Kerusakan { get; set; }

        public string KondisiHp { get; set; }

        public string YangBelumDicek { get; set; }

        public string Kelengkapan { get; set; }

        public string Warna { get; set; }

        public string KataSandiPola { get; set; }

        public int TechnicianId { get; set; }

        public int SalesId { get; set; }

        public string StatusServisan { get; set; }

        public DateTime TanggalKonfirmasi { get; set; }

        public string IsiKonfirmasi { get; set; }

        public decimal Biaya { get; set; }

        public int Discount { get; set; }

        public decimal Dp { get; set; }

        public decimal TambahanBiaya { get; set; }

        public decimal TotalBiaya { get; set; }

        public decimal HargaSparepart { get; set; }

        public decimal Sisa { get; set; }

        public decimal LabaRugi { get; set; }

        public DateTime TanggalPengambilan { get; set; }

        [JsonIgnore]
        public ICollection<SparepartModel> Spareparts
        {
            get => _spareparts;
            set
            {
                _spareparts = value;
                NotifyPropertyChanged(nameof(Spareparts));
            }
        }

        private ICollection<SparepartModel> _spareparts;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public bool Equals(ServiceModel other)
        {
            if (this == null && other == null) return true;

            return other != null && NomorNota == other.NomorNota && Tanggal == other.Tanggal && NamaPelanggan == other.NamaPelanggan &&
                   NoHp == other.NoHp && TipeHp == other.TipeHp && Imei == other.Imei && Kerusakan == other.Kerusakan &&
                   KondisiHp == other.KondisiHp && YangBelumDicek == other.YangBelumDicek && Kelengkapan == other.Kelengkapan &&
                   Warna == other.Warna && KataSandiPola == other.KataSandiPola && TechnicianId == other.TechnicianId &&
                   SalesId == other.SalesId && StatusServisan == other.StatusServisan && TanggalKonfirmasi == other.TanggalKonfirmasi &&
                   IsiKonfirmasi == other.IsiKonfirmasi && Biaya == other.Biaya && Dp == other.Dp && TambahanBiaya == other.TambahanBiaya &&
                   TotalBiaya == other.TotalBiaya && HargaSparepart == other.HargaSparepart && Sisa == other.Sisa &&
                   LabaRugi == other.LabaRugi && TanggalPengambilan == other.TanggalPengambilan;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ServiceModel);
        }

        public override int GetHashCode()
        {
            return NomorNota.GetHashCode();
        }
    }
}
