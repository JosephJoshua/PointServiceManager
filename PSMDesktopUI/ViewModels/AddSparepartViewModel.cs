﻿using Caliburn.Micro;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Models;
using System;
using System.Threading.Tasks;

namespace PSMDesktopUI.ViewModels
{
    public class AddSparepartViewModel : Screen
    {
        private readonly ISparepartEndpoint _sparepartEndpoint;

        private int _nomorNota;
        private string _nama;
        private double _harga;

        public int NomorNota
        {
            get => _nomorNota;

            set
            {
                _nomorNota = value;
                NotifyOfPropertyChange(() => NomorNota);
            }
        }

        public string Nama
        {
            get => _nama;

            set
            {
                _nama = value;

                NotifyOfPropertyChange(() => Nama);
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public double Harga
        {
            get => _harga;

            set
            {
                _harga = value;

                NotifyOfPropertyChange(() => Harga);
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public bool CanAdd
        {
            get => !string.IsNullOrWhiteSpace(Nama) && Harga > 0;
        }

        public AddSparepartViewModel(ISparepartEndpoint sparepartEndpoint)
        {
            _sparepartEndpoint = sparepartEndpoint;
        }

        public void SetNomorNota(int nomorNota)
        {
            NomorNota = nomorNota;
        }

        public async Task Add()
        {
            SparepartModel sparepart = new SparepartModel
            {
                NomorNota = NomorNota,
                Nama = Nama,
                Harga = (decimal)Harga,
                TanggalPembelian = DateTime.Today,
            };

            await _sparepartEndpoint.Insert(sparepart);
            await TryCloseAsync(true);
        }

        public async Task Cancel()
        {
            await TryCloseAsync(false);
        }
    }
}
