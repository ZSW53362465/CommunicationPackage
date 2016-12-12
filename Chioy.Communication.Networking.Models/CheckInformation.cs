using Chioy.Communication.Networking.Common;
using Chioy.Communication.Networking.Models.ReportMetadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Chioy.Communication.Networking.Models
{
    public class CheckInformation
    {
        public ProductType  Type { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string Sex { get; set; }
        public CardType CardType { get; set; }
        public string ID { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }

        public int PatientType { get; set; }

        public bool LoadSuccess { get; set; }

        public string ExceptionMessage { get; set; }

        public static CheckInformation LoadFromXmlFile(string xmlFile)
        {
            
            XDocument document = XDocument.Load(xmlFile);
            XElement root = document.Root;
            var newChecker = new CheckInformation();

            var requiredElements = root.Elements().Where(e => bool.Parse(e.Attribute("required").Value));
            foreach (XElement item in root.Elements())
            {
                var requiredAtt = item.Attribute("required");
                if (requiredAtt != null)
                {
                    if (bool.Parse(requiredAtt.Value))
                    {
                        if (string.IsNullOrEmpty(item.Value))
                        {
                            newChecker.LoadSuccess = false;
                            newChecker.ExceptionMessage = string.Format("{0} is required field, you need set value in {1}", item.Name, xmlFile);
                            return newChecker;
                        }
                    }
                }
            }
            try
            {
                newChecker.Type = (ProductType)Convert.ToInt32(root.Element("Type").Value);
                newChecker.Name = root.Element("Name").Value;
                newChecker.Birthday = DateTime.Parse(root.Element("Birthday").Value);
                newChecker.Sex = root.Element("Sex").Value;
                newChecker.CardType= (CardType)Convert.ToInt32(root.Element("CardType").Value);
                newChecker.ID = root.Element("ID").Value;
                newChecker.Age = Convert.ToInt32(root.Element("Age").Value);
                newChecker.Height = Convert.ToInt32(root.Element("Height").Value);
                newChecker.Weight = Convert.ToInt32(root.Element("Weight").Value);
                newChecker.PhoneNo = root.Element("PhoneNo").Value;
                newChecker.Address = root.Element("Address").Value;
                newChecker.PatientType = Convert.ToInt32(root.Element("PatientType").Value);
            }
            catch (Exception ex)
            {
                newChecker.LoadSuccess = false;
                newChecker.ExceptionMessage = "type of field is incorrect,please check xml file " + xmlFile;
                return newChecker;
            }
            newChecker.LoadSuccess = true;
            return newChecker;
        }
    }
  
}
