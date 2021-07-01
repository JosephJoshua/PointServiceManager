using System.Net;

namespace PSMDataManager.Exceptions
{
    public class SqlApiException : ApiException
    {
        public SqlApiException(string message) : base(HttpStatusCode.InternalServerError, message) { }
    }
}