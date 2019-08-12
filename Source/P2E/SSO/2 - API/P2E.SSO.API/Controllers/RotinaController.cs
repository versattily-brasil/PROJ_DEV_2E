using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;

namespace P2E.SSO.API.Controllers
{
    public class RotinaController : ControllerBase
    {
        private readonly IRotinaRepository _rotinaRepository;
        private readonly IRotinaGrupoOperacaoRepository _rotinaGrupoOperacaoRepository;
        private readonly IGrupoRepository _grupoRepository;
        private readonly IOperacaoRepository _operacaoRepository;
        public RotinaController(IRotinaRepository rotinaRepository,
                                IRotinaGrupoOperacaoRepository rotinaGrupoOperacaoRepository, 
                                IGrupoRepository grupoRepository, 
                                IOperacaoRepository operacaoRepository)
        {
            _rotinaRepository = rotinaRepository;
            _rotinaGrupoOperacaoRepository = rotinaGrupoOperacaoRepository;
            _grupoRepository = grupoRepository;
            _operacaoRepository = operacaoRepository;
        }

        // GET: api/rotina
        [HttpGet]
        [Route("api/v1/rotina/")]
        public DataPage<Rotina> Get([FromQuery] string descricao, [FromQuery] string nome, [FromQuery] DataPage<Rotina> page)
        {
            page = _rotinaRepository.GetByPage(page, descricao, nome);
            return page;
        }

        // GET: api/rotina/5
        [HttpGet]
        [Route("api/v1/rotina/{id}")]
        public Rotina Get(long id)
        {
            Rotina rotina = new Rotina();

            if (id > 0)
            {
                rotina = _rotinaRepository.Find(p => p.CD_ROT == id);
                rotina.RotinaGrupoOperacao = _rotinaGrupoOperacaoRepository.FindAll(p => p.CD_ROT == id).ToList();

                rotina.Grupo = _grupoRepository.FindAll().ToList();
                rotina.Operacao = _operacaoRepository.FindAll().ToList();

                int i = 0;
                foreach (var pns in rotina.RotinaGrupoOperacao)
                {
                    rotina.RotinaGrupoOperacao[i].Grupo = rotina.Grupo.Find(p => p.CD_GRP == pns.CD_GRP);
                    rotina.RotinaGrupoOperacao[i].Operacao = rotina.Operacao.Find(p => p.CD_OPR == pns.CD_OPR);
                    i++;
                }
            }

            return rotina;            
        }

        // POST: api/rotina
        [HttpPost]
        [Route("api/v1/rotina")]
        public object Post([FromBody] Rotina item)
        {
            try
            {
                if (item.IsValid() && _rotinaRepository.ValidarDuplicidades(item))
                {
                    _rotinaRepository.Insert(item);
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

        // PUT: api/rotina/5
        [HttpPut]
        [Route("api/v1/rotina/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] Rotina item)
        {
            try
            {
                if (item.IsValid() && _rotinaRepository.ValidarDuplicidades(item))
                {
                    _rotinaGrupoOperacaoRepository.Delete(a => a.CD_ROT == item.CD_ROT);

                    if (id > 0)
                        _rotinaRepository.Update(item);
                    else
                        _rotinaRepository.Insert(item);

                    foreach (var rotinaGrupoOperacao in item.RotinaGrupoOperacao)
                    {
                        rotinaGrupoOperacao.CD_ROT = item.CD_ROT;

                        _rotinaGrupoOperacaoRepository.Insert(rotinaGrupoOperacao);
                    }

                    return Ok(item);
                    //return new { message = "OK" };
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

        // DELETE: api/rotina/5
        [HttpDelete]
        [Route("api/v1/rotina/{id}")]
        public object Delete(long id)
        {
            try
            {
                var item = this.Get(id);                
                
                // Exclui cada Grupo-Operacao vinculado à rotina
                foreach (var rgo in item.RotinaGrupoOperacao)
                {
                    _rotinaGrupoOperacaoRepository.ExcluirRotinaGrupoOperacao(rgo.CD_ROT_GRP);
                }
                
                _rotinaRepository.Delete(item);

                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }        
    }
}