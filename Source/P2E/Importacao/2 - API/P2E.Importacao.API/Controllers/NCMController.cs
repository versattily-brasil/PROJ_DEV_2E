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
    public class NCMController : ControllerBase
    {
        private readonly INCMRepository _ncmRepository;

        public NCMController(INCMRepository NCMRepository)
        {
            _ncmRepository = NCMRepository;
        }

        // GET: api/TaxaConversaoCambio
        [HttpGet]
        [Route("api/v1/listancm/nroncm/{id}")]
        public IEnumerable<NCM> NroNCM(string id)
        {
            var result = _ncmRepository.FindAll(i => i.TX_NRO_NCM == id);
            return result;
        }

        // GET: api/NCM
        [HttpGet]
        [Route("api/v1/listancm/todos")]
        public IEnumerable<NCM> Get()
        {
            var result = _ncmRepository.FindAll();
            return result;
        }

        // GET: api/NCM
        [HttpGet]
        [Route("api/v1/listancm/")]
        public DataPage<NCM> Get([FromQuery] string tx_dsc, [FromQuery] DataPage<NCM> page)
        {
            page = _ncmRepository.GetByPage(page, tx_dsc);

            return page;
        }

        // GET: api/NCM/5
        [HttpGet]
        [Route("api/v1/listancm/{id}")]
        public NCM Get(long id)
        {
            NCM NCM = _ncmRepository.Find(p => p.CD_NCM == id);

            return NCM;
        }

        // POST: api/NCM
        [HttpPost]
        [Route("api/v1/listancm")]
        public object Post([FromBody] NCM item)
        {
            try
            {
                _ncmRepository.Insert(item);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }

        // PUT: api/NCM/5
        [HttpPut]
        [Route("api/v1/listancm/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] NCM item)
        {
            try
            {
                if (id > 0)
                    _ncmRepository.Update(item);
                else
                    _ncmRepository.Insert(item);

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
        [Route("api/v1/listancm/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var objeto = _ncmRepository.FindById(id);

                _ncmRepository.Delete(objeto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao tentar excluir o registro. {ex.Message}");
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        [Route("api/v1/listancm/deleteall")]
        public IActionResult DeleteAll()
        {
            try
            {
                _ncmRepository.DeleteAll();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao tentar excluir os registros. {ex.Message}");
            }
        }
    }
}