using Chioy.Communication.Networking.Interface;
using Chioy.Communication.Networking.Models.ProductModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Service.Provider
{
    public interface IBMDDataProvider : IDataProvider
    {
        Transfer_Patient RequestPatientByPatientID(string patientId);

        List<Transfer_Patient> RequestAllPatients();

        List<Transfer_BMD_Measure_Result> Request_BMD_Measure_ResultByPatientID(string patientId);

        Transfer_BMD_Measure_Result Request_BMD_Measure_Result(string checkId);

        List<Transfer_BMD_Measure_Result> Request_BMD_Measure_All_Result();

        List<Transfer_PatientCheck> Request_BMD_Summary_ResultByPatientID(string patientId);

        List<Transfer_PatientCheck> Request_BMD_Summary_All_Result();

        Transfer_PatientCheck Request_BMD_Summary_ResultByCheckId(string checkId);
    }
}
