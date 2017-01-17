using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Models.ReportMetadata
{
    public class KRSGCheckResult:BaseCheckResult
    {
        private float bMI = 0;
        private float height = 0;
        private string measureDateTime = "1990/1/1";
        private string scanCode = "";
        private float temperature = 0;
        private float weight = 0;
        private string deviceID = "";
        private int order = 0;
        private int transferIndex = 0;
        private int transferTotalCount = 0;
        private int totalCount = 0;
        private int currentCount = 0;
        private int status = 0;
        private string patientID = "";
        private string patientName = "";
        private int gender = 1;
        private string brithday = "1990/1/1";

        public float BMI
        {
            get
            {
                return bMI;
            }

            set
            {
                bMI = value;
            }
        }

        public float Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
            }
        }

        public string MeasureDateTime
        {
            get
            {
                return measureDateTime;
            }

            set
            {
                measureDateTime = value;
            }
        }

        public string ScanCode
        {
            get
            {
                return scanCode;
            }

            set
            {
                scanCode = value;
            }
        }

        public float Temperature
        {
            get
            {
                return temperature;
            }

            set
            {
                temperature = value;
            }
        }

        public float Weight
        {
            get
            {
                return weight;
            }

            set
            {
                weight = value;
            }
        }

        public string DeviceID
        {
            get
            {
                return deviceID;
            }

            set
            {
                deviceID = value;
            }
        }

        public int Order
        {
            get
            {
                return order;
            }

            set
            {
                order = value;
            }
        }

        public int TransferIndex
        {
            get
            {
                return transferIndex;
            }

            set
            {
                transferIndex = value;
            }
        }

        public int TransferTotalCount
        {
            get
            {
                return transferTotalCount;
            }

            set
            {
                transferTotalCount = value;
            }
        }

        public int TotalCount
        {
            get
            {
                return totalCount;
            }

            set
            {
                totalCount = value;
            }
        }

        public int CurrentCount
        {
            get
            {
                return currentCount;
            }

            set
            {
                currentCount = value;
            }
        }

        public int Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }

        public string PatientID
        {
            get
            {
                return patientID;
            }

            set
            {
                patientID = value;
            }
        }

        public string PatientName
        {
            get
            {
                return patientName;
            }

            set
            {
                patientName = value;
            }
        }

        public int Gender
        {
            get
            {
                return gender;
            }

            set
            {
                gender = value;
            }
        }

        public string Brithday
        {
            get
            {
                return brithday;
            }

            set
            {
                brithday = value;
            }
        }
    }
}
