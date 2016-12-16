using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Models.ProductModel
{
    public class CustomerFields : Entity
    {
        public string PatientID { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string DataType { get; set; }
    }
}
