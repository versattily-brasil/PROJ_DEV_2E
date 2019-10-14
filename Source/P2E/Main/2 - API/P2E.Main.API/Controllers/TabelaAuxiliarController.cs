using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2E.Main.API.ViewModel;
using P2E.Main.Domain.Entities;
using P2E.Main.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2E.Main.API.Controllers
{
    [ApiController]
    public class TabelaAuxiliarController : ControllerBase
    {
        private readonly ITabelaAuxiliarRepository _tabelaAuxiliarRepository;
        private readonly IMapper _mapper;
        public TabelaAuxiliarController(ITabelaAuxiliarRepository tabelaAuxiliarRepository, IMapper mapper)
        {
            _mapper = mapper;
            _tabelaAuxiliarRepository = tabelaAuxiliarRepository;
        }

        // GET: api/tabela
        [HttpGet]
        [Route("api/v1/tabelaauxiliar/")]
        public IEnumerable<TabelaAuxiliarVM> Get()
        {
            //var result = _exemploRepository.FindAll();
            var result = _tabelaAuxiliarRepository.MetodoCustomizado("");
            return _mapper.Map<List<TabelaAuxiliarVM>>(result);
        }

        // GET: api/tabela/5
        [HttpGet]
        [Route("api/v1/tabelaAuxiliar/{id}")]
        public TabelaAuxiliarVM Get(string id)
        {
            var result = _tabelaAuxiliarRepository.Find(p => p.TX_TABELA == id);
            return _mapper.Map<TabelaAuxiliarVM>(result);
        }

        // POST: api/tabela
        [HttpPost]
        [Route("api/v1/tabelaauxiliar")]
        public object Post([FromBody] TabelaAuxiliarVM tabelaVM)
        {
            try
            {
                var exemplo = _mapper.Map<TabelaAuxiliar>(tabelaVM);
                if (exemplo.IsValid())
                {
                    _tabelaAuxiliarRepository.Insert(exemplo);
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

        // PUT: api/tabela/5
        [HttpPut]
        [Route("api/v1/tabelaauxiliar/{id}")]
        public object Put(string id, [FromBody] TabelaAuxiliarVM tabelaAuxiliarVM)
        {
            try
            {
                var exemplo = _mapper.Map<TabelaAuxiliar>(tabelaAuxiliarVM);
                if (exemplo.IsValid())
                {
                    if (id != string.Empty)
                        _tabelaAuxiliarRepository.Update(exemplo);
                    else
                        _tabelaAuxiliarRepository.Insert(exemplo);
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

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        [Route("api/v1/tabelaauxiliar/{id}")]
        public object Delete(string id)
        {
            try
            {
                var objeto = _tabelaAuxiliarRepository.FindById(id);
                _tabelaAuxiliarRepository.Delete(objeto);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }
    }
}
