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
    public class HistoricoController : ControllerBase
    {
        private readonly IHistoricoRepository _historicoRepository;

        public HistoricoController(IHistoricoRepository historicoRepository)
        {
            _historicoRepository = historicoRepository;
        }

        // GET: api/Historico
        [HttpGet]
        [Route("api/v1/historico/todos")]
        public IEnumerable<Historico> Get()
        {
            var result = _historicoRepository.FindAll();
            return result;
        }

        // GET: api/Historico
        [HttpGet]
        [Route("api/v1/historico/")]
        public DataPage<Historico> Get([FromQuery] string tx_dsc, [FromQuery] DataPage<Historico> page)
        {
            page = _historicoRepository.GetByPage(page, tx_dsc);

            return page;
        }

        // GET: api/Historico/5
        [HttpGet]
        [Route("api/v1/historico/{id}")]
        public Historico Get(long id)
        {
            Historico historico = _historicoRepository.Find(p => p.CD_HIST == id);

            return historico;
        }

        // POST: api/Historico
        [HttpPost]
        [Route("api/v1/historico")]
        public object Post([FromBody] Historico item)
        {
            try
            {
                _historicoRepository.Insert(item);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }

        // PUT: api/Historico/5
        [HttpPut]
        [Route("api/v1/historico/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] Historico item)
        {
            try
            {
                if (id > 0)
                    _historicoRepository.Update(item);
                else
                    _historicoRepository.Insert(item);

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
        [Route("api/v1/historico/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var objeto = _historicoRepository.FindById(id);

                _historicoRepository.Delete(objeto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao tentar excluir o registro. {ex.Message}");
            }
        }
    }
}
