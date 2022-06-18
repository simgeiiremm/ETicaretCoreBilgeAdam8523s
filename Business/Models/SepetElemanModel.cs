using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class SepetElemanModel
    {
        public int UrunId { get; set; }
        public int KullaniciId { get; set; }
        public string UrunAdi { get; set; }
        public double BirimFiyati { get; set; }
    }
}
