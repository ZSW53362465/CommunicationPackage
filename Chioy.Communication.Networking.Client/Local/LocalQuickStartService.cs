using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Models;
using Chioy.Communication.Networking.Models.ReportMetadata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml;

namespace Chioy.Communication.Networking.Client
{
    public class LocalQuickStartService
    {
        private volatile static LocalQuickStartService _instance = null;

        private static readonly object lockHelper = new object();

        public event EventHandler<DataEventArgs> CommunicationEvent;

        public event KRExceptionEventHandler ExceptionEvent;

        private string _configPath = string.Empty;

        private const string DEFAULT_CONFIG_PATH = "../Conf/Common.ini";

        private const string XML_CONFIG_SECTION = "CheckerInforPath";

        private const string XML_CONFIG_INPATH = "InPath";
        private const string XML_CONFIG_OUTPATH = "OutPath";

        public string InPath = string.Empty;

        public static LocalQuickStartService Instance()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                    {
                        _instance = new LocalQuickStartService();
                    }
                }
            }
            return _instance;
        }

        public void InitializeService(string configPath = null)
        {
            _configPath = string.IsNullOrEmpty(configPath) ? DEFAULT_CONFIG_PATH : configPath;
            if (!File.Exists(_configPath))
            {
                ExceptionEvent?.Invoke(new KRException("InitializeService", "配置文件丢失", "配置文件不存在，请核对"));
            }
            var sb = new StringBuilder(200);
            CommunicationHelper.GetPrivateProfileString(XML_CONFIG_SECTION, XML_CONFIG_INPATH, "", sb, 200, _configPath);
            InPath = sb.ToString().Trim();
            if (!Directory.Exists(InPath))
            {
                ExceptionEvent?.Invoke(new KRException("InitializeService", "客户信息目录异常", "找不到客户信息目录，请核对后重试"));
            }

        }

        public CheckInformation GetCheckInformation()
        {
            DirectoryInfo dirinfo = new DirectoryInfo(InPath);
            FileInfo[] files;
            if (dirinfo.Exists)
            {
                files = dirinfo.GetFiles("*.xml");
                if (files.Length > 0)
                {
                    FileComparer fc = new FileComparer();
                    Array.Sort(files, fc);
                    CheckInformation info = CheckInformation.LoadFromXmlFile(files[0].FullName);
                    return info;
                }
                else
                {
                    return new CheckInformation() { LoadSuccess = false, ExceptionMessage = "存放客户信息的路径里无客户信息文件。 " + InPath };
                }
            }
            else
            {
                return new CheckInformation() { LoadSuccess = false, ExceptionMessage = "存放客户信息的文件夹不存在，请核对配置文件或者启动参数的路径是否正确。 " + InPath };
            }
        }

        public void SaveReportMetadata<T>(ExamResultMetadata<T> reportMetadata) where T : BaseCheckResult
        {
            var outPath = getOutPath();
            var xml=CommunicationHelper.SerializerToXml(reportMetadata);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            doc.Save(outPath + "Report_" + DateTime.Now.ToString("yyyy-MM-dd HH：mm：ss：ffff") + ".xml");
        }

        public string SaveReportToBmp(BitmapEncoder encoder, FrameworkElement element,double height,double width)
        {
            var outPath = getOutPath();
            string extend = ".png";
            if (encoder is JpegBitmapEncoder)
            {
                extend = ".jpg";
            }
            else if (encoder is BmpBitmapEncoder)
            {
                extend = ".bmp";
            }

            var savePath = outPath + "Report_" + DateTime.Now.ToString("yyyy-MM-dd HH：mm：ss：ffff") + extend;
            encoder.Frames.Add(BitmapFrame.Create(CommunicationHelper.GetElementBitmap(element, height, width)));
            using (Stream stm = File.Create(savePath))
            {
                encoder.Save(stm);
            }
            return savePath;
        }


        public string SaveReportToBmp(BitmapEncoder encoder, FrameworkElement element)
        {
            return SaveBmp(encoder, element,element.ActualHeight,element.ActualWidth);
        }

        private string SaveBmp(BitmapEncoder encoder, FrameworkElement element, double height, double width)
        {
            var outPath = getOutPath();
            string extend = ".png";
            if (encoder is JpegBitmapEncoder)
            {
                extend = ".jpg";
            }
            else if (encoder is BmpBitmapEncoder)
            {
                extend = ".bmp";
            }

            var savePath = outPath + "Report_" + DateTime.Now.ToString("yyyy-MM-dd HH：mm：ss：ffff") + extend;
            encoder.Frames.Add(BitmapFrame.Create(CommunicationHelper.GetElementBitmap(element, height, width)));
            using (Stream stm = File.Create(savePath))
            {
                encoder.Save(stm);
            }
            return savePath;
        }

        public class FileComparer : IComparer
        {
            int IComparer.Compare(Object o1, Object o2)
            {
                FileInfo fi1 = o1 as FileInfo;
                FileInfo fi2 = o2 as FileInfo;
                return fi1.CreationTime.CompareTo(fi2.CreationTime);
            }
        }

        private string getOutPath()
        {
            var sb = new StringBuilder(200);
            CommunicationHelper.GetPrivateProfileString(XML_CONFIG_SECTION, XML_CONFIG_OUTPATH, "", sb, 200, _configPath);
            return sb.ToString().Trim();
        }
    }
}
