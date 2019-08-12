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
    public class GrupoController : ControllerBase
    {
        private readonly IGrupoRepository _grupoRepository;
        public GrupoController(IGrupoRepository grupoRepository)
        {
            _grupoRepository = grupoRepository;
        }

        // GET: api/grupo
        [HttpGet]
        [Route("api/v1/grupo/todos")]
        public IEnumerable<Grupo> Get()
        {
            var result = _grupoRepository.FindAll();
            return result;
        }

        // GET: api/grupo
        [HttpGet]
        [Route("api/v1/grupo/")]
        public DataPage<Grupo> Get([FromQuery] string tx_dsc, [FromQuery] DataPage<Grupo> page)
        {
            page = _grupoRepository.GetByPage(page, tx_dsc);
            return page;
        }

        // GET: api/grupo/5
        [HttpGet]
        [Route("api/v1/grupo/{id}")]
        public Grupo Get(long id)
        {
            return _grupoRepository.Find(p => p.CD_GRP == id);
        }

        // POST: api/grupo
        [HttpPost]
        [Route("api/v1/grupo")]
        public object Post([FromBody] Grupo item)
        {
            try
            {
                if (item.IsValid() && _grupoRepository.ValidarDuplicidades(item))
                {
                    _grupoRepository.Insert(item);
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

        // PUT: api/grupo/5
        [HttpPut]
        [Route("api/v1/grupo/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] Grupo item)
        {
            try
            {
                if (item.IsValid() && _grupoRepository.ValidarDuplicidades(item))
                {
                    if (id > 0)
                        _grupoRepository.Update(item);
                    else
                        _grupoRepository.Insert(item);

                    return Ok(item);
                }
                else
                {
                    return BadRequest(item.Messages);
                    //return new { message = item.Notifications.FirstOrDefault().Message };
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
                // return new { message = "Error." + ex.Message };
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        [Route("api/v1/grupo/{id}")]
        public object Delete(int id)
        {
            try
            {
                var objeto = _grupoRepository.FindById(id);
                _grupoRepository.Delete(objeto);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }
    }
}
