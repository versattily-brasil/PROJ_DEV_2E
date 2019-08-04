using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using P2E.Shared.Model;
using P2E.SSO.API.ViewModel;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;

namespace P2E.SSO.API.Controllers
{
    public class ModuloController : ControllerBase
    {
        private readonly IModuloRepository _moduloRepository;
        public ModuloController(IModuloRepository moduloRepository)
        {
            _moduloRepository = moduloRepository;
        }

        // GET: api/Modulo
        [HttpGet]
        [Route("api/v1/modulo/todos")]
        public IEnumerable<Modulo> Get()
        {
            var result = _moduloRepository.FindAll();
            return result;
        }

        // GET: api/Modulo
        [HttpGet]
        [Route("api/v1/modulo/")]
        public DataPage<Modulo> Get([FromQuery] string tx_dsc, [FromQuery] DataPage<Modulo> page)
        {
            page = _moduloRepository.GetByPage(page, tx_dsc);
            return page;
        }

        // GET: api/Modulo/5
        [HttpGet]
        [Route("api/v1/modulo/{id}")]
        public Modulo Get(long id)
        {
            return _moduloRepository.Find(p => p.CD_MOD == id);
        }

        // POST: api/Modulo
        [HttpPost]
        [Route("api/v1/modulo")]
        public object Post([FromBody] Modulo item)
        {
            try
            {
                if (item.IsValid())
                {
                    _moduloRepository.Insert(item);
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

        // PUT: api/Modulo/5
        [HttpPut]
        [Route("api/v1/modulo/{id}")]
        public object Put(int id, [FromBody] Modulo item)
        {
            try
            {
                if (item.IsValid())
                {
                    if (id > 0)
                        _moduloRepository.Update(item);
                    else
                        _moduloRepository.Insert(item);
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

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        [Route("api/v1/modulo/{id}")]
        public object Delete(int id)
        {
            try
            {
                var objeto = _moduloRepository.FindById(id);
                _moduloRepository.Delete(objeto);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }
    }
}
