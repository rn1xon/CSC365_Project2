using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CSC365_Project2.Models;
using CsvHelper.Configuration;
using CsvHelper;

namespace CSC365_Project2.Project1Ops
{
    internal class FileOperations
    {
        /// <summary>
        /// Reads a Patient Record file (VAERSData) and converts each line into a patient record, and returns a list of all the patient recs in that file
        /// Optionally include a HashSet of VAERS_IDs
        /// When included, only patient records from the file with a VAERS_ID in the HashSet will be included in the results.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="OnlyTheseVaersIds">When passed in, the method will only include Patient Rec if the VAERS_ID is in the HashSet</param>
        /// <returns></returns>
        public List<PatientRec> ReadPatientRecFile(string fileName, HashSet<int>? OnlyTheseVaersIds = null)
        {
            bool firstLine = true;
            List<PatientRec> rtn = [];

            using (StreamReader sr = new(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!firstLine)
                    {
                        string[] data = line.Split(',');
                        // Add the record when:
                        // * the incoming line contains a VAERS_ID which parses to an int AND
                        // (
                        // * there was no hashset passed OR
                        // * the hashset passed has values and the incoming VAERS_ID is one of those values
                        // )
                        if (int.TryParse(data[0], out int vaers_id) && (OnlyTheseVaersIds == null || OnlyTheseVaersIds.Contains(vaers_id)))
                        {
                            PatientRec rec = new PatientRec();
                            rec.VAERS_ID = vaers_id;
                            rec.RECVDATE = data[1];
                            rec.STATE = data[2];
                            if (decimal.TryParse(data[3], out decimal age))
                            {
                                rec.AGE_YRS = age;
                            }
                            else
                            {
                                rec.AGE_YRS = 0; //data[3] was unable ot parse to an integer value
                            }
                            rec.CAGE_YR = data[4];
                            rec.CAGE_MO = data[5];
                            rec.SEX = data[6];
                            rec.RPT_DATE = data[7];
                            rec.SYMPTOM_TEXT = data[8];
                            rec.DIED = data[9];
                            rec.DATEDIED = data[10];
                            rec.L_THREAT = data[11];
                            rec.ER_VISIT = data[12];
                            rec.HOSPITAL = data[13];
                            rec.HOSPDAYS = data[14];
                            rec.X_STAY = data[15];
                            rec.DISABLE = data[16];
                            rec.RECOVD = data[17];
                            rec.VAX_DATE = data[18];
                            rec.ONSET_DATE = data[19];
                            rec.NUMDAYS = data[20];
                            rec.LAB_DATA = data[21];
                            rec.V_ADMINBY = data[22];
                            rec.V_FUNDBY = data[23];
                            rec.OTHER_MEDS = data[24];
                            rec.CUR_ILL = data[25];
                            rec.HISTORY = data[26];
                            rec.PRIOR_VAX = data[27];
                            rec.SPLTTYPE = data[28];
                            rec.FORM_VERS = data[29];
                            rec.TODAYS_DATE = data[30];
                            rec.BIRTH_DEFECT = data[31];
                            rec.OFC_VISIT = data[32];
                            rec.ER_ED_VISIT = data[33];
                            rec.ALLERGIES = data[34];
                            rtn.Add(rec);
                        }
                    }
                    firstLine = false;
                }
            }

            return rtn;
        }

