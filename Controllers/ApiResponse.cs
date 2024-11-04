using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_web_api.Controllers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message {get; set;}
        public T? Data {set; get;}
        public List<string>? Errors {get; set;}
        public int StatusCode {get; set;}
        public DateTime TimeStamp {get; set;}

        //constructor for successful response
        public ApiResponse (bool success, string message, T? data ,  List<string>? errors, int statusCode)
        {
            Success = success;
            Message = message;
            Data = data;
            Errors = errors;
            StatusCode = statusCode;
            TimeStamp = DateTime.UtcNow;
        }


        public static ApiResponse<T> SuccessResponse(T data, int statusCode, string message = ""){
            return new ApiResponse<T>(true, message, data, null, statusCode );
        }


        //static method for creating an error response
        public static ApiResponse<T> ErrorResponse(List<string> error, int statusCode, string message = ""){
            return new ApiResponse<T>(false, message, default(T), error, statusCode );
        }
    }
}