using System.Collections.ObjectModel;

namespace Chioy.Communication.Networking.Client.DB.Models
{
    public class TableMapModel : ObservableCollection<TableFieldMapModel>
    {
        public static TableMapModel CreateEmptyPatientTableMap()
        {
            var model = new TableMapModel
                            {
                                new TableFieldMapModel
                                    {
                                        CanUserDelete = false,
                                        DisplayName = "病案号",
                                        IsWhere = true,
                                        LocalField = "PatientID",
                                        Type = "String"
                                    },
                                new TableFieldMapModel
                                    {
                                        CanUserDelete = false,
                                        DisplayName = "姓名",
                                        IsWhere = false,
                                        LocalField = "Name",
                                        Type = "String"
                                    },
                                new TableFieldMapModel
                                    {
                                        CanUserDelete = false,
                                        DisplayName = "性别",
                                        IsWhere = false,
                                        LocalField = "Gender",
                                        Type = "Int",
                                        Comment = "0 - Male, 1 - Female"
                                    },
                                new TableFieldMapModel
                                    {
                                        CanUserDelete = false,
                                        DisplayName = "生日",
                                        IsWhere = false,
                                        LocalField = "BirthDay",
                                        Type = "Date"
                                    },
                                new TableFieldMapModel
                                    {
                                        CanUserDelete = false,
                                        DisplayName = "年龄",
                                        IsWhere = false,
                                        LocalField = "Age",
                                        Type = "Int"
                                    },
                                new TableFieldMapModel
                                    {
                                        CanUserDelete = false,
                                        DisplayName = "申请医生",
                                        IsWhere = false,
                                        LocalField = "RequestDoctor",
                                        Type = "Int"
                                    },
                                new TableFieldMapModel
                                    {
                                        CanUserDelete = false,
                                        DisplayName = "申请科室",
                                        IsWhere = false,
                                        LocalField = "RequestDepot",
                                        Type = "Int"
                                    },
                                new TableFieldMapModel
                                    {
                                        CanUserDelete = false,
                                        DisplayName = "申请时间",
                                        IsWhere = false,
                                        LocalField = "RequestDate",
                                        Type = "Date"
                                    },
                                    new TableFieldMapModel
                                    {
                                        CanUserDelete = false,
                                        DisplayName = "检查科室",
                                        IsWhere = false,
                                        LocalField = "ExamDepartment",
                                        Type = "String"
                                    },
                                    new TableFieldMapModel
                                    {
                                        CanUserDelete = false,
                                        DisplayName = "检查医师",
                                        IsWhere = false,
                                        LocalField = "ExamDoctor",
                                        Type = "String"
                                    },
                                    new TableFieldMapModel
                                    {
                                        CanUserDelete = false,
                                        DisplayName = "诊断医师",
                                        IsWhere = false,
                                        LocalField = "DiagnosticianDoctor",
                                        Type = "String"
                                    },
                            };

            return model;
        }
    }

    public class TableFieldMapModel
    {
        private bool _canUserDelete = true;
        private string _targetField = string.Empty;
        public string DisplayName { get; set; }

        public string LocalField { get; set; }

        public string TargetField
        {
            get { return _targetField; }
            set { _targetField = value; }
        }


        //public string TargetField { get; set; }

        public bool IsWhere { get; set; }

        public string Comment { get; set; }

        public string Type { get; set; }

        public bool CanUserDelete
        {
            get { return _canUserDelete; }
            set { _canUserDelete = value; }
        }

        public int Size { get; set; }
    }
}