using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC365_Project2.Models
{
    internal class PatientRec
    {
        public int VAERS_ID { get; set; }
        public string RECVDATE { get; set; } = String.Empty;
        public string STATE { get; set; } = String.Empty;
        public decimal AGE_YRS { get; set; }
        public string CAGE_YR { get; set; } = String.Empty;
        public string CAGE_MO { get; set; } = String.Empty;
        public string SEX { get; set; } = String.Empty;
        public string RPT_DATE { get; set; } = String.Empty;
        public string SYMPTOM_TEXT { get; set; } = String.Empty;
        public string DIED { get; set; } = String.Empty;
        public string DATEDIED { get; set; } = String.Empty;
        public string L_THREAT { get; set; } = String.Empty;
        public string ER_VISIT { get; set; } = String.Empty;
        public string HOSPITAL { get; set; } = String.Empty;
        public string HOSPDAYS { get; set; } = String.Empty;
        public string X_STAY { get; set; } = String.Empty;
        public string DISABLE { get; set; } = String.Empty;
        public string RECOVD { get; set; } = String.Empty;
        public string VAX_DATE { get; set; } = String.Empty;
        public string ONSET_DATE { get; set; } = String.Empty;
        public string NUMDAYS { get; set; } = String.Empty;
        public string LAB_DATA { get; set; } = String.Empty;
        public string V_ADMINBY { get; set; } = String.Empty;
        public string V_FUNDBY { get; set; } = String.Empty;
        public string OTHER_MEDS { get; set; } = String.Empty;
        public string CUR_ILL { get; set; } = String.Empty;
        public string HISTORY { get; set; } = String.Empty;
        public string PRIOR_VAX { get; set; } = String.Empty;
        public string SPLTTYPE { get; set; } = String.Empty;
        public string FORM_VERS { get; set; } = String.Empty;
        public string TODAYS_DATE { get; set; } = String.Empty;
        public string BIRTH_DEFECT { get; set; } = String.Empty;
        public string OFC_VISIT { get; set; } = String.Empty;
        public string ER_ED_VISIT { get; set; } = String.Empty;
        public string ALLERGIES { get; set; } = String.Empty;
    }
}