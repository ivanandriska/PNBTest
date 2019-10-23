using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMS.Api.Models;
using System;
using System.Data;
using Newtonsoft.Json;

namespace SMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        private readonly SmsContext _context;

        public SmsController(SmsContext context)
        {
            _context = context;

            if (_context.SmsItems.Count() == 0)
            {
                // Create a new SmsItem if collection is empty,
                // wich means you can't delete all SmsItems.
                _context.SmsItems.Add(new SmsItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        [HttpGet("teste")]
        public ActionResult<Models.SmsResposta> teste()
        {
            Models.SmsResposta modResposta = new SmsResposta();
            modResposta.resposta = "Sucess";

            return modResposta;
        }

        //api/sms/retornoStatus?SeuNum=XX&Celular=0&status=XX&Data=XX&Operadora=XX
        [HttpGet("retornoStatus")]
        public ActionResult<Models.SmsResposta> GetRetornoStatus(string SeuNum, long Celular, string status, string Data, string Operadora)
        {
            // Checando Campos de Entrada
            if (SeuNum == null || SeuNum.Trim().Length <= 0 ||
                status.Trim().Length <= 0 ||
                Data.Trim().Length <= 0)
            { 
                return BadRequest();
            }

            Object[] param = new Object[5];

            param[0] = SeuNum;
            param[1] = Celular;
            param[2] = status;
            param[3] = Convert.ToDateTime(Data);
            param[4] = Operadora;

            System.Threading.Tasks.Task.Factory.StartNew(new Processar().processarRetornoStatus, param);

            Models.SmsResposta modResposta = new SmsResposta();
            modResposta.resposta = "Sucess";

            return modResposta;
        }

        //api/sms/retornoResposta?SeuNum=XX&Celular=0&Texto=XX&ShortCode=XX&Operadora=XX&Data=XX
        [HttpGet("retornoResposta")]
        public ActionResult<Models.SmsResposta> GetRetornoResposta(string SeuNum, long Celular, string Texto, string ShortCode, string Operadora, string Data)
        {

            // Checando Campos de Entrada
            if (SeuNum == null || SeuNum.Trim().Length <= 0 ||
                Texto.Trim().Length <= 0 ||
                Data.Trim().Length <= 0)
            {
                return BadRequest();
            }

            Object[] param = new Object[6];

            param[0] = SeuNum;
            param[1] = Celular;
            param[2] = Texto;
            param[3] = ShortCode;
            param[4] = Convert.ToDateTime(Data);
            param[5] = Operadora;

            System.Threading.Tasks.Task.Factory.StartNew(new Processar().processarRetornoResposta, param);


            Models.SmsResposta modResposta = new SmsResposta();
            modResposta.resposta = "Sucess";

            return modResposta;
        }

        //api/sms/processarAguardandoEnvio
        [HttpPost("processarAguardandoEnvio")]
        public ActionResult<Models.SmsResposta> processarAguardandoEnvio([FromBody] CaseBusiness.CC.Mensageria.Mensagem mensagemAEnviar)
        {
            // Checando Campos de Entrada
            if (mensagemAEnviar == null)
                return BadRequest();

            new CaseBusiness.CC.Mensageria.Mensagem().ProcessarAguardandoEnvioIndividual(mensagemAEnviar);

            Models.SmsResposta modResposta = new SmsResposta();
            modResposta.resposta = "Sucess";

            return modResposta;
        }


        //api/sms/requisitarEnvioMensagem
        [HttpPost("requisitarEnvioMensagem")]
        public ActionResult<Models.SmsResposta> requisitarEnvioMensagem([FromBody] CaseBusiness.CC.Mensageria.MensagemRequisicao requisitarEnvio)
        {
            // Checando Campos de Entrada
            if (requisitarEnvio == null )
                return BadRequest();

            new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).RequisitarEnvioMensagem(requisitarEnvio);

            Models.SmsResposta modResposta = new SmsResposta();
            modResposta.resposta = "Sucess";

            return modResposta;
        }
    }
}

