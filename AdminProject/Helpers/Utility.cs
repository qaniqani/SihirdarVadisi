using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using AdminProject.Models;
using File = System.IO.File;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Helpers
{
    public class Utility
    {
        public static Admin SessionCheck()
        {
            try
            {
                if (HttpContext.Current.Session["Admin"] != null)
                    return HttpContext.Current.Session["Admin"] as Admin;

                return null;
                var admin = new Admin
                {
                    Authorization = "Master,Media,Setting",
                    CreatedDate = DateTime.Now,
                    Id = 1,
                    LastLoginDate = DateTime.Now,
                    Name = "Kamil",
                    Password = "1234",
                    Status = StatusTypes.Active,
                    Username = "kamil"
                };

                return admin;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<MenuItem> CreateTree(IEnumerable<Category> allCategories)
        {
            var allNodes = allCategories.Select(category => new MenuItem
            {
                Id = category.Id,
                Name = category.Name,
                Number = category.SequenceNumber,
                ParentId = category.ParentId,
                Picture = category.Picture,
                Url = category.Url,
                Status = category.Status,
                CategoryType = category.CategoryType,
                CategoryTagType = category.CategoryTagType
            }).ToList();

            var lookup = allNodes.Where(a => a.ParentId != 0).ToLookup(category => category.ParentId);

            foreach (var node in allNodes)
                node.ParentItem = lookup[node.Id];

            return allNodes.Where(a => a.ParentId == 0).ToList();
        }

        public static string StripHtml(string txt)
        {
            return Regex.Replace(txt, "<(.|\\n)*?>", string.Empty);
        }

        public static string RenderMenu(IEnumerable<MenuItem> nodes)
        {
            var ul = "<ol>";

            foreach (var node in nodes)
            {
                var li = $"<li id=\"list_{node.Id}\" >";

                li +=
                    string.Format(
                        "<div class=\"cat{0}\" data-id=\"{0}\" data-categoryType=\"{4}\">{2} {1} - Type: {3}</div>",
                        node.Id, node.Name,
                        node.Status == StatusTypes.Active
                            ? "<span class='entypo-check'></span>"
                            : "<span class='entypo-cancel'></span>", node.CategoryType, node.CategoryType.ToInt32());

                if (node.ParentItem != null && node.ParentItem.Any())
                    li += RenderMenu(node.ParentItem);

                ul += li + "</li>";
            }

            ul += "</ol>";

            return ul;
        }

        public static string RenderUlMainMenu(IEnumerable<MenuItem> nodes)
        {
            var ul = "<ul class=\"dropdown-menu\">";

            var comingTag = "data-status=\"coming-soon\"";
            var newTag = "data-status=\"new\"";

            foreach (var node in nodes)
            {
                string urlCheck;
                string dropClass;
                string dropAClass;
                if (!node.ParentItem.Any())
                {
                    dropClass = "";
                    dropAClass = "";
                    if (node.Url.Contains("http") || node.Url.Contains("https"))
                        urlCheck = node.Url;
                    else
                        urlCheck = "/haber/" + node.Url;
                }
                else
                {
                    dropClass = "class=\"dropdown\"";
                    urlCheck = "#";
                    dropAClass = "class=\"dropdown-toggle\" data-toggle=\"dropdown\" role =\"button\" aria-haspopup=\"true\" aria-expanded=\"false\"";
                }

                var li = "<li " + dropClass + " " +
                    (node.CategoryTagType == CategoryTagTypes.New ? newTag : node.CategoryTagType == CategoryTagTypes.Coming ? comingTag : "") 
                    + ">";


                //var urlCheck = !node.ParentItem.Any() ? node.Url.Contains("http") ? node.Url : node.Url.Contains("https") ? node.Url : "/haber/" + node.Url : "javascript:;";

                if (node.CategoryType.ToInt32() == 4) urlCheck = node.Url;

                li += $"<a href=\"{urlCheck}\" {dropAClass}>{node.Name}</a>";

                if (node.ParentItem != null && node.ParentItem.Any())
                    li += RenderUlMainMenu(node.ParentItem);

                ul += li + "</li>";
            }

            ul += "</ul>";

            return ul;
        }

        public static string RenderUlFooterMenu(IEnumerable<MenuItem> nodes)
        {
            var div = "";

            foreach (var node in nodes)
            {
                div += "<div class=\"vadi--footer--bottom__links col-sm-4 col-md-2\"><h4>" + node.Name + "</h4><ul>";
                foreach (var menuItem in node.ParentItem)
                {
                    div += "<li><a href=\"/haber/" + menuItem.Url + "\">" + menuItem.Name + "</a></li>";
                }
                div += "</ul></div>";
            }

            return div;
        }

        public static void FileUpload(HttpPostedFileBase file, string path, int maxWitdh, int maxHeight)
        {
            if (file.ContentLength <= 0) return;

            file.SaveAs(path);

            if (!ImageSizeMeasuring(path, maxWitdh, maxHeight))
                return;

            var buffer = ImageResize.ImgCrop(path, maxWitdh, maxHeight);

            using (var ms = new MemoryStream(buffer))
            {
                var b = Bitmap.FromStream(ms) as Bitmap;

                b?.Save(path);
            }
        }

        public static void FileUpload(HttpPostedFileBase file, string path)
        {
            if (file.ContentLength <= 0) return;

            file.SaveAs(path);
        }

        public static void DeleteFile(string path)
        {
            try
            {
                var fi = new FileInfo(HttpContext.Current?.Server.MapPath(path));
                fi.Delete();
            }
            catch { }
        }

        public static void ChangeFileName(string path, string newPath)
        {
            try
            {
                File.Move(HttpContext.Current.Server.MapPath(path), HttpContext.Current.Server.MapPath(newPath));
            }
            catch { }
        }

        public static bool ImageSizeMeasuring(string path, int maxWidth, int maxHeigth)
        {
            using (var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                using (var imgPhoto = Image.FromStream(fs))
                {
                    var sourceWidth = imgPhoto.Width;
                    var sourceHeight = imgPhoto.Height;

                    return sourceHeight > maxHeigth && sourceWidth > maxWidth;
                }
            }
        }

        public static string UrlSeo(string text)
        {
            text = text.Trim();
            var ci = new CultureInfo("tr-TR");
            text = text.ToLower(ci);
            text = text.Replace(" ", "-");
            text = text.Replace("'", "");
            text = text.Replace(".", "");
            text = text.Replace(":", "");
            text = text.Replace(",", "");
            text = text.Replace(";", "");
            text = text.Replace("#", "");
            text = text.Replace("#8217;", "");
            text = text.Replace("/", "-");
            text = text.Replace("|", "");
            text = text.Replace(@"\", "-");
            text = text.Replace("<", "");
            text = text.Replace(">", "");
            text = text.Replace("&", "");
            text = text.Replace("[", "");
            text = text.Replace("]", "");
            text = text.Replace("+", "-");
            text = text.Replace("*", "-");
            text = text.Replace("(", "");
            text = text.Replace(")", "");
            text = text.Replace("$", "");
            text = text.Replace("=", "");
            text = text.Replace("?", "");
            text = text.Replace("€", "");
            text = text.Replace("ı", "i");
            text = text.Replace("ö", "o");
            text = text.Replace("ü", "u");
            text = text.Replace("ş", "s");
            text = text.Replace("ç", "c");
            text = text.Replace("ğ", "g");
            text = text.Replace("İ", "i");
            text = text.Replace("Ö", "o");
            text = text.Replace("Ü", "u");
            text = text.Replace("Ş", "s");
            text = text.Replace("Ç", "c");
            text = text.Replace("Ğ", "g");
            text = text.Replace("I", "i");
            text = text.Replace("!", "");
            text = text.Replace("?", "");
            text = text.Replace("!", "");
            text = text.Replace("?", "");
            text = text.Replace("`", "");
            text = text.Replace("´", "");
            text = text.Replace("’", "");
            text = text.Replace("ʻ", "");
            text = text.Replace("ʽ", "");
            text = text.Replace("ʾ", "");
            text = text.Replace("ʿ", "");
            text = text.Replace("ˊ", "");
            text = text.Replace("˝", "");
            text = text.Replace("ˮ", "");
            text = text.Replace("΄", "");
            text = text.Replace("ʹ", "");
            text = text.Replace("΄", "");
            text = text.Replace("′", "");
            text = text.Replace("‴", "");
            return text;
        }

        public static string GuidName(string name)
        {
            var ci = new CultureInfo("tr-TR");
            name = UrlSeo(name.ToString(ci));
            name = name.Insert(name.IndexOf('.'), "-" + Guid.NewGuid());
            return name;
        }

        public static string CorrectLetters(string vocable)
        {
            var ci = new CultureInfo("tr-TR");
            vocable = vocable.ToLower(ci);
            vocable = char.ToUpper(vocable[0]) + vocable.Substring(1);
            return vocable;
        }

        public static string EncryptIt(string toEnrypt)
        {
            if (string.IsNullOrEmpty(toEnrypt)) return "";

            var data = System.Text.Encoding.ASCII.GetBytes(toEnrypt);
            var rgbKey = System.Text.Encoding.ASCII.GetBytes("88912190");
            var rgbIv = System.Text.Encoding.ASCII.GetBytes("09121988");

            //1024-bit encryption
            var memoryStream = new MemoryStream(1024);
            var desCryptoServiceProvider = new DESCryptoServiceProvider();

            var cryptoStream = new CryptoStream(memoryStream,
                desCryptoServiceProvider.CreateEncryptor(rgbKey, rgbIv),
                CryptoStreamMode.Write);

            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();

            var result = new byte[(int)memoryStream.Position];
            memoryStream.Position = 0;
            memoryStream.Read(result, 0, result.Length);

            cryptoStream.Close();
            return Convert.ToBase64String(result);
        }

        public static string DecryptIt(string toDecrypt)
        {
            var decrypted = string.Empty;
            if (string.IsNullOrEmpty(toDecrypt)) return decrypted;

            try
            {
                var data = Convert.FromBase64String(toDecrypt);
                var rgbKey = System.Text.Encoding.ASCII.GetBytes("88912190");
                var rgbIV = System.Text.Encoding.ASCII.GetBytes("09121988");

                var memoryStream = new MemoryStream(data.Length);

                var desCryptoServiceProvider = new DESCryptoServiceProvider();

                var cryptoStream = new CryptoStream(memoryStream,
                    desCryptoServiceProvider.CreateDecryptor(rgbKey, rgbIV),
                    CryptoStreamMode.Read);

                memoryStream.Write(data, 0, data.Length);
                memoryStream.Position = 0;

                decrypted = new StreamReader(cryptoStream).ReadToEnd();

                cryptoStream.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return decrypted;
        }

        public static byte[] ImageCrop(string imageName, int imageMaxHeight, int imageMaxWidth)
        {
            try
            {
                return ImageResize.ImgCrop(HttpContext.Current.Server.MapPath(imageName), imageMaxWidth, imageMaxHeight);
            }
            catch { return null; }
        }

        public static byte[] ImageFix(string imageName, int imageMaxHeight, int imageMaxWidth, string colorCode)
        {
            colorCode = !string.IsNullOrEmpty(colorCode) ? colorCode : "#FFFFFF";

            var pBuffer = ImageResize.FixedSize(HttpContext.Current.Server.MapPath(imageName), imageMaxWidth, imageMaxHeight, colorCode);

            ImageResize.GetEncoder(ImageFormat.Jpeg);

            var qualityParam = new EncoderParameter(Encoder.Compression, 50L);
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            byte[] buffer;
            using (var ms = new MemoryStream(pBuffer))
            {
                var b = Bitmap.FromStream(ms) as Bitmap;
                if (b != null) b.Save(ms, ImageFormat.Jpeg);
                buffer = ms.ToArray();
                //ReturnPath = SavePath + ImageMaxWidth + "x" + ImageMaxHeight + "_" + ImageName + ".jpg";

                //b.Save(HttpContext.Current.Server.MapPath("~" + ReturnPath), jpegCodec, encoderParams);
            }
            return buffer;
        }

        public static string ImageCropSave(string imageName, string savePath, string tempPath, int imageMaxHeight, int imageMaxWidth)
        {
            string returnPath;
            var pBuffer = ImageResize.ImgCrop(HttpContext.Current.Server.MapPath(tempPath), imageMaxWidth, imageMaxHeight);

            var jpegCodec = ImageResize.GetEncoder(ImageFormat.Jpeg);

            var qualityParam = new EncoderParameter(Encoder.Compression, 50L);
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            using (MemoryStream ms = new MemoryStream(pBuffer))
            {
                var b = Bitmap.FromStream(ms) as Bitmap;
                //b.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                returnPath = savePath + imageMaxWidth + "x" + imageMaxHeight + "_" + imageName + ".jpg";

                if (b != null) b.Save(HttpContext.Current.Server.MapPath("~" + returnPath), jpegCodec, encoderParams);
            }
            return returnPath;
        }

        public static string ImageFixSave(string imageName, string savePath, string tempPath, int imageMaxHeight, int imageMaxWidth, string colorCode)
        {
            string returnPath;

            colorCode = !string.IsNullOrEmpty(colorCode) ? colorCode : "#FFFFFF";

            var pBuffer = ImageResize.FixedSize(HttpContext.Current.Server.MapPath(tempPath), imageMaxWidth, imageMaxHeight, colorCode);

            var jpegCodec = ImageResize.GetEncoder(ImageFormat.Jpeg);

            var qualityParam = new EncoderParameter(Encoder.Compression, 50L);
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            using (var ms = new MemoryStream(pBuffer))
            {
                var b = Bitmap.FromStream(ms) as Bitmap;
                //b.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                returnPath = savePath + imageMaxWidth + "x" + imageMaxHeight + "_" + imageName + ".jpg";

                b?.Save(HttpContext.Current.Server.MapPath("~" + returnPath), jpegCodec, encoderParams);
            }
            return returnPath;
        }

        public static int CreateCaptcha(int heigth, int width, string font, int punto, int xCoordinate, int yCoordinate,
            string backgroundImage, string imageSavePath)
        {
            using (var bmp = new Bitmap(width, heigth))
            {
                var g = Graphics.FromImage(bmp);
                var f = new Font(font, punto);
                var r = new Random();
                var sayi = r.Next(1000, 99999);
                //ViewState["captcha"] = sayi;
                var img = Image.FromFile(backgroundImage);

                g.DrawImage(img, 1, 1);

                g.DrawString(sayi.ToString(), f, Brushes.Black, xCoordinate, yCoordinate);

                g.CompositingQuality = CompositingQuality.HighQuality;

                bmp.Save(imageSavePath, ImageFormat.Png);

                return sayi;
            }
        }

        public static bool EmailSending(RuntimeSettings setting, string sendMail, string mailSubject, string mailBody)
        {
            try
            {
                if (string.IsNullOrEmpty(sendMail)) return false;

                var eMail = new MailMessage(sendMail, setting.EmailAddress)
                {
                    Subject = mailSubject,
                    IsBodyHtml = true,
                    Body = mailBody
                };
                var smtp = new SmtpClient
                {
                    Credentials = new System.Net.NetworkCredential(setting.EmailAddress, setting.EmailPassword),
                    Port = Convert.ToInt32(setting.Port),
                    Host = setting.Smtp
                };
                smtp.Send(eMail);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool DateTimeParsing(string strDateTime, out DateTime dateTime)
        {
            var formats = new[] { "dd.MM.yyyy", "dd.MM.yyyy HH:mm:ss", "dd.MM.yyyy HH:mm" };

            if (!string.IsNullOrEmpty(strDateTime))
                return DateTime.TryParseExact(strDateTime, formats, CultureInfo.InstalledUICulture, DateTimeStyles.None,
                    out dateTime);

            dateTime = new DateTime(1970, 1, 1);
            return false;
        }

        private static readonly IDictionary<string, string> _extensionMappings =
            new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
            {
                #region Big freaking list of mime types
                {"application/atom+xml", ".atom"},
                {"application/internet-property-stream", ".acx"},
                {"application/msaccess", ".accdb"},
                {"application/msaccess.addin", ".accda"},
                {"application/msaccess.cab", ".accdc"},
                {"application/msaccess.ftemplate", ".accft"},
                {"application/msaccess.runtime", ".accdr"},
                {"application/msaccess.webapplication", ".accdw"},
                {"application/msword", ".doc"},
                {"application/oleobject", ".ods"},
                {"application/onenote", ".one"},
                {"application/pdf", ".pdf"},
                {"application/vnd.adobe.air-application-installer-package+zip", ".air"},
                {"application/vnd.ms-excel", ".xls"},
                {"application/vnd.ms-excel.sheet.binary.macroEnabled.12", ".xlsb"},
                {"application/vnd.ms-excel.sheet.macroEnabled.12", ".xlsm"},
                {"application/vnd.ms-excel.template.macroEnabled.12", ".xltm"},
                {"application/vnd.ms-powerpoint", ".ppt"},
                {"application/vnd.ms-powerpoint.addin.macroEnabled.12", ".ppam"},
                {"application/vnd.ms-powerpoint.template.macroEnabled.12", ".potm"},
                {"application/vnd.ms-word.template.macroEnabled.12", ".dotm"},
                {"application/vnd.ms-xpsdocument", ".xps"},
                {"application/vnd.oasis.opendocument.presentation", ".odp"},
                {"application/vnd.oasis.opendocument.text", ".odt"},
                {"application/vnd.openxmlformats-officedocument.presentationml.presentation", ".pptx"},
                {"application/vnd.openxmlformats-officedocument.presentationml.slideshow", ".ppsx"},
                {"application/vnd.openxmlformats-officedocument.presentationml.template", ".potx"},
                {"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ".xlsx"},
                {"application/vnd.openxmlformats-officedocument.spreadsheetml.template", ".xltx"},
                {"application/vnd.openxmlformats-officedocument.wordprocessingml.document", ".docx"},
                {"application/x-7z-compressed", ".7z"},
                {"application/x-bridge-url", ".adobebridge"},
                {"application/x-director", ".dxr"},
                {"application/x-gtar", ".gtar"},
                {"application/x-gzip", ".gz"},
                {"application/xhtml+xml", ".xhtml"},
                {"application/x-mpeg", ".amc"},
                {"application/x-msaccess", ".mdb"},
                {"application/x-ms-application", ".application"},
                {"application/x-shockwave-flash", ".swf"},
                {"application/x-tar", ".tar"},
                {"application/x-zip-compressed", ".zip"},
                {"audio/aac", ".AAC"},
                {"audio/ac3", ".ac3"},
                {"audio/aiff", ".aiff"},
                {"audio/audible", ".aa"},
                {"audio/basic", ".au"},
                {"audio/vnd.audible.aax", ".aax"},
                {"audio/vnd.dlna.adts", ".ADT"},
                {"audio/wav", ".wav"},
                {"audio/x-aiff", ".aif"},
                {"audio/x-ms-wma", ".wma"},
                {"drawing/x-dwf", ".dwf"},
                {"image/bmp", ".bmp"},
                {"image/gif", ".gif"},
                {"image/jpeg", ".jpg"},
                {"image/png", ".png"},
                {"image/tiff", ".tif"},
                {"image/vnd.wap.wbmp", ".wbmp"},
                {"image/x-icon", ".ico"},
                {"image/x-jg", ".art"},
                {"image/x-portable-pixmap", ".ppm"},
                {"text/h323", ".323"},
                {"text/html", ".html"},
                {"text/plain", ".txt"},
                {"text/xml", ".xml"},
                {"text/x-vcard", ".vcf"},
                {"video/3gpp", ".3gpp"},
                {"video/3gpp2", ".3g2"},
                {"video/mp4", ".mp4"},
                {"video/mpeg", ".mpg"},
                {"video/quicktime", ".mov"},
                {"video/x-flv", ".flv"},
                {"video/x-ms-asf", ".asr"},
                {"video/x-msvideo", ".avi"},
                {"video/x-sgi-movie", ".movie"}
                #endregion
            };

        public static string GetExtension(string mimeType)
        {
            string extension;

            return _extensionMappings.TryGetValue(mimeType, out extension) ? extension : "";
        }
    }
}