using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace P2E.SSO.API.Controllers
{
    public class RotinaController : ControllerBase
    {
        private readonly IRotinaRepository _rotinaRepository;
        private readonly IRotinaServicoRepository _rotinaServicoRepository;
        private readonly IServicoRepository _servicoRepository;
        private readonly IRotinaGrupoOperacaoRepository _rotinaGrupoOperacaoRepository;
        private readonly IRotinaAssociadaRepository _rotinaAssociadaRepository;
        public RotinaController(IRotinaRepository rotinaRepository,
                                IRotinaServicoRepository rotinaServicoRepository,
                                IServicoRepository servicoRepository,
                                IRotinaAssociadaRepository rotinaAssociadaRepository,
                                IRotinaGrupoOperacaoRepository rotinaGrupoOperacaoRepository)
        {
            _rotinaRepository = rotinaRepository;
            _rotinaServicoRepository = rotinaServicoRepository;
            _servicoRepository = servicoRepository;
            _rotinaGrupoOperacaoRepository = rotinaGrupoOperacaoRepository;
            _rotinaAssociadaRepository = rotinaAssociadaRepository;
        }

        // GET: api/rotina/todos
        [HttpGet]
        [Route("api/v1/rotina/todos")]
        public IEnumerable<Rotina> Get()
        {
            return _rotinaRepository.FindAll();
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
                rotina.RotinaServico = _rotinaServicoRepository.FindAll(p => p.CD_ROT == id).ToList();

                rotina.Servicos = _servicoRepository.FindAll().ToList();

                int i = 0;
                foreach (var pns in rotina.RotinaServico)
                {
                    rotina.RotinaServico[i].Servico = rotina.Servicos.Find(p => p.CD_SRV == pns.CD_SRV);
                    i++;
                }
            }

            return rotina;
        }

        // GET: api/rotina/5
        [HttpGet]
        [Route("api/v1/rotina/associadas/{id}")]
        public List<RotinaAssociada> Associadas(int id)
        {
            var retorno = new List<RotinaAssociada>();

            retorno = _rotinaAssociadaRepository.FindAll(p => p.CD_ROT_PRINCIPAL == id).ToList();

            foreach (var item in retorno)
            {
                var rotina = _rotinaRepository.Find(p => p.CD_ROT == item.CD_ROT_ASS);
                item.Rotina = rotina;
                item.NomeRotinaAssociada = rotina.TX_NOME;
            }

            return retorno;
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
                    _rotinaServicoRepository.Delete(a => a.CD_ROT == item.CD_ROT);

                    _rotinaAssociadaRepository.Delete(o => o.CD_ROT_PRINCIPAL == item.CD_ROT);

                    if (id > 0)
                        _rotinaRepository.Update(item);
                    else
                        _rotinaRepository.Insert(item);

                    foreach (var rotinaServico in item.RotinaServico)
                    {
                        rotinaServico.CD_ROT = item.CD_ROT;

                        _rotinaServicoRepository.Insert(rotinaServico);
                    }

                    if (item.RotinasAssociadas != null)
                    {
                        foreach (var rotinaAssociada in item.RotinasAssociadas)
                        {
                            rotinaAssociada.CD_ROT_PRINCIPAL = item.CD_ROT;
                            _rotinaAssociadaRepository.Insert(rotinaAssociada);
                        }
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
        public IActionResult Delete(long id)
        {
            try
            {
                var item = this.Get(id);

                _rotinaGrupoOperacaoRepository.Delete(o => o.CD_ROT == id);
                _rotinaRepository.Delete(item);
                return Ok();

                //var rotinaGrupos = _rotinaGrupoOperacaoRepository.Find(p => p.CD_ROT == id);

                //if(rotinaGrupos != null)
                //    return BadRequest("Não foi possivel excluir essa rotina pois ela está associada a um grupo de usuários.");
                //else
                //{                
                //    _rotinaRepository.Delete(item);
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