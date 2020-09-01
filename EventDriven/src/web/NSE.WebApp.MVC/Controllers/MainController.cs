using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace NSE.WebApp.MVC.Controllers
{
    public class MainController : Controller
    {
        protected bool ResponsePossuiErros(ResponseResult resposta)
        {
            if (resposta != null && resposta.errors.Mensagens.Any()) {

                foreach (var mensagem in resposta.errors.Mensagens)
                    ModelState.AddModelError(string.Empty,mensagem);

                return true;
            }

            return false;
        }

       

    }
}
