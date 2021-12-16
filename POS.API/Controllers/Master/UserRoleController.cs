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
    public class UserRoleController : ControllerBase
    {
        #region Private methods
        private IRepositoryWrapper _repository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        private Boolean IsForeignKeyReference(Guid id)
        {
            Boolean result =
                (_repository.Accounts.GetAccountByRoleId(id).Count > 0)
                //|| (_repository.Accounts.GetAccountById(roleId) != null)
                ;

            return result;
        }

        #endregion

        public UserRoleController(IRepositoryWrapper repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Return all user roles in the system. Order by role name column
        /// </summary>
        /// <response code="200">Return all user roles in the system. Order by role column</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public IActionResult Get()
        {
            var userRoles = _repository.MasterRole.FindAll().OrderBy(x => x.Name);

            return Ok(userRoles);
        }

        /// <summary>
        /// Return the user role, searched for the specific 'id'.
        /// </summary>
        /// <response code="200">Return the user role, searched for the specific 'id'</response>
        /// <response code="404">User role not found for the specific 'id'</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult Get(Guid id)
        {
            var userRole = _repository.MasterRole.GetRoleById(id);
            if (userRole == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(userRole);
            }
        }

        /// <summary>
        /// Return the user role, searched for the specific 'id'.
        /// </summary>
        /// <response code="200">Return the user role, searched for the specific 'id'</response>
        /// <response code="404">User role not found for the specific 'id'</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Route("{roleName}")]
        public IActionResult Get(string roleName)
        {
            var userRole = _repository.MasterRole.GetRoleByName(roleName);
            if (userRole == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(userRole);
            }
        }

        /// <summary>
        /// Creates a user role in the system.
        /// </summary>
        /// <response code="201">User role created successfull in the system</response>
        /// <response code="400">Unable to create due to validation error</response>
        /// <response code="409">Dupplicate record found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        public IActionResult Create([FromBody] MasterRoleForCreateEntity userRole)
        {
            if (userRole == null || !ModelState.IsValid)
            {
                _logger.LogError("User role object sent from client is null.");
                return BadRequest("Please provide all required information.");
            }

            Boolean isDupplicate = _repository.MasterRole.IsDuplicate(userRole.Name);

            if (isDupplicate)
            {
                return Conflict("Dupplicate record found");
            }

            var createUserRole = _mapper.Map<MasterRole>(userRole);

            _repository.MasterRole.CreateRole(createUserRole, userRole.UserId);
            _repository.Save();

            var createdUserRole = _mapper.Map<MasterRoleEntity>(createUserRole);

            return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + createdUserRole.Id,
                createdUserRole);
        }

        /// <summary>
        /// Update the user role, for the specific 'id'
        /// </summary>
        /// <response code="204">Update the user role, for the specific 'id'</response>
        /// <response code="400">Unable to update due to validation error</response>
        /// <response code="404">User role not found for the specific 'id'</response>
        /// <response code="409">Dupplicate record found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Put(Guid id, [FromBody] MasterRoleForUpdateEntity userRole)
        {
            if (userRole == null || !ModelState.IsValid)
            {
                _logger.LogError("User role object sent from client is null.");
                return BadRequest("Please provide all required information.");
            }

            Boolean isDupplicate = _repository.MasterRole.IsDuplicateForUpdate(userRole.Name, id);

            if (isDupplicate)
            {
                return Conflict("Dupplicate record found");
            }

            var userRoleEntity = _repository.MasterRole.GetRoleById(id);

            if (userRoleEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(userRole, userRoleEntity);

            _repository.MasterRole.UpdateRole(userRoleEntity, userRole.UserId);
            _repository.Save();

            return NoContent();
        }

        /// <summary>
        /// Delete the user role, for the specific 'id'
        /// </summary>
        /// <response code="204">Delete the user role, for the specific 'id'</response>
        /// <response code="404">User role not found for the specific 'id'</response>
        /// <response code="409">Cannot delete, due to referencing data found in other tables</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete(Guid id)
        {   
            if(IsForeignKeyReference(id) == true)
            {
                return Conflict("Cannot delete, due to referencing data found in other tables.");
            }

            var userRole = _repository.MasterRole.GetRoleById(id);
            if (userRole == null)
            {
                return NotFound();
            }
            
            _repository.MasterRole.Delete(userRole);
            _repository.Save();

            return NoContent();
        }
    }
}
