using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MvcWebUI.Areas.Sepet.Controllers
{
    [Area("Sepet")]
    [Authorize]
    public class HomeController : Controller
    {
        const string _key = "sepet";
        private readonly IUrunService _urunService;

        public HomeController(IUrunService urunService)
        {
            _urunService = urunService; 
        }
        public IActionResult Ekle(int urunId)
        {
            int kullaniciId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            UrunModel urun = _urunService.Query().SingleOrDefault(u => u.Id == urunId);
            List<SepetElemanModel> sepet = GetSession();
            SepetElemanModel eleman = new SepetElemanModel()
            {
                UrunId = urun.Id,
                KullaniciId = kullaniciId,
                UrunAdi = urun.Adi,
                BirimFiyati = urun.BirimFiyati ?? 0
            };
            sepet.Add(eleman);
            SetSession(sepet);
            TempData["SepetMessage"] = "Ürün başarıyla sepete eklendi.";
            return RedirectToAction("Index", "Urunler", new {area = ""});          
        }
        public IActionResult Getir()
        {
            int kullaniciId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            List<SepetElemanModel> sepet = GetSession(); 
            
            
            List<SepetElemanModel> kullaniciSepet = sepet.Where(s => s.KullaniciId == kullaniciId).ToList();

            //not: group by
            List<SepetElemanGroupByModel> kullaniciSepetGroupBy =
                (from ks in kullaniciSepet
                    //group ks by ks.UrunAdi bu şekilde yapamayız birden fazla özellik sorgulamamız gerektiği için
                group ks by new { ks.UrunId, ks.KullaniciId, ks.UrunAdi }
                into ksGroupBy
                select new SepetElemanGroupByModel()
                {
                    UrunId = ksGroupBy.Key.UrunId,
                    KullaniciId = ksGroupBy.Key.KullaniciId,
                    UrunAdi = ksGroupBy.Key.UrunAdi,
                    ToplamBirimFiyati = ksGroupBy.Sum(ksg => ksg.BirimFiyati),
                    ToplamBirimFiyatiDisplay = ksGroupBy.Sum(ksg => ksg.BirimFiyati).ToString("C2"),
                    UrunSayisi = ksGroupBy.Count()
                }).ToList();

            //return View(kullaniciSepet);
            return View("GrupGetir", kullaniciSepetGroupBy);
        }
        public IActionResult Sil(int urunId, int kullaniciId)
        {
            List<SepetElemanModel> sepet = GetSession();
            SepetElemanModel eleman = sepet.FirstOrDefault(s => s.UrunId == urunId && s.KullaniciId == kullaniciId);
            sepet.Remove(eleman);
            SetSession(sepet);
            TempData["SepetMessage"] = "Ürün sepetten silindi.";
            return RedirectToAction(nameof(Getir));   
        }
        public IActionResult Temizle()
        {
            int kullaniciId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            List<SepetElemanModel> sepet = GetSession();
            List<SepetElemanModel> kullaniciSepeti = sepet.Where(s => s.KullaniciId == kullaniciId).ToList();
            foreach (SepetElemanModel kullaniciEleman in kullaniciSepeti)
            {
                sepet.Remove(kullaniciEleman);
            }
            SetSession(sepet);
            TempData["SepetMessage"] = "Sepet temizlendi.";
            return RedirectToAction(nameof(Getir));
           
        }
        public List<SepetElemanModel> GetSession()
        {
            List<SepetElemanModel> sepet = new List<SepetElemanModel>();
            string sepetJson = HttpContext.Session.GetString(_key);
            if(sepetJson != null)
                sepet = JsonConvert.DeserializeObject<List<SepetElemanModel>>(sepetJson);
            return sepet;              
        }
        private void SetSession(List<SepetElemanModel> sepet)
        {
            string sepetJson = JsonConvert.SerializeObject(sepet);
            HttpContext.Session.SetString(_key, sepetJson);
        }
    }
}
