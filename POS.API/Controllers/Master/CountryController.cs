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
    //[ApiVersion("1.0")]
    [Route("api/master/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        public CountryController(IRepositoryWrapper repository, ILoggerManager logger, IMapper mapper)
        {
            this._repository = repository;
            this._logger = logger;
            this._mapper = mapper;
        }

        /// <summary>
        /// Return all active Countries in the system. Order by Country Name column
        /// </summary>
        /// <response code="200">Return all active countries in the system. Order by country name column</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public IActionResult Get()
        {
            var countries = _repository.MasterCountry.FindAll().Where(x => x.IsActive == 1).OrderBy(x => x.Name);
            return Ok(countries);
        }

    }
}
