using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NSE.Identidade.API.Controllers
{
    [ApiController]
    public abstract class MainController : Controller
    {
        public ICollection<string> Erros = new List<string>();

        protected IActionResult CustomResponse(object result = null)
        {
            if (OperacaoValida())
                return Ok(result);

            //Detalhes do problma que vc encontrou, boa pratica
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                {"mensagens",Erros.ToArray() }
            }));
        }

        protected IActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(x => x.Errors);

            foreach (var item in erros)
                AdicionarErroProcessamento(item.ErrorMessage);

            return CustomResponse();
        }

        protected bool OperacaoValida()
           => !Erros.Any();

        protected void AdicionarErroProcessamento(string erro)
           => Erros.Add(erro);

        protected void LimparErroProcessamento(string erro)
           => Erros.Clear();
        
    }
}
