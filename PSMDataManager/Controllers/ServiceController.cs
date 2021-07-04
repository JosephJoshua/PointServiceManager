using PSMDataManager.Exceptions;
using PSMDataManager.Library.DataAccess;
using PSMDataManager.Library.Models;
using PSMDataManager.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace PSMDataManager.Controllers
{
    [Authorize]
    public class ServiceController : ApiController
    {
        [HttpGet]
        public List<ServiceModel> Get()
        {
            try
            {
                ServiceData data = new ServiceData();
                return data.GetServices();
            }
            catch (Exception ex)
            {
                throw new SqlApiException(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Service/{nomorNota}")]
        public ServiceModel GetByNomorNota(int nomorNota)
        {
            try
            {
                ServiceData data = new ServiceData();
                return data.GetServiceByNomorNota(nomorNota);
            }
            catch (Exception ex)
            {
                throw new SqlApiException(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "CustomerService")]
        public IHttpActionResult Post(AddServiceBindingModel model)
        {
            if (model.TanggalKonfirmasi == DateTime.MinValue)
            {
                model.TanggalKonfirmasi = new DateTime(1753, 1, 1, 0, 0, 0);
            }

            if (model.TanggalPengambilan == DateTime.MinValue)
            {
                model.TanggalPengambilan = new DateTime(1753, 1, 1, 0, 0, 0);
            }

            try
            {
                ServiceData data = new ServiceData();
                int nomorNota = data.InsertService(new ServiceModel
                {
                    NamaPelanggan = model.NamaPelanggan,
                    NoHp = model.NoHp,
                    TipeHp = model.TipeHp,
                    Imei = model.Imei,
                    Kerusakan = model.Kerusakan,
                    KondisiHp = model.KondisiHp,
                    YangBelumDicek = model.YangBelumDicek,
                    Kelengkapan = model.Kelengkapan,
                    Warna = model.Warna,
                    KataSandiPola = model.KataSandiPola,
                    TechnicianId = model.TechnicianId,
                    SalesId = model.SalesId,
                    StatusServisan = model.StatusServisan,
                    TanggalKonfirmasi = model.TanggalKonfirmasi,
                    IsiKonfirmasi = model.IsiKonfirmasi,
                    Biaya = model.Biaya,
                    Discount = model.Discount,
                    Dp = model.Dp,
                    TambahanBiaya = model.TambahanBiaya,
                    HargaSparepart = model.HargaSparepart,
                    TanggalPengambilan = model.TanggalPengambilan,
                });

                return Ok(nomorNota);
            }
            catch (Exception ex)
            {
                throw new SqlApiException(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "CustomerService")]
        public IHttpActionResult Put(EditServiceBindingModel model)
        {
            if (model.TanggalKonfirmasi == DateTime.MinValue)
            {
                model.TanggalKonfirmasi = new DateTime(1753, 1, 1, 0, 0, 0);
            }

            if (model.TanggalPengambilan == DateTime.MinValue)
            {
                model.TanggalPengambilan = new DateTime(1753, 1, 1, 0, 0, 0);
            }

            try
            {
                ServiceData data = new ServiceData();
                data.UpdateService(new ServiceModel
                {
                    NomorNota = model.NomorNota,
                    NamaPelanggan = model.NamaPelanggan,
                    NoHp = model.NoHp,
                    TipeHp = model.TipeHp,
                    Imei = model.Imei,
                    Kerusakan = model.Kerusakan,
                    KondisiHp = model.KondisiHp,
                    YangBelumDicek = model.YangBelumDicek,
                    Kelengkapan = model.Kelengkapan,
                    Warna = model.Warna,
                    KataSandiPola = model.KataSandiPola,
                    TechnicianId = model.TechnicianId,
                    SalesId = model.SalesId,
                    StatusServisan = model.StatusServisan,
                    TanggalKonfirmasi = model.TanggalKonfirmasi,
                    IsiKonfirmasi = model.IsiKonfirmasi,
                    Biaya = model.Biaya,
                    Discount = model.Discount,
                    Dp = model.Dp,
                    TambahanBiaya = model.TambahanBiaya,
                    HargaSparepart = model.HargaSparepart,
                    TanggalPengambilan = model.TanggalPengambilan,
                });

                return Ok();
            }
            catch (Exception ex)
            {
                throw new SqlApiException(ex.Message);
            }
        }

        [HttpDelete]
        [Route("api/Service/{nomorNota}")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Delete(int nomorNota)
        {
            try
            {
                ServiceData data = new ServiceData();
                data.DeleteService(nomorNota);

                return Ok();
            }
            catch (Exception ex)
            {
                throw new SqlApiException(ex.Message);
            }
        }
    }
}