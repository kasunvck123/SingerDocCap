using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SS.DocCap.Application.Common.Interfaces;
using SS.DocCap.Application.Common.Models;
using SS.DocCap.Application.ExAPI.Model;
using SS.DocCap.Application.ExAPI.Queries;
using SS.DocCap.Application.Inquiry.Queries.GetInquiry;
using SS.DocCap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SS.DocCap.Controllers
{
 
    public class ExAPIController : ApiController
    {
        private readonly IApplicationLogger _logger;
        private readonly string requestname = "/UploadImage";
        private readonly ICurrentUserService _currentUserService;

        public ExAPIController(IApplicationLogger logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

       // [Authorize]
        [HttpGet("GetRSLPeriods")]
        public async Task<ActionResult<List<RSLPeriodTBL>>> GetRSLPeriods()
        {
       
            return await Mediator.Send(new GetRSLPeriods());
        }
    }
}
