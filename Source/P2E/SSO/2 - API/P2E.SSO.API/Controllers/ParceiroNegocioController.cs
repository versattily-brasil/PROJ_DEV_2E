using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;
using System;
using System.Linq;
using System.Net;

namespace P2E.SSO.API.Controllers
{
    public class ParceiroNegocioController : ControllerBase
    {
        private readonly IParceiroNegocioRepository _parceiroNegocioRepository;
        private readonly IParceiroNegocioModuloRepository _parceiroNegocioModuloRepository;
        private readonly IServicoRepository _servicoRepository;
        private readonly IModuloRepository _moduloRepository;

        public ParceiroNegocioController(IParceiroNegocioRepository parceiroNegocioRepository,
                                         IParceiroNegocioModuloRepository parceiroNegocioModuloRepository,
                                         IModuloRepository moduloRepository,
                                         IServicoRepository servicoRepository)
        {
            _parceiroNegocioRepository = parceiroNegocioRepository;
            _parceiroNegocioModuloRepository = parceiroNegocioModuloRepository;
            _servicoRepository = servicoRepository;
            _moduloRepository = moduloRepository;
        }

        // GET: api/parceironegocio
        [HttpGet]
        [Route("api/v1/parceironegocio/")]
        public DataPage<ParceiroNegocio> Get([FromQuery] string razaosocial, [FromQuery] string cnpj, [FromQuery] DataPage<ParceiroNegocio> page)
        {
            page = _parceiroNegocioRepository.GetByPage(page, razaosocial, cnpj);
            return page;
        }

        // GET: api/parceironegocio/5
        [HttpGet]
        [Route("api/v1/parceironegocio/{id}")]
        public ParceiroNegocio Get(long id)
        {
            ParceiroNegocio parceiro = new ParceiroNegocio();

            if (id > 0)
            {
                parceiro = _parceiroNegocioRepository.Find(p => p.CD_PAR == id);
                parceiro.ParceiroNegocioServicoModulo = _parceiroNegocioModuloRepository.FindAll(p => p.CD_PAR == id).ToList();

                parceiro.Servico = _servicoRepository.FindAll().ToList();
                parceiro.Modulo = _moduloRepository.FindAll().ToList();

                int i = 0;
                foreach (var pns in parceiro.ParceiroNegocioServicoModulo)
                {
                    parceiro.ParceiroNegocioServicoModulo[i].Modulo = parceiro.Modulo.Find(p => p.CD_MOD == pns.CD_MOD);
                    parceiro.ParceiroNegocioServicoModulo[i].Servico = parceiro.Servico.Find(p => p.CD_SRV == pns.CD_SRV);
                    i++;
                }

                //parceiro
            }

            return parceiro;

            //return  _parceiroNegocioRepository.Find(p => p.CD_PAR == id);
        }

        // POST: api/parceironegocio
        [HttpPost]
        [Route("api/v1/parceironegocio")]
        public object Post([FromBody] ParceiroNegocio item)
        {
            try
            {
                if (item.IsValid() && _parceiroNegocioRepository.ValidarDuplicidades(item))
                {
                    _parceiroNegocioRepository.Insert(item);
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

        // PUT: api/parceironegocio/5
        [HttpPut]
        [Route("api/v1/parceironegocio/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] ParceiroNegocio parceiro)
        {
            try
            {
                if (parceiro.IsValid() && _parceiroNegocioRepository.ValidarDuplicidades(parceiro))
                {
                    _parceiroNegocioModuloRepository.Delete(a => a.CD_PAR == parceiro.CD_PAR);

                    if (id > 0)
                        _parceiroNegocioRepository.Update(parceiro);
                    else
                        _parceiroNegocioRepository.Insert(parceiro);

                    foreach (var parceiroServico in parceiro.ParceiroNegocioServicoModulo)
                    {
                        parceiroServico.CD_PAR = parceiro.CD_PAR;

                        _parceiroNegocioModuloRepository.Insert(parceiroServico);
                    }

                    return Ok(parceiro);
                }
                else
                {
                    return BadRequest(parceiro.Messages);
                    //return (parceiro);
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // DELETE: api/parceironegocio/5
        [HttpDelete]
        [Route("api/v1/parceironegocio/{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                var item = _parceiroNegocioRepository.FindById(id);

                _parceiroNegocioModuloRepository.Delete(o => o.CD_PAR == id);
                _parceiroNegocioRepository.Delete(item);
                return Ok();

                //var rotinaGrupos = _parceiroNegocioModuloRepository.Find(p => p.CD_PAR == id);

                //if (rotinaGrupos != null)
                //    return BadRequest("Não foi possivel excluir esse parceiro pois ela está associado a Serviço.");
                //else
                //{
                //   
                //    _parceiroNegocioRepository.Delete(item);
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
