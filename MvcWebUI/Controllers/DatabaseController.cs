﻿using DataAccess.Contexts;
using DataAccess.Entities;
using DataAccess.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace MvcWebUI.Controllers
{
    public class DatabaseController : Controller
    {
        //[NonAction]
        [Obsolete("Bu method artık kullanılmamaktadır!")]
        public IActionResult Seed() // ~/Database/Seed
        {
            //ETicaretContext db = new ETicaretContext();
            //// veritabanı kodları
            //db.Dispose();
            using (ETicaretContext db = new ETicaretContext())
            {
                // verileri silme
                var urunMagazaEntities = db.UrunMagazalar.ToList();
                db.UrunMagazalar.RemoveRange(urunMagazaEntities);

                var magazaEntities = db.Magazalar.ToList();
                db.Magazalar.RemoveRange(magazaEntities);

                var urunEntities = db.Urunler.ToList();
                db.RemoveRange(urunEntities);

                var kategoriEntities = db.Kategoriler.ToList();
                db.RemoveRange(kategoriEntities);

                var kullaniciDetayiEntities = db.KullaniciDetaylari.ToList();
                db.KullaniciDetaylari.RemoveRange(kullaniciDetayiEntities);

                var kullaniciEntities = db.Kullanicilar.ToList();
                db.Kullanicilar.RemoveRange(kullaniciEntities);

                var rolEntities = db.Roller.ToList();
                db.Roller.RemoveRange(rolEntities);

                var sehirEntities = db.Sehirler.ToList();
                db.Sehirler.RemoveRange(sehirEntities);

                var ulkeEntities = db.Ulkeler.ToList();
                db.Ulkeler.RemoveRange(ulkeEntities);

                if (kategoriEntities.Count > 0)
                {
                    db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Urunler', RESEED, 0)");
                    db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Kategoriler', RESEED, 0)");
                    db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Kullanicilar', RESEED, 0)");
                    db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Roller', RESEED, 0)");
                    db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Sehirler', RESEED, 0)");
                    db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Ulkeler', RESEED, 0)");
                    db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Magazalar', RESEED, 0)");
                }

                // verileri ekleme
                db.Kategoriler.Add(new Kategori()
                {
                    Adi = "Bilgisayar",
                    Urunler = new List<Urun>()
                    {
                        new Urun() // 1
                        {
                            Adi = "Dizüstü Bilgisayar",
                            BirimFiyati = 3000.5,
                            StokMiktari = 10,
                            SonKullanmaTarihi = new DateTime(2032, 1, 27)
                        },
                        new Urun() // 2
                        {
                            Adi = "Bilgisayar Faresi",
                            BirimFiyati = 20.5,
                            StokMiktari = 20,
                            SonKullanmaTarihi = DateTime.Parse("19.05.2027", new CultureInfo("tr-TR")) 
                            // İngilizce bölgesel ayar için: en-US, sadece tarih ve ondalık veri tipleri için CultureInfo kullanılmalı,
                            // ~/Program.cs içersinde tüm uygulama için tek seferde AppCore üzerinden tanımlanıp kullanılabilir.
                        },
                        new Urun()
                        {
                            Adi = "Bilgisayar Klavyesi",
                            BirimFiyati = 40,
                            StokMiktari = 21,
                            Aciklamasi = "Bilgisayar Bileşeni"
                        },
                         new Urun()
                        {
                            Adi = "Bilgisayar Monitörü",
                            BirimFiyati = 2500,
                            StokMiktari = 27,
                            Aciklamasi = "Bilgisayar Bileşeni"
                        }
                    }
                });
                db.Kategoriler.Add(new Kategori()
                {
                    Adi = "Ev Eğlence Sistemi",
                    Aciklamasi = "Ev Sinema Sistemleri ve Televizyonlar",
                    Urunler = new List<Urun>()
                    {
                        new Urun()
                        {
                            Adi = "Hoparlör",
                            BirimFiyati = 2500,
                            StokMiktari = 5
                        },
                        new Urun()
                        {
                            Adi = "Amfi",
                            BirimFiyati = 5000,
                            StokMiktari = 9
                        },
                        new Urun()
                        {
                            Adi = "Ekolayzer",
                            BirimFiyati = 1000,
                            StokMiktari = 11
                        }
                    }
                });

                db.Ulkeler.Add(new Ulke()
                {
                    Adi = "Türkiye",
                    Sehirler = new List<Sehir>()
                    {
                        new Sehir()
                        {
                            Adi = "Ankara"
                        },
                        new Sehir()
                        {
                            Adi = "İstanbul"
                        },
                        new Sehir()
                        {
                            Adi = "İzmir"
                        }
                    }
                });
                db.Ulkeler.Add(new Ulke()
                {
                    Adi = "Amerika Birleşik Devletleri",
                    Sehirler = new List<Sehir>()
                    {
                        new Sehir()
                        {
                            Adi = "New York"
                        },
                        new Sehir()
                        {
                            Adi = "Los Angeles"
                        }
                    }
                });

                db.SaveChanges();

                db.Magazalar.Add(new Magaza()
                {
                    Adi = "Hepsiburada",
                    SanalMi = true,
                    Puani = 5,
                    UrunMagazalar = new List<UrunMagaza>()
                    {
                        new UrunMagaza()
                        {
                            UrunId = db.Urunler.SingleOrDefault(u => u.Adi == "Dizüstü Bilgisayar").Id
                        },
                        new UrunMagaza()
                        {
                            UrunId = db.Urunler.SingleOrDefault(u => u.Adi == "Bilgisayar Monitörü").Id
                        }
                    }
                });

                db.SaveChanges();

                db.Roller.Add(new Rol()
                {
                    Adi = "Admin",
                    Kullanicilar = new List<Kullanici>()
                    {
                        new Kullanici()
                        {
                            KullaniciAdi = "cagil",
                            Sifre = "cagil",
                            AktifMi = true,
                            KullaniciDetayi = new KullaniciDetayi()
                            {
                                Adres = "Çankaya",
                                Cinsiyet = Cinsiyet.Erkek,
                                Eposta = "cagil@eticaret.com",
                                UlkeId = db.Ulkeler.SingleOrDefault(u => u.Adi == "Türkiye").Id,
                                SehirId = db.Sehirler.SingleOrDefault(s => s.Adi == "Ankara").Id
                            }
                        }
                    }
                });
                db.Roller.Add(new Rol()
                {
                    Adi = "Kullanıcı",
                    Kullanicilar = new List<Kullanici>()
                    {
                        new Kullanici()
                        {
                            KullaniciAdi = "leo",
                            Sifre = "leo",
                            AktifMi = true,
                            KullaniciDetayi = new KullaniciDetayi()
                            {
                                Adres = "Çankaya",
                                Cinsiyet = Cinsiyet.Erkek,
                                Eposta = "leo@eticaret.com",
                                UlkeId = db.Ulkeler.SingleOrDefault(u => u.Adi == "Amerika Birleşik Devletleri").Id,
                                SehirId = db.Sehirler.SingleOrDefault(s => s.Adi == "Los Angeles").Id
                            }
                        }
                    }
                });

                db.SaveChanges();
            }

            return Content("<label style=\"color:red;\"><b>İlk veriler başarıyla oluşturuldu.</b></label>", "text/html", Encoding.UTF8);
        }
    }
}
