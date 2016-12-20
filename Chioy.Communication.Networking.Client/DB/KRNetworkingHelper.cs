using Chioy.Communication.Networking.Client.DB.DBHelper;
using Chioy.Communication.Networking.Client.DB.Models;
using Chioy.Communication.Networking.Client.FTP;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace Chioy.Communication.Networking.Client.DB
{
    public class KRNetworkingHelper
    {
        private DataRow _row;
        public string _fullPath;
        private DateTime _nowDateTime;

        public KRNetworkingHelper(DataRow dataRow)
        {
            _row = dataRow;
            _nowDateTime = DateTime.Now;
        }
        public bool SaveReport(RenderTargetBitmap p_bitmap)
        {
            KRNetworkingConfig config = KRNetworkingConfig.Config;
            bool isSucc = false;
            if (config.ReportSaveModel.ReportSaveType == "文件夹")
            {
                isSucc = true;
                if (config.ReportSaveModel.IsReportChecked)
                {
                    isSucc = SaveReportByDir(p_bitmap);
                }
            }
            else if (config.ReportSaveModel.ReportSaveType == "FTP")
            {
                isSucc = SaveReportByFtp(p_bitmap);
            }
            else if (config.ReportSaveModel.ReportSaveType == "无")
            {
                isSucc = true;
            }

            return isSucc;
        }

        private bool SaveReportByDir(RenderTargetBitmap p_bitmap)
        {
            KRNetworkingConfig config = KRNetworkingConfig.Config;

            if (config.ReportSaveModel.ReportSaveType != "文件夹")
            {
                return false;
            }

            string path = config.ReportSaveModel.DirAdresse;

            if (config.ReportSaveModel.IsCreateChildDir)
            {
                IOrderedEnumerable<FileFormatModel> list = config.ReportSaveModel.ChildrenRule.OrderBy(r => r.Index);
                path = CreateDir(path, list);
            }

            string fileName = AnalyseFileString(config.ReportSaveModel.FileFormat) + "." +
                              config.ReportSaveModel.ImageExt;

            string full = Path.Combine(path, fileName);

            BitmapEncoder encoder = GetPic(p_bitmap);

            using (Stream stm = File.Create(full))
            {
                encoder.Save(stm);
            }

            _fullPath = path;

            return true;
        }

        private bool SaveReportByFtp(RenderTargetBitmap p_bitmap)
        {
            KRNetworkingConfig config = KRNetworkingConfig.Config;

            if (config.ReportSaveModel.ReportSaveType != "FTP")
            {
                return false;
            }

            string fileName = AnalyseFileString(config.ReportSaveModel.FileFormat) + "." +
                              config.ReportSaveModel.ImageExt;

            BitmapEncoder encoder = GetPic(p_bitmap);


            using (Stream stm = File.Create(fileName))
            {
                encoder.Save(stm);
            }

            string path = config.ReportSaveModel.FtpAdresse;
            FtpHelper client = new FtpHelper();

            //var ftpHelper = new FtpHelper(path, config.ReportSaveModel.FtpUser, config.ReportSaveModel.FtpPassword);
            WebException exception;
            if (config.ReportSaveModel.IsCreateChildDir)
            {
                IOrderedEnumerable<FileFormatModel> list = config.ReportSaveModel.ChildrenRule.OrderBy(r => r.Index);
                path = GetDir(path, list);

                path = path.Replace("\\", "/");

                client.CreateDirectory(path, out exception);
            }

            _fullPath = Path.Combine(path, fileName);

            client.Upload(new FileInfo(fileName).FullName, path, fileName);
            return true;
        }

        private static BitmapEncoder GetPic(RenderTargetBitmap p_bitmap)
        {
            KRNetworkingConfig config = KRNetworkingConfig.Config;

            BitmapEncoder encoder = null;
            //encoder.Frames.Add(BitmapFrame.Create(bmp));

            if (config.ReportSaveModel.ImageExt == "JPG")
            {
                encoder = new JpegBitmapEncoder();
            }
            else if (config.ReportSaveModel.ImageExt == "BMP")
            {
                encoder = new BmpBitmapEncoder();
            }
            else if (config.ReportSaveModel.ImageExt == "PNG")
            {
                encoder = new PngBitmapEncoder();
            }

            encoder.Frames.Add(BitmapFrame.Create(p_bitmap));
            return encoder;
        }

        private string CreateDir(string p_path, IEnumerable<FileFormatModel> p_list)
        {
            string path = GetDir(p_path, p_list);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        private string GetDir(string p_path, IEnumerable<FileFormatModel> p_list)
        {
            var childDir = new StringBuilder();
            foreach (FileFormatModel ffm in p_list)
            {
                if (childDir.Length > 0)
                {
                    childDir.Append(@"\");
                }
                childDir.AppendFormat(@"{0}", AnalyseFileString(ffm.FileFormat));
            }

            return Path.Combine(p_path, childDir.ToString());
        }

        private string AnalyseFileString(string p_fileString)
        {
            string result = p_fileString;
            var regex = new Regex(@"\[(.*?)\]");

            MatchCollection mc = regex.Matches(p_fileString);

            for (int i = 0; i < mc.Count; i++)
            {
                Match m = mc[i];
                string filed = m.Value.Replace("[", "").Replace("]", "");
                string value = GetFiledValue(filed);
                result = result.Replace(m.Value, value);
            }

            var regex1 = new Regex(@"\<(.*?)\>");
            mc = regex1.Matches(p_fileString);

            for (int i = 0; i < mc.Count; i++)
            {
                Match m = mc[i];
                string filed = m.Value.Replace("<", "").Replace(">", "");
                string value = _nowDateTime.ToString(filed);
                result = result.Replace(m.Value, value);
            }

            return result;
        }

        private string GetSquareValue(string p_string)
        {
            string result = p_string;

            var regex = new Regex(@"\[(.*?)\]");

            MatchCollection mc = regex.Matches(p_string);

            for (int i = 0; i < mc.Count; i++)
            {
                Match m = mc[i];
                string filed = m.Value.Replace("[", "").Replace("]", "");
                string value = GetFiledValue(filed);
                result = result.Replace(m.Value, value);
            }

            return result;
        }

        private string GetFiledValue(string p_filed)
        {
            if (!_row.Table.Columns.Contains(p_filed))
            {
                return string.Empty;
            }

            object result = _row[p_filed];

            if (result == null)
            {
                result = string.Empty;
            }

            return result.ToString();
        }
        public bool SaveCallBackData(RenderTargetBitmap p_bitmap = null)
        {
            KRNetworkingConfig config = KRNetworkingConfig.Config;
            DatabaseConfigModel dbConfig = config.DatabaseConfigModel;
            if (config.DataCallBackModel.CallbackType == "无")
            {
                return true;
            }

            CallBackSqlAndParam sqlandParam = config.DataCallBackModel.GetSqlStringByParam(dbConfig.DatabaseSoft);

            foreach (DbParameterAndKey p in sqlandParam.Parameters)
            {
                object value = null;
                string upperKey = p.Key.ToUpper();
                switch (upperKey)
                {
                    case "<IMAGE>":
                        if (p_bitmap != null)
                        {
                            BitmapEncoder encoder = GetPic(p_bitmap);
                            using (var memoryStream = new MemoryStream())
                            {
                                encoder.Save(memoryStream);
                                byte[] bytes = memoryStream.ToArray();
                                value = bytes;
                                p.Parameter.Size = bytes.Length;
                            }
                        }
                        break;
                    case "<IMAGEPATH>":
                        if (!string.IsNullOrEmpty(_fullPath))
                        {
                            if (config.ReportSaveModel.ReportSaveType == "FTP")
                            {
                                string ftpadress = config.ReportSaveModel.FtpAdresse;
                                string adress = _fullPath.Replace("\\", "/");
                                int len = ftpadress.Length;
                                value = adress.Substring(len);
                                p.Parameter.Size = _fullPath.Length - len;
                            }
                            else
                            {
                                value = _fullPath.Replace("\\", "/");
                                p.Parameter.Size = _fullPath.Length;
                            }
                        }
                        break;
                    default:
                        //if (_row.Table.Columns.Contains(p.Key))
                        //{
                        //var rowValue = _row[p.Key];
                        //if (p.Key.ToUpper() == "CHECKTYPE")
                        //{
                        //    CheckType cy = CheckType.ABIPWV;
                        //    var isSucc = Enum.TryParse<CheckType>(rowValue.ToString(), true, out cy);
                        //    if (isSucc)
                        //    {
                        //        rowValue = KRNetworkingConfig.Config.PatientMapModel.GetTargetCheckByCheckType(cy);
                        //    }
                        //}
                        //else
                        //{
                        string rowValue = GetSquareValue(p.Key);
                        //}
                        value = rowValue;
                        if (p.Key.ToUpper() == "[GENDER]")
                        {
                            if (rowValue == "0" || rowValue == "男")
                            {
                                value = "男";
                            }
                            else
                            {
                                value = "女";
                            }
                        }

                        //}
                        //else
                        //{

                        //}
                        break;
                }

                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    value = DBNull.Value;
                }

                if (dbConfig.DatabaseSoft == DatabaseSoft.PostgreSQL)
                {
                    var para = p.Parameter as NpgsqlParameter;
                    if (para != null)
                    {
                        if (para.NpgsqlDbType == NpgsqlDbType.Date || para.NpgsqlDbType == NpgsqlDbType.Timestamp)
                        {
                            p.Parameter.Value = Convert.ToDateTime(value.ToString());
                        }
                        else
                        {
                            p.Parameter.Value = value;
                        }
                    }
                }
                else
                {
                    p.Parameter.Value = value;
                }
            }

            IDatabaseHelper dbHelper =
                DatabaseHelper.Open(DataBaseSoft.TransDatabaseSoft(dbConfig.DatabaseSoft, dbConfig.IsAdvancedSetting),
                                    config.DatabaseConfigModel.ConnectionString);

            var paraeters = new DbParameter[sqlandParam.Parameters.Count];

            for (int i = 0; i < paraeters.Length; i++)
            {
                paraeters[i] = sqlandParam.Parameters[i].Parameter;
            }

            int result = -1;

            try
            {
                if (config.DataCallBackModel.CallbackType == "表" &&
                    config.DataCallBackModel.TargetTableUpdateType == "更新")
                {
                    result = dbHelper.ExecuteNonQuery(sqlandParam.UpdateSql, paraeters);
                }

                if (result <= 0)
                {
                    result = dbHelper.ExecuteNonQuery(sqlandParam.InsertSql, paraeters);
                }

                if (config.DataCallBackModel.CallbackType == "存储过程")
                {
                    result++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result > 0;
        }
    }
}