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
        private readonly IRotinaGrupoOperacaoRepository _rotinaGrupoOperacaoRepository;
        private readonly IUsuarioGrupoRepository _usuarioGrupoRepository;
        public GrupoController(IGrupoRepository grupoRepository, 
            IRotinaGrupoOperacaoRepository rotinaGrupoOperacaoRepository, IUsuarioGrupoRepository usuarioGrupoRepository)
        {
            _grupoRepository = grupoRepository;
            _rotinaGrupoOperacaoRepository = rotinaGrupoOperacaoRepository;
            _usuarioGrupoRepository = usuarioGrupoRepository;
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
            Grupo grupo = _grupoRepository.Find(p => p.CD_GRP == id);
            grupo.RotinaGrupoOperacao = _rotinaGrupoOperacaoRepository.FindAll(o => o.CD_GRP == id).ToList();
            return grupo;
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
                    _rotinaGrupoOperacaoRepository.Delete(o=>o.CD_GRP == item.CD_GRP);

                    if (id > 0)
                        _grupoRepository.Update(item);
                    else
                        _grupoRepository.Insert(item);

                    foreach (var rotinaGrupoOperacao in item.RotinaGrupoOperacao)
                    {
                        rotinaGrupoOperacao.CD_GRP = item.CD_GRP;

                        _rotinaGrupoOperacaoRepository.Insert(rotinaGrupoOperacao);
                    }

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
        public IActionResult Delete(int id)
        {
            try
            {
                var objeto = _grupoRepository.FindById(id);

                _usuarioGrupoRepository.Delete(o => o.CD_GRP == id);
                _rotinaGrupoOperacaoRepository.Delete(o => o.CD_GRP == id);
                _grupoRepository.Delete(objeto);
                return Ok();

                //var rotinaGrupos = _rotinaGrupoOperacaoRepository.Find(p => p.CD_GRP == id);
                //var usuarioGrupos = _usuarioGrupoRepository.Find(p => p.CD_GRP == id);

                //if (rotinaGrupos != null)
                //{
                //    return BadRequest("Não foi possivel excluir esse grupo pois ela está associada a uma rotina.");
                //}
                //else if (usuarioGrupos != null)
                //{
                //    return BadRequest("Não foi possivel excluir esse grupo pois ela está associada a um usuario.");
                //}                    
                //else
                //{
                //    _usuarioGrupoRepository.Delete(o => o.CD_GRP == id);
                //    _rotinaGrupoOperacaoRepository.Delete(o => o.CD_GRP == id);
                //    _grupoRepository.Delete(objeto);
                //    return Ok();
                //}
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao tentar excluir o registro. {ex.Message}");
            }            
        }
    }
}
