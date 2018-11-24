using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Core.Extensions
{
    public static class HttpErrorWriter
    {
        public static void AddApplicationError(this HttpResponse response, string msg)
        {
            response.Headers.Add("Application-Error", msg);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}
