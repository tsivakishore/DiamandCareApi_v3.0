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
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DiamandCare.WebApi
{
    public class UploadImagesRepository
    {
        private string _dcDb = Settings.Default.DiamandCareConnection;
        string InstitutionImagesPath = string.Empty;
        string ImageUrlPath = string.Empty;
        DataTable _dtName = null;
        public static int UserID;

        public UploadImagesRepository()
        {
            UserID = Helper.FindUserByID().UserID;
            InstitutionImagesPath = ConfigurationManager.AppSettings["InstitutionImagesPath"].ToString();
            ImageUrlPath = ConfigurationManager.AppSettings["ImageUrlPath"].ToString();
        }


        public async Task<Tuple<bool, string>> SaveImages(MultipartFormDataStreamProvider multipartFormDataStreamProvider, System.Web.HttpFileCollection hfc, string fileUploadPath)
        {
            UploadImagesModel uploadImagesModel = new UploadImagesModel();
            Tuple<bool, string> resTuple = null;
            int saveImgStatus = -1;

            try
            {
                int iUploadedCnt = 0;
                var formData = multipartFormDataStreamProvider.FormData;
                foreach (var prop in typeof(UploadImagesModel).GetProperties())
                {
                    var curVal = formData[prop.Name];

                    if (curVal != null && !string.IsNullOrEmpty(curVal))
                    {
                        prop.SetValue(uploadImagesModel, To(curVal, prop.PropertyType), null);
                    }
                }

                if (!Directory.Exists(InstitutionImagesPath + uploadImagesModel.UserID))
                    Directory.CreateDirectory(InstitutionImagesPath + uploadImagesModel.UserID);

                for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                {
                    System.Web.HttpPostedFile hpf = hfc[iCnt];

                    if (hpf.ContentLength > 0)
                    {
                        if (!File.Exists(InstitutionImagesPath + uploadImagesModel.UserID + "\\" + Path.GetFileName(hpf.FileName)))
                        {
                            hpf.SaveAs(InstitutionImagesPath + uploadImagesModel.UserID + "\\" + Path.GetFileName(hpf.FileName));
                            iUploadedCnt = iUploadedCnt + 1;
                        }
                    }
                }

                using (SqlConnection cxn = new SqlConnection(_dcDb))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserID", uploadImagesModel.UserID, DbType.Int32);
                    parameters.Add("@Description", uploadImagesModel.Description, DbType.String);
                    parameters.Add("@InstituteImgDocumentTable", CreateTable(uploadImagesModel, multipartFormDataStreamProvider.FileData.ToList(), fileUploadPath, ImageUrlPath + uploadImagesModel.UserID).AsTableValuedParameter());
                    parameters.Add("@CreatedBy", UserID, DbType.Int32);

                    saveImgStatus = await cxn.ExecuteScalarAsync<int>("dbo.Insert_InstituteImage", parameters, commandType: CommandType.StoredProcedure);
                }

                resTuple = Tuple.Create(true, "Created successfully");
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
            return resTuple;
        }

        private DataTable CreateTable(UploadImagesModel uploadImagesModel, List<MultipartFileData> MultipartFileData, string fileUploadPath, string imgUrl)
        {
            _dtName = new DataTable();
            _dtName.Columns.Add("UserID");
            _dtName.Columns.Add("ImageUrl");
            _dtName.Columns.Add("ImageName");
            _dtName.Columns.Add("ImageContent", typeof(byte[]));

            DataRow newDataRow = null;

            foreach (MultipartFileData fileContent in MultipartFileData)
            {
                ContentDispositionHeaderValue contentDispositionValue = fileContent.Headers.ContentDisposition;
                string Name = UnquoteToken(contentDispositionValue.Name) ?? String.Empty;
                uploadImagesModel.ImageName = UnquoteToken(contentDispositionValue.FileName) ?? String.Empty;
                fileUploadPath = fileContent.LocalFileName;
                newDataRow = _dtName.NewRow();

                uploadImagesModel.ImageContent = File.ReadAllBytes(fileUploadPath);
                newDataRow = ReturnDataRow(uploadImagesModel.UserID, imgUrl + @"\" + uploadImagesModel.ImageName, uploadImagesModel.ImageContent, uploadImagesModel.ImageName, uploadImagesModel.Description);
                _dtName.Rows.Add(newDataRow);

            }

            return _dtName;
        }

        public async Task<Tuple<bool, string, List<UploadImagesModel>>> GetImagesByInstitute(string InstituteName)
        {
            Tuple<bool, string, List<UploadImagesModel>> result = null;
            List<UploadImagesModel> lstDetails = new List<UploadImagesModel>();

            try
            {
                var parameters = new DynamicParameters();
                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    parameters.Add("@InstituteName", InstituteName, DbType.String);
                    con.Open();
                    var list = await con.QueryAsync<UploadImagesModel>("[dbo].[Select_InstituteImages]", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 300);

                    lstDetails = list.Select(x => new UploadImagesModel
                    {
                        ImageUrl = x.ImageUrl,
                        Description = x.Description,
                        SchoolName = x.SchoolName,
                        ImageName = x.ImageName
                    }).ToList();

                    con.Close();
                }

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
                obj.ImageUrl = String.Format(@"{0}/{1}", folderName, Images[i].Name);
                obj.ImageName = Images[i].Name;
                obj.ImageName = Images[i].Name;
                lstImages.Add(obj);
            }

            return lstImages;
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

        private DataRow ReturnDataRow(int UserID, string ImageUrl, byte[] ImageContent, string ImageName, string Description)
        {
            DataRow row = null;
            row = _dtName.NewRow();
            row["UserID"] = UserID;
            row["ImageUrl"] = ImageUrl;
            row["ImageContent"] = ImageContent;
            row["ImageName"] = ImageName;
            return row;
        }
        private static string UnquoteToken(string token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return token;
            }

            if (token.StartsWith("\"", StringComparison.Ordinal) && token.EndsWith("\"", StringComparison.Ordinal) && token.Length > 1)
            {
                return token.Substring(1, token.Length - 2);
            }

            return token;
        }
    }
}