using DiamandCare.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DiamandCare.WebApi
{
    [RoutePrefix("api/uploadimages")]
    public class UploadImagesController : ApiController
    {
        private UploadImagesRepository _repo = null;
        string fileUploadPath = string.Empty;
        string imagesLoadPath = string.Empty;
        public UploadImagesController(UploadImagesRepository repository)
        {
            _repo = repository;
            fileUploadPath = ConfigurationManager.AppSettings["FileUploadPath"].ToString();
            imagesLoadPath = ConfigurationManager.AppSettings["ImagesLoadPath"].ToString();
        }

        [Route("GetImagesByInstitute")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<UploadImagesModel>>> GetImagesByInstitute(string InstituteName)
        {
            Tuple<bool, string, List<UploadImagesModel>> result = null;
            try
            {

                result = await _repo.GetImagesByInstitute(InstituteName);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("SaveImages")]
        [HttpPost]
        public async Task<Tuple<bool, string>> SaveImages()
        {
            Tuple<bool, string> resTuple = null;

            try
            {
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

                if (!Directory.Exists(fileUploadPath))
                    Directory.CreateDirectory(fileUploadPath);
                else
                {
                    Array.ForEach(Directory.GetFiles(fileUploadPath),
                    delegate (string path)
                    {
                        File.Delete(path);
                    });
                }

                var provider = new MultipartFormDataStreamProvider(fileUploadPath);
                var result = await Request.Content.ReadAsMultipartAsync(provider);

                if (result.FormData == null)
                    return Tuple.Create(false, "BadRequest");

                resTuple = await _repo.SaveImages(result, hfc);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return resTuple;
        }

        private object To(IConvertible obj, Type t)
        {
            Type u = Nullable.GetUnderlyingType(t);

            if (u != null)
            {
                return (obj == null) ? GetDefaultValue(t) : Convert.ChangeType(obj, u);
            }
            else
            {
                return Convert.ChangeType(obj, t);
            }
        }

        private object GetDefaultValue(Type t)
        {
            if (t.GetTypeInfo().IsValueType)
            {
                return Activator.CreateInstance(t);
            }
            return null;
        }
    }
}