        /// <summary>
        /// Reads a Symptoms Record file (VAERSSYMPTOMS) and converts each line into a Symptoms object, and returns a list of all the Symptoms objects in that file
        /// Optionally include a HashSet of VAERS_IDs
        /// When included, only symptoms from the file with a VAERS_ID in the HashSet will be included in the results.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="OnlyTheseVaersIds">When passed in, the method will only include Patient Rec if the VAERS_ID is in the HashSet</param>
        /// <returns></returns>
        public List<Symptoms> ReadSymptomsFile(string fileName, HashSet<int>? OnlyTheseVaersIds = null)
        {
            bool firstLine = true;
            List<Symptoms> rtn = new();
            using (StreamReader sr = new(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!firstLine)
                    {
                        string[] data = line.Split(',');
                        // Add the record when:
                        // * the incoming line contains a VAERS_ID which parses to an int AND
                        // (
                        // * there was no hashset passed OR
                        // * the hashset passed has values and the incoming VAERS_ID is one of those values
                        // )
                        if (int.TryParse(data[0], out int vaers_id) && (OnlyTheseVaersIds == null || OnlyTheseVaersIds.Contains(vaers_id)))
                        {
                            {
                                Symptoms symp = new Symptoms();
                                symp.VAERS_ID = vaers_id;
                                symp.SYMPTOM1 = data[1];
                                symp.SYMPTOMVERSION1 = data[2];
                                symp.SYMPTOM2 = data[3];
                                symp.SYMPTOMVERSION2 = data[4];
                                symp.SYMPTOM3 = data[5];
                                symp.SYMPTOMVERSION3 = data[6];
                                symp.SYMPTOM4 = data[7];
                                symp.SYMPTOMVERSION4 = data[8];
                                symp.SYMPTOM5 = data[9];
                                symp.SYMPTOMVERSION5 = data[10];
                                rtn.Add(symp);
                            }
                        }
                    }
                    firstLine = false;
                }
                return rtn;
            }
        }

        /// <summary>
        /// Reads a Vaccine file (*VAERSVAX) and converts each line into a VaxInfo object and returns a list of all the VaxInfo objects sourced from the file
        /// Optionally include a Hashset and a VAX_TYPE 
        /// When both are included
        /// 1. Only records with a VAX_TYPE matching VaxType are collected
        /// 2. If a record is collected, the VAERS_ID is added to the OnlyTheseVaersIds hash set 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="OnlyTheseVaersIds"></param>
        /// <returns></returns>
        public List<VaxInfo> ReadVaxInfoFile(string fileName, HashSet<int>? OnlyTheseVaersIds = null, string[]? VaxTypes = null)
        {
            bool firstLine = true;
            List<VaxInfo> rtn = new();
            using (StreamReader sr = new(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!firstLine)
                    {
                        string[] data = line.Split(',');

                        if (int.TryParse(data[0], out int vaers_id))
                        {
                            // Add the record when:
                            // * the incoming line contains a VAERS_ID which parses to an int AND
                            // (
                            // * VaxTypes is null OR
                            // * VaxTypes has values AND the incoming record's VAX_TYPE matches one of those values
                            // )
                            VaxInfo vax = new VaxInfo();
                            vax.VAERS_ID = vaers_id;
                            vax.VAX_TYPE = data[1];
                            vax.VAX_MANU = data[2];
                            vax.VAX_LOT = data[3];
                            vax.VAX_DOSE_SERIES = data[4];
                            vax.VAX_ROUTE = data[5];
                            vax.VAX_SITE = data[6];
                            vax.VAX_NAME = data[7];

                            if (VaxTypes == null || VaxTypes.Contains(vax.VAX_TYPE))
                            {
                                rtn.Add(vax);

                                if (OnlyTheseVaersIds != null)
                                {
                                    // Store any VAERS_ID's collected in the Hash set
                                    OnlyTheseVaersIds.Add(vax.VAERS_ID);
                                }
                            }
                        }
                    }

                    firstLine = false;
                }
            }

            return rtn;
        }


        /// <summary>
        /// Reads all the Patient Rec files in the directory provided
        /// Optionally collects only records from the source files if they have a VaersId that is in the Hash set of Vaers Ids provided 
        /// </summary>
        /// <param name="dirName"></param>
        /// <param name="onlyTheseVaersIds"></param>
        /// <returns></returns>
        public List<PatientRec> ReadAllPatientRecFiles(string dirName, HashSet<int>? onlyTheseVaersIds = null)
        {
            List<PatientRec> rtn = new();

            foreach (string fileName in Directory.GetFiles(dirName, "*VAERSData.csv"))
            {
                rtn.AddRange(ReadPatientRecFile(fileName));
            }

            return rtn;
        }

        /// <summary>
        /// Reads all the Symptom files in the directory provided
        /// Optionally collects only records from the source files if they have a VaersId that is in the Hash set of Vaers Ids provided 
        /// </summary>
        /// <param name="dirName"></param>
        /// <param name="onlyTheseVaersIds"></param>
        /// <returns></returns>
        public List<Symptoms> ReadAllSymptomsFiles(string dirName, HashSet<int>? onlyTheseVaersIds = null)
        {
            List<Symptoms> rtn = new();

            foreach (string fileName in Directory.GetFiles(dirName, "*VAERSSYMPTOMS.csv"))
            {
                rtn.AddRange(ReadSymptomsFile(fileName, onlyTheseVaersIds));
            }

            return rtn;
        }


        /// <summary>
        /// Optionally collects only records from the source files if they have a VAX_TYPE matching the string parameter collectOnlyTheseVaxTypes provided
        /// When vaersIdsCollected is provided (is not null), this method with fill the Hash set with all the VAERS_ID collected
        /// The resulting hashset can be used when reading the other files to only collect records with a VAERS_ID in the Hash set
        /// </summary>
        /// <param name="dirName"></param>
        /// <param name="vaersIdsCollected"></param>
        /// <param name="collectOnlyTheseTypes"></param>
        /// <returns></returns>
        public List<VaxInfo> ReadAllVaxInfoFiles(string dirName, HashSet<int>? vaersIdsCollected = null, string[]? collectOnlyTheseVaxTypes = null)
        {
            List<VaxInfo> rtn = new();

            foreach (string fileName in Directory.GetFiles(dirName, "*VAERSVAX.csv"))
            {
                rtn.AddRange(ReadVaxInfoFile(fileName, vaersIdsCollected, collectOnlyTheseVaxTypes));
            }

            return rtn;
        }

        /// <summary>
        /// Given an IEnumerable of Anonymous type, write the CSV content to the file path.
        /// Deletes the file if it already exists.
        /// </summary>
        /// <param name="joinedList"></param>
        /// <param name="outputPath"></param>
        public void WriteCSVFile(IEnumerable<dynamic> joinedList, string outputPath)
        {
            // Delete the file if it exists
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                Quote = '"',
                Escape = '"',
                Mode = CsvMode.NoEscape, // Prevents additional escaping
                BadDataFound = null,
            };

            using (var writer = new StreamWriter(outputPath))
            using (var csv = new CsvWriter(writer, config))
            {
                // Write the records to the CSV
                csv.WriteRecords(joinedList);
            }
        }
       
        

        private void WriteOutGroupTotal(int currentBucketDeathCount, StreamWriter writer, string ageBucket)
        {
            writer.WriteLine($"Number of deaths for Age Group {ageBucket}: {currentBucketDeathCount:N0}");
            writer.WriteLine();
            writer.WriteLine();

        }


        /// <summary>
        /// Takes in a existing combined dataset and puts it into a collection of VaersReport items
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public List<VaersReport> ReadVaersReportData(string fileName)
        {

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                BadDataFound = null,
                MissingFieldFound = null
            };


            using var reader = new StreamReader($"data/{fileName}");
            using var csv = new CsvReader(reader, config);

            IEnumerable<VaersReport> records = csv.GetRecords<VaersReport>();

            return records.ToList();
        }
    }

}
