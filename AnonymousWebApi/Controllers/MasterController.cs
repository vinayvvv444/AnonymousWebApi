using AnonymousWebApi.Data.Contracts;
using AnonymousWebApi.Data.DomainModel.Master;
using AnonymousWebApi.Data.EFCore.Repository.Master;
using AnonymousWebApi.Models.DataTableModels;
using AnonymousWebApi.Models.DT_Dto;
using AnonymousWebApi.Models.Master;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly ILogger<MasterController> _loggerNew;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private CountryRepository _countryRepository;
        private StateRepository _stateRepository;

        public MasterController(IMapper mapper, CountryRepository countryRepository, StateRepository stateRepository, ILoggerManager logger, ILogger<MasterController> loggerNew)
        {
            _mapper = mapper;
            _logger = logger;
            _countryRepository = countryRepository;
            _loggerNew = loggerNew;
            _stateRepository = stateRepository;
        }


        #region Country master

        [HttpPost]
        [Authorize]
        [Route("PostCountry")]
        public async Task<IActionResult> AddCountry(CountryModel model)
        {
            await _countryRepository.Add(_mapper.Map<CountryModel, Country>(model)).ConfigureAwait(false);
            return Ok();
           
        }

        [HttpPost]
        [Route("CreateCountry")]
        public async Task<IActionResult> AddCountryUsingDapper(CountryModel model)
        {
            await _countryRepository.AddCountryUsingDapper(_mapper.Map<CountryModel, Country>(model)).ConfigureAwait(false);
            return Ok();
        }

        [HttpGet]
        [Route("GetAllCountry")]
        public async Task<IActionResult> GetAllCountries()
        {

            var data = await _countryRepository.GetAllCountries().ConfigureAwait(false);
            var mapData = _mapper.Map<IEnumerable<Country>, ICollection<CountryModel>>(data);
            
            return Ok(new { data = mapData, recordsTotal = mapData.Count, recordsFiltered = mapData.Count, draw = 0 });
        }

        [HttpGet]
       
        [Route("GetLogs")]
        public IEnumerable<string> Get()
        {
            _loggerNew.LogInformation("Info logging");
            _logger.LogInfo("Here is info message from the controller.");
            _logger.LogDebug("Here is debug message from the controller.");
            throw new Exception("Exception while fetching all the students from the storage.");
            _logger.LogWarn("Here is warn message from the controller.");
            _logger.LogError("Here is error message from the controller.");

            return new string[] { "value1", "value2" };
        }

        [HttpGet("GetData")]
        public IActionResult GetJson()
        {
            string str = "{\"data\":[{\"id\":860,\"firstName\":\"Superman\",\"lastName\":\"Yoda\"},{\"id\":870,\"firstName\":\"Foo\",\"lastName\":\"Whateveryournameis\"},{\"id\":590,\"firstName\":\"Toto\",\"lastName\":\"Titi\"},{\"id\":860,\"firstName\":\"Superman\",\"lastName\":\"Yoda\"},{\"id\":870,\"firstName\":\"Foo\",\"lastName\":\"Whateveryournameis\"},{\"id\":590,\"firstName\":\"Toto\",\"lastName\":\"Titi\"},{\"id\":860,\"firstName\":\"Superman\",\"lastName\":\"Yoda\"},{\"id\":870,\"firstName\":\"Foo\",\"lastName\":\"Whateveryournameis\"},{\"id\":590,\"firstName\":\"Toto\",\"lastName\":\"Titi\"},{\"id\":860,\"firstName\":\"Superman\",\"lastName\":\"Yoda\"},{\"id\":870,\"firstName\":\"Foo\",\"lastName\":\"Whateveryournameis\"},{\"id\":590,\"firstName\":\"Toto\",\"lastName\":\"Titi\"},{\"id\":860,\"firstName\":\"Superman\",\"lastName\":\"Yoda\"},{\"id\":870,\"firstName\":\"Foo\",\"lastName\":\"Whateveryournameis\"},{\"id\":590,\"firstName\":\"Toto\",\"lastName\":\"Titi\"},{\"id\":860,\"firstName\":\"Superman\",\"lastName\":\"Yoda\"},{\"id\":870,\"firstName\":\"Foo\",\"lastName\":\"Whateveryournameis\"},{\"id\":590,\"firstName\":\"Toto\",\"lastName\":\"Titi\"}]}";
            JObject json = JObject.Parse(str);
            return Ok(json);
        }


        [HttpPut]
        [Route("GetCountryDT")]
        public async Task<IActionResult> Post([FromBody]PagingRequest paging)
        {
            var data = await _countryRepository.GetAllCountries().ConfigureAwait(false);
            var mapData = _mapper.Map<IEnumerable<Country>, IEnumerable<CountryModel>>(data);

            var pagingResponse = new PagingResponseCountry()
            {
                Draw = paging.Draw
            };

            if (!paging.SearchCriteria.IsPageLoad)
            {
                IEnumerable<CountryModel> query = null;

                if (!string.IsNullOrEmpty(paging.SearchCriteria.Filter))
                {
                    // query = _context.Users.Where(emp => emp.Name.Contains(paging.SearchCriteria.Filter) ||
                    //emp.Email.Contains(paging.SearchCriteria.Filter));
                    query = mapData.Where(x => x.Name.Contains(paging.SearchCriteria.Filter));
                }
                else
                {
                    query = mapData;
                }

                var recordsTotal = query.Count();

                var colOrder = paging.Order[0];

                switch (colOrder.Column)
                {
                    case 0:
                        query = colOrder.Dir == "asc" ? query.OrderBy(emp => emp.Id) : query.OrderByDescending(emp => emp.Id);
                        break;
                    case 1:
                        query = colOrder.Dir == "asc" ? query.OrderBy(emp => emp.Name) : query.OrderByDescending(emp => emp.Name);
                        break;
                    case 2:
                        query = colOrder.Dir == "asc" ? query.OrderBy(emp => emp.CreatedUser) : query.OrderByDescending(emp => emp.CreatedUser);
                        break;
                    case 3:
                        query = colOrder.Dir == "asc" ? query.OrderBy(emp => emp.CreatedDate) : query.OrderByDescending(emp => emp.CreatedDate);
                        break;
                }

                pagingResponse.Countries = query.Skip(paging.Start).Take(paging.Length).ToArray();
                pagingResponse.RecordsTotal = recordsTotal;
                pagingResponse.RecordsFiltered = recordsTotal;
            }

            return Ok(pagingResponse);
        }


        #endregion


        #region State master

        [HttpPost]
        [Authorize]
        [Route("CreateState")]
        public async Task<IActionResult> AddState(StateModel model)
        {
            await _stateRepository.Add(_mapper.Map<StateModel, State>(model)).ConfigureAwait(false);
            return Ok();

        }

        [HttpGet]
        [Route("GetAllStates")]
        public async Task<IActionResult> GetAllStates()
        {
            return Ok(await _stateRepository.GetAll().ConfigureAwait(false));
        }

        #endregion


        #region Quartz scheduler

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            var scheduler = await Task.Run(() =>
            StdSchedulerFactory.GetDefaultScheduler().GetAwaiter().GetResult()).ConfigureAwait(false);
            return Ok();
        }

        #endregion




    }
}