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
    public class ProductController : ControllerBase
    {
        #region Private methods
        private IRepositoryWrapper _repository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        private Boolean IsForeignKeyReference(Guid id)
        {
            Boolean result = false;
            //(_repository.Accounts.GetAccountByRoleId(id).Count > 0)
            //|| (_repository.Accounts.GetAccountById(roleId) != null)
            ;

            return result;
        }

        #endregion

        public ProductController(IRepositoryWrapper repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }


        /// <summary>
        /// Return all Products in the system. Order by name column
        /// </summary>
        /// <response code="200">Return all Products in the system. Order by name column</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public IActionResult Get()
        {
            var rec = _repository.MasterProduct.FindAll().OrderBy(x => x.Name);

            return Ok(rec);
        }

        /// <summary>
        /// Return the Product, searched for the specific 'id'.
        /// </summary>
        /// <response code="200">Return the Product, searched for the specific 'id'</response>
        /// <response code="404">Product not found for the specific 'id'</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult Get(Guid id)
        {
            var rec = _repository.MasterProduct.GetProductById(id);
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
        /// Return next available product code
        /// </summary>
        /// <response code="200">Return next available product code</response>
        [HttpGet]
        [Route("getavailabelcode")]
        public IActionResult GetAvailabelCode()
        {
            int nextCode = _repository.MasterProduct.GetAvailableNextCode();


            return Ok(new { code = nextCode });
        }

        /// <summary>
        /// Return the Product, searched for the specific seached fields.
        /// </summary>
        /// <response code="200">Return the Product, searched for the specific seached fields</response>
        /// <response code="404">Product not found for the specific seached fields</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Route("getsearchresult")]
        public IActionResult GetSearchResult([FromBody] MasterProductForSearch searchRec)
        {
            var rec = _repository.MasterProduct.SearchProductList(searchRec);
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
        /// Creates a Product in the system.
        /// </summary>
        /// <response code="201">Product created successfull in the system</response>
        /// <response code="400">Unable to create due to validation error</response>
        /// <response code="409">Dupplicate record found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        public IActionResult Create([FromBody] MasterProductForCreateEntity rec)
        {
            if (rec == null || !ModelState.IsValid)
            {
                _logger.LogError("Product object sent from client is null.");
                return BadRequest("Please provide all required information.");
            }

            Boolean isDupplicate = _repository.MasterProduct.IsDuplicate(rec.Name);

            if (isDupplicate)
            {
                return Conflict("Dupplicate record found");
            }

            isDupplicate = _repository.MasterProduct.IsDuplicateCode(rec.Code);

            if (isDupplicate)
            {
                return Conflict("Dupplicate 'Product Code' found");
            }

            isDupplicate = _repository.MasterProduct.IsDuplicateBarcode(rec.Barcode);

            if (isDupplicate)
            {
                return Conflict("Dupplicate 'Barcode' found");
            }

            var createRec = _mapper.Map<MasterProduct>(rec);

            _repository.MasterProduct.CreateProduct(createRec);
            _repository.Save();

            var createdRec = _mapper.Map<MasterProduct>(createRec);

            return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + createdRec.Id,
                createdRec);
        }

        /// <summary>
        /// Update the Product, for the specific 'id'
        /// </summary>
        /// <response code="204">Update the Product, for the specific 'id'</response>
        /// <response code="400">Unable to update due to validation error</response>
        /// <response code="404">Product not found for the specific 'id'</response>
        /// <response code="409">Dupplicate record found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Put(Guid id, [FromBody] MasterProductForUpdateEntity rec)
        {
            if (rec == null || !ModelState.IsValid)
            {
                _logger.LogError("Product object sent from client is null.");
                return BadRequest("Please provide all required information.");
            }

            Boolean isDupplicate = _repository.MasterProduct.IsDuplicateForUpdate(rec.Name, id);

            if (isDupplicate)
            {
                return Conflict("Dupplicate record found");
            }

            isDupplicate = _repository.MasterProduct.IsDuplicateCodeForUpdate(rec.Code, id);

            if (isDupplicate)
            {
                return Conflict("Dupplicate 'Product code' found");
            }

            isDupplicate = _repository.MasterProduct.IsDuplicateBarcodeForUpdate(rec.Barcode, id);

            if (isDupplicate)
            {
                return Conflict("Dupplicate 'Barcode' found");
            }

            var recEntity = _repository.MasterProduct.GetProductById(id);

            if (recEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(rec, recEntity);

            _repository.MasterProduct.Update(recEntity);
            _repository.Save();

            return NoContent();
        }

        /// <summary>
        /// Delete the Product, for the specific 'id'
        /// </summary>
        /// <response code="204">Delete the Product, for the specific 'id'</response>
        /// <response code="404">Product not found for the specific 'id'</response>
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

            var rec = _repository.MasterProduct.GetProductById(id);
            if (rec == null)
            {
                return NotFound();
            }

            _repository.MasterProduct.Delete(rec);
            _repository.Save();

            return NoContent();
        }
    }
}
