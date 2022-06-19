﻿using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class UrunModel : RecordBase
    {
        // Fluent Validation, ekstra
        #region Entity
        [Required(ErrorMessage = "{0} gereklidir!")]
        [MinLength(2, ErrorMessage = "{0} minimum {1} karakter olmalıdır!")]
        [MaxLength(100, ErrorMessage = "{0} maksimum {1} karakter olmalıdır!")]
        [DisplayName("Ürün Adı")]
        public string Adi { get; set; }

        [StringLength(500, ErrorMessage = "{0} maksimum {1} karakter olmalıdır!")]
        [DisplayName("Açıklaması")]
        public string Aciklamasi { get; set; }

        [DisplayName("Birim Fiyatı")]
        [Required(ErrorMessage = "{0} gereklidir!")]
        [Range(0, double.MaxValue, ErrorMessage = "{0} {1} ile {2} arasında olmalıdır!")]
        public double? BirimFiyati { get; set; }

        [DisplayName("Stok Miktarı")]
        [Required(ErrorMessage = "{0} gereklidir!")]
        [Range(0, 1000000, ErrorMessage = "{0} {1} ile {2} arasında olmalıdır!")]
        public int? StokMiktari { get; set; }

        [DisplayName("Son Kullanma Tarihi")]
        public DateTime? SonKullanmaTarihi { get; set; }

        [DisplayName("Kategori")]
        [Required(ErrorMessage = "{0} gereklidir!")]
        public int? KategoriId { get; set; }
       
        public byte[] Image { get; set; }
        [StringLength(5)]
        public string ImageExtension { get; set; }


        #endregion

        #region Sayfanın ihtiyacı
        [DisplayName("Birim Fiyatı")]
        public string BirimFiyatiDisplay { get; set; }

        [DisplayName("Son Kullanma Tarihi")]
        public string SonKullanmaTarihiDisplay { get; set; }

        [DisplayName("Kategori")]
        public string KategoriAdiDisplay { get; set; }

        [DisplayName("Mağazalar")]
        //[Required]
        public List<int> MagazaIdleri { get; set; }

        [DisplayName("Mağazalar")]
        public string MagazaAdiDisplay { get; set; }

        public int MagazaId { get; set; }

        [DisplayName("İmaj")]
        public string ImgSrcDisplay { get; set; }
        #endregion
    }
}
