using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2E.SSO.API.ViewModel;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;

namespace P2E.SSO.API.Controllers
{
    [ApiController]
    public class ModuloController : ControllerBase
    {
        private readonly IModuloRepository _moduloRepository;
        private readonly IMapper _mapper;
        public ModuloController(IModuloRepository moduloRepository, IMapper mapper)
        {
            _mapper = mapper;
            _moduloRepository = moduloRepository;
        }

        // GET: api/Modulo
        [HttpGet]
        [Route("api/v1/modulo/")]
        public IEnumerable<ModuloVM> Get()
        {
            var result = _moduloRepository.MetodoCustomizado(0);
            return _mapper.Map<List<ModuloVM>>(result);
        }

        // GET: api/Modulo/5
        [HttpGet]
        [Route("api/v1/modulo/{id}")]
        public ModuloVM Get(int id)
        {
            var result = _moduloRepository.Find(p => p.CD_MOD == id);
            return _mapper.Map<ModuloVM>(result);
        }

        // POST: api/Modulo
        [HttpPost]
        [Route("api/v1/modulo")]
        public object Post([FromBody] ModuloVM moduloVM)
        {
            try
            {
                var modulo = _mapper.Map<Modulo>(moduloVM);
                if (modulo.IsValid())
                {
                    _moduloRepository.Insert(modulo);
                    return new { message = "OK" };
                }
                else
                {
                    return new { message = modulo.Notifications.FirstOrDefault().Message };
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
        public object Put(int id, [FromBody] ModuloVM moduloVM)
        {
            try
            {
                var modulo = _mapper.Map<Modulo>(moduloVM);
                if (modulo.IsValid())
                {
                    if (id > 0)
                        _moduloRepository.Update(modulo);
                    else
                        _moduloRepository.Insert(modulo);
                    return new { message = "OK" };
                }
                else
                {
                    return new { message = modulo.Notifications.FirstOrDefault().Message };
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
