using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTI2017_V2.Models
{
    public class RezervareModel
    {
        public int Id { get; set; }
        public int IdVacanta { get; set; }
        public int IdUser { get; set; }
        public DateTime datastart { get; set; }
        public DateTime dataend { get; set; }
        public int NrPersoane { get; set; }
        public int PretTotal { get; set; }
    }
}
