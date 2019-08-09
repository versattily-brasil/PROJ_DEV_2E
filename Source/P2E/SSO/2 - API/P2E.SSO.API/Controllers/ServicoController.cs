using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2E.Shared.Model;
using P2E.SSO.API.ViewModel;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;

namespace P2E.SSO.API.Controllers
{
    [ApiController]
    public class ServicoController : ControllerBase
    {
        private readonly IServicoRepository _servicoRepository;
        
        public ServicoController(IServicoRepository servicoRepository)
        {
            _servicoRepository = servicoRepository;
        }

        // GET: api/Modulo
        [HttpGet]
        [Route("api/v1/servico/todos")]
        public IEnumerable<Servico> Get()
        {
            var result = _servicoRepository.FindAll();
            return result;
        }

        // GET: api/Servico
        [HttpGet]
        [Route("api/v1/servico/")]
        public DataPage<Servico> Get([FromQuery] string txt_dec, [FromQuery] DataPage<Servico> page)
        {
            page = _servicoRepository.GetByPage(page, txt_dec);
            return page;
        }

        // GET: api/Servico/5
        [HttpGet]
        [Route("api/v1/servico/{id}")]
        public Servico Get(long id)
        {
            return _servicoRepository.Find(p => p.CD_SRV == id);
        }

        // POST: api/Servico
        [HttpPost]
        [Route("api/v1/servico")]
        public object Post([FromBody] Servico item)
        {
            try
            {
                if (item.IsValid() && _servicoRepository.ValidarDuplicidades(item))
                {
                    _servicoRepository.Insert(item);
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

        // PUT: api/Servico/5
        [HttpPut("api/v1/servico/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] Servico item)
        {
            try
            {
                if (item.IsValid() && _servicoRepository.ValidarDuplicidades(item))
                {
                    if (id > 0)
                        _servicoRepository.Update(item);
                    else
                        _servicoRepository.Insert(item);

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

        // DELETE: api/ApiWithActions/5
        [HttpDelete("api/v1/servico/{id}")]
        public object Delete(int id)
        {
            try
            {
                var objeto = _servicoRepository.FindById(id);
                _servicoRepository.Delete(objeto);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }
    }
}
