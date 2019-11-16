using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2E.Importacao.Domain.Entities;
using P2E.Importacao.Domain.Repositories;
using P2E.Shared.Model;

namespace P2E.Importacao.API.Controllers
{
    public class TriagemBotController : ControllerBase
    {
        private readonly ITriagemBotRepository _TriagemBotRepository;

        public TriagemBotController(ITriagemBotRepository triagemBotRepository)
        {
            _TriagemBotRepository = triagemBotRepository;
        }

        // GET: api/Importacao
        [HttpGet]
        [Route("api/v1/triagembot/cd_par")]
        public List<TriagemBot> GetParceirosNeg()
        {
            var result = _TriagemBotRepository.FindAll().ToList();

            var clientes = from r in result.ToList()
                           group r by r.CD_PAR_NEG into grupo
                           select new TriagemBot()
                           {
                               CD_PAR_NEG = grupo.Key
                           };

            return clientes.ToList();
        }

        // GET: api/TriagemBot
        [HttpGet]
        [Route("api/v1/triagembot/despacho/{cd_par}")]
        public IEnumerable<TriagemBot> TriagemDespacho(int cd_par)
        {
            var result = _TriagemBotRepository.FindAll(i => i.OP_ACOMP_DESP_IMP == 0 && i.CD_PAR_NEG == cd_par);
            return result;
        }

        // GET: api/TriagemBot
        [HttpGet]
        [Route("api/v1/triagembot/comprovante-imp/{cd_par}")]
        public IEnumerable<TriagemBot> TriagemComprovImp(int cd_par)
        {
            var result = _TriagemBotRepository.FindAll(i => i.OP_COMPROV_IMP == 0 && i.CD_PAR_NEG == cd_par);
            return result;
        }

        // GET: api/TriagemBot
        [HttpGet]
        [Route("api/v1/triagembot/obter-exoneracao-icms/{cd_par}")]
        public IEnumerable<TriagemBot> TriagemExoneraICMS(int cd_par)
        {
            var result = _TriagemBotRepository.FindAll(i => i.OP_EXONERA_ICMS == 0 && i.CD_PAR_NEG == cd_par);
            return result;
        }

        // GET: api/TriagemBot
        [HttpGet]
        [Route("api/v1/triagembot/extrato-retif/{cd_par}")]
        public IEnumerable<TriagemBot> TriagemExtratoRetif(int cd_par)
        {
            var result = _TriagemBotRepository.FindAll(i => i.OP_EXTRATO_RETIF == 0 && i.CD_PAR_NEG == cd_par);
            return result;
        }

        // GET: api/TriagemBot
        [HttpGet]
        [Route("api/v1/triagembot/status-desembaraco/{cd_par}")]
        public IEnumerable<TriagemBot> TriagemStatusDesemb(int cd_par)
        {
            var result = _TriagemBotRepository.FindAll(i => i.OP_STATUS_DESEMB == 0 && i.CD_PAR_NEG == cd_par);
            return result;
        }

        // GET: api/TriagemBot
        [HttpGet]
        [Route("api/v1/triagembot/tela-debito/{cd_par}")]
        public IEnumerable<TriagemBot> TriagemTelaDebito(int cd_par)
        {
            var result = _TriagemBotRepository.FindAll(i => i.OP_TELA_DEBITO == 0 && i.CD_PAR_NEG == cd_par);
            return result;
        }

        // GET: api/TriagemBot
        [HttpGet]
        [Route("api/v1/triagembot/extrato-pdf-xml/{cd_par}")]
        public IEnumerable<TriagemBot> TriagemExtratoPdfXml(int cd_par)
        {
            var result = _TriagemBotRepository.FindAll(i => i.OP_EXTRATO_PDF_XML == 0 && i.CD_PAR_NEG == cd_par);
            return result;
        }
        // GET: api/TriagemBot
        [HttpGet]
        [Route("api/v1/triagembot/todos")]
        public IEnumerable<TriagemBot> Get()
        {
            var result = _TriagemBotRepository.FindAll();
            return result;
        }

        // GET: api/TriagemBot
        [HttpGet]
        [Route("api/v1/triagembot/")]
        public DataPage<TriagemBot> Get([FromQuery] string nr_di, [FromQuery] DataPage<TriagemBot> page)
        {
            page = _TriagemBotRepository.GetByPage(page, nr_di);

            return page;
        }

        // GET: api/TriagemBot/5
        [HttpGet]
        [Route("api/v1/triagembot/{id}")]
        public TriagemBot Get(long id)
        {
            TriagemBot TriagemBot = _TriagemBotRepository.Find(p => p.CD_TRIAGEM == id);

            return TriagemBot;
        }

        // POST: api/TriagemBot
        [HttpPost]
        [Route("api/v1/triagembot")]
        public object Post([FromBody] TriagemBot item)
        {
            try
            {
                _TriagemBotRepository.Insert(item);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }

        // PUT: api/TriagemBot/5
        [HttpPut]
        [Route("api/v1/triagembot/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] TriagemBot item)
        {
            try
            {
                if (id > 0)
                    _TriagemBotRepository.Update(item);
                else
                    _TriagemBotRepository.Insert(item);

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
                // return new { message = "Error." + ex.Message };
            }
        }

        // DELETE: api/TriagemBot/5
        [HttpDelete]
        [Route("api/v1/triagembot/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var objeto = _TriagemBotRepository.FindById(id);

                _TriagemBotRepository.Delete(objeto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao tentar excluir o registro. {ex.Message}");
            }
        }

        // DELETE: api/TriagemBot/5
        [HttpDelete]
        [Route("api/v1/triagembot/deleteall")]
        public IActionResult DeleteAll()
        {
            try
            {
                _TriagemBotRepository.DeleteAll();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao tentar excluir os registros. {ex.Message}");
            }
        }
    }
}