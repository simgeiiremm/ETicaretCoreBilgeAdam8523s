using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class SepetElemanGroupByModel
    {
        public int UrunId { get; set; }
        public int KullaniciId { get; set; }
        [DisplayName("Ürün Adı")]
        public string UrunAdi { get; set; }
        [DisplayName("Toplam Birim Fiyat")]
        public string ToplamBirimFiyatiDisplay { get; set; }
        public double ToplamBirimFiyati { get; set; }
        [DisplayName("Ürün Sayısı")]
        public int UrunSayisi { get; set; }

    }
}
