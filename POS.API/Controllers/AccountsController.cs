using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using POS.API.Extensions;
using POS.Business.Data;
using POS.Business.Interfaces;
using POS.Data.Entities;
using POS.Data.Models;
using System;
using System.Linq;

namespace POS.API.Controllers
{
    [Authorize]
    //[ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        #region Private methods
        private IRepositoryWrapper _repository;
        private ILoggerManager _logger;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private IAuthManager _authManager;

        private Boolean IsForeignKeyReference(Guid id)
        {
            Boolean result = false;

            result =
                (_repository.Accounts.GetAccountByRoleId(id).Count > 0)
                //|| (_repository.Accounts.GetAccountById(roleId) != null)
                ;

            return result;
        }

        #endregion
        public AccountsController(IRepositoryWrapper repository, ILoggerManager logger, IMapper mapper, IOptions<AppSettings> appSettings, IAuthManager authManager)
        {
            this._repository = repository;
            this._logger = logger;
            this._mapper = mapper;
            this._appSettings = appSettings.Value;
            this._authManager = authManager;
        }

        /// <summary>
        /// Login or validate user credentails
        /// </summary>
        /// <response code="200">Return the token and login details after validating user</response>
        /// <response code="400">Unable to procced due to validation error</response>
        /// <response code="401">Unable to login due to invalid or expired credentials. Unauthoorized access.</response>
        /// <response code="500">Internal server error</response>
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] AccountsForLoginEntity loginEntity)
        {
            if (loginEntity == null || !ModelState.IsValid)
            {
                _logger.LogError("Login object sent from client is null.");
                return BadRequest("Please provide all required information.");
            }

            var login = _repository.Accounts.ValidateUser(loginEntity);
            if (login == null)
            {
                return Unauthorized("Invalid or expired login!");
            }

            var token = _authManager.Authenticate(login.Username);

            return Ok(new { Token = token, Userid = login.Id, Username = login.Username });
        }

        /// <summary>
        /// Return all users in the system. Order by Username column
        /// </summary>
        /// <response code="200">Return all users in the system. Order by Username column</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public IActionResult Get()
        {
            var accounts = _repository.Accounts.FindAll().OrderBy(x => x.Username);
            return Ok(accounts);
        }

        /// <summary>
        /// Return the user account, searched for the specific 'id'.
        /// </summary>
        /// <response code="200">Return the user account, searched for the specific 'id'</response>
        /// <response code="404">User account not found for the specific 'id'</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult Get(Guid id)
        {
            var accounts = _repository.Accounts.GetAccountById(id);
            if (accounts == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(accounts);
            }
        }

        /// <summary>
        /// Return the user account, searched for the specific 'name'.
        /// </summary>
        /// <response code="200">Return the user account, searched for the specific 'name'</response>
        /// <response code="404">User account not found for the specific search name</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Route("{searchText}")]
        public IActionResult Get(string searchText)
        {
            var accounts = _repository.Accounts.GetAccountByFirstLastOrUserName(searchText);
            if (accounts == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(accounts);
            }
        }


        /// <summary>
        /// Creates a new account in the system
        /// </summary>
        /// <response code="201">User account created successfull in the system</response>
        /// <response code="400">Unable to create due to validation error</response>
        /// <response code="409">Dupplicate record found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        public IActionResult Create([FromBody] AccountsForCreateEntity accountsEntity)
        {
            if (accountsEntity == null || !ModelState.IsValid)
            {
                _logger.LogError("Accounts object sent from client is null.");
                return BadRequest("Please provide all required information.");
            }

            Boolean isDupplicate = _repository.Accounts.IsDuplicate(accountsEntity.Username);

            if (isDupplicate)
            {
                return Conflict("Dupplicate record found");
            }

            var createAccount = _mapper.Map<Accounts>(accountsEntity);

            _repository.Accounts.CreateAccount(createAccount, accountsEntity.UserId);
            _repository.Save();

            var createdAccount = _mapper.Map<AccountsEntity>(createAccount);

            return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + createdAccount.Id,
                createdAccount);
        }

        /// <summary>
        /// Update the user account, for the specific 'id'
        /// </summary>
        /// <response code="204">Update the user account, for the specific 'id'</response>
        /// <response code="400">Unable to update due to validation error</response>
        /// <response code="404">User account not found for the specific 'id'</response>
        /// <response code="500">Internal server error</response>
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Put(Guid id, [FromBody] AccountsForUpdateEntity account)
        {
            if (account == null || !ModelState.IsValid)
            {
                _logger.LogError("User account object sent from client is null.");
                return BadRequest("Please provide all required information.");
            }

            //Duplicate check not required here, because we does not allow to update username. 

            var accountEntity = _repository.Accounts.GetAccountById(id);

            if (accountEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(account, accountEntity);

            _repository.Accounts.UpdateAccount(accountEntity, account.UserId);
            _repository.Save();

            return NoContent();
        }

        /// <summary>
        /// Delete the user role, for the specific 'id'
        /// </summary>
        /// <response code="409">User account cannot be deleted</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete(Guid id)
        {
            return Conflict("User account cannot be deleted.");

            //if (IsForeignKeyReference(id) == true)
            //{
            //    return Conflict("Cannot delete, due to referencing data found in other tables.");
            //}

            //var account = _repository.Accounts.GetAccountById(id);
            //if (account == null)
            //{
            //    return NotFound();
            //}

            //_repository.Accounts.Delete(account);
            //_repository.Save();

            //return NoContent();
        }


        /// <summary>
        /// Update/reset password, for the specific 'username'
        /// </summary>
        /// <response code="204">Update/reset password, for the specific 'username'</response>
        /// <response code="400">Unable to update due to validation error</response>
        /// <response code="404">User account not found for the specific 'username'</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Route("resetpassword")]
        public IActionResult ResetPassword([FromBody] AccountsForLoginEntity loginEntity)
        {
            if (loginEntity == null || !ModelState.IsValid)
            {
                _logger.LogError("Login object sent from client is null.");
                return BadRequest("Please provide all required information.");
            }

            var accountEntity = _repository.Accounts.GetAccountByUserName(loginEntity.Username);

            if (accountEntity == null)
            {
                return NotFound();
            }


            _mapper.Map(loginEntity, accountEntity);

            _repository.Accounts.ResetPassword(accountEntity, loginEntity.UserId);
            _repository.Save();

            return Ok();
        }
    }
}

