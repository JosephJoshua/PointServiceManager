﻿using CrystalDecisions.CrystalReports.Engine;
using DevExpress.Xpf.Core;
using PSMDesktopUI.Library.Models;
using System.Globalization;

namespace PSMDesktopUI.Views
{
    public partial class ServiceInvoicePreviewView : ThemedWindow
    {
        private ServiceInvoiceModel _invoiceModel;

        public ServiceInvoicePreviewView()
        {
            InitializeComponent();
            ReportViewer.Owner = GetWindow(this);
        }

        public void SetInvoiceModel(ServiceInvoiceModel model, string reportPath)
        {
            _invoiceModel = model;
            LoadReport(reportPath);
        }

        private void LoadReport(string reportPath)
        {
            CultureInfo culture = new CultureInfo("id-ID");

            ReportDocument report = new ReportDocument();
            report.Load(reportPath);

            report.SetParameterValue("NomorNota", _invoiceModel.NomorNota);
            report.SetParameterValue("NamaPelanggan", _invoiceModel.NamaPelanggan);
            report.SetParameterValue("NoHp", _invoiceModel.NoHp);
            report.SetParameterValue("TipeHp", _invoiceModel.TipeHp);
            report.SetParameterValue("Imei", _invoiceModel.Imei);
            report.SetParameterValue("Kerusakan", _invoiceModel.Kerusakan);
            report.SetParameterValue("Biaya", _invoiceModel.TotalBiaya.ToString("C", culture));
            report.SetParameterValue("Dp", _invoiceModel.Dp.ToString("C", culture));
            report.SetParameterValue("Sisa", _invoiceModel.Sisa.ToString("C", culture));
            report.SetParameterValue("Kelengkapan", _invoiceModel.Kelengkapan);
            report.SetParameterValue("YangBelumDicek", _invoiceModel.YangBelumDicek);
            report.SetParameterValue("Tanggal", _invoiceModel.Tanggal);

            ReportViewer.ViewerCore.ReportSource = report;
        }
    }
}
