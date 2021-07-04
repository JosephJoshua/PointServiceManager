using PSMDataManager.Exceptions;
using PSMDataManager.Library.DataAccess;
using PSMDataManager.Library.Models;
using PSMDataManager.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace PSMDataManager.Controllers
{
    public class SparepartController : ApiController
    {
        [HttpGet]
        [Route("api/Sparepart")]
        public List<SparepartModel> Get()
        {
            try
            {
                SparepartData data = new SparepartData();
                return data.GetSpareparts();
            }
            catch (Exception ex)
            {
                throw new SqlApiException(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Sparepart/{nomorNota}")]
        public List<SparepartModel> GetByService(int nomorNota)
        {
            try
            {
                SparepartData data = new SparepartData();
                return data.GetSparepartByNomorNota(nomorNota);
            }
            catch (Exception ex)
            {
                throw new SqlApiException(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/Sparepart")]
        [Authorize(Roles = "Buyer")]
        public IHttpActionResult Post(AddSparepartBindingModel model)
        {
            try
            {
                SparepartData data = new SparepartData();
                data.InsertSparepart(new SparepartModel 
                { 
                    NomorNota = model.NomorNota,
                    Nama = model.Nama,
                    Harga = model.Harga,
                    TanggalPembelian = model.TanggalPengambilan,
                });

                return Ok();
            }
            catch (Exception ex) 
            { 
                throw new SqlApiException(ex.Message);
            }
        }

        [HttpDelete]
        [Route("api/Sparepart/{id}")]
        [Authorize(Roles = "Buyer")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                SparepartData data = new SparepartData();
                data.DeleteSparepart(id);

                return Ok();
            }
            catch (Exception ex) 
            { 
                throw new SqlApiException(ex.Message);
            }
        }
    }
}