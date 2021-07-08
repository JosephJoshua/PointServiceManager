using CrystalDecisions.CrystalReports.Engine;
using DevExpress.Xpf.Core;
using PSMDesktopUI.Library.Models;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;

namespace PSMDesktopUI.Views
{
    public partial class ServiceInvoicePreviewView : ThemedWindow
    {
        private ServiceInvoiceModel _invoiceModel;
        private string _printerName;

        public ServiceInvoicePreviewView()
        {
            InitializeComponent();
            ReportViewer.Owner = GetWindow(this);
        }

        public void SetInvoiceModel(ServiceInvoiceModel model, string reportPath, string printerName)
        {
            _invoiceModel = model;
            _printerName = printerName;

            LoadReport(reportPath);
        }

        private void LoadReport(string reportPath)
        {
            CultureInfo culture = new CultureInfo("id-ID");
            ReportDocument report = new ReportDocument();

            report.Load(reportPath);

            try
            {
                report.PrintOptions.PrinterName = _printerName;
            }
            catch (COMException)
            {
                DXMessageBox.Show("Printer yang terdapat di settings tidak dapat ditemukan. Tolong atur ulang nama printer servisan.", "Print Servisan", 
                    MessageBoxButton.OK);
                Close();
            }

            if (_invoiceModel.NoHp == null) _invoiceModel.NoHp = " ";
            if (_invoiceModel.Imei == null) _invoiceModel.Imei = " ";
            if (_invoiceModel.Kelengkapan == null) _invoiceModel.Kelengkapan = " ";
            if (_invoiceModel.YangBelumDicek == null) _invoiceModel.YangBelumDicek = " ";
            if (_invoiceModel.KondisiHp == null) _invoiceModel.KondisiHp = " ";

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
            report.SetParameterValue("KondisiHp", _invoiceModel.KondisiHp);
            report.SetParameterValue("YangBelumDicek", _invoiceModel.YangBelumDicek);
            report.SetParameterValue("Tanggal", _invoiceModel.Tanggal);

            ReportViewer.ViewerCore.ReportSource = report;
        }
    }
}
