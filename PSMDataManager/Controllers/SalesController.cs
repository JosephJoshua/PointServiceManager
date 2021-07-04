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
    public class SalesController : ApiController
    {
        [HttpGet]
        [Route("api/Sales")]
        public List<SalesModel> Get()
        {
            try
            {
                SalesData data = new SalesData();
                return data.GetSales();
            }
            catch (Exception ex)
            {
                throw new SqlApiException(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Sales/{id}")]
        public SalesModel GetById(int id)
        {
            try
            {
                SalesData data = new SalesData();
                return data.GetSalesById(id);
            }
            catch (Exception ex)
            {
                throw new SqlApiException(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/Sales")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Post(AddSalesBindingModel model)
        {
            try
            {
                SalesData data = new SalesData();
                data.InsertSales(new SalesModel { Nama = model.Nama });

                return Ok();
            }
            catch (Exception ex)
            {
                throw new SqlApiException(ex.Message);
            }
        }

        [HttpDelete]
        [Route("api/Sales/{id}")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                SalesData data = new SalesData();
                data.DeleteSales(id);

                return Ok();
            }
            catch (Exception ex)
            {
                throw new SqlApiException(ex.Message);
            }
        }
    }
}