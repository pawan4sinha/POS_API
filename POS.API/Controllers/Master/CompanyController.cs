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
    public class CompanyController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        public CompanyController(IRepositoryWrapper repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a Company in the system.
        /// </summary>
        /// <response code="201">Company created successfull in the system</response>
        /// <response code="400">Unable to create due to validation error</response>
        /// <response code="409">Dupplicate record found</response>
        /// <response code="409">Cannot create, becasue default already exsits</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        public IActionResult Create([FromBody] CompanyForCreateEntity comnpanyDetail)
        {
            if (comnpanyDetail == null || !ModelState.IsValid)
            {
                _logger.LogError("Company object sent from client is null.");
                return BadRequest("Please provide all required information.");
            }

            Boolean isDupplicate = _repository.MasterCompany.IsDuplicate(comnpanyDetail.Name);

            if(isDupplicate)
            {
                return Conflict("Dupplicate record found");
            }

            Boolean isDefaultExists = _repository.MasterCompany.IsDefaultExists();

            if (isDefaultExists && comnpanyDetail.SetDefault == 1)
            {
                return Conflict("Default already exsits");
            }

            var createCompany = _mapper.Map<MasterCompany>(comnpanyDetail);

            _repository.MasterCompany.CreateCompany(createCompany, comnpanyDetail.UserId);
            _repository.Save();

            var createdCompany = _mapper.Map<MasterCompanyEntity>(createCompany);

            return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + createdCompany.Id,
                createdCompany);
        }


        /// <summary>
        /// Return all companies in the system. Order by company name column
        /// </summary>
        /// <response code="200">Return all companies in the system. Order by company column</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public IActionResult Get()
        {
            var companies = _repository.MasterCompany.FindAll().OrderBy(x => x.Name);

            return Ok(companies);
        }

        /// <summary>
        /// Return the company, searched for the specific 'id'.
        /// </summary>
        /// <response code="200">Return the company, searched for the specific 'id'</response>
        /// <response code="404">Company not found for the specific 'id'</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult Get(Guid id)
        {
            var company = _repository.MasterCompany.GetCompanyById(id);
            if (company == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(company);
            }
        }

        /// <summary>
        /// Update the company, for the specific 'id'
        /// </summary>
        /// <response code="204">Update the company, for the specific 'id'</response>
        /// <response code="400">Unable to update due to validation error</response>
        /// <response code="404">Company not found for the specific 'id'</response>
        /// <response code="409">Dupplicate record found</response>
        /// <response code="409">Cannot update, becasue default already exsits</response>
        /// <response code="500">Internal server error</response>
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Put(Guid id, [FromBody] CompanyForUpdateEntity company)
        {
            if (company == null || !ModelState.IsValid)
            {
                _logger.LogError("Company object sent from client is null.");
                return BadRequest("Please provide all required information.");
            }

            Boolean isDupplicate = _repository.MasterCompany.IsDuplicateForUpdate(company.Name, id);

            if (isDupplicate)
            {
                return Conflict("Dupplicate record found");
            }

            Boolean isDefaultExists = _repository.MasterCompany.IsDefaultExistsForUpdate(id);

            if (isDefaultExists && company.SetDefault == 1)
            {
                return Conflict("Default already exsits");
            }

            var companyEntity = _repository.MasterCompany.GetCompanyById(id);

            if (companyEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(company, companyEntity);

            _repository.MasterCompany.UpdateCompany(companyEntity, company.UserId);
            _repository.Save();

            return NoContent();
        }

        /// <summary>
        /// Delete the company, for the specific 'id'
        /// </summary>
        /// <response code="204">Delete the company, for the specific 'id'</response>
        /// <response code="404">Company not found for the specific 'id'</response>
        /// <response code="409">Default set company cannot be deleted</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete(Guid id)
        {
            var company = _repository.MasterCompany.GetCompanyById(id);
            if (company == null)
            {
                return NotFound();
            }

            Boolean IsDeletingRecordIsDefault = _repository.MasterCompany.IsDeletingRecordIsDefault(id);

            if (IsDeletingRecordIsDefault)
            {
                return Conflict("Default set company cannot be deleted");
            }

            _repository.MasterCompany.Delete(company);
            _repository.Save();

            return NoContent();
        }
    }
}
