﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Contexts;
using DataAccess.Entities;
using Business.Services;
using Business.Services.Bases;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using MvcWebUI.Models;
using Business.Models.Filters;
using MvcWebUI.Settings;

namespace MvcWebUI.Controllers
{
    [Authorize]
    public class UrunlerController : Controller
    {
        //private readonly ETicaretContext _context;

        //public UrunlerController(ETicaretContext context)
        //{
        //    _context = context;
        //}
        private readonly IUrunService _urunService;
        private readonly IKategoriService _kategoriService;
        private readonly IMagazaService _magazaService;

        public UrunlerController(IUrunService urunService, IKategoriService kategoriService, IMagazaService magazaService)
        {
            _urunService = urunService;
            _kategoriService = kategoriService;
            _magazaService = magazaService;
        }

        // GET: Urunler
        //public IActionResult Index()
        //{
        //    var eTicaretContext = _context.Urunler.Include(u => u.Kategori);
        //    return View(eTicaretContext.ToList());
        //}
        [AllowAnonymous]
        public IActionResult Index(UrunlerIndexViewModel viewModel)
        {
            var result = _urunService.List(viewModel.Filtre, viewModel.SayfaNo, AppSettings.SayfadakiKayitSayisi, viewModel.Expression, viewModel.IsDirectionAscending);
            //UrunlerIndexViewModel viewModel = new UrunlerIndexViewModel()
            //{
            //    Urunler = result.Data,
            //    Kategoriler = new SelectList(_kategoriService.Query().ToList(), "Id", "Adi", filtre.KategoriId),
            //    Magazalar = new MultiSelectList(_magazaService.Query().ToList(), "Id", "Adi", filtre.MagazaIdleri),
            //    Filtre = filtre
            //};
            viewModel.Urunler = result.Data;
            viewModel.Kategoriler = new SelectList(_kategoriService.Query().ToList(), "Id", "Adi");
            viewModel.Magazalar = new MultiSelectList(_magazaService.Query().ToList(), "Id", "Adi");
            viewModel.ToplamKayitSayisi = _urunService.GetTotalRecordsCount(viewModel.Filtre);
            var sayfalar = _urunService.GetPages(viewModel.ToplamKayitSayisi, AppSettings.SayfadakiKayitSayisi);
            viewModel.Sayfalar = new SelectList(sayfalar, viewModel.SayfaNo);
            viewModel.Siralar = new SelectList(_urunService.GetExpressions());
            return View(viewModel);
        }

        // GET: Urunler/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                //return NotFound();
                return View("Hata");
            }

            UrunModel urun = _urunService.Query().SingleOrDefault(u => u.Id == id.Value);
            if (urun == null)
            {
                return View("Hata", "Ürün bulunamadı!");
            }

