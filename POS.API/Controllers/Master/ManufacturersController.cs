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
    public class ManufacturersController : ControllerBase
    {
        #region Private methods
        private IRepositoryWrapper _repository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        private Boolean IsForeignKeyReference(Guid id)
        {
            Boolean result = false
            //(_repository.ProductSubCategory.GetProductSubCategoryById(id) != null)
            //|| (_repository.Accounts.GetAccountById(roleId) != null)
            ;

            return result;
        }

        #endregion

        public ManufacturersController(IRepositoryWrapper repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Return all 'Manufacturers' in the system. Order by name column
        /// </summary>
        /// <response code="200">Return all 'Manufacturers' in the system. Order by name column</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public IActionResult Get()
        {
            var rec = _repository.MasterManufacturers.FindAll().OrderBy(x => x.Name).OrderBy(x => x.SortOrder);

            return Ok(rec);
        }

        /// <summary>
        /// Return the 'Manufacturer', searched for the specific 'id'.
        /// </summary>
        /// <response code="200">Return the 'Manufacturer', searched for the specific 'id'</response>
        /// <response code="404">'Manufacturer' not found for the specific 'id'</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult Get(Guid id)
        {
            var rec = _repository.MasterManufacturers.GetManufacturerById(id);
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
        /// Return the 'Manufacturers', searched for the specific 'name'.
        /// </summary>
        /// <response code="200">Return the 'Manufacturers', searched for the specific 'name'</response>
        /// <response code="404">'Manufacturers' not found for the specific 'name'</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Route("{searchText}")]
        public IActionResult Get(string searchText)
        {
            var rec = _repository.MasterManufacturers.GetManufacturerByName(searchText);
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
        /// Creates a 'Manufacturer' in the system.
        /// </summary>
        /// <response code="201">'Manufacturer' created successfull in the system</response>
        /// <response code="400">Unable to create due to validation error</response>
        /// <response code="409">Dupplicate record found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        public IActionResult Create([FromBody] MasterManufacturersForCreateEntity rec)
        {
            if (rec == null || !ModelState.IsValid)
            {
                _logger.LogError("'Manufacturer' object sent from client is null.");
                return BadRequest("Please provide all required information.");
            }

            Boolean isDupplicate = _repository.MasterManufacturers.IsDuplicate(rec.Name);

            if (isDupplicate)
            {
                return Conflict("Dupplicate record found");
            }

            var createRec = _mapper.Map<MasterManufacturers>(rec);

            _repository.MasterManufacturers.CreateManufacturer(createRec);
            _repository.Save();

            var createdRec = _mapper.Map<MasterManufacturers>(createRec);

            return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + createdRec.Id,
                createdRec);
        }

        /// <summary>
        /// Update the 'Manufacturer', for the specific 'id'
        /// </summary>
        /// <response code="204">Update the 'Manufacturer', for the specific 'id'</response>
        /// <response code="400">Unable to update due to validation error</response>
        /// <response code="404">'Manufacturer' not found for the specific 'id'</response>
        /// <response code="409">Dupplicate record found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Put(Guid id, [FromBody] MasterManufacturersForUpdateEntity rec)
        {
            if (rec == null || !ModelState.IsValid)
            {
                _logger.LogError("'Manufacturer' object sent from client is null.");
                return BadRequest("Please provide all required information.");
            }

            Boolean isDupplicate = _repository.MasterManufacturers.IsDuplicateForUpdate(rec.Name, id);

            if (isDupplicate)
            {
                return Conflict("Dupplicate record found");
            }

            var recEntity = _repository.MasterManufacturers.GetManufacturerById(id);

            if (recEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(rec, recEntity);

            _repository.MasterManufacturers.Update(recEntity);
            _repository.Save();

            return NoContent();
        }

        /// <summary>
        /// Delete the 'Manufacturer', for the specific 'id'
        /// </summary>
        /// <response code="204">Delete the 'Manufacturer', for the specific 'id'</response>
        /// <response code="404">'Manufacturer' not found for the specific 'id'</response>
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

            var rec = _repository.MasterManufacturers.GetManufacturerById(id);
            if (rec == null)
            {
                return NotFound();
            }

            _repository.MasterManufacturers.Delete(rec);
            _repository.Save();

            return NoContent();
        }
    }
}
