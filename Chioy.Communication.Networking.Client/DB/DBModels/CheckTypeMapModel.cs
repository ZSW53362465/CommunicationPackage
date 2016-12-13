using System.Collections.ObjectModel;
using System.Data;

namespace Chioy.Communication.Networking.Client.DB.Models
{
    public class CheckTypeMapModel
    {
        public int ID { get; set; }

        public string CheckType { get; set; }

        public string TargetCheckType { get; set; }
    }

    public class CheckTypeMapListModel : ObservableCollection<CheckTypeMapModel>
    {
        public CheckTypeMapListModel()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="p_table">CheckType表数据</param>
        public CheckTypeMapListModel(DataTable p_table)
        {
            //foreach (DataRow r in p_table .AsEnumerable())
            //{
                var model = new CheckTypeMapModel();
                model.ID = (int) 1;
                model.CheckType = "超声骨密度";            
                Add(model);
            //}
        }
    }
}