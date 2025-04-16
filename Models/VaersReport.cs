using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC365_Project2.Models
{
    public class VaersReport
    {
        public int VAERS_ID { get; set; }
        public string RECVDATE { get; set; }
        public string STATE { get; set; }
        public decimal? AGE_YRS { get; set; }
        public string CAGE_YR { get; set; }
        public string CAGE_MO { get; set; }
        public string SEX { get; set; }
        public string RPT_DATE { get; set; }
        public string SYMPTOM_TEXT { get; set; }
        public string DIED { get; set; }
        public string DATEDIED { get; set; }
        public string L_THREAT { get; set; }
        public string ER_VISIT { get; set; }
        public string HOSPITAL { get; set; }
        public string HOSPDAYS { get; set; }
        public string X_STAY { get; set; }
        public string DISABLE { get; set; }
        public string RECOVD { get; set; }
        public string VAX_DATE { get; set; }
        public string ONSET_DATE { get; set; }
        public string NUMDAYS { get; set; }
        public string LAB_DATA { get; set; }
        public string V_ADMINBY { get; set; }
        public string V_FUNDBY { get; set; }
        public string OTHER_MEDS { get; set; }
        public string CUR_ILL { get; set; }
        public string HISTORY { get; set; }
        public string PRIOR_VAX { get; set; }
        public string SPLTTYPE { get; set; }
        public string FORM_VERS { get; set; }
        public string TODAYS_DATE { get; set; }
        public string BIRTH_DEFECT { get; set; }
        public string OFC_VISIT { get; set; }
        public string ER_ED_VISIT { get; set; }
        public string ALLERGIES { get; set; }
        public string VAX_TYPE { get; set; }
        public string VAX_MANU { get; set; }
        public string VAX_LOT { get; set; }
        public string VAX_DOSE_SERIES { get; set; }
        public string VAX_ROUTE { get; set; }
        public string VAX_SITE { get; set; }
        public string VAX_NAME { get; set; }
        public string SYMPTOM1 { get; set; }
        public string SYMPTOMVERSION1 { get; set; }
        public string SYMPTOM2 { get; set; }
        public string SYMPTOMVERSION2 { get; set; }
        public string SYMPTOM3 { get; set; }
        public string SYMPTOMVERSION3 { get; set; }
        public string SYMPTOM4 { get; set; }
        public string SYMPTOMVERSION4 { get; set; }
        public string SYMPTOM5 { get; set; }
        public string SYMPTOMVERSION5 { get; set; }
    }

}
