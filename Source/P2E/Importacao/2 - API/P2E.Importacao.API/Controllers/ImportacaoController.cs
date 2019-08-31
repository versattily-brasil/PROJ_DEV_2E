using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2E.Importacao.Domain.Entities;
using P2E.Importacao.Domain.Repositories;
using P2E.Shared.Model;

namespace P2E.Importacao.API.Controllers
{
    [Route("api/[controller]")]
    public class ImportacaoController : ControllerBase
    {
        private readonly IImportacaoRepository _ImportacaoRepository;
        
        public ImportacaoController(IImportacaoRepository ImportacaoRepository)
        {
            _ImportacaoRepository = ImportacaoRepository;
        }

        // GET: api/Importacao
        [HttpGet]
        [Route("api/v1/Importacao/todos")]
        public IEnumerable<TBImportacao> Get()
        {
            var result = _ImportacaoRepository.FindAll();
            return result;
        }

        // GET: api/Importacao
        [HttpGet]
        [Route("api/v1/Importacao/")]
        public DataPage<TBImportacao> Get([FromQuery] string tx_dsc, [FromQuery] DataPage<TBImportacao> page)
        {
            page = _ImportacaoRepository.GetByPage(page, tx_dsc);

            return page;
        }

        // GET: api/Importacao/5
        [HttpGet]
        [Route("api/v1/Importacao/{id}")]
        public TBImportacao Get(long id)
        {
            TBImportacao Importacao = _ImportacaoRepository.Find(p => p.CD_IMP == id);
            
            return Importacao;
        }

        // POST: api/Importacao
        [HttpPost]
        [Route("api/v1/Importacao")]
        public object Post([FromBody] TBImportacao item)
        {
            try
            {
                _ImportacaoRepository.Insert(item);
                return new { message = "OK" };
            }
            catch (Exception ex)
            {
                return new { message = "Error." + ex.Message };
            }
        }

        // PUT: api/Importacao/5
        [HttpPut]
        [Route("api/v1/Importacao/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] TBImportacao item)
        {
            try
            {
                if (id > 0)
                    _ImportacaoRepository.Update(item);
                else
                    _ImportacaoRepository.Insert(item);

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
                // return new { message = "Error." + ex.Message };
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        [Route("api/v1/Importacao/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var objeto = _ImportacaoRepository.FindById(id);

                _ImportacaoRepository.Delete(objeto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao tentar excluir o registro. {ex.Message}");
            }
        }























        //// GET: api/<controller>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
