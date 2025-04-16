using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC365_Project2.Models
{
    internal class VaxInfo
    {
        public int VAERS_ID { get; set; }
        public string VAX_TYPE { get; set; } = String.Empty;
        public string VAX_MANU { get; set; } = String.Empty;
        public string VAX_LOT { get; set; } = String.Empty;
        public string VAX_DOSE_SERIES { get; set; } = String.Empty;
        public string VAX_ROUTE { get; set; } = String.Empty;
        public string VAX_SITE { get; set; } = String.Empty;
        public string VAX_NAME { get; set; } = String.Empty;

    }
}