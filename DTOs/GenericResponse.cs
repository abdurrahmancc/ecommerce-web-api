using Ecommerce_web_api.Controllers;
using Microsoft.AspNetCore.Http;

namespace Ecommerce_web_api.DTOs
{
    public class GenericResponse<R>
    {
        public R? Data { get; set; }
        public string? ErrorMessage { get; set; }



        public GenericResponse( R? data, string errorMessage)
        {
            Data = data;
            ErrorMessage = errorMessage;
        }


        public static GenericResponse<R> SuccessResponse(R data,  string message = null)
        {
            return new GenericResponse<R>(data, message);
        }

        public static GenericResponse<R>ErrorResponse(string message, R? data = default)
        {
            return new GenericResponse<R>(data, message);
        }
    } 
}