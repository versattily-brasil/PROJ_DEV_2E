using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace P2E.SSO.API.Controllers
{
    [Authorize]
    public class OperacaoController : ControllerBase
    {
        private readonly IOperacaoRepository _operacaoRepository;
        public OperacaoController(IOperacaoRepository operacaoRepository)
        {
            _operacaoRepository = operacaoRepository;
        }

        // GET: api/operacao
        [HttpGet]
        [Route("api/v1/operacao/todos")]
        public IEnumerable<Operacao> Get()
        {
            var result = _operacaoRepository.FindAll();
            return result;
        }

        // GET: api/operacao
        [HttpGet]
        [Route("api/v1/operacao/")]
        public DataPage<Operacao> Get([FromQuery] string tx_dsc, [FromQuery] DataPage<Operacao> page)
        {
            page = _operacaoRepository.GetByPage(page, tx_dsc);
            return page;
        }

        // GET: api/operacao/5
        [HttpGet]
        [Route("api/v1/operacao/{id}")]
        public Operacao Get(long id)
        {
            return _operacaoRepository.Find(p => p.CD_OPR == id);
        }

        // POST: api/operacao
        [HttpPost]
        [Route("api/v1/operacao")]
        public object Post([FromBody] Operacao item)
        {
            try
            {
                if (item.IsValid() && _operacaoRepository.ValidarDuplicidades(item))
                {
                    _operacaoRepository.Insert(item);
                    return new { message = "OK" };
                }
                else
                {
                    return new { message = item.Notifications.FirstOrDefault().Message };
                }

            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }

        // PUT: api/operacao/5
        [HttpPut]
        [Route("api/v1/operacao/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] Operacao item)
        {
            try
            {
                if (item.IsValid() && _operacaoRepository.ValidarDuplicidades(item))
                {
                    if (id > 0)
                        _operacaoRepository.Update(item);
                    else
                        _operacaoRepository.Insert(item);

                    return Ok(item);
                }
                else
                {
                    return BadRequest(item.Messages);
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // DELETE: api/operacao/5
        [HttpDelete]
        [Route("api/v1/operacao/{id}")]
        public object Delete(long id)
        {
            try
            {
                var item = _operacaoRepository.FindById(id);
                _operacaoRepository.Delete(item);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }
    }
}