            return View(urun);
        }

        // GET: Urunler/Create
        //public IActionResult Create()
        //{
        //    ViewData["KategoriId"] = new SelectList(_context.Kategoriler, "Id", "Adi");
        //    return View();
        //}
        //[HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["KategoriId"] = new SelectList(_kategoriService.Query().ToList(), "Id", "Adi"); // dropdownlist
            ViewBag.Magazalar = new MultiSelectList(_magazaService.Query().ToList(), "Id", "Adi"); // listbox
            //return View();
            UrunModel model = new UrunModel()
            {
                SonKullanmaTarihi = DateTime.Today,
                BirimFiyati = 0,
                StokMiktari = 0
            };
            return View(model);
        }

        // POST: Urunler/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult Create(Urun urun)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(urun);
        //        _context.SaveChanges();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["KategoriId"] = new SelectList(_context.Kategoriler, "Id", "Adi", urun.KategoriId);
        //    return View(urun);
        //}
        [Authorize(Roles = "Admin")]
        public IActionResult Create(UrunModel urun, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (ImajDosyasiniGuncelle(urun, image) == false)
                {
                    ModelState.AddModelError("", $"Dosya yüklenemedi: Uzantılar {AppSettings.AcceptedImageExtensions} olmalı ve dosya boyutu maksiumum {AppSettings.AcceptedImageLength} MB olmalıdır!");
                }
                else
                {
                    var result = _urunService.Add(urun);
                    if (result.IsSuccessful)
                    {
                        TempData["Mesaj"] = result.Message;
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError("", result.Message);
                }
            }
            ViewData["KategoriId"] = new SelectList(_kategoriService.Query().ToList(), "Id", "Adi", urun.KategoriId);
            return View(urun);
        }

        // GET: Urunler/Edit/5
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var urun = _context.Urunler.Find(id);
        //    if (urun == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["KategoriId"] = new SelectList(_context.Kategoriler, "Id", "Adi", urun.KategoriId);
        //    return View(urun);
        //}
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id) // Urunler/Edit
        {
            if (id == null)
                return View("Hata", "Id gereklidir!");
            UrunModel urun = _urunService.Query().SingleOrDefault(u => u.Id == id);
            if (urun == null)
                return View("Hata", "Ürün bulunamadı!");
            ViewBag.KategoriId = new SelectList(_kategoriService.Query().ToList(), "Id", "Adi", urun.KategoriId);          
            return View(urun);
        }

        // POST: Urunler/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult Edit(Urun urun)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Update(urun);
        //        _context.SaveChanges();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["KategoriId"] = new SelectList(_context.Kategoriler, "Id", "Adi", urun.KategoriId);
        //    return View(urun);
        //}
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(UrunModel urun, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (ImajDosyasiniGuncelle(urun, image) == false)
                {
                    ModelState.AddModelError("", $"Dosya yüklenemedi: Uzantılar {AppSettings.AcceptedImageExtensions} olmalı ve dosya boyutu maksiumum {AppSettings.AcceptedImageLength} MB olmalıdır!");
                }
                else
                {
                    var result = _urunService.Update(urun);
                    if (result.IsSuccessful)
                        //return Redirect("https://bilgeadam.com");
                        return RedirectToAction(nameof(Index));
                    ModelState.AddModelError("", result.Message);
                }
            }
            ViewBag.KategoriId = new SelectList(_kategoriService.Query().ToList(), "Id", "Adi", urun.KategoriId);
            return View(urun);
        }
        private bool? ImajDosyasiniGuncelle(UrunModel model, IFormFile yuklenenImaj)
        {
            #region Dosya validasyonu
            bool? sonuc = null;
            string yuklenenDosyaAdi = null, yuklenenDosyaUzantisi = null;
            if (yuklenenImaj != null && yuklenenImaj.Length > 0) // yüklenen imaj verisi varsa
            {
                sonuc = false; // validasyonu geçemedi ilk değer ataması
                yuklenenDosyaAdi = yuklenenImaj.FileName; // asusrog.jpg
                yuklenenDosyaUzantisi = Path.GetExtension(yuklenenDosyaAdi); // .jpg
                string[] imajDosyaUzantilari = AppSettings.AcceptedImageExtensions.Split(',');
                foreach (string imajDosyaUzantisi in imajDosyaUzantilari)
                {
                    if (yuklenenDosyaUzantisi.ToLower() == imajDosyaUzantisi.ToLower().Trim())
                    {
                        sonuc = true; // imaj uzantısı validasyonunu geçti
                        break;
                    }
                }
                if (sonuc == true) // eğer imaj uzantısı validasyonunu geçtiyse imaj boyutunu valide edelim
                {
                    // 1 byte = 8 bits
                    // 1 kilobyte = 1024 bytes
                    // 1 megabyte = 1024 kilobytes = 1024 * 1024 bytes = 1.048.576 bytes
                    double imajDosyaBoyutu = AppSettings.AcceptedImageLength * Math.Pow(1024, 2); // bytes
                    if (yuklenenImaj.Length > imajDosyaBoyutu)
                        sonuc = false; // imaj boyutu validasyonunu geçemedi
                }
            }
            #endregion

            #region Dosyanın kaydedilmesi
            if (sonuc == true)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    yuklenenImaj.CopyTo(memoryStream);
                    model.Image = memoryStream.ToArray();
                    model.ImageExtension = yuklenenDosyaUzantisi;
                }
            }
            #endregion

            return sonuc;
        }


        // GET: Urunler/Delete/5
        //[Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (!(User.Identity.IsAuthenticated && User.IsInRole("Admin")))
                return RedirectToAction("YetkisizIslem", "Hesaplar");
            if (id == null)
            {
                return View("Hata", "Id gereklidir!");
            }

            var result = _urunService.Delete(id.Value);
            if (result.IsSuccessful)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        public IActionResult DeleteImage(int urunId)
        {
            _urunService.DeleteImage(urunId);
            return RedirectToAction(nameof(Details), new {id = urunId});
        }
	}
}
