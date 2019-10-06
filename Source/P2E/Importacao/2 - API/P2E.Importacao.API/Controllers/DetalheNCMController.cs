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
    public class DetalheNCMController : ControllerBase
    {
        private readonly IDetalheNCMRepository _detalheNCMRepository;

        public DetalheNCMController(IDetalheNCMRepository detalheNCMRepository)
        {
            _detalheNCMRepository = detalheNCMRepository;
        }

        // GET: api/DetalheNCM
        [HttpGet]
        [Route("api/v1/ncm/todos")]
        public IEnumerable<DetalheNCM> Get()
        {
            var result = _detalheNCMRepository.FindAll();
            return result;
        }

        // GET: api/DetalheNCM
        [HttpGet]
        [Route("api/v1/ncm/")]
        public DataPage<DetalheNCM> Get([FromQuery] string tx_dsc, [FromQuery] DataPage<DetalheNCM> page)
        {
            page = _detalheNCMRepository.GetByPage(page, tx_dsc);

            return page;
        }

        // GET: api/DetalheNCM/5
        [HttpGet]
        [Route("api/v1/ncm/{id}")]
        public DetalheNCM Get(long id)
        {
            DetalheNCM DetalheNCM = _detalheNCMRepository.Find(p => p.CD_DET_NCM == id);

            return DetalheNCM;
        }

        // POST: api/DetalheNCM
        [HttpPost]
        [Route("api/v1/ncm")]
        public object Post([FromBody] DetalheNCM item)
        {
            try
            {
                _detalheNCMRepository.Insert(item);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }

        // PUT: api/DetalheNCM/5
        [HttpPut]
        [Route("api/v1/ncm/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] DetalheNCM item)
        {
            try
            {
                if (id > 0)
                    _detalheNCMRepository.Update(item);
                else
                    _detalheNCMRepository.Insert(item);

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
        [Route("api/v1/ncm/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var objeto = _detalheNCMRepository.FindById(id);

                _detalheNCMRepository.Delete(objeto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao tentar excluir o registro. {ex.Message}");
            }
        }
    }
}
