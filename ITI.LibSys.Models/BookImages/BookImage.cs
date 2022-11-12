using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.LibSys.Models
{
    public class BookImage
    {
        public int ID { get; set; }
        public int BookID { get; set; }
        public string Path { get; set; }
        public virtual Book Book { get; set; }
    }
}
