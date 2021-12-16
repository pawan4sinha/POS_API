using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Business.Data;
using POS.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.API.Controllers.Master
{
    [Authorize]
    [Route("api/master/[controller]")]
    [ApiController]
    public class ExpenseTypeController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        public ExpenseTypeController(IRepositoryWrapper repository, ILoggerManager logger, IMapper mapper)
        {
            this._repository = repository;
            this._logger = logger;
            this._mapper = mapper;
        }

        /// <summary>
        /// Return all 'Expense Type' in the system. Order by name column
        /// </summary>
        /// <response code="200">Return all 'Expense Type' in the system. Order by name column</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public IActionResult Get()
        {
            var rec = _repository.MasterExpenseType.FindAll().OrderBy(x => x.Name);
            return Ok(rec);
        }
    }
}
