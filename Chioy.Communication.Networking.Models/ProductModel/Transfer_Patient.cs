using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Chioy.Communication.Networking.Models.ProductModel
{
    [DataContract]
    public class Transfer_Patient : Entity
    {
        [DataMember]
        public string PatientID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string Profession { get; set; }
        [DataMember]
        public string Organization { get; set; }
        [DataMember]
        public string CredentialName { get; set; }
        [DataMember]
        public int Age { get; set; }
        [DataMember]
        public int Gender { get; set; }
        [DataMember]
        public string Provice { get; set; }
        [DataMember]
        public DateTime Birthday { get; set; }
        [DataMember]
        public DateTime LastCheckDate { get; set; }
        [DataMember]
        public int LastCheckType { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string PostCode { get; set; }
        [DataMember]
        public string PhoneArea { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public int ScanCode { get; set; }
        [DataMember]
        public string Department { get; set; }
        [DataMember]
        public string Work { get; set; }
        [DataMember]
        public string Diagnosis { get; set; }
        [DataMember]
        public int Reservation { get; set; }
        [DataMember]
        public string OldFileNo { get; set; }
        [DataMember]
        public int IdentityType { get; set; }
        [DataMember]
        public string  Race { get; set; }
        [DataMember]
        public string Nation { get; set; }
        [DataMember]
        public List<CustomerFields> CustomerFields { get; set; }
    }
}
