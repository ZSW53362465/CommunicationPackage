using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Media.Imaging;
using Chioy.Communication.Networking.Client.DB.DBHelper;
using Chioy.Communication.Networking.Client.DB.Models;
using Chioy.Communication.Networking.Client.FTP;
using Chioy.Communication.Networking.Client.FTP.Helper;
using Chioy.Communication.Networking.Models.ReportMetadata;
using Npgsql;
using NpgsqlTypes;
using Chioy.Communication.Networking.Client.Client;

namespace Chioy.Communication.Networking.Client.DB
{
    public class KRNetworkingHelper<T> where T : BaseCheckResult
    {
        private readonly ExamResultMetadata<T> _result;
        private string _fullPath;
        private DateTime _nowDateTime;
        private Exception _uploadException;
        private bool _uploadSuccess;
        private FtpHelper _client;


        public KRNetworkingHelper(ExamResultMetadata<T> result)
        {
            _result = result;
            _nowDateTime = DateTime.Now;
        }

        public bool SaveReport(RenderTargetBitmap pBitmap, ref string finalDir)
        {
            var config = KRNetworkingConfig.Config;
            var isSucc = false;
            if (config.ReportSaveModel.ReportSaveType == "文件夹")
            {
                isSucc = true;
                if (config.ReportSaveModel.IsReportChecked)
                    isSucc = SaveReportByDir(pBitmap, ref finalDir);
            }
            else if (config.ReportSaveModel.ReportSaveType == "FTP")
            {
                isSucc = SaveReportByFtp(pBitmap, ref finalDir);
            }
            else if (config.ReportSaveModel.ReportSaveType == "无")
            {
                isSucc = true;
            }

            return isSucc;
        }

        public bool SaveReportByDir(RenderTargetBitmap pBitmap, ref string finalDir)
        {
            var config = KRNetworkingConfig.Config;

            if (config.ReportSaveModel.ReportSaveType != "文件夹")
                return false;

            var path = config.ReportSaveModel.DirAdresse;
            var chirldDir = string.Empty;
            if (config.ReportSaveModel.IsCreateChildDir)
            {
                var list = config.ReportSaveModel.ChildrenRule.OrderBy(r => r.Index);
                chirldDir = CreateDir(path, list);
            }

            var fileName = AnalyseFileString(config.ReportSaveModel.FileFormat) + "." +
                           config.ReportSaveModel.ImageExt;

            var full = finalDir = Path.Combine(path, Path.Combine(chirldDir, fileName));

            var encoder = GetPic(pBitmap);

            using (Stream stm = File.Create(full))
            {
                encoder.Save(stm);
            }

            _fullPath = finalDir;

            return File.Exists(finalDir);
        }

        public bool SaveReportByFtp(RenderTargetBitmap pBitmap, ref string finalDir)
        {
            if (pBitmap == null) return false;
            _uploadSuccess = false;
            var config = KRNetworkingConfig.Config;

            if (config.ReportSaveModel.ReportSaveType != "FTP")
                return false;

            var fileName = AnalyseFileString(config.ReportSaveModel.FileFormat) + "." +
                           config.ReportSaveModel.ImageExt;

            var encoder = GetPic(pBitmap);


            using (Stream stm = File.Create(fileName))
            {
                encoder.Save(stm);
            }

            var path = config.ReportSaveModel.FtpAdresse;
            //FtpHelper client = new FtpHelper();

            _client = new FtpHelper(path, config.ReportSaveModel.FtpUser, config.ReportSaveModel.FtpPassword, 21);
            _client.UploadFileCompleted += Client_UploadFileCompleted;
            var chirldDir = string.Empty;
            if (config.ReportSaveModel.IsCreateChildDir)
            {
                var list = config.ReportSaveModel.ChildrenRule.OrderBy(r => r.Index);
                chirldDir = GetDir(path, list);


                chirldDir = chirldDir.Replace("\\", "/").Trim();

                //client.CreateDirectory(path, out exception);
            }

            _client.Upload(new FileInfo(fileName).FullName, chirldDir, fileName);
            if (!_uploadSuccess)
            {
                var reUploadCount = 3;
                while (reUploadCount > 0)
                {
                    Trace.WriteLine("上传失败,开始断点续传....");
                    var directoryInfo = new FileInfo(fileName).Directory;
                    if (directoryInfo != null)
                        _client.UploadResume(directoryInfo.FullName, fileName, path, fileName);
                    Thread.Sleep(500);
                    if (_uploadSuccess) break;
                    reUploadCount--;
                }
            }
            _client.UploadFileCompleted -= Client_UploadFileCompleted;
            _client = null;
            if (_uploadException != null)
                throw _uploadException;

            return _uploadSuccess;
        }

