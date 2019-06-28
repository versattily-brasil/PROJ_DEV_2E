using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2E.Main.API.ViewModel;
using P2E.Main.Domain.Entities;
using P2E.Main.Domain.Repositories;

namespace P2E.Main.API.Controllers
{
    [ApiController]
    public class ExemploController : ControllerBase
    {
        private readonly IExemploRepository _exemploRepository;
        private readonly IMapper _mapper;
        public ExemploController(IExemploRepository exemploRepository, IMapper mapper)
        {
            _mapper = mapper;
            _exemploRepository = exemploRepository;
        }

        // GET: api/Exemplo
        [HttpGet]
        [Route("api/v1/exemplo/")]
        public IEnumerable<ExemploVM> Get()
        {
            var result = _exemploRepository.FindAll();
            return _mapper.Map<List<ExemploVM>>(result);
        }

        // GET: api/Exemplo/5
        [HttpGet]
        [Route("api/v1/exemplo/{id}")]
        public ExemploVM Get(int id)
        {
            var result = _exemploRepository.Find(p => p.ExemploId == id);
            return _mapper.Map<ExemploVM>(result);
        }

        // POST: api/Exemplo
        [HttpPost]
        [Route("api/v1/exemplo")]
        public object Post([FromBody] ExemploVM exemploVM)
        {
            try
            {
                var exemplo = _mapper.Map<Exemplo>(exemploVM);
                if (exemplo.IsValid())
                {
                    _exemploRepository.Insert(exemplo);
                    return new { message = "OK" };
                }
                else {
                    return new { message = exemplo.Notifications.FirstOrDefault().Message };
                }
                
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }

        // PUT: api/Exemplo/5
        [HttpPut]
        [Route("api/v1/exemplo/{id}")]
        public object Put(int id, [FromBody] ExemploVM exemploVM)
        {
            try
            {
                var exemplo = _mapper.Map<Exemplo>(exemploVM);
                if (exemplo.IsValid())
                {
                    if (id > 0)
                        _exemploRepository.Update(exemplo);
                    else
                        _exemploRepository.Insert(exemplo);
                    return new { message = "OK" };
                }
                else {
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
        [Route("api/v1/exemplo/{id}")]
        public object Delete(int id)
        {
            try
            {
                var objeto = _exemploRepository.FindById(id);
                _exemploRepository.Delete(objeto);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }
    }
}
