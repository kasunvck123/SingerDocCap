using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SS.DocCap.Application.Auth.Queries.GetAuth;
using SS.DocCap.Application.Common.Interfaces;
using SS.DocCap.Application.Common.Models;
using SS.DocCap.Application.ExAPI.Model;
using SS.DocCap.Application.Inquiry.Queries.GetInquiry;
using SS.DocCap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SS.DocCap.Application.ExAPI.Queries
{

    public class GetRSLPeriods : IRequest<List<RSLPeriodTBL>>
    {

    }

    public class GetRSLPeriodsHandler : IRequestHandler<GetRSLPeriods, List<RSLPeriodTBL>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IOptions<ConfigurationManager> _configurationService;
        private readonly IMapper _mapper;
        private readonly IApplicationLogger _logger;
        private readonly string requestname = "/GetRSLPeriodsHandler";


        public GetRSLPeriodsHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService, IOptions<ConfigurationManager> configurationService, IApplicationLogger iApplicationLogger)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _configurationService = configurationService;
            _logger = iApplicationLogger;

        }



        public async Task<List<RSLPeriodTBL>> Handle(GetRSLPeriods request, CancellationToken cancellationToken)
        {
            try
            {
                await _logger.Log(LogLevel.Information, new LogFormat
                {
                    Request = requestname,
                    RequestType = "POST",
                    UserID = _currentUserService.UserId,
                    Email = _currentUserService.UserId,
                    Message = "GetRSLPeriod",
                    Description = ""
                });

        
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(_configurationService.Value.RSLPeriodAPI))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {

                            await _logger.Log(LogLevel.Information, new LogFormat
                            {
                                Request = requestname,
                                RequestType = "GET",
                                UserID = _currentUserService.UserId,
                                Email = _currentUserService.UserId,
                                Message = "GetRSLPeriod-ServiceURL",
                                Description = ""
                            });
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var settings = new JsonSerializerSettings
                            {
                                DateFormatString = "yyyy-MM-ddTH:mm:ss.fffZ",
                                //DateFormatString = "MM-dd-yyyy",
                               DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
                                DateParseHandling = DateParseHandling.None,
                                Culture = System.Globalization.CultureInfo.CurrentCulture
                                
                            };

                            var format = "dd/MM/yyyy"; // your datetime format
                            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format,Culture = CultureInfo.InvariantCulture };

                            var settings2 = new JsonSerializerSettings
                            {
                                DateParseHandling = DateParseHandling.DateTimeOffset
                            };

                            var repositories = JsonConvert.DeserializeObject<List<RSLPeriod>>(apiResponse);

                            repositories = repositories.OrderByDescending(x => x.Todate).ToList();

                            var temRSL = new List<RSLPeriod>();
                            foreach(var item in repositories)
                            {
                                temRSL.Add(new RSLPeriod { FromDate = item.FromDate.AddHours(5).AddMinutes(30), Todate = item.Todate.AddHours(5).AddMinutes(30), RSLID = item.RSLID });
                            }
                            var RslPeriodVMList = new List<RSLPeriodTBL>();
                            var dblist = _context.RSLPeriodTBL.Where(x => x.IsActive == true).ToList();

                            await _logger.Log(LogLevel.Information, new LogFormat
                            {
                                Request = requestname,
                                RequestType = "POST",
                                UserID = _currentUserService.UserId,
                                Email = _currentUserService.UserId,
                                Message = "GetRSLPeriod-ServiceURL",
                                Description = temRSL
                            });


                            foreach (var item in temRSL)
                            {

                                if(DateTime.Now.Date >= Convert.ToDateTime(item.FromDate) && DateTime.Now.Date <= Convert.ToDateTime(item.Todate))
                                {
                                    RslPeriodVMList.Add(new RSLPeriodTBL { RSLID = item.RSLID, RSLPeriodTitle = item.FromDate.ToString("dd-MM-yyyy") + " - " + item.Todate.ToString("dd-MM-yyyy"),IsCurrent=true });
                                }
                                else
                                {
                                    RslPeriodVMList.Add(new RSLPeriodTBL { RSLID = item.RSLID, RSLPeriodTitle = item.FromDate.ToString("dd-MM-yyyy") + " - " + item.Todate.ToString("dd-MM-yyyy"), IsCurrent = false });
                                }



                                if (dblist.FirstOrDefault(x => x.RSLID == item.RSLID) == null)
                                {
                                    _context.RSLPeriodTBL.Add(new RSLPeriodTBL { RSLID = item.RSLID, RSLPeriodTitle = item.FromDate.ToString("dd-MM-yyyy") + " - " + item.Todate.ToString("dd-MM-yyyy"), IsActive = true });
                                }
                            }
                            await _context.SaveChangesAsync(cancellationToken);
                            return RslPeriodVMList;
                        }
                        else
                        {
                            var dblist = _context.RSLPeriodTBL.Where(x => x.IsActive == true).ToList();
                            await _logger.Log(LogLevel.Information, new LogFormat
                            {
                                Request = requestname,
                                RequestType = "GET",
                                UserID = _currentUserService.UserId,
                                Email = _currentUserService.UserId,
                                Message = "GetRSLPeriod-DB",
                                Description = dblist
                            });
                            return dblist;
                        }
                    }
                }
            }
            catch (HttpRequestException e)
            {



                await _logger.Log(LogLevel.Information, new LogFormat
                {
                    Request = requestname,
                    RequestType = "GET",
                    UserID = _currentUserService.UserId,
                    Email = _currentUserService.UserId,
                    Message = "GetRSLPeriod",
                    Description = e.Message
                });
                return _context.RSLPeriodTBL.Where(x => x.IsActive == true).ToList();
            }


            //List<RSLPeriod> rSLPeriods = new List<RSLPeriod>();
            //rSLPeriods.Add(new RSLPeriod { name="qwqweqwe" });
            //return rSLPeriods;
        }
    }


    public class userAuth
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string authType { get; set; }
    }

    public class ser
    {
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string documentType { get; set; }
        public string rslPeriod { get; set; }
        public string rslPeriodId { get; set; }
        public string documentNo { get; set; }
        public string documentAmount { get; set; }
        public bool isChequeIssue { get; set; }
    }



}
