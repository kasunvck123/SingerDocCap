using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SS.DocCap.Application.Common.Exceptions;
using SS.DocCap.Application.Common.Interfaces;
using SS.DocCap.Application.Common.Models;
using SS.DocCap.Application.Common.Utility.Ldap;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SS.DocCap.Application.Auth.Queries.GetAuth
{
    public class GetAuthQuery : IRequest<AuthDetailVm>
    {
        public string UserName { get; set; }
        public string Password
        {
            get; set;
        }
        public string AuthType
        {
            get; set;
        }
    }

    public class AuthQueryHandler : IRequestHandler<GetAuthQuery, AuthDetailVm>
    {
        private readonly IApplicationDbContext _context;

        private readonly IOptions<ConfigurationManager> _configurationService;
        private readonly IMapper _mapper;
        private readonly IApplicationLogger _logger;
        private readonly string requestname = "/UserAuth";
        private readonly ICurrentUserService _currentUserService;
        private readonly ApacheLDAP _ApacheLDAP;
        private readonly Search _Search;



        public AuthQueryHandler(IApplicationLogger logger, IApplicationDbContext context, IMapper mapper, IOptions<ConfigurationManager> configurationservice, ICurrentUserService currentUserService, ApacheLDAP apacheLDAP, Search search)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _configurationService = configurationservice;
            _currentUserService = currentUserService;
            _ApacheLDAP = apacheLDAP;
            _Search = search;

        }



        public async Task<AuthDetailVm> Handle(GetAuthQuery request, CancellationToken cancellationToken)
        {
            var vm = new AuthDetailVm();

            // var isValid = _ApacheLDAP.ValidateCredentials(request.UserName, request.Password);

            var sd1 = String.Format(_configurationService.Value.BMSAuth, request.UserName, request.Password);

            if (request.AuthType == "ApacheLDAP")
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        using (var response = await httpClient.GetAsync(String.Format(_configurationService.Value.BMSAuth, request.UserName, request.Password)))
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                await _logger.Log(LogLevel.Information, new LogFormat
                                {
                                    Request = requestname,
                                    RequestType = "GET",
                                    UserID = _currentUserService.UserId,
                                    Email = _currentUserService.UserId,
                                    Message = "AuthQueryHandler-ServiceURL",
                                    Description = ""
                                });
                                string apiResponse = await response.Content.ReadAsStringAsync();
                                var BMSUserModel = JsonConvert.DeserializeObject<BMSUserModel>(apiResponse);


                                await _logger.Log(LogLevel.Information, new LogFormat
                                {
                                    Request = requestname,
                                    RequestType = "POST",
                                    UserID = _currentUserService.UserId,
                                    Email = _currentUserService.UserId,
                                    Message = "AuthQueryHandler-ServiceURL",
                                    Description = BMSUserModel
                                });

                                if (BMSUserModel.IsAuthenticated)
                                {
                                    if (!string.IsNullOrEmpty(BMSUserModel.BranchID)|| BMSUserModel.BranchID != null)
                                    {


                                        await _logger.Log(LogLevel.Information, new LogFormat
                                        {
                                            Request = requestname,
                                            RequestType = "GET",
                                            UserID = _currentUserService.UserId,
                                            Email = _currentUserService.UserId,
                                            Message = "UserAuthSuccess",
                                            Description = "Branch User"
                                        });

                                        var securutyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationService.Value.JwtKey));

                                        var credentials = new SigningCredentials(securutyKey, SecurityAlgorithms.HmacSha256);

                                        var claims = new[]
                                        {
                new Claim (JwtRegisteredClaimNames.Sub,request.UserName),
                new Claim (JwtRegisteredClaimNames.Email,BMSUserModel.Email),
                new Claim ("Name",BMSUserModel.FirstName),
                new Claim ("Id",BMSUserModel.UserId),
                new Claim ("BranchID",BMSUserModel.BranchID),
                new Claim ("BranchCode",BMSUserModel.BranchCode),
                new Claim ("BranchName",BMSUserModel.BranchName),
                new Claim ("HierachyID",BMSUserModel.HierachyID),
                new Claim ("Department",BMSUserModel.Department),
                new Claim ("EmployeeType",BMSUserModel.EmployeeType),
                new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                    };

                                        var sd = new JwtSecurityTokenHandler();


                                        var token = new JwtSecurityToken(
                                            issuer: _configurationService.Value.JwtIssuer,
                                            audience: _configurationService.Value.JwtIssuer, claims,
                                            expires: DateTime.Now.AddMonths(2),
                                            signingCredentials: credentials);

                                        var encordetoken = new JwtSecurityTokenHandler().WriteToken(token);

                                        vm = new AuthDetailVm { UserName = request.UserName, Email = BMSUserModel.Email, JwtToken = encordetoken, Name = BMSUserModel.FirstName, BranchCode = BMSUserModel.BranchCode, BranchID = BMSUserModel.BranchID, BranchName = BMSUserModel.BranchName, Department = BMSUserModel.Department, EmployeeType = BMSUserModel.EmployeeType, HierachyID = BMSUserModel.HierachyID, UserId = BMSUserModel.UserId };
                                        return vm;
                                    }
                                    else
                                    {
                                        await _logger.Log(LogLevel.Information, new LogFormat
                                        {
                                            Request = requestname,
                                            RequestType = "GET",
                                            UserID = _currentUserService.UserId,
                                            Email = _currentUserService.UserId,
                                            Message = "UserAuthSuccess",
                                            Description = "HO User"
                                        });
                                        //var user = await _identityService.FindByNameAsync(request.UserName);

                                        var securutyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationService.Value.JwtKey));

                                        var credentials = new SigningCredentials(securutyKey, SecurityAlgorithms.HmacSha256);

                                        var claims = new[]
                                        {
                new Claim (JwtRegisteredClaimNames.Sub,request.UserName),
                new Claim (JwtRegisteredClaimNames.Email,BMSUserModel.Email),
                new Claim ("Name",BMSUserModel.Name),
                new Claim ("Id",BMSUserModel.UserId),
                new Claim ("BranchID",string.Empty),
                new Claim ("BranchCode",string.Empty),
                new Claim ("BranchName",string.Empty),
                new Claim ("HierachyID",string.Empty),
                new Claim ("Department",BMSUserModel.Department),
                new Claim ("EmployeeType",BMSUserModel.EmployeeType),
                new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

                                        var sd = new JwtSecurityTokenHandler();


                                        var token = new JwtSecurityToken(
                                            issuer: _configurationService.Value.JwtIssuer,
                                            audience: _configurationService.Value.JwtIssuer, claims,
                                            expires: DateTime.Now.AddMonths(2),
                                            signingCredentials: credentials);

                                        var encordetoken = new JwtSecurityTokenHandler().WriteToken(token);

                                        vm = new AuthDetailVm { UserName = request.UserName, Email = BMSUserModel.Email, JwtToken = encordetoken, Name = BMSUserModel.Name, BranchCode = string.Empty, BranchName = string.Empty };
                                        return vm;
                                    }
                                }
                                else
                                {
                                    //Auth Failed
                                    var result = await _context.AppUser.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(request.UserName) && x.Password.Equals(request.Password));

                                    if (result != null)
                                    {
                                        await _logger.Log(LogLevel.Information, new LogFormat
                                        {
                                            Request = requestname,
                                            RequestType = "GET",
                                            UserID = _currentUserService.UserId,
                                            Email = _currentUserService.UserId,
                                            Message = "UserAuthSuccess",
                                            Description = result
                                        });
                                        //var user = await _identityService.FindByNameAsync(request.UserName);

                                        var securutyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationService.Value.JwtKey));

                                        var credentials = new SigningCredentials(securutyKey, SecurityAlgorithms.HmacSha256);

                                        var claims = new[]
                                        {
                new Claim (JwtRegisteredClaimNames.Sub,request.UserName),
                new Claim (JwtRegisteredClaimNames.Email,result.Email),
                new Claim ("Name",result.Name),
                new Claim ("Id",result.Email),
                new Claim ("BranchID",string.Empty),
                new Claim ("BranchCode",string.Empty),
                new Claim ("BranchName",string.Empty),
                new Claim ("HierachyID",string.Empty),
                new Claim ("Department",string.Empty),
                new Claim ("EmployeeType",string.Empty),
                new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

                                        var sd = new JwtSecurityTokenHandler();


                                        var token = new JwtSecurityToken(
                                            issuer: _configurationService.Value.JwtIssuer,
                                            audience: _configurationService.Value.JwtIssuer, claims,
                                            expires: DateTime.Now.AddMonths(2),
                                            signingCredentials: credentials);

                                        var encordetoken = new JwtSecurityTokenHandler().WriteToken(token);

                                        vm = new AuthDetailVm { UserName = request.UserName, Email = result.Email, JwtToken = encordetoken, Name = result.Name, BranchCode = string.Empty, BranchName = string.Empty };
                                        return vm;
                                    }
                                    else
                                    {
                                        await _logger.Log(LogLevel.Information, new LogFormat
                                        {
                                            Request = requestname,
                                            RequestType = "GET",
                                            UserID = _currentUserService.UserId,
                                            Email = _currentUserService.UserId,
                                            Message = "Invalid User name or Password",
                                            Description = "GET:ApacheLDAP" + request
                                        });
                                        throw new NotFoundException("Invalid User name or Password !");
                                    }
                                }



                            }
                            else
                            {
                                //API response error
                                var result = await _context.AppUser.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(request.UserName) && x.Password.Equals(request.Password));

                                if (result != null)
                                {
                                    await _logger.Log(LogLevel.Information, new LogFormat
                                    {
                                        Request = requestname,
                                        RequestType = "GET",
                                        UserID = _currentUserService.UserId,
                                        Email = _currentUserService.UserId,
                                        Message = "UserAuthSuccess",
                                        Description = result
                                    });
                                    //var user = await _identityService.FindByNameAsync(request.UserName);

                                    var securutyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationService.Value.JwtKey));

                                    var credentials = new SigningCredentials(securutyKey, SecurityAlgorithms.HmacSha256);

                                    var claims = new[]
                                    {
                new Claim (JwtRegisteredClaimNames.Sub,request.UserName),
                new Claim (JwtRegisteredClaimNames.Email,result.Email),
                new Claim ("Name",result.Name),
                new Claim ("Id",result.Email),
                new Claim ("BranchID",string.Empty),
                new Claim ("BranchCode",string.Empty),
                new Claim ("BranchName",string.Empty),
                new Claim ("HierachyID",string.Empty),
                new Claim ("Department",string.Empty),
                new Claim ("EmployeeType",string.Empty),
                new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

                                    var sd = new JwtSecurityTokenHandler();


                                    var token = new JwtSecurityToken(
                                        issuer: _configurationService.Value.JwtIssuer,
                                        audience: _configurationService.Value.JwtIssuer, claims,
                                        expires: DateTime.Now.AddMonths(2),
                                        signingCredentials: credentials);

                                    var encordetoken = new JwtSecurityTokenHandler().WriteToken(token);

                                    vm = new AuthDetailVm { UserName = request.UserName, Email = result.Email, JwtToken = encordetoken, Name = result.Name, BranchCode = string.Empty, BranchName = string.Empty };
                                    return vm;
                                }
                                else
                                {
                                    await _logger.Log(LogLevel.Information, new LogFormat
                                    {
                                        Request = requestname,
                                        RequestType = "GET",
                                        UserID = _currentUserService.UserId,
                                        Email = _currentUserService.UserId,
                                        Message = "Invalid User name or Password",
                                        Description = "GET:ApacheLDAP" + request
                                    });
                                    throw new NotFoundException("Invalid User name or Password !");
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {

                        var result = await _context.AppUser.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(request.UserName) && x.Password.Equals(request.Password));

                        if (result != null)
                        {



                            await _logger.Log(LogLevel.Information, new LogFormat
                            {
                                Request = requestname,
                                RequestType = "GET",
                                UserID = _currentUserService.UserId,
                                Email = _currentUserService.UserId,
                                Message = "UserAuthSuccess",
                                Description = result
                            });


                            //var user = await _identityService.FindByNameAsync(request.UserName);

                            var securutyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationService.Value.JwtKey));

                            var credentials = new SigningCredentials(securutyKey, SecurityAlgorithms.HmacSha256);

                            var claims = new[]
                            {
                new Claim (JwtRegisteredClaimNames.Sub,request.UserName),
                new Claim (JwtRegisteredClaimNames.Email,result.Email),
                new Claim ("Name",result.Name),
                new Claim ("Id",result.Email),
                new Claim ("BranchID",string.Empty),
                new Claim ("BranchCode",string.Empty),
                new Claim ("BranchName",string.Empty),
                new Claim ("HierachyID",string.Empty),
                new Claim ("Department",string.Empty),
                new Claim ("EmployeeType",string.Empty),
                new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

                            var sd = new JwtSecurityTokenHandler();


                            var token = new JwtSecurityToken(
                                issuer: _configurationService.Value.JwtIssuer,
                                audience: _configurationService.Value.JwtIssuer, claims,
                                expires: DateTime.Now.AddMonths(2),
                                signingCredentials: credentials);

                            var encordetoken = new JwtSecurityTokenHandler().WriteToken(token);

                            vm = new AuthDetailVm { UserName = request.UserName, Email = result.Email, JwtToken = encordetoken, Name = result.Name, BranchCode = string.Empty, BranchName = string.Empty };
                            return vm;
                        }
                        else
                        {
                            await _logger.Log(LogLevel.Information, new LogFormat
                            {
                                Request = requestname,
                                RequestType = "GET",
                                UserID = _currentUserService.UserId,
                                Email = _currentUserService.UserId,
                                Message = "Invalid User name or Password",
                                Description = "Exception:" + e
                            });
                            throw new NotFoundException("Invalid User name or Password !");
                        }
                    }
                }



                ////var isValid = _ApacheLDAP.ValidateCredentials(request.UserName, request.Password);
                //if (isValid)
                //{
                //    var userDetails = _Search.getUser("uid", request.UserName);
                //    var Detaillist = JsonConvert.DeserializeObject<List<DeserilaizeHelp>>(userDetails);
                //    var EmployeeType = Detaillist.Where(x => x.type.ToLower() == "employeetype").FirstOrDefault().value.ToString();
                //    var FirstName = Detaillist.Where(x => x.type.ToLower() == "cn").FirstOrDefault().value.ToString();
                //    var LastName = Detaillist.Where(x => x.type.ToLower() == "sn").FirstOrDefault().value.ToString();
                //    var Email = Detaillist.Where(x => x.type.ToLower() == "mail").FirstOrDefault().value.ToString();
                //    var Department = Detaillist.Where(x => x.type.ToLower() == "departmentnumber").FirstOrDefault().value.ToString();
                //    var MobileNo = Detaillist.Where(x => x.type.ToLower() == "mobile").FirstOrDefault().value.ToString();
                //    var UserId = Detaillist.Where(x => x.type.ToLower() == "uid").FirstOrDefault().value.ToString();

                //    await _logger.Log(LogLevel.Information, new LogFormat
                //    {
                //        Request = requestname,
                //        RequestType = "GET",
                //        UserID = _currentUserService.UserId,
                //        Email = _currentUserService.UserId,
                //        Message = "UserAuthSuccess",
                //        Description = userDetails
                //    });

                //    var securutyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationService.Value.JwtKey));

                //    var credentials = new SigningCredentials(securutyKey, SecurityAlgorithms.HmacSha256);

                //    var claims = new[]
                //    {
                //new Claim (JwtRegisteredClaimNames.Sub,request.UserName),
                //new Claim (JwtRegisteredClaimNames.Email,Email),
                //new Claim ("Name",FirstName+" "+LastName),
                //new Claim ("Id",UserId),
                //new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                //    };

                //    var sd = new JwtSecurityTokenHandler();


                //    var token = new JwtSecurityToken(
                //        issuer: _configurationService.Value.JwtIssuer,
                //        audience: _configurationService.Value.JwtIssuer, claims,
                //        expires: DateTime.Now.AddMonths(2),
                //        signingCredentials: credentials);

                //    var encordetoken = new JwtSecurityTokenHandler().WriteToken(token);

                //    vm = new AuthDetailVm { UserName = request.UserName, Email = Email, JwtToken = encordetoken, Name = FirstName + " " + LastName };
                //    return vm;




                //}
                //else
                //{



                //}



            }
            else if (request.AuthType == "Service")
            {
                var result = await _context.AppUser.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(request.UserName) && x.Password.Equals(request.Password));

                if (result != null)
                {



                    await _logger.Log(LogLevel.Information, new LogFormat
                    {
                        Request = requestname,
                        RequestType = "GET",
                        UserID = _currentUserService.UserId,
                        Email = _currentUserService.UserId,
                        Message = "UserAuthSuccess",
                        Description = result
                    });


                    //var user = await _identityService.FindByNameAsync(request.UserName);

                    var securutyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationService.Value.JwtKey));

                    var credentials = new SigningCredentials(securutyKey, SecurityAlgorithms.HmacSha256);

                    var claims = new[]
                    {
                new Claim (JwtRegisteredClaimNames.Sub,request.UserName),
                new Claim (JwtRegisteredClaimNames.Email,result.Email),
                new Claim ("Name",result.Name),
                new Claim ("Id",result.Email),
                new Claim ("BranchID",string.Empty),
                new Claim ("BranchCode",string.Empty),
                new Claim ("BranchName",string.Empty),
                new Claim ("HierachyID",string.Empty),
                new Claim ("Department",string.Empty),
                new Claim ("EmployeeType",string.Empty),
                new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

                    var sd = new JwtSecurityTokenHandler();


                    var token = new JwtSecurityToken(
                        issuer: _configurationService.Value.JwtIssuer,
                        audience: _configurationService.Value.JwtIssuer, claims,
                        expires: DateTime.Now.AddMonths(2),
                        signingCredentials: credentials);

                    var encordetoken = new JwtSecurityTokenHandler().WriteToken(token);

                    vm = new AuthDetailVm { UserName = request.UserName, Email = result.Email, JwtToken = encordetoken, Name = result.Name, BranchCode = string.Empty, BranchName = string.Empty };
                    return vm;
                }
                else
                {
                    await _logger.Log(LogLevel.Information, new LogFormat
                    {
                        Request = requestname,
                        RequestType = "GET",
                        UserID = _currentUserService.UserId,
                        Email = _currentUserService.UserId,
                        Message = "Invalid User name or Password",
                        Description = "GET-Service User:" + request
                    });
                    throw new NotFoundException("Invalid User name or Password !");
                }
            }
            else
            {
                await _logger.Log(LogLevel.Information, new LogFormat
                {
                    Request = requestname,
                    RequestType = "GET",
                    UserID = _currentUserService.UserId,
                    Email = _currentUserService.UserId,
                    Message = "Invalid User name or Password",
                    Description = "GET Auth Type error:" + request
                });
                throw new NotFoundException("Invalid User name or Password !");
            }
        }




        private RefershToken GenerateRefreshToken()
        {
            RefershToken refershToken = new RefershToken();

            var randonNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randonNumber);
                refershToken.Token = Convert.ToBase64String(randonNumber);

            }

            refershToken.ExpiryDate = DateTime.UtcNow.AddMonths(6);

            return refershToken;
        }
        public struct RefershToken
        {

            public string Token { get; set; }
            public DateTime ExpiryDate { get; set; }
        }
    }
    public class DeserilaizeHelp
    {
        public string type { get; set; }
        public string value { get; set; }
    }
}
