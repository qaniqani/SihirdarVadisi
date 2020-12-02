using nQuant;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Sihirdar.ImageService.Controllers
{
    public class ImageController : Controller
    {
        [HttpGet]
        public string Test()
        {
            return "test";
        }

        public string ConvertImage(string dataUri)
        {


            var base64Data = Regex.Match(dataUri, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;            
            
            var binData = Convert.FromBase64String(base64Data);

            try
            {
                using (var stream = new MemoryStream(binData))
                {
                    var fileName = Guid.NewGuid() + ".png";

                    var url = Server.MapPath("~/uploads/") + fileName;

                    var quantizer = new WuQuantizer();
                    using (var bitmap = new Bitmap(stream))
                    {
                        using (var quantized = quantizer.QuantizeImage(bitmap, 
                            10, 70))
                        {
                            quantized.Save(url, ImageFormat.Png);
                        }
                    }

                    return fileName;
                }
            }
            catch (Exception ex)
            {
                
                return "error";
            }
        }
    }
}
