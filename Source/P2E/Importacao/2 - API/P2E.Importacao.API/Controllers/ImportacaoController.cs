using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2E.Importacao.Domain.Entities;
using P2E.Importacao.Domain.Repositories;
using P2E.Shared.Model;
using System;
using System.Collections.Generic;
using System.Net;

namespace P2E.Importacao.API.Controllers
{
    public class ImportacaoController : ControllerBase
    {
        private readonly IImportacaoRepository _ImportacaoRepository;

        public ImportacaoController(IImportacaoRepository ImportacaoRepository)
        {
            _ImportacaoRepository = ImportacaoRepository;
        }

        // GET: api/Importacao
        [HttpGet]
        [Route("api/v1/importacao/todos")]
        public IEnumerable<TBImportacao> Get()
        {
            var result = _ImportacaoRepository.FindAll();
            return result;
        }

        // GET: api/Importacao/baixarextrato
        [HttpGet]
        [Route("api/v1/importacao/tela-debito")]
        public IEnumerable<TBImportacao> TelaDebito()
        {
            var result = _ImportacaoRepository.FindAll(p => p.OP_TELA_DEBITO == 0);
            return result;
        }

        // GET: api/Importacao/status_desembaraco
        [HttpGet]
        [Route("api/v1/importacao/status-desembaraco")]
        public IEnumerable<TBImportacao> StatusDesembaraco()
        {
            var result = _ImportacaoRepository.FindAll(p => p.OP_STATUS_DESEMB == 0);
            return result;
        }

        // GET: api/Importacao/comprovante_imp
        [HttpGet]
        [Route("api/v1/importacao/comprovante-imp")]
        public IEnumerable<TBImportacao> ComprovanteImp()
        {
            var result = _ImportacaoRepository.FindAll(p => p.OP_COMPROVANTE_IMP == 0);
            return result;
        }

        // GET: api/Importacao/baixarextrato
        [HttpGet]
        [Route("api/v1/importacao/extrato-pdf-xml")]
        public IEnumerable<TBImportacao> ExtratoPdf()
        {
            var result = _ImportacaoRepository.FindAll(p => p.OP_EXTRATO_PDF == 0 || p.OP_EXTRATO_XML == 0);
            return result;
        }

        // GET: api/Importacao/baixarextrato
        [HttpGet]
        [Route("api/v1/importacao/extrato-retif")]
        public IEnumerable<TBImportacao> ExtratoRetificacao()
        {
            var result = _ImportacaoRepository.FindAll(p => p.OP_EXTRATO_RETIF == 0);
            return result;
        }

        /// <summary>
        /// Obtem uma lista de DI's que ainda não passaram pelo processo de exoneração de ICMS
        /// </summary>
        /// <returns></returns>
        // GET: api/Importacao
        [HttpGet]
        [Route("api/v1/importacao/obter-exoneracao-icms")]
        public IEnumerable<TBImportacao> ObterExoneracaoIcms()
        {
            var result = _ImportacaoRepository.FindAll();
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
