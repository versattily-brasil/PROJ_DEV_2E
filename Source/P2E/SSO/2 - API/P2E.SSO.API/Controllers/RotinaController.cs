using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using P2E.Shared.Model;
using P2E.SSO.Domain.Entities;
using P2E.SSO.Domain.Repositories;

namespace P2E.SSO.API.Controllers
{
    public class RotinaController : ControllerBase
    {
        private readonly IRotinaRepository _rotinaRepository;
        public RotinaController(IRotinaRepository rotinaRepository)
        {
            _rotinaRepository = rotinaRepository;
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
            return _rotinaRepository.Find(p => p.CD_ROT == id);
        }

        // POST: api/rotina
        [HttpPost]
        [Route("api/v1/rotina")]
        public object Post([FromBody] Rotina item)
        {
            try
            {
                if (item.IsValid())
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
        public object Put(int id, [FromBody] Rotina item)
        {
            try
            {
                if (item.IsValid())
                {
                    if (id > 0)
                        _rotinaRepository.Update(item);
                    else
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

        // DELETE: api/rotina/5
        [HttpDelete]
        [Route("api/v1/rotina/{id}")]
        public object Delete(long id)
        {
            try
            {
                var item = _rotinaRepository.FindById(id);
                _rotinaRepository.Delete(item);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }
        //private readonly IRotinaRepository _rotinaRepository;
        //private readonly IMapper _mapper;
        //public RotinaController(IRotinaRepository rotinaRepository, IMapper mapper)
        //{
        //    _mapper = mapper;
        //    _rotinaRepository = rotinaRepository;
        //}

        //// GET: api/rotina
        //[HttpGet]
        //[Route("api/v1/rotina/")]
        //public IEnumerable<RotinaVM> Get()
        //{
        //    var result = _rotinaRepository.FindAll();
        //    return _mapper.Map<List<RotinaVM>>(result);
        //}

        //// GET: api/rotina/5
        //[HttpGet]
        //[Route("api/v1/rotina/{id}")]
        //public RotinaVM Get(int id)
        //{
        //    var result = _rotinaRepository.Find(p => p.CD_ROT == id);
        //    return _mapper.Map<RotinaVM>(result);
        //}

        //// POST: api/rotina
        //[HttpPost]
        //[Route("api/v1/rotina")]
        //public object Post([FromBody] RotinaVM rotinaVM)
        //{
        //    try
        //    {
        //        var rotina = _mapper.Map<Rotina>(rotinaVM);
        //        if (rotina.IsValid())
        //        {
        //            _rotinaRepository.Insert(rotina);
        //            return new { message = "OK" };
        //        }
        //        else
        //        {
        //            return new { message = rotina.Notifications.FirstOrDefault().Message };
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return new { message = "Error." + ex.Message };
        //    }
        //}

        //// PUT: api/rotina/5
        //[HttpPut]
        //[Route("api/v1/rotina/{id}")]
        //public object Put(int id, [FromBody] RotinaVM rotinaVM)
        //{
        //    try
        //    {
        //        var rotina = _mapper.Map<Rotina>(rotinaVM);
        //        if (rotina.IsValid())
        //        {
        //            if (id > 0)
        //                _rotinaRepository.Update(rotina);
        //            else
        //                _rotinaRepository.Insert(rotina);
        //            return new { message = "OK" };
        //        }
        //        else
        //        {
        //            return new { message = rotina.Notifications.FirstOrDefault().Message };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new { message = "Error." + ex.Message };
        //    }
        //}

        //// DELETE: api/rotina/5
        //[HttpDelete]
        //[Route("api/v1/rotina/{id}")]
        //public object Delete(int id)
        //{
        //    try
        //    {
        //        var rotina = _rotinaRepository.FindById(id);
        //        _rotinaRepository.Delete(rotina);
        //        return new { message = "OK" };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new { message = "Error." + ex.Message };
        //    }
        //}
    }
}