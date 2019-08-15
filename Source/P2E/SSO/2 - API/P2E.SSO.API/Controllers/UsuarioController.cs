﻿using System;
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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioModuloRepository _usuarioModuloRepository;
        private readonly IUsuarioGrupoRepository _usuarioGrupoRepository;
        private readonly IGrupoRepository _grupoRepository;
        private readonly IModuloRepository _moduloRepository;
        private readonly IRotinaRepository _rotinaRepository;
        private readonly IRotinaGrupoOperacaoRepository _rotinaGrupoOperacaoRepository;
        private readonly IOperacaoRepository _operacaoRepository;
        private readonly IServicoRepository _servicoRepository;

        private readonly IMapper _mapper;
        public UsuarioController(IUsuarioRepository usuarioRepository, 
                                 IUsuarioModuloRepository usuarioModuloRepository, 
                                 IUsuarioGrupoRepository usuarioGrupoRepository, 
                                 IModuloRepository moduloRepository, 
                                 IGrupoRepository grupoRepository,
                                 IRotinaRepository rotinaRepository, 
                                 IRotinaGrupoOperacaoRepository rotinaGrupoOperacaoRepository, 
                                 IOperacaoRepository operacaoRepository, 
                                 IServicoRepository servicoRepository, 
                                 IMapper mapper)
        {
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
            _usuarioModuloRepository = usuarioModuloRepository;
            _usuarioGrupoRepository = usuarioGrupoRepository;
            _grupoRepository = grupoRepository;
            _moduloRepository = moduloRepository;
            _rotinaRepository = rotinaRepository;
            _rotinaGrupoOperacaoRepository = rotinaGrupoOperacaoRepository;
            _operacaoRepository = operacaoRepository;
            _servicoRepository = servicoRepository;
        }

        [HttpGet]
        [Route("api/v1/usuario/")]
        public DataPage<Usuario> Get([FromQuery] string tx_nome, [FromQuery] DataPage<Usuario> page)
        {
            page = _usuarioRepository.GetByPage(page, tx_nome);
            return page;
        }

        [HttpGet]
        [Route("api/v1/usuario/permissoes/{id}")]
        public List<UsuarioGrupo> GetPermissoes(int id)
        {
            // Obtem os grupos em que o usuario está associado
            var usuarioGrupos = _usuarioGrupoRepository.FindAll(o => o.CD_USR == id).ToList();

            foreach (var usuarioGrupo in usuarioGrupos)
            {
                // Obtem rotinas que estão associadas ao grupo
                var rotinaGrupos = _rotinaGrupoOperacaoRepository.FindAll(p => p.CD_GRP == usuarioGrupo.CD_GRP);

                foreach (var rotinaGrupo in rotinaGrupos)
                {
                    // Carrega a rotina
                    rotinaGrupo.Rotina = _rotinaRepository.Find(p => p.CD_ROT == rotinaGrupo.CD_ROT);

                    // Carrega as Permissões
                    rotinaGrupo.Rotina.Operacoes = _operacaoRepository.FindAll(p=> p.CD_OPR == rotinaGrupo.CD_OPR).ToList();

                    // Carrega os Serviços
                    rotinaGrupo.Rotina.Servico = _servicoRepository.Find(p => p.CD_SRV == rotinaGrupo.Rotina.CD_SRV);
                }

                usuarioGrupo.ListaRotinaGrupoOperacao.AddRange(rotinaGrupos);
            }

            return usuarioGrupos;
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
                if (usuario.IsValid() && _usuarioRepository.ValidarDuplicidades(usuario))
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

        // POST: api/usuario/login
        [HttpPost]
        [Route("api/v1/usuario/login")]
        public Usuario PostLogin([FromBody] Usuario usuario)
        {
            return _usuarioRepository.Find(o => o.TX_LOGIN == usuario.TX_LOGIN && o.TX_SENHA == usuario.TX_SENHA);
        }

        // PUT: api/usuario/5
        [HttpPut]
        [Route("api/v1/usuario/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] Usuario usuario)
        {
            try
            {
                if (usuario.IsValid() && _usuarioRepository.ValidarDuplicidades(usuario))
                {
                    _usuarioModuloRepository.Delete(o => o.CD_USR == usuario.CD_USR);
                    _usuarioGrupoRepository.Delete(o => o.CD_USR == usuario.CD_USR);

                    if (id > 0)
                        _usuarioRepository.Update(usuario);
                    else
                        _usuarioRepository.Insert(usuario);

                    foreach (var usuarioModulo in usuario.UsuarioModulo)//lista de associações
                    {
                        usuarioModulo.CD_USR = usuario.CD_USR;
                        _usuarioModuloRepository.Insert(usuarioModulo);
                    }

                    foreach (var usuarioGrupo in usuario.UsuarioGrupo)
                    {
                        usuarioGrupo.CD_USR = usuario.CD_USR;
                        _usuarioGrupoRepository.Insert(usuarioGrupo);
                    }

                    return Ok(usuario);
                }
                else
                {
                    return BadRequest(usuario.Messages);
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
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