using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Services;
using System;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class CatalogoController : MainController
    {
        //private readonly ICatalogoService _catalogoService;
        private readonly ICatalogoServiceRefit _catalogoServiceRefit;

        public CatalogoController(ICatalogoServiceRefit catalogoServiceRefit)
        {
            _catalogoServiceRefit = catalogoServiceRefit;
        }

        [HttpGet]
        [Route("")]
        [Route("vitrine")]
        public async Task<IActionResult> Index()
        {
            var produtos = await _catalogoServiceRefit.ObterTodos();

            return View(produtos);
        }

        [HttpGet]
        [Route("produto-detalhe/{id}")]
        public async Task<IActionResult> ProdutoDetalhe(Guid id)
        {
            var produtos = await _catalogoServiceRefit.ObterPorId(id);

            return View(produtos);
        }

    }
}
