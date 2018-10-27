using Dapper;
using DiamandCare.Core;
using DiamandCare.WebApi.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DiamandCare.WebApi
{
    public class UploadImagesRepository
    {
        private string _dcDb = Settings.Default.DiamandCareConnection;
        string fileUploadPath = string.Empty;
        string imagesLoadPath = string.Empty;
        string applicationDirectort = string.Empty;
        public static int UserID;

        public UploadImagesRepository()
        {
            UserID = Helper.FindUserByID().UserID;
            fileUploadPath = ConfigurationManager.AppSettings["FileUploadPath"].ToString();
            imagesLoadPath = ConfigurationManager.AppSettings["ImagesLoadPath"].ToString();
            //imagesLoadPath = new Uri(imagesLoadPath).LocalPath;
            applicationDirectort = ReturlRootUrl();
            var filePath = HttpContext.Current.Server.MapPath("~/Image/" + "School");
            var sPath = System.Web.Hosting.HostingEnvironment.MapPath("/FilePath/");
        }
        public async Task<Tuple<bool, string, List<UploadImagesModel>>> GetImagesByInstitute(string InstituteName)
        {
            Tuple<bool, string, List<UploadImagesModel>> result = null;
            List<UploadImagesModel> lstDetails = new List<UploadImagesModel>();

            try
            {
                lstDetails = GetImagesPath(imagesLoadPath + "SaradaSchool");

                var parameters = new DynamicParameters();
                //using (SqlConnection con = new SqlConnection(_dcDb))
                //{
                //    parameters.Add("@InstituteName", InstituteName, DbType.String);
                //    con.Open();
                //    var list = await con.QueryAsync<UploadImagesModel>("[dbo].[Select_ReportTypes]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);
                //    lstDetails = list as List<UploadImagesModel>;
                //    con.Close();
                //}

                if (lstDetails != null && lstDetails.Count() > 0)
                    result = Tuple.Create(true, "", lstDetails);
                else
                    result = Tuple.Create(false, AppConstants.NO_RECORDS_FOUND, lstDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
                result = Tuple.Create(false, "", lstDetails);
            }
            return result;
        }

        public async Task<Tuple<bool, string>> SaveImages(MultipartFormDataStreamProvider multipartFormDataStreamProvider, System.Web.HttpFileCollection hfc)
        {
            string FileSaveFolderName = string.Empty;
            UploadImagesModel uploadImagesModel = new UploadImagesModel();
            Tuple<bool, string> resTuple = null;

            try
            {
                int iUploadedCnt = 0;
                var formData = multipartFormDataStreamProvider.FormData;
                foreach (var prop in typeof(UploadImagesModel).GetProperties())
                {
                    var curVal = formData[prop.Name];
                    if (prop.Name == "InstituteName")
                        FileSaveFolderName = formData[prop.Name];

                    if (curVal != null && !string.IsNullOrEmpty(curVal))
                    {
                        prop.SetValue(uploadImagesModel, To(curVal, prop.PropertyType), null);
                    }
                }

                if (!Directory.Exists(imagesLoadPath + FileSaveFolderName))
                    Directory.CreateDirectory(imagesLoadPath + FileSaveFolderName);

                for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                {
                    System.Web.HttpPostedFile hpf = hfc[iCnt];

                    if (hpf.ContentLength > 0)
                    {
                        if (!File.Exists(imagesLoadPath + FileSaveFolderName + "\\" + Path.GetFileName(hpf.FileName)))
                        {
                            hpf.SaveAs(imagesLoadPath + FileSaveFolderName + "\\" + Path.GetFileName(hpf.FileName));
                            iUploadedCnt = iUploadedCnt + 1;
                        }
                    }
                }


                resTuple = Tuple.Create(true, "Created successfully");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return resTuple;
        }

        public List<UploadImagesModel> GetImagesPath(String folderName)
        {
            DirectoryInfo Folder;
            FileInfo[] Images;
            UploadImagesModel obj = new UploadImagesModel();
            List<UploadImagesModel> lstImages = new List<UploadImagesModel>();
            Folder = new DirectoryInfo(folderName);
            Images = Folder.GetFiles();
            List<String> imagesList = new List<String>();

            for (int i = 0; i < Images.Length; i++)
            {
                obj.Url = String.Format(@"{0}/{1}", folderName, Images[i].Name);
                obj.FileName = Images[i].Name;
                obj.InstituteName = Images[i].Name;
                lstImages.Add(obj);
            }

            return lstImages;
        }

        public string ReturlRootUrl()
        {
            //if (!Directory.Exists(Path.Combine(_strDirectoryPath, instituteName)))
            //{
            //    Directory.CreateDirectory(Path.Combine(_strDirectoryPath, instituteName));
            //}

            string _strDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            var lastFolder = Path.GetDirectoryName(_strDirectoryPath);
            var pathWithoutLastFolder = Path.GetDirectoryName(lastFolder);
            return pathWithoutLastFolder + "\\";
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