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
    [ApiController]
    public class DetalheNCMController : ControllerBase
    {
        private readonly IDetalheNCMRepository _DetalheNCMRepository;

        public DetalheNCMController(IDetalheNCMRepository DetalheNCMRepository)
        {
            _DetalheNCMRepository = DetalheNCMRepository;
        }

        // GET: api/DetalheNCM
        [HttpGet]
        [Route("api/v1/detalhencm/todos")]
        public IEnumerable<DetalheNCM> Get()
        {
            var result = _DetalheNCMRepository.FindAll();
            return result;
        }

        // GET: api/DetalheNCM
        [HttpGet]
        [Route("api/v1/detalhencm/")]
        public DataPage<DetalheNCM> Get([FromQuery] string tx_dsc, [FromQuery] DataPage<DetalheNCM> page)
        {
            page = _DetalheNCMRepository.GetByPage(page, tx_dsc);

            return page;
        }

        // GET: api/DetalheNCM/5
        [HttpGet]
        [Route("api/v1/detalhencm/{id}")]
        public DetalheNCM Get(long id)
        {
            DetalheNCM DetalheNCM = _DetalheNCMRepository.Find(p => p.CD_DET_NCM == id);

            return DetalheNCM;
        }

        // POST: api/DetalheNCM
        [HttpPost]
        [Route("api/v1/detalhencm")]
        public object Post([FromBody] DetalheNCM item)
        {
            try
            {
                _DetalheNCMRepository.Insert(item);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }

        // PUT: api/DetalheNCM/5
        [HttpPut]
        [Route("api/v1/detalhencm/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] DetalheNCM item)
        {
            try
            {
                if (id > 0)
                    _DetalheNCMRepository.Update(item);
                else
                    _DetalheNCMRepository.Insert(item);

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
        [Route("api/v1/detalhencm/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var objeto = _DetalheNCMRepository.FindById(id);

                _DetalheNCMRepository.Delete(objeto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao tentar excluir o registro. {ex.Message}");
            }
        }
    }
}
