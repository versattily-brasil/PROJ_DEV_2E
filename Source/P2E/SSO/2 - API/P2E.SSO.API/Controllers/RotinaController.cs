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
    public class RotinaController : ControllerBase
    {
        private readonly IRotinaRepository _rotinaRepository;
        private readonly IMapper _mapper;
        public RotinaController(IRotinaRepository rotinaRepository, IMapper mapper)
        {
            _mapper = mapper;
            _rotinaRepository = rotinaRepository;
        }

        // GET: api/rotina
        [HttpGet]
        [Route("api/v1/rotina/")]
        public IEnumerable<RotinaVM> Get()
        {
            var result = _rotinaRepository.FindAll();
            return _mapper.Map<List<RotinaVM>>(result);
        }

        // GET: api/rotina/5
        [HttpGet]
        [Route("api/v1/rotina/{id}")]
        public RotinaVM Get(int id)
        {
            var result = _rotinaRepository.Find(p => p.CD_ROT == id);
            return _mapper.Map<RotinaVM>(result);
        }

        // POST: api/rotina
        [HttpPost]
        [Route("api/v1/rotina")]
        public object Post([FromBody] RotinaVM rotinaVM)
        {
            try
            {
                var rotina = _mapper.Map<Rotina>(rotinaVM);
                if (rotina.IsValid())
                {
                    _rotinaRepository.Insert(rotina);
                    return new { message = "OK" };
                }
                else
                {
                    return new { message = rotina.Notifications.FirstOrDefault().Message };
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
        public object Put(int id, [FromBody] RotinaVM rotinaVM)
        {
            try
            {
                var rotina = _mapper.Map<Rotina>(rotinaVM);
                if (rotina.IsValid())
                {
                    if (id > 0)
                        _rotinaRepository.Update(rotina);
                    else
                        _rotinaRepository.Insert(rotina);
                    return new { message = "OK" };
                }
                else
                {
                    return new { message = rotina.Notifications.FirstOrDefault().Message };
                }
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }

        // DELETE: api/rotina/5
        [HttpDelete]
        [Route("api/v1/rotina/{id}")]
        public object Delete(int id)
        {
            try
            {
                var rotina = _rotinaRepository.FindById(id);
                _rotinaRepository.Delete(rotina);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }
    }
}