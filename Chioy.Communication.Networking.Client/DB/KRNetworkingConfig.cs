using Chioy.Communication.Networking.Client.DB.Models;
using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace Chioy.Communication.Networking.Client.DB
{
    /// <summary>
    /// 联网设置
    /// </summary>
    public class KRNetworkingConfig
    {
        #region 成员变量

        private const string PathDefault = @"..\Conf\KRNetworkingConfig.xml";
        private const string FileDefault = "../conf/KRNetworkingConfig";
        private const string FileBak = "_Bak";
        private const string FileExtDefault = ".xml";
        private static string _version = "1.00";

        #endregion

        #region 公有属性

        /// <summary>
        /// 版本号
        /// </summary>
        [XmlAttribute]
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary>
        /// 数据库接连设置
        /// </summary>
        public DatabaseConfigModel DatabaseConfigModel { get; set; }

        public PatientMapModel PatientMapModel { get; set; }

        public ReportSaveModel ReportSaveModel { get; set; }

        public DataCallBackModel DataCallBackModel { get; set; }

        #endregion

        #region 公有方法

        /// <summary>
        /// 保存设置 当p_path 不为空时 相当于设置别存为
        /// </summary>
        /// <param name="pPath">路径</param>
        public void Save(string pPath = null)
        {
            string path = GetPath(pPath);

            FileStream fs = null;
            System.Xml.XmlTextWriter writer = null;
            try
            {
                var xs = new XmlSerializer(typeof(KRNetworkingConfig));

                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                writer = new System.Xml.XmlTextWriter(fs, System.Text.Encoding.UTF8);
                xs.Serialize(writer, this);
            }
            catch (IOException ioex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("序列化失败！原因：" + ex.Message));
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 加载设置 当p_path 为空是 加载默认路径
        /// </summary>
        /// <param name="pPath">路径</param>
        /// <returns></returns>
        public static KRNetworkingConfig Load(string pPath = null)
        {
            bool succeed = false;
            string path = GetPath(pPath);

            KRNetworkingConfig config = null;

            FileStream fs = null;

            string version = _version;

            try
            {
                if (!File.Exists(path))
                {
                    Config = config = CreateNew(path);
                    BackupConfig();

                    return config;
                }

                fs = new FileStream(path, FileMode.Open, FileAccess.Read);

                var xs = new XmlSerializer(typeof(KRNetworkingConfig));
                //fs.Seek(0, SeekOrigin.Begin);
                Config = config = (KRNetworkingConfig)xs.Deserialize(fs);

                fs.Seek(0, SeekOrigin.Begin);
                XElement root = XElement.Load(fs);

                fs.Close();

                XAttribute verAttr = root.Attribute("Version");

                if (verAttr == null || verAttr.Value != version)
                {
                    //fs.Close();
                    config.Version = version;
                    fs.Close();
                    config.Save(); //CreateNew(path);
                    //_systemConfig = config;
                    //return config;
                }

                succeed = true;
            }
            catch (IOException ex)
            {
                //KLog.Logger.Error("KRNetworkingConfig文件读写异常！", ex);
            }
            catch (Exception ex)
            {
                //KLog.Logger.Error("设置反序列化异常！", ex);
                //throw new Exception("设置反序列化异常！");
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            if (!succeed)
            {
                RestoreBakConfig(path);
                config = Load();
            }
            else
            {
                BackupConfig();
            }

            return config;
        }

        /// <summary>
        /// 创建一个新的配置
        /// </summary>
        /// <param name="pPath"></param>
        /// <returns></returns>
        private static KRNetworkingConfig CreateNew(string pPath = null)
        {
            string path = GetPath(pPath);
            //KLog.Logger.Error(string.Format("---创建新的KRNetworkingConfig文件，日期:{0}!!!", DateTime.Now.ToString()));
            var config = new KRNetworkingConfig(true);
            config.Save(path);
            return config;
        }

        /// <summary>
        /// 获取备份路径
        /// </summary>
        /// <param name="pPath"></param>
        /// <returns></returns>
        private static string GetBackupPath(string pPath = null)
        {
            string desPath = string.Format("{0}{1}{2}", FileDefault, FileBak, FileExtDefault);
            string path = string.IsNullOrEmpty(pPath) ? desPath : pPath;

            return path;
        }

        /// <summary>
        /// 备份配置文件
        /// </summary>
        /// <param name="pPath"></param>
        public static void BackupConfig(string pPath = null)
        {
            try
            {
                string srcPath = GetPath(pPath);
                string desPath = GetBackupPath();
                File.Copy(srcPath, desPath, true);
            }
            catch (Exception ex)
            {
                //KLog.Logger.Error("KRNetworkingConfig文件备份出现异常！", ex);
            }
        }

        /// <summary>
        /// 还原备份的配置文件
        /// </summary>
        /// <param name="pPath"></param>
        public static void RestoreBakConfig(string pPath = null)
        {
            try
            {
                string desPath = GetPath(pPath);
                string srcPath = GetBackupPath();
                if (File.Exists(srcPath))
                {
                    File.Copy(srcPath, desPath, true);
                }
                else
                {
                    CreateNew(pPath);
                }
            }
            catch (Exception ex)
            {
                //KLog.Logger.Error("KRNetworkingConfig文件还原出现异常！", ex);
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取路径
        /// </summary>
        /// <param name="pPath"></param>
        /// <returns></returns>
        private static string GetPath(string pPath = null)
        {
            string path = string.IsNullOrEmpty(pPath) ? PathDefault : pPath;

            return path;
        }

        #endregion

        #region 静态属性

        public static KRNetworkingConfig Config { get; set; }

        #endregion

        #region 构造方法

        public KRNetworkingConfig(bool p_isInstantiation)
        {
            if (p_isInstantiation)
            {
                DatabaseConfigModel = new DatabaseConfigModel();
                PatientMapModel = new PatientMapModel(ConnectionString.Value);
                ReportSaveModel = new ReportSaveModel();
                DataCallBackModel = new DataCallBackModel();
            }
        }

        public KRNetworkingConfig()
        {
        }

        #endregion
    }
}