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
    public class CEMercanteItensController : ControllerBase
    {
        private readonly ICEMercanteItensRepository _ceMercanteItensRepository;

        public CEMercanteItensController( ICEMercanteItensRepository ceMercanteItensRepository )
        {
            _ceMercanteItensRepository = ceMercanteItensRepository;
        }

        // GET: api/CEMercanteItens
        [HttpGet]
        [Route( "api/v1/cemercanteitens/todos" )]
        public IEnumerable<CEMercanteItens> Get()
        {
            var result = _ceMercanteItensRepository.FindAll();
            return result;
        }

        // GET: api/CEMercanteItens
        [HttpGet]
        [Route( "api/v1/cemercanteitens/" )]
        public DataPage<CEMercanteItens> Get( [FromQuery] string tx_dsc, [FromQuery] DataPage<CEMercanteItens> page )
        {
            page = _ceMercanteItensRepository.GetByPage( page, tx_dsc );

            return page;
        }

        // GET: api/CEMercanteItens/5
        [HttpGet]
        [Route( "api/v1/cemercanteitens/{id}" )]
        public CEMercanteItens Get( long id )
        {
            CEMercanteItens CEMercanteItens = _ceMercanteItensRepository.Find( p => p.CD_CE_ITEM == id );

            return CEMercanteItens;
        }

        // POST: api/CEMercanteItens
        [HttpPost]
        [Route( "api/v1/cemercanteitens" )]
        public object Post( [FromBody] CEMercanteItens item )
        {
            try
            {
                _ceMercanteItensRepository.Insert( item );
                return new { message = "OK" };
            }
            catch ( Exception ex )
            {
                return new { message = "Error." + ex.Message };
            }
        }

        // PUT: api/CEMercanteItens/5
        [HttpPut]
        [Route( "api/v1/cemercanteitens/{id}" )]
        [ProducesResponseType( StatusCodes.Status400BadRequest )]
        public IActionResult Put( int id, [FromBody] CEMercanteItens item )
        {
            try
            {
                if ( id > 0 )
                    _ceMercanteItensRepository.Update( item );
                else
                    _ceMercanteItensRepository.Insert( item );

                return Ok( item );
            }
            catch ( Exception ex )
            {
                return StatusCode( (int)HttpStatusCode.BadRequest, ex.Message );
                // return new { message = "Error." + ex.Message };
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        [Route( "api/v1/cemercanteitens/{id}" )]
        public IActionResult Delete( int id )
        {
            try
            {
                var objeto = _ceMercanteItensRepository.FindById( id );

                _ceMercanteItensRepository.Delete( objeto );
                return Ok();
            }
            catch ( Exception ex )
            {
                return BadRequest( $"Erro ao tentar excluir o registro. {ex.Message}" );
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        [Route( "api/v1/cemercanteitens/deleteall" )]
        public IActionResult DeleteAll()
        {
            try
            {
                _ceMercanteItensRepository.DeleteAll();
                return Ok();
            }
            catch ( Exception ex )
            {
                return BadRequest( $"Erro ao tentar excluir os registros. {ex.Message}" );
            }
        }
    }
}