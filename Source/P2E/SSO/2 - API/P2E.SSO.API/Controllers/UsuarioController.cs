using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using P2E.SSO.API.ViewModel;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;

namespace P2E.SSO.API.Controllers
{
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        public UsuarioController(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
        }

        // GET: api/usuario
        [HttpGet]
        [Route("api/v1/usuario/")]
        public IEnumerable<UsuarioVM> Get()
        {
            var result = _usuarioRepository.FindAll();
            return _mapper.Map<List<UsuarioVM>>(result);
        }

        // GET: api/usuario/5
        [HttpGet]
        [Route("api/v1/usuario/{id}")]
        public UsuarioVM Get(int id)
        {
            var result = _usuarioRepository.Find(p => p.CD_USR == id);
            return _mapper.Map<UsuarioVM>(result);
        }

        // POST: api/usuario
        [HttpPost]
        [Route("api/v1/usuario")]
        public object Post([FromBody] UsuarioVM exemploVM)
        {
            try
            {
                var exemplo = _mapper.Map<Usuario>(exemploVM);
                if (exemplo.IsValid())
                {
                    _usuarioRepository.Insert(exemplo);
                    return new { message = "OK" };
                }
                else
                {
                    return new { message = exemplo.Notifications.FirstOrDefault().Message };
                }

            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }

        // PUT: api/usuario/5
        [HttpPut]
        [Route("api/v1/usuario/{id}")]
        public object Put(int id, [FromBody] UsuarioVM exemploVM)
        {
            try
            {
                var exemplo = _mapper.Map<Usuario>(exemploVM);
                if (exemplo.IsValid())
                {
                    if (id > 0)
                        _usuarioRepository.Update(exemplo);
                    else
                        _usuarioRepository.Insert(exemplo);
                    return new { message = "OK" };
                }
                else
                {
                    return new { message = exemplo.Notifications.FirstOrDefault().Message };
                }
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }

        // DELETE: api/usuario/5
        [HttpDelete]
        [Route("api/v1/usuario/{id}")]
        public object Delete(int id)
        {
            try
            {
                var objeto = _usuarioRepository.FindById(id);
                _usuarioRepository.Delete(objeto);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }
    }
}