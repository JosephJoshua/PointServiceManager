using Microsoft.AspNet.Identity;
using PSMDataManager.Exceptions;
using PSMDataManager.Library.DataAccess;
using PSMDataManager.Library.Models;
using System;
using System.Linq;
using System.Web.Http;

namespace PSMDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        [HttpGet]
        public UserModel GetById()
        {
            try
            {
                string userId = RequestContext.Principal.Identity.GetUserId();
                UserData data = new UserData();

                return data.GetUserById(userId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new SqlApiException(ex.Message);
            }
        }
    }
}
