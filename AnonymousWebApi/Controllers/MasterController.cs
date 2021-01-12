using AnonymousWebApi.ActionFilters;
using AnonymousWebApi.Data.Contracts;
using AnonymousWebApi.Data.Contracts.Master;
using AnonymousWebApi.Data.DomainModel.Master;
using AnonymousWebApi.Data.EFCore.Repository.Master;
using AnonymousWebApi.Models.DataTableModels;
using AnonymousWebApi.Models.DT_Dto;
using AnonymousWebApi.Models.Master;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Quartz.Impl;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AnonymousWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[AutoValidateAntiforgeryToken]
    public class MasterController : ControllerBase
    {
        private readonly ILogger<MasterController> _loggerNew;
        private readonly IMemoryCache memoryCache;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private CountryRepository _countryRepository;
        private StateRepository _stateRepository;
        private DistrictRepository _districtRepository;

        // changes done from development[changes from development branch] branch
        // changes done from [ changes from master branch] [dev changes] development branch
        // test source tree checkin

        ///Master controller
        public MasterController(IMapper mapper,
            CountryRepository countryRepository,
            StateRepository stateRepository,
            DistrictRepository districtRepository,
            ILoggerManager logger,
            ILogger<MasterController> loggerNew,
            IMemoryCache memoryCache)
        {
            _mapper = mapper;
            _logger = logger;
            _countryRepository = countryRepository;
            _loggerNew = loggerNew;
            this.memoryCache = memoryCache;
            _stateRepository = stateRepository;
            _districtRepository = districtRepository;
        }


        #region Country master

        /// <summary>
        /// Add country method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        // [Authorize]
        [Route("PostCountry")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCountry([FromBody] CountryModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var country = await _countryRepository.Add(_mapper.Map<CountryModel, Country>(model)).ConfigureAwait(false);
            //if(country == null)
            //{
            //    return BadRequest("Error creating country");
            //}

            var memoryCacheCountry = memoryCache.Get("Countries") as List<Country>;
            memoryCacheCountry.Add(country);

            memoryCache.Remove("Countries");
            memoryCache.Set("Countries", memoryCacheCountry);

            return Ok(country);

        }

        [HttpPost]
        [Route("CreateCountry")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCountryUsingDapper(CountryModel model)
        {
            await _countryRepository.AddCountryUsingDapper(_mapper.Map<CountryModel, Country>(model)).ConfigureAwait(false);
            return Ok();
        }

        [HttpPost]
        [Route("CreateCountryUsingTransaction")]
        public async Task<IActionResult> AddCountryUsingTransaction(CountryModel model)
        {
            await _countryRepository.AddCountryUsingTransaction(_mapper.Map<CountryModel, Country>(model)).ConfigureAwait(false);
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
        [Route("GetAllCountriesUsingFromSqlRaw")]
        public IActionResult GetAllCountriesUsingFromSqlRaw()
        {
            IEnumerable<Country> CountryList;


            if (!memoryCache.TryGetValue("Countries", out CountryList))
            {
                var result = _countryRepository.GetAllCountriesUsingFromSqlRaw();
                memoryCache.Set("Countries", result);
            }

            CountryList = memoryCache.Get("Countries") as List<Country>;

            var mapData = _mapper.Map<IEnumerable<Country>, IEnumerable<CountryModel>>(CountryList);
            return Ok(mapData);
        }

        [HttpGet]
        [Route("GetAllCountriesUsingFromSqlRawSP")]
        public IActionResult GetAllCountriesUsingFromSqlRawSp()
        {
            var result = _countryRepository.GetAllCountriesUsingFromSqlRawSP();
            var mapData = _mapper.Map<IEnumerable<Country>, IEnumerable<CountryModel>>(result);
            return Ok(mapData);
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

        //[Authorize]
        [HttpGet]
        [Route("GetAllCountryUsingEFCore")]
        public async Task<IActionResult> GetAllCountryUsingEFCore()
        {
            var result = await _countryRepository.GetAll().ConfigureAwait(false);
            return Ok(_mapper.Map<IEnumerable<Country>, IEnumerable<CountryModel>>(result));
        }

        [Authorize]
        [HttpGet]
        [Route("GetCountryById/{countryId}")]
        public async Task<IActionResult> GetCountryById(int countryId)
        {
            var countryData = await _countryRepository.Get(countryId).ConfigureAwait(false);
            if (countryData == null)
                return NotFound();

            return Ok(countryData);
        }

        [HttpGet("GetData")]
        public IActionResult GetJson()
        {
            string str = "{\"data\":[{\"id\":860,\"firstName\":\"Superman\",\"lastName\":\"Yoda\"},{\"id\":870,\"firstName\":\"Foo\",\"lastName\":\"Whateveryournameis\"},{\"id\":590,\"firstName\":\"Toto\",\"lastName\":\"Titi\"},{\"id\":860,\"firstName\":\"Superman\",\"lastName\":\"Yoda\"},{\"id\":870,\"firstName\":\"Foo\",\"lastName\":\"Whateveryournameis\"},{\"id\":590,\"firstName\":\"Toto\",\"lastName\":\"Titi\"},{\"id\":860,\"firstName\":\"Superman\",\"lastName\":\"Yoda\"},{\"id\":870,\"firstName\":\"Foo\",\"lastName\":\"Whateveryournameis\"},{\"id\":590,\"firstName\":\"Toto\",\"lastName\":\"Titi\"},{\"id\":860,\"firstName\":\"Superman\",\"lastName\":\"Yoda\"},{\"id\":870,\"firstName\":\"Foo\",\"lastName\":\"Whateveryournameis\"},{\"id\":590,\"firstName\":\"Toto\",\"lastName\":\"Titi\"},{\"id\":860,\"firstName\":\"Superman\",\"lastName\":\"Yoda\"},{\"id\":870,\"firstName\":\"Foo\",\"lastName\":\"Whateveryournameis\"},{\"id\":590,\"firstName\":\"Toto\",\"lastName\":\"Titi\"},{\"id\":860,\"firstName\":\"Superman\",\"lastName\":\"Yoda\"},{\"id\":870,\"firstName\":\"Foo\",\"lastName\":\"Whateveryournameis\"},{\"id\":590,\"firstName\":\"Toto\",\"lastName\":\"Titi\"}]}";
            JObject json = JObject.Parse(str);
            return Ok(json);
        }


        [HttpPost]
        [Route("GetCountryDT")]
        public async Task<IActionResult> Post([FromBody] PagingRequest paging)
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

                if (!string.IsNullOrEmpty(paging.Search.Value))
                {
                    // query = _context.Users.Where(emp => emp.Name.Contains(paging.SearchCriteria.Filter) ||
                    //emp.Email.Contains(paging.SearchCriteria.Filter));
                    query = mapData.Where(x => x.Name.ToLower().Contains(paging.Search.Value.ToLower()));
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

        [HttpPost]
        [Authorize]
        [Route("UpdateState")]
        public async Task<IActionResult> UpdateState(StateModel model)
        {
            await _stateRepository.Update(_mapper.Map<StateModel, State>(model)).ConfigureAwait(false);
            return Ok();

        }

        [HttpGet]
        [Route("GetAllStates")]
        public async Task<IActionResult> GetAllStates()
        {
            return Ok(await _stateRepository.GetAll().ConfigureAwait(false));
        }

        [HttpGet]
        [Route("GetAllStatesByCountryId/{countryId}")]
        public IActionResult GetAllStatesByCountryId(int countryId)
        {
            var result = _stateRepository.GetAllStatesByCountryIdUsingFromSqlRaw(countryId);
            var mapData = _mapper.Map<IEnumerable<State>, IEnumerable<StateModel>>(result);
            return Ok(mapData);
        }

        [HttpGet]
        [Route("GetStateById/{id}")]
        public async Task<IActionResult> GetStateById(int id)
        {
            return Ok(await _stateRepository.Get(id).ConfigureAwait(false));
        }

        [HttpPost]
        [Route("GetStateDT")]
        public async Task<IActionResult> GetAllStateDT([FromBody] PagingRequest paging)
        {
            var data = await _stateRepository.GetAll().ConfigureAwait(false);
            var mapData = _mapper.Map<IEnumerable<State>, IEnumerable<StateModel>>(data);

            var pagingResponse = new PagingResponseState()
            {
                Draw = paging.Draw
            };

            if (!paging.SearchCriteria.IsPageLoad)
            {
                IEnumerable<StateModel> query = null;

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

                if (!string.IsNullOrEmpty(paging.Search.Value))
                {
                    // query = _context.Users.Where(emp => emp.Name.Contains(paging.SearchCriteria.Filter) ||
                    //emp.Email.Contains(paging.SearchCriteria.Filter));
                    query = mapData.Where(x => x.Name.ToLower().Contains(paging.Search.Value.ToLower()) ||
                    x.CountryModel.Name.ToLower().Contains(paging.Search.Value.ToLower()));
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

                pagingResponse.States = query.Skip(paging.Start).Take(paging.Length).ToArray();
                pagingResponse.RecordsTotal = recordsTotal;
                pagingResponse.RecordsFiltered = recordsTotal;
            }

            return Ok(pagingResponse);
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



        #region Profiling

        //[HttpGet]
        //public IEnumerable<string> Get1()
        //{
        //    string url1 = string.Empty;
        //    string url2 = string.Empty;
        //    using (MiniProfiler.Current.Step("Get method"))
        //    {
        //        using (MiniProfiler.Current.Step("Prepare data"))
        //        {
        //            using (MiniProfiler.Current.CustomTiming("SQL", "SELECT * FROM Config"))
        //            {
        //                // Simulate a SQL call
        //                Thread.Sleep(500);
        //                url1 = "https://google.com";
        //                url2 = "https://stackoverflow.com/";
        //            }
        //        }
        //        using (MiniProfiler.Current.Step("Use data for http call"))
        //        {
        //            using (MiniProfiler.Current.CustomTiming("HTTP", "GET " + url1))
        //            {
        //                var client = new WebClient();
        //                var reply = client.DownloadString(url1);
        //            }

        //            using (MiniProfiler.Current.CustomTiming("HTTP", "GET " + url2))
        //            {
        //                var client = new WebClient();
        //                var reply = client.DownloadString(url2);
        //            }
        //        }
        //    }
        //    return new string[] { "value1", "value2" };
        //}

        #endregion


        #region Action filter

        [HttpGet]
        [Route("GetAllCountryForUsingActionFilter")]
        public async Task<IActionResult> GetAllCountryForUsingActionFilter()
        {
            var data = _mapper.Map<IEnumerable<CountryModel>>(await _countryRepository.GetAllCountries().ConfigureAwait(false));
            return Ok(data);
        }

        //[HttpGet]
        [HttpGet("GetCountryByIdForUsingActionFilter/{id}", Name = "GetCountryByIdForUsingActionFilter")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Country>))]
        // [Route("GetCountryByIdForUsingActionFilter")]
        public IActionResult GetCountryByIdForUsingActionFilter(int id)
        {
            var countryData = HttpContext.Items["entity"] as Country;
            var data = _mapper.Map<CountryModel>(countryData);
            return Ok(data);
        }

        #endregion


        #region District

        [HttpPost]
        // [Authorize]
        [Route("PostDistrict")]
        public async Task<IActionResult> AddDistrict([FromBody] DistrictModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dist = await _districtRepository.Add(_mapper.Map<DistrictModel, District>(model)).ConfigureAwait(false);

            return Ok(dist);

        }

        #endregion


    }
}