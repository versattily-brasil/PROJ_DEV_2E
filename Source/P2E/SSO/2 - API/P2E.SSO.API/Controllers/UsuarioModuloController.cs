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
    public class UsuarioModuloController : ControllerBase
    {
        private readonly IUsuarioModuloRepository _usuarioModuloRepository;
        private readonly IMapper _mapper;
        public UsuarioModuloController(IUsuarioModuloRepository usuarioModuloRepository, IMapper mapper)
        {
            _mapper = mapper;
            _usuarioModuloRepository = usuarioModuloRepository;
        }

        // GET: api/UsuarioModulo
        [HttpGet]
        [Route("api/v1/usuarioModulo/")]
        public IEnumerable<UsuarioModuloVM> Get()
        {
            var result = _usuarioModuloRepository.MetodoCustomizado(0);
            return _mapper.Map<List<UsuarioModuloVM>>(result);
        }

        // GET: api/UsuarioModulo/5
        [HttpGet]
        [Route("api/v1/usuarioModulo/{id}")]
        public UsuarioModuloVM Get(int id)
        {
            var result = _usuarioModuloRepository.Find(p => p.CD_USR_MOD == id);
            return _mapper.Map<UsuarioModuloVM>(result);
        }

        // POST: api/UsuarioModulo
        [HttpPost]
        [Route("api/v1/usuarioModulo")]
        public object Post([FromBody] UsuarioModuloVM usuarioModuloVM)
        {
            try
            {
                var modulo = _mapper.Map<UsuarioModulo>(usuarioModuloVM);
                _usuarioModuloRepository.Insert(modulo);
                return new { message = "OK" };

            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }

        // PUT: api/UsuarioModulo/5
        [HttpPut("{id}")]
        [Route("api/v1/usuarioModulo/{id}")]
        public object Put(int id, [FromBody] UsuarioModuloVM usuarioModuloVM)
        {
            try
            {
                var usuariomodulo = _mapper.Map<UsuarioModulo>(usuarioModuloVM);

                if (id > 0)
                    _usuarioModuloRepository.Update(usuariomodulo);
                else
                    _usuarioModuloRepository.Insert(usuariomodulo);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Route("api/v1/usuarioModulo/{id}")]
        public object Delete(int id)
        {
            try
            {
                var objeto = _usuarioModuloRepository.FindById(id);
                _usuarioModuloRepository.Delete(objeto);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }
    }
}
