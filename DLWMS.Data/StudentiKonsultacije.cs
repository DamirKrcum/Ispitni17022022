using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLWMS.Data
{
    [Table("StudentiKonsultacije")]
    public class StudentiKonsultacije
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int PredmetId { get; set; }
        public DateTime Datum { get; set; }
        public string Napomena { get; set; }
        public Predmet _predmet { get; set; }
    }
}
