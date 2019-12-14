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
    public class CEMercanteController : ControllerBase
    {
        private readonly ICEMercanteRepository _cemercanteRepository;

        public CEMercanteController( ICEMercanteRepository ceMercanteRepository )
        {
            _cemercanteRepository = ceMercanteRepository;
        }

        // GET: api/CEMercante
        [HttpGet]
        [Route( "api/v1/cemercante/todos" )]
        public IEnumerable<CEMercante> Get()
        {
            var result = _cemercanteRepository.FindAll();
            return result;
        }

        // GET: api/CEMercante
        [HttpGet]
        [Route( "api/v1/cemercante/" )]
        public DataPage<CEMercante> Get( [FromQuery] string tx_dsc, [FromQuery] DataPage<CEMercante> page )
        {
            page = _cemercanteRepository.GetByPage( page, tx_dsc );

            return page;
        }

        // GET: api/CEMercante/5
        [HttpGet]
        [Route( "api/v1/cemercante/{id}" )]
        public CEMercante Get( long id )
        {
            CEMercante CEMercante = _cemercanteRepository.Find( p => p.CD_CE == id );

            return CEMercante;
        }

        // POST: api/CEMercante
        [HttpPost]
        [Route( "api/v1/cemercante" )]
        public object Post( [FromBody] CEMercante item )
        {
            try
            {
                _cemercanteRepository.Insert( item );
                return new { message = "OK" };
            }
            catch ( Exception ex )
            {
                return new { message = "Error." + ex.Message };
            }
        }

        // PUT: api/CEMercante/5
        [HttpPut]
        [Route( "api/v1/cemercante/{id}" )]
        [ProducesResponseType( StatusCodes.Status400BadRequest )]
        public IActionResult Put( int id, [FromBody] CEMercante item )
        {
            try
            {
                if ( id > 0 )
                    _cemercanteRepository.Update( item );
                else
                    _cemercanteRepository.Insert( item );

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
        [Route( "api/v1/cemercante/{id}" )]
        public IActionResult Delete( int id )
        {
            try
            {
                var objeto = _cemercanteRepository.FindById( id );

                _cemercanteRepository.Delete( objeto );
                return Ok();
            }
            catch ( Exception ex )
            {
                return BadRequest( $"Erro ao tentar excluir o registro. {ex.Message}" );
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        [Route( "api/v1/cemercante/deleteall" )]
        public IActionResult DeleteAll()
        {
            try
            {
                _cemercanteRepository.DeleteAll();
                return Ok();
            }
            catch ( Exception ex )
            {
                return BadRequest( $"Erro ao tentar excluir os registros. {ex.Message}" );
            }
        }
    }
}