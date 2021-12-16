using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Business.Data;
using POS.Business.Interfaces;
using POS.Data.Entities;
using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.API.Controllers.Master
{
    [Authorize]
    [Route("api/master/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        #region Private methods
        private IRepositoryWrapper _repository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        private Boolean IsForeignKeyReference(Guid id)
        {
            Boolean result = false;
                //(_repository.MasterTaxType.GetTaxTypeById(id) != null)
                ;

            return result;
        }

        #endregion

        public WalletController(IRepositoryWrapper repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Return all 'Wallets' in the system. Order by  name column
        /// </summary>
        /// <response code="200">Return all 'Wallets' in the system. Order by name column</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public IActionResult Get()
        {
            var rec = _repository.MasterWallet.FindAll().OrderBy(x => x.Name);

            return Ok(rec);
        }

        /// <summary>
        /// Return the 'Wallets', searched for the specific 'id'.
        /// </summary>
        /// <response code="200">Return the 'Wallets', searched for the specific 'id'</response>
        /// <response code="404">'Wallets' not found for the specific 'id'</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult Get(Guid id)
        {
            var rec = _repository.MasterWallet.GetWalletById(id);
            if (rec == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rec);
            }
        }

        /// <summary>
        /// Creates a 'Wallet' in the system.
        /// </summary>
        /// <response code="201">'Wallet' created successfull in the system</response>
        /// <response code="400">Unable to create due to validation error</response>
        /// <response code="409">Dupplicate record found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        public IActionResult Create([FromBody] MasterWalletForCreateEntity rec)
        {
            if (rec == null || !ModelState.IsValid)
            {
                _logger.LogError("Wallet object sent from client is null.");
                return BadRequest("Please provide all required information.");
            }

            Boolean isDupplicate = _repository.MasterWallet.IsDuplicate(rec.Name);

            if (isDupplicate)
            {
                return Conflict("Dupplicate record found");
            }

            var createRec = _mapper.Map<MasterWallet>(rec);

            _repository.MasterWallet.CreateWallet(createRec);
            _repository.Save();

            var createdRec = _mapper.Map<MasterWallet>(createRec);

            return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + createdRec.Id,
                createdRec);
        }

        /// <summary>
        /// Update the 'Wallet', for the specific 'id'
        /// </summary>
        /// <response code="204">Update the 'Wallet', for the specific 'id'</response>
        /// <response code="400">Unable to update due to validation error</response>
        /// <response code="404">'Wallet' not found for the specific 'id'</response>
        /// <response code="409">Dupplicate record found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Put(Guid id, [FromBody] MasterWalletForUpdateEntity rec)
        {
            if (rec == null || !ModelState.IsValid)
            {
                _logger.LogError("Wallet object sent from client is null.");
                return BadRequest("Please provide all required information.");
            }

            Boolean isDupplicate = _repository.MasterWallet.IsDuplicateForUpdate(rec.Name, id);

            if (isDupplicate)
            {
                return Conflict("Dupplicate record found");
            }

            var recEntity = _repository.MasterWallet.GetWalletById(id);

            if (recEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(rec, recEntity);

            _repository.MasterWallet.Update(recEntity);
            _repository.Save();

            return NoContent();
        }

        /// <summary>
        /// Delete the 'Wallet', for the specific 'id'
        /// </summary>
        /// <response code="204">Delete the 'Wallet', for the specific 'id'</response>
        /// <response code="404">'Wallet' not found for the specific 'id'</response>
        /// <response code="409">Cannot delete, due to referencing data found in other tables</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete(Guid id)
        {
            if (IsForeignKeyReference(id) == true)
            {
                return Conflict("Cannot delete, due to referencing data found in other tables.");
            }

            var rec = _repository.MasterWallet.GetWalletById(id);
            if (rec == null)
            {
                return NotFound();
            }

            _repository.MasterWallet.Delete(rec);
            _repository.Save();

            return NoContent();
        }
    }
}
