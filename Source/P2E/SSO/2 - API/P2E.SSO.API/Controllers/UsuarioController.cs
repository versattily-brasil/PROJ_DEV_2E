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
        private readonly IUsuarioModuloRepository _usuarioModuloRepository;
        private readonly IUsuarioGrupoRepository _usuarioGrupoRepository;

        private readonly IMapper _mapper;
        public UsuarioController(IUsuarioRepository usuarioRepository, IUsuarioModuloRepository usuarioModuloRepository, IUsuarioGrupoRepository usuarioGrupoRepository, IMapper mapper)
        {
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
            _usuarioModuloRepository = usuarioModuloRepository;
            _usuarioGrupoRepository = usuarioGrupoRepository;
        }

        // GET: api/usuario
        [HttpGet]
        [Route("api/v1/usuario/")]
        public IEnumerable<Usuario> Get()
        {
            var result = _usuarioRepository.FindAll();
            return result;
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
        public object Put(int id, [FromBody] Usuario usuario)
        {
            try
            {
                if (usuario.IsValid())
                {
                    _usuarioModuloRepository.Delete(o => o.CD_USR == usuario.CD_USR);
                    _usuarioGrupoRepository.Delete(o => o.CD_USR == usuario.CD_USR);

                    if (id > 0)
                        _usuarioRepository.Update(usuario);
                    else
                        _usuarioRepository.Insert(usuario);

                    foreach(var usuarioModulo in usuario.UsuarioModulos)
                    {
                        usuarioModulo.CD_USR = usuario.CD_USR;
                        _usuarioModuloRepository.Insert(usuarioModulo);
                    }

                    foreach (var usuarioGrupo in usuario.UsuarioGrupo)
                    {
                        usuarioGrupo.CD_USR = usuario.CD_USR;
                        _usuarioGrupoRepository.Insert(usuarioGrupo);
                    }

                    return new { message = "OK" };
                }
                else
                {
                    return new { message = usuario.Notifications.FirstOrDefault().Message };
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