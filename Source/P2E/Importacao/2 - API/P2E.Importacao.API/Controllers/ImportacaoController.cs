using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2E.Importacao.Domain.Entities;
using P2E.Importacao.Domain.Repositories;
using P2E.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace P2E.Importacao.API.Controllers
{
    public class ImportacaoController : ControllerBase
    {
        private readonly IImportacaoRepository _ImportacaoRepository;
        //private readonly IParceiroNegocioRepository _ParceiroNegocioRepository;

        public ImportacaoController(IImportacaoRepository ImportacaoRepository/*, IParceiroNegocioRepository parceiroNegocioRepository*/)
        {
            _ImportacaoRepository = ImportacaoRepository;
            //_ParceiroNegocioRepository = parceiroNegocioRepository;
        }

        // GET: api/Importacao
        [HttpGet]
        [Route("api/v1/importacao/todos")]
        public IEnumerable<TBImportacao> Get()
        {
            var result = _ImportacaoRepository.FindAll();
            return result;
        }

        // GET: api/Importacao
        [HttpGet]
        [Route("api/v1/importacao/cd_par")]
        public List<TBImportacao> GetParceirosNeg()
        {
            var result = _ImportacaoRepository.FindAll().ToList();

            var clientes = from r in result.ToList()
                           group r by r.CD_PAR into grupo
                           select new TBImportacao()
                           {
                               CD_PAR = grupo.Key
                           };

            return clientes.ToList();
        }

        // GET: api/Importacao/baixarextrato
        [HttpGet]
        [Route("api/v1/importacao/despacho/{cd_par}")]
        public IEnumerable<TBImportacao> AcompanhaDespacho(int cd_par)
        {
            var result = _ImportacaoRepository.FindAll(p => p.CD_PAR == cd_par);
            return result;
        }

        // GET: api/Importacao/baixarextrato
        [HttpGet]
        [Route("api/v1/importacao/tela-debito/{cd_par}")]
        public IEnumerable<TBImportacao> TelaDebito(int cd_par)
        {
            var result = _ImportacaoRepository.FindAll(p => p.OP_TELA_DEBITO == 0 && p.CD_PAR == cd_par);
            return result;
        }

        // GET: api/Importacao/status_desembaraco
        [HttpGet]
        [Route("api/v1/importacao/status-desembaraco/{cd_par}")]
        public IEnumerable<TBImportacao> StatusDesembaraco(int cd_par)
        {
            var result = _ImportacaoRepository.FindAll(p => p.OP_STATUS_DESEMB == 0 && p.CD_PAR == cd_par);
            return result;
        }

        // GET: api/Importacao/comprovante_imp
        [HttpGet]
        [Route("api/v1/importacao/comprovante-imp/{cd_par}")]
        public IEnumerable<TBImportacao> ComprovanteImp(int cd_par)
        {
            var result = _ImportacaoRepository.FindAll(p => p.OP_COMPROVANTE_IMP == 0 && p.CD_PAR == cd_par);
            return result;
        }

        // GET: api/Importacao/baixarextrato
        [HttpGet]
        [Route("api/v1/importacao/extrato-pdf-xml/{cd_par}")]
        public IEnumerable<TBImportacao> ExtratoPdf(int cd_par)
        {
            var result = _ImportacaoRepository.FindAll(p => p.CD_PAR == cd_par);
            return result;
        }

        // GET: api/Importacao/baixarextrato
        [HttpGet]
        [Route("api/v1/importacao/extrato-retif/{cd_par}")]
        public IEnumerable<TBImportacao> ExtratoRetificacao(int cd_par)
        {
            var result = _ImportacaoRepository.FindAll(p => p.OP_EXTRATO_RETIF == 0 && p.CD_PAR == cd_par);
            return result;
        }

        /// <summary>
        /// Obtem uma lista de DI's que ainda não passaram pelo processo de exoneração de ICMS
        /// </summary>
        /// <returns></returns>
        // GET: api/Importacao
        [HttpGet]
        [Route("api/v1/importacao/obter-exoneracao-icms/{cd_par}")]
        public IEnumerable<TBImportacao> ObterExoneracaoIcms(int cd_par)
        {
            var result = _ImportacaoRepository.FindAll(p => p.CD_PAR == cd_par);
            return result;
        }

        // GET: api/Importacao
        [HttpGet]
        [Route("api/v1/importacao/")]
        public DataPage<TBImportacao> Get([FromQuery] string tx_dsc, [FromQuery] DataPage<TBImportacao> page)
        {
            page = _ImportacaoRepository.GetByPage(page, tx_dsc);

            return page;
        }

        // GET: api/Importacao/5
        [HttpGet]
        [Route("api/v1/importacao/{id}")]
        public TBImportacao Get(long id)
        {
            TBImportacao Importacao = _ImportacaoRepository.Find(p => p.CD_IMP == id);

            return Importacao;
        }

        // POST: api/Importacao
        [HttpPost]
        [Route("api/v1/importacao")]
        public object Post([FromBody] TBImportacao item)
        {
            try
            {
                _ImportacaoRepository.Insert(item);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }

        // PUT: api/Importacao/5
        [HttpPut]
        [Route("api/v1/importacao/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] TBImportacao item)
        {
            try
            {
                if (id > 0)
                    _ImportacaoRepository.Update(item);
                else
                    _ImportacaoRepository.Insert(item);

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
                // return new { message = "Error." + ex.Message };
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        [Route("api/v1/importacao/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var objeto = _ImportacaoRepository.FindById(id);

                _ImportacaoRepository.Delete(objeto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao tentar excluir o registro. {ex.Message}");
            }
        }
    }
}
