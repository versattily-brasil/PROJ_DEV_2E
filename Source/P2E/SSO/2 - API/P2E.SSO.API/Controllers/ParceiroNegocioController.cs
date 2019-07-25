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
    public class ParceiroNegocioController : ControllerBase
    {
        private readonly IParceiroNegocioRepository _parceiroNegocioRepository;
        public ParceiroNegocioController(IParceiroNegocioRepository parceiroNegocioRepository)
        {
            _parceiroNegocioRepository = parceiroNegocioRepository;
        }

        // GET: api/parceironegocio
        [HttpGet]
        [Route("api/v1/parceironegocio/{pagesize}/{currentpage}")]
        public ParceiroNegocioVM Get([FromBody] ParceiroNegocioVM vm)
        {
            //var vm = new ParceiroNegocioVM() { CurrentPage = currentPage, PageSize = pageSize};

            vm.Lista = _parceiroNegocioRepository.GetByPage(vm.razaoSocial, vm.CNPJ, vm.CurrentPage, vm.PageSize);

            return vm;
        }

        // GET: api/parceironegocio/5
        [HttpGet]
        [Route("api/v1/parceironegocio/{id}")]
        public ParceiroNegocio Get(long id)
        {
            return  _parceiroNegocioRepository.Find(p => p.CD_PAR == id);
        }

        // POST: api/parceironegocio
        [HttpPost]
        [Route("api/v1/parceironegocio")]
        public object Post([FromBody] ParceiroNegocio item)
        {
            try
            {
                if (item.IsValid())
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
        public object Put(int id, [FromBody] ParceiroNegocio item)
        {
            try
            {
                if (item.IsValid())
                {
                    if (id > 0)
                        _parceiroNegocioRepository.Update(item);
                    else
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

        // DELETE: api/parceironegocio/5
        [HttpDelete]
        [Route("api/v1/parceironegocio/{id}")]
        public object Delete(long id)
        {
            try
            {
                var item = _parceiroNegocioRepository.FindById(id);
                _parceiroNegocioRepository.Delete(item);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }
    }
}
