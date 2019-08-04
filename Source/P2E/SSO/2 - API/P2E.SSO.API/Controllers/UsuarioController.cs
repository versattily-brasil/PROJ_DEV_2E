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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioModuloRepository _usuarioModuloRepository;
        private readonly IUsuarioGrupoRepository _usuarioGrupoRepository;
        private readonly IGrupoRepository _grupoRepository;
        private readonly IModuloRepository _moduloRepository;

        private readonly IMapper _mapper;
        public UsuarioController(IUsuarioRepository usuarioRepository, IUsuarioModuloRepository usuarioModuloRepository, IUsuarioGrupoRepository usuarioGrupoRepository, IModuloRepository moduloRepository, IGrupoRepository grupoRepository, IMapper mapper)
        {
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
            _usuarioModuloRepository = usuarioModuloRepository;
            _usuarioGrupoRepository = usuarioGrupoRepository;
            _grupoRepository = grupoRepository;
            _moduloRepository = moduloRepository;
        }

        [HttpGet]
        [Route("api/v1/usuario/")]
        public DataPage<Usuario> Get([FromQuery] string tx_nome, [FromQuery] DataPage<Usuario> page)
        {
            page = _usuarioRepository.GetByPage(page, tx_nome);
            return page;
        }

        // GET: api/usuario/5
        [HttpGet]
        [Route("api/v1/usuario/{id}")]
        public Usuario Get(int id)
        {
            Usuario result = new Usuario();

            if (id > 0)
            {
                result = _usuarioRepository.Find(p => p.CD_USR == id);
                result.UsuarioModulo = _usuarioModuloRepository.FindAll(o => o.CD_USR == id).ToList();
                result.UsuarioGrupo = _usuarioGrupoRepository.FindAll(o => o.CD_USR == id).ToList();
                result.Grupo = _grupoRepository.FindAll().ToList();
                result.Modulo = _moduloRepository.FindAll().ToList();

            }
            else
            {
                result.Grupo = _grupoRepository.FindAll().ToList();
                result.Modulo = _moduloRepository.FindAll().ToList();
            }

            return result;
        }

        // POST: api/usuario
        [HttpPost]
        [Route("api/v1/usuario")]
        public object Post([FromBody] Usuario usuario)
        {
            try
            {
                if (usuario.IsValid())
                {
                    _usuarioRepository.Insert(usuario);
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

                    foreach (var usuarioModulo in usuario.UsuarioModulo)
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
                _usuarioModuloRepository.ExcluirUsuarioModulos(id);                               
                _usuarioGrupoRepository.ExcluirUsuarioGrupo(id);

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