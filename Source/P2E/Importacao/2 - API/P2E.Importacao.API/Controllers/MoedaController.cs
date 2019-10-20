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
    public class MoedaController : ControllerBase
    {
        private readonly IMoedaRepository _moedaRepository;

        public MoedaController(IMoedaRepository MoedaRepository)
        {
            _moedaRepository = MoedaRepository;
        }

        // GET: api/Moeda
        [HttpGet]
        [Route("api/v1/moeda/todos")]
        public IEnumerable<Moeda> Get()
        {
            var result = _moedaRepository.FindAll();
            return result;
        }

        // GET: api/Moeda
        [HttpGet]
        [Route("api/v1/moeda/")]
        public DataPage<Moeda> Get([FromQuery] string tx_dsc, [FromQuery] DataPage<Moeda> page)
        {
            page = _moedaRepository.GetByPage(page, tx_dsc);

            return page;
        }

        // GET: api/Moeda/5
        [HttpGet]
        [Route("api/v1/moeda/{id}")]
        public Moeda Get(long id)
        {
            Moeda Moeda = _moedaRepository.Find(p => p.CD_MOEDA == id);

            return Moeda;
        }

        // POST: api/Moeda
        [HttpPost]
        [Route("api/v1/moeda")]
        public object Post([FromBody] Moeda item)
        {
            try
            {
                _moedaRepository.Insert(item);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }

        // PUT: api/Moeda/5
        [HttpPut]
        [Route("api/v1/moeda/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] Moeda item)
        {
            try
            {
                if (id > 0)
                    _moedaRepository.Update(item);
                else
                    _moedaRepository.Insert(item);

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
        [Route("api/v1/moeda/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var objeto = _moedaRepository.FindById(id);

                _moedaRepository.Delete(objeto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao tentar excluir o registro. {ex.Message}");
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        [Route("api/v1/moeda/deleteall")]
        public IActionResult DeleteAll()
        {
            try
            {
                _moedaRepository.DeleteAll();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao tentar excluir os registros. {ex.Message}");
            }
        }
    }
}