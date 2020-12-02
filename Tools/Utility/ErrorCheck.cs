using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Tools.Utility
{
    public class ErrorCheck
    {
        public static string GetErrorMessage(HttpStatusCode statusCode, string defaultMessage)
        {
            if (statusCode == HttpStatusCode.ServiceUnavailable)
                return "Şu anda Riot Sunucularına ulaşılamıyor. Lütfen daha sonra tekrar deneyiniz.";

            if (statusCode == HttpStatusCode.InternalServerError)
                return "Riot Sunucularında bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";

            if (statusCode == HttpStatusCode.Unauthorized)
                return "Riot Sunucularına erişim izni sağlanamıyor. Lütfen daha sonra tekrar deneyiniz.";

            if (statusCode == HttpStatusCode.BadRequest)
                return "Hatalı istek gönderildi. Lütfen tekrar deneyiniz.";

            if (statusCode == HttpStatusCode.NotFound)
                return "Aradığınız kayıt bulunamadı. Lütfen düzenleyip tekrar deneyiniz.";

            if (statusCode == HttpStatusCode.Forbidden)
                return "Geçersiz istek.";

            return defaultMessage;
        }
    }
}