using ApiZero.Modelo;
using ApiZero.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiZero.Controllers
{
    [RoutePrefix("api/apizero/v1")]
    public class HomeController : ApiController
    {
      //  [Authorize(Roles = "Admin,User")]
        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        [Route("usuariologado")]
        // api/apizero/v1/usuariologado
        public HttpResponseMessage GetUsuarioLogado()
        {
            var usuarioLogado = "Usuário logado: " + User.Identity.Name;
            return Request.CreateResponse(HttpStatusCode.OK, usuarioLogado);
        }

        [HttpGet]
        [Route("datahoraatual")]
        // api/apizero/v1/datahoraatual
        public HttpResponseMessage GetDataHora()
        {
            var dataHoraAtual = "Data/Hora atual: " + DateTime.Now.ToString();
            return Request.CreateResponse(HttpStatusCode.OK, dataHoraAtual);
        }

        [HttpPost]
        [Route("gravarlote")]
        // api/apizero/v1/gravarlote
        public HttpResponseMessage GravarLoteRegistrosBanco([FromBody] Quota[] quotas)
        {
            DAL_BRQ acessoDados = new DAL_BRQ();

            var result = acessoDados.GravarLoteRegistrosBanco(quotas.ToList());

            if (result)
               return Request.CreateResponse(HttpStatusCode.OK, result);

            return Request.CreateResponse(HttpStatusCode.InternalServerError, "Falha ao gravar registros em lote");
        }

        [HttpGet]
        [Route("recuperarlote/{patharquivo}")]
        // api/apizero/v1/recuperarlote
        public HttpResponseMessage RecuperarLoteRegistrosBanco(string pathArquivo)
        {
            DAL_BRQ acessoDados = new DAL_BRQ();

            pathArquivo = !pathArquivo.Contains(".TXT") ? pathArquivo + ".TXT" : pathArquivo;

            var result = acessoDados.RecuperarRegistrosBanco(pathArquivo);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


    }
}
