using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SS.DocCap.Application.Auth.Queries.GetAuth;
using SS.DocCap.Application.Common.Interfaces;
using SS.DocCap.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SS.DocCap.Controllers
{
    public class UserAuthController : ApiController
    {
        private readonly IApplicationLogger _logger;
        private readonly string requestname = "/UserAuth";
        private readonly ICurrentUserService _currentUserService;
        private readonly IOptions<ConfigurationManager> _configurationService;

        public UserAuthController(IApplicationLogger logger, ICurrentUserService currentUserService, IOptions<ConfigurationManager> configurationService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
            _configurationService = configurationService;
        }


        [HttpPost]
        public async Task<AuthDetailVm> Post(GetAuthQuery getAuthQuery)
        {

           

            await _logger.Log(LogLevel.Information, new LogFormat
            {
                Request = requestname,
                RequestType = "POST",
                UserID = _currentUserService.UserId,
                Email = _currentUserService.UserId,
                Message = "UserAuth:POST",
                Description = "GetAuthQuery:" + Request
            });
            return await Mediator.Send(new GetAuthQuery { UserName = getAuthQuery.UserName, Password = getAuthQuery.Password,AuthType=getAuthQuery.AuthType});
        }

        //[HttpPost("multiauth")]
        //public async Task<AuthDetailVm> multiauth(MultiAuthQuery getAuthQuery)
        //{
        //    await _logger.Log(LogLevel.Information, new LogFormat
        //    {
        //        Request = requestname,
        //        RequestType = "GET",
        //        UserID = getAuthQuery.UserName,
        //        Email = getAuthQuery.UserName,
        //        Message = "multiauth:" + getAuthQuery.Provider,
        //        Description = "User Invoked"
        //    });
        //    return await Mediator.Send(new MultiAuthQuery { UserName = getAuthQuery.UserName, Key = getAuthQuery.Key, Name = getAuthQuery.Name, Provider = getAuthQuery.Provider });
        //}


        //[HttpGet("GetPermissionLevel")]
        //public async Task<ActionResult<List<Domain.Entities.UserGroups>>> GetPermission(string email)
        //{
        //    await _logger.Log(LogLevel.Information, new LogFormat
        //    {
        //        Request = requestname,
        //        RequestType = "GET",
        //        UserID = _currentUserService.UserId,
        //        Email = _currentUserService.UserId,
        //        Message = "UserAuth:GetPermissionLevel",
        //        Description = "GetPermissionLevel"

        //    });
        //    return await Mediator.Send(new GetUserPermissionQuery { email = email });
        //}
    }
}
