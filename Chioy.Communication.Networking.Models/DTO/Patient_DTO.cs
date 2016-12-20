using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Chioy.Communication.Networking.Models.DTO
{
    public class Patient_DTO : Entity
    {
        public string PatientID { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Profession { get; set; }
        public string Organization { get; set; }
        public string CredentialName { get; set; }
        public int Age { get; set; }
        public int Gender { get; set; }
        public string Provice { get; set; }
        public string Birthday { get; set; }
        public string LastCheckDate { get; set; }
        public int LastCheckType { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string PhoneArea { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        public int ScanCode { get; set; }
        public string Department { get; set; }
        public string Work { get; set; }
        public string Diagnosis { get; set; }
        public int Reservation { get; set; }
        public string OldFileNo { get; set; }
        public int IdentityType { get; set; }
        public string Race { get; set; }
        public string Nation { get; set; }
        public string RequestDoctor { get; set; }
        public string RequestDepartment { get; set; }
        public string RequestDate { get; set; }
        public string ExamDoctor { get; set; }
        public string DiagnosticianDoctor { get; set; }
        public string ExamDepartment { get; set; }

        public List<CustomerFields> CustomerFields { get; set; }
    }
}
