using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2E.Importacao.Domain.Entities;
using P2E.Importacao.Domain.Repositories;
using P2E.Shared.Model;
using System;
using System.Collections.Generic;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace P2E.Importacao.API.Controllers
{
    public class VistoriaController : ControllerBase
    {
        private readonly IVistoriaRepository _vistoriaRepository;

        public VistoriaController(IVistoriaRepository vistoriaRepository)
        {
            _vistoriaRepository = vistoriaRepository;
        }

        // GET: api/Vistoria
        [HttpGet]
        [Route("api/v1/vistoria/todos")]
        public IEnumerable<Vistoria> Get()
        {
            var result = _vistoriaRepository.FindAll();
            return result;
        }

        // GET: api/Vistoria
        [HttpGet]
        [Route("api/v1/vistoria/")]
        public DataPage<Vistoria> Get([FromQuery] string tx_dsc, [FromQuery] DataPage<Vistoria> page)
        {
            page = _vistoriaRepository.GetByPage(page, tx_dsc);

            return page;
        }

        // GET: api/Vistoria/5
        [HttpGet]
        [Route("api/v1/vistoria/{id}")]
        public Vistoria Get(long id)
        {
            //carregar as informações da importação
            Vistoria vistoria = _vistoriaRepository.Find(p => p.CD_IMP == id);

            return vistoria;
        }

        // POST: api/Vistoria
        [HttpPost]
        [Route("api/v1/vistoria")]
        public object Post([FromBody] Vistoria item)
        {
            try
            {
                _vistoriaRepository.Insert(item);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }

        // PUT: api/Vistoria/5
        [HttpPut]
        [Route("api/v1/vistoria/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] Vistoria item)
        {
            try
            {
                if (id > 0)
                    _vistoriaRepository.Update(item);
                else
                    _vistoriaRepository.Insert(item);

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
        [Route("api/v1/vistoria/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var objeto = _vistoriaRepository.Find(p => p.CD_IMP == id);

                _vistoriaRepository.Delete(objeto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao tentar excluir o registro. {ex.Message}");
            }
        }
    }
}
