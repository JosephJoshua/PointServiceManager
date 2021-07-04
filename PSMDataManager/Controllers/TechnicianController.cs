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
    public class TechnicianController : ApiController
    {
        [HttpGet]
        [Route("api/Technician")]
        public List<TechnicianModel> Get()
        {
            try
            {
                TechnicianData data = new TechnicianData();
                return data.GetTechnicians();
            }
            catch (Exception ex)
            {
                throw new SqlApiException(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Technician/{id}")]
        public TechnicianModel GetById(int id)
        {
            try
            {
                TechnicianData data = new TechnicianData();
                return data.GetTechnicianById(id);
            }
            catch (Exception ex)
            {
                throw new SqlApiException(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/Technician")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Post(AddTechnicianBindingModel model)
        {
            try
            {
                TechnicianData data = new TechnicianData();
                data.InsertTechnician(new TechnicianModel { Nama = model.Nama });

                return Ok();
            }
            catch (Exception ex)
            {
                throw new SqlApiException(ex.Message);
            }
        }

        [HttpDelete]
        [Route("api/Technician/{id}")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                TechnicianData data = new TechnicianData();
                data.DeleteTechnician(id);
                return Ok();
            }
            catch (Exception ex) 
            { 
                throw new SqlApiException(ex.Message);
            }
        }
    }
}
