using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2E.Importacao.Domain.Entities;
using P2E.Importacao.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Net;


namespace P2E.Importacao.API.Controllers
{
    public class EnvioPLIController : ControllerBase
    {
        private readonly IEnvioPLIRepository _envioPLIRepository;

        public EnvioPLIController(IEnvioPLIRepository envioPLIRepository)
        {
            _envioPLIRepository = envioPLIRepository;
        }

        // GET: api/enviopli
        [HttpGet]
        [Route("api/v1/enviopli/todos")]
        public IEnumerable<EnvioPLI> Get()
        {
            var result = _envioPLIRepository.FindAll();
            return result;
        }

        // GET: api/enviopli
        [HttpGet]
        [Route("api/v1/enviopli/pendentes")]
        public IEnumerable<EnvioPLI> Pendentes()
        {
            var result = _envioPLIRepository.FindAll(p => p.OP_STATUS == 0);
            return result;
        }


        // GET: api/enviopli/5
        [HttpGet]
        [Route("api/v1/enviopli/{id}")]
        public EnvioPLI Get(int id)
        {
            //carregar as informações da importação
            EnvioPLI vistoria = _envioPLIRepository.Find(p => p.CD_ENV_PLI == id);

            return vistoria;
        }

        // POST: api/enviopli
        [HttpPost]
        [Route("api/v1/enviopli")]
        public object Post([FromBody] EnvioPLI item)
        {
            try
            {
                _envioPLIRepository.Insert(item);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }

        // PUT: api/enviopli/5
        [HttpPut]
        [Route("api/v1/enviopli/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] EnvioPLI item)
        {
            try
            {
                if (id > 0)
                    _envioPLIRepository.Update(item);
                else
                    _envioPLIRepository.Insert(item);

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // DELETE: api/enviopli/5
        [HttpDelete]
        [Route("api/v1/enviopli/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var objeto = _envioPLIRepository.Find(p => p.CD_ENV_PLI == id);

                _envioPLIRepository.Delete(objeto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao tentar excluir o registro. {ex.Message}");
            }
        }
    }
}