        private void Client_UploadFileCompleted(object sender, UploadFileCompletedEventLibArgs e)
        {
            if (e.TransmissionState == TransmissionState.Success)
            {
                _uploadSuccess = true;
                _uploadException = null;
            }
            else
            {
                _uploadException = e.WebException;
                Trace.WriteLine("上传失败");
            }
        }

        private static BitmapEncoder GetPic(BitmapSource pBitmap)
        {
            var config = KRNetworkingConfig.Config;

            BitmapEncoder encoder = null;
            //encoder.Frames.Add(BitmapFrame.Create(bmp));

            switch (config.ReportSaveModel.ImageExt)
            {
                case "JPG":
                    encoder = new JpegBitmapEncoder();
                    break;
                case "BMP":
                    encoder = new BmpBitmapEncoder();
                    break;
                case "PNG":
                    encoder = new PngBitmapEncoder();
                    break;
                default:
                    encoder = new PngBitmapEncoder();
                    break;
            }

            encoder?.Frames.Add(BitmapFrame.Create(pBitmap));
            return encoder;
        }

        private string CreateDir(string pPath, IEnumerable<FileFormatModel> pList)
        {
            var path = GetDir(pPath, pList);
            var tempPath = Path.Combine(pPath, path);
            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);

            return path;
        }

        private string GetDir(string pPath, IEnumerable<FileFormatModel> pList)
        {
            var childDir = new StringBuilder();
            foreach (var ffm in pList)
            {
                var str = AnalyseFileString(ffm.FileFormat).Trim();
                childDir.Append(str);
                if (!string.IsNullOrEmpty(str))
                    childDir.Append("\\");
            }
            var pathStrs = pPath.Split('/');
            if (!string.IsNullOrEmpty(pathStrs[pathStrs.Length - 1]))
                pPath += "/";
            var dir = childDir.ToString();
            return dir.Remove(dir.Length - 1, 1);
        }

        private string AnalyseFileString(string pFileString)
        {
            var result = pFileString;
            var regex = new Regex(@"\[(.*?)\]");

            var mc = regex.Matches(pFileString);

            for (var i = 0; i < mc.Count; i++)
            {
                var m = mc[i];
                var filed = m.Value.Replace("[", "").Replace("]", "");
                var value = GetFiledValue(filed);
                result = result.Replace(m.Value, value);
            }

            var regex1 = new Regex(@"\<(.*?)\>");
            mc = regex1.Matches(pFileString);

            for (var i = 0; i < mc.Count; i++)
            {
                var m = mc[i];
                var filed = m.Value.Replace("<", "").Replace(">", "");
                var value = _nowDateTime.ToString(filed);
                result = result.Replace(m.Value, value);
            }

            return result;
        }

        private string GetSquareValue(string pString)
        {
            var result = pString;

            var regex = new Regex(@"\[(.*?)\]");

            var mc = regex.Matches(pString);

            for (var i = 0; i < mc.Count; i++)
            {
                var m = mc[i];
                var filed = m.Value.Replace("[", "").Replace("]", "");
                var value = GetFiledValue(filed);
                result = result.Replace(m.Value, value);
            }

            return result;
        }

        private string GetFiledValue(string pFiled)
        {
            try
            {
                var property = _result.GetType().GetProperty(pFiled);
                if (property != null)
                    return property.GetValue(_result, null).ToString();
                if (_result.CheckResult != null)
                {
                    property = _result.CheckResult.GetType().GetProperty(pFiled);
                    if (property != null)
                        return property.GetValue(_result.CheckResult, null).ToString();
                }
            }
            catch (ArgumentNullException ex)
            {
                Trace.WriteLine("没有找到属性" + pFiled);
                throw new ArgumentNullException(pFiled, "没有找到" + pFiled);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"字段{pFiled}，写入失败，请核对目标数据库字段类型是否和本地数据类型相匹配");
                throw new Exception($"字段{pFiled}，写入失败，请核对目标数据库字段类型是否和本地数据类型相匹配");
            }

