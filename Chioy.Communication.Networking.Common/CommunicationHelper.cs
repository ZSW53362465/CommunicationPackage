using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace Chioy.Communication.Networking.Common
{
    public class CommunicationHelper
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal,
                                                        int size, string filePath);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        public static extern long WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString,
            string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        public static extern int GetPrivateProfileInt(string section, string key, int def, string filePath);

        public static string SerializeToJsonStr<T>(T obj, Encoding encoding = null)
        {
            encoding = encoding == null ? Encoding.UTF8 : encoding;
            var jsonStr = string.Empty;
            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    json.WriteObject(stream, obj);
                    jsonStr = encoding.GetString(stream.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return jsonStr;
        }

        public static T SerializeToObj<T>(string jsonStr, Encoding encoding = null)
        {
            encoding = encoding == null ? Encoding.UTF8 : encoding;
            T rtnObj = default(T);
            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
            try
            {
                using (var mStream = new MemoryStream(encoding.GetBytes(jsonStr)))
                {
                    rtnObj = (T)json.ReadObject(mStream);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return rtnObj;
        }

        public static string GetBase64FromImage(string imagefile)
        {
            string strbaser64 = "";
            if (File.Exists(imagefile))
            {
                try
                {
                    Bitmap bmp = new Bitmap(imagefile);
                    strbaser64 = GetBase64FromImage(bmp);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return strbaser64;
        }

        public static string GetBase64FromImage(Bitmap bmp)
        {
            string strbaser64;
            try
            {
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, bmp.RawFormat);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                strbaser64 = Convert.ToBase64String(arr);
                bmp.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return strbaser64;
        }

        public static T DeserializeToObj<T>(string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(typeof(T));
                    return (T)xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

        public static string SerializerToXml<T>(T t)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(typeof(T));
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "    ";
                settings.NewLineChars = "\r\n";
                using (XmlWriter xmlWriter = XmlWriter.Create(Stream, settings))
                {
                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    namespaces.Add(string.Empty, string.Empty);
                    xml.Serialize(xmlWriter, t, namespaces);
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }

        public static RenderTargetBitmap GetElementBitmap(FrameworkElement p_element,double height,double width, double dpiX = 96d,
            double dpiY = 96d)
        {
            Thickness margin = p_element.Margin;
            double scaleX = dpiX / 96d;
            double scaleY = dpiY / 96d;
            var bmp = new RenderTargetBitmap(
                (int)(width * scaleX + margin.Left + margin.Right)
                , (int)(height * scaleY + margin.Top + margin.Bottom)
                , dpiX
                , dpiY
                , PixelFormats.Pbgra32);
            bmp.Render(p_element);

            return bmp;
        }
        public static ConfigSetting GetConfigSetting()
        {

            Configuration config = ConfigurationManager.OpenExeConfiguration( ConfigurationUserLevel.None);

            var baseAddress = config.AppSettings.Settings["BaseAddress"].Value;
            var wPort = config.AppSettings.Settings["WCFPort"].Value;
            var hPort = config.AppSettings.Settings["HttpPort"].Value;
            int wcfPort = 0;
            int httpPort = 0;
            int.TryParse(wPort, out wcfPort);
            int.TryParse(hPort, out httpPort);
            return new ConfigSetting(wcfPort, httpPort, baseAddress);
        }
    }
    public class UIHelper
    {
        private static DispatcherOperationCallback exitFrameCallback = new DispatcherOperationCallback(ExitFrame);

        /// <summary>  
        /// Processes all UI messages currently in the message queue.  
        /// </summary>  
        public static void DoEvents()
        {
            // Create new nested message pump.  
            DispatcherFrame nestedFrame = new DispatcherFrame();

            // Dispatch a callback to the current message queue, when getting called,   
            // this callback will end the nested message loop.  
            // note that the priority of this callback should be lower than the that of UI event messages.  
            DispatcherOperation exitOperation = Dispatcher.CurrentDispatcher.BeginInvoke(
                DispatcherPriority.Background, exitFrameCallback, nestedFrame);

            // pump the nested message loop, the nested message loop will   
            // immediately process the messages left inside the message queue.  
            Dispatcher.PushFrame(nestedFrame);

            // If the "exitFrame" callback doesn't get finished, Abort it.  
            if (exitOperation.Status != DispatcherOperationStatus.Completed)
            {
                exitOperation.Abort();
            }
        }

        private static Object ExitFrame(Object state)
        {
            DispatcherFrame frame = state as DispatcherFrame;
            // Exit the nested message loop.  
            if (frame != null)
            {
                frame.Continue = false;
            }
            return null;
        }
     
    }
}
