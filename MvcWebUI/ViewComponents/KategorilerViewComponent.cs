using Business.Services.Bases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace MvcWebUI.ViewComponents
{
    public class KategorilerViewComponent: ViewComponent
    {
        private IKategoriService _kategoriService;

        public KategorilerViewComponent(IKategoriService kategoriService)
        {
            _kategoriService = kategoriService;
        }

        public ViewViewComponentResult Invoke()
        {
            var task = _kategoriService.GetCategoriesAsync();
            var result = task.Result;
            var kategoriler = result.Data;
            return View(kategoriler);
        }
    }
}