            return string.Empty;
        }

        public bool SaveCallBackData(RenderTargetBitmap pBitmap = null)
        {
            var config = KRNetworkingConfig.Config;
            var dbConfig = config.DatabaseConfigModel;
            if (config.DataCallBackModel.CallbackType == "无")
                return true;

            var sqlandParam = config.DataCallBackModel.GetSqlStringByParam(dbConfig.DatabaseSoft);

            foreach (var p in sqlandParam.Parameters)
                try
                {
                    object value = null;
                    var upperKey = p.Key.ToUpper();
                    switch (upperKey)
                    {
                        case "<IMAGE>":
                            if (pBitmap != null)
                            {
                                var encoder = GetPic(pBitmap);
                                using (var memoryStream = new MemoryStream())
                                {
                                    encoder.Save(memoryStream);
                                    var bytes = memoryStream.ToArray();
                                    value = bytes;
                                    p.Parameter.Size = bytes.Length;
                                }
                            }
                            break;
                        case "<IMAGEPATH>":
                            if (!string.IsNullOrEmpty(_fullPath))
                                if (config.ReportSaveModel.ReportSaveType == "FTP")
                                {
                                    var ftpadress = config.ReportSaveModel.FtpAdresse;
                                    var adress = _fullPath.Replace("\\", "/");
                                    var len = ftpadress.Length;
                                    value = adress.Substring(len);
                                    p.Parameter.Size = _fullPath.Length - len;
                                }
                                else
                                {
                                    value = _fullPath.Replace("\\", "/");
                                    p.Parameter.Size = _fullPath.Length;
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
                            var rowValue = GetSquareValue(p.Key);
                            //}
                            value = rowValue;
                            if (p.Key.ToUpper() == "[GENDER]")
                                if (rowValue == "0" || rowValue == "男")
                                    value = "男";
                                else
                                    value = "女";

                            //}
                            //else
                            //{

                            //}
                            break;
                    }

                    if (string.IsNullOrEmpty(value?.ToString()))
                        value = DBNull.Value;

                    if (dbConfig.DatabaseSoft == DatabaseSoft.PostgreSQL)
                    {
                        var para = p.Parameter as NpgsqlParameter;
                        if (para == null) continue;
                        if (para.NpgsqlDbType == NpgsqlDbType.Date || para.NpgsqlDbType == NpgsqlDbType.Timestamp)
                            p.Parameter.Value = Convert.ToDateTime(value.ToString());
                        else
                            p.Parameter.Value = value;
                    }
                    else
                    {
                        p.Parameter.Value = value;
                    }
                }
                catch (Exception ex)
                {
                    ClientHelper.TraceException("KRNetworkingHelper.SaveCallBackData", "数据映射失败", ex.Message);
                }

            var dbHelper =
                DatabaseHelper.Open(DataBaseSoft.TransDatabaseSoft(dbConfig.DatabaseSoft, dbConfig.IsAdvancedSetting),
                    config.DatabaseConfigModel.ConnectionString);

            var parameters = new DbParameter[sqlandParam.Parameters.Count];

            for (var i = 0; i < parameters.Length; i++)
                parameters[i] = sqlandParam.Parameters[i].Parameter;

            var result = -1;

            try
            {
                if (config.DataCallBackModel.CallbackType == "表" &&
                    config.DataCallBackModel.TargetTableUpdateType == "更新")
                    result = dbHelper.ExecuteNonQuery(sqlandParam.UpdateSql, parameters);

                if (result <= 0)
                    result = dbHelper.ExecuteNonQuery(sqlandParam.InsertSql, parameters);

                if (config.DataCallBackModel.CallbackType == "存储过程")
                    result++;
            }
            catch (Exception ex)
            {
                ClientHelper.TraceException("KRNetworkingHelper.SaveCallBackData", "数据回写失败", ex.Message);
            }

            return result > 0;
        }
    }
}