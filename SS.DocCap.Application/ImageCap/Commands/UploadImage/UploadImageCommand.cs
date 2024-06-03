using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SS.DocCap.Application.Common.Interfaces;
using SS.DocCap.Application.Common.Models;
using SS.DocCap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SS.DocCap.Application.ImageCap.Commands.UploadImage
{
    public class UploadImageCommand : IRequest<int>
    {
        public string DocumentType { get; set; }
        public string RSLPeriod { get; set; }
        public string DocumentNo { get; set; }
        public decimal DocumentAmount { get; set; }
        public string Remark { get; set; }
        public IFormFile AttachedImage { get; set; }
        public string CapturedImage { get; set; }
        public bool IsAttachment { get; set; }
        public bool IsChequeIssue { get; set; }
        public string RSLPeriodId { get; set; }
    }

    public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private IHostingEnvironment _env;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly ICurrentDateTimeService _currentDateTimeService;
        private readonly IApplicationLogger _logger;
        private readonly string requestname = "/UploadImageCommandHandler";
        public readonly ICurrentUserService _currentUserService;

        public UploadImageCommandHandler(IApplicationDbContext context, IHostingEnvironment env, IHttpContextAccessor httpcontext, ICurrentDateTimeService currentDateTimeService, IApplicationLogger iApplicationLogger, ICurrentUserService currentUserService)
        {
            _context = context;
            _env = env;
            _httpcontext = httpcontext;
            _currentDateTimeService = currentDateTimeService;
            _logger = iApplicationLogger;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _logger.Log(LogLevel.Information, new LogFormat
                {
                    Request = requestname,
                    RequestType = "POST",
                    UserID = _currentUserService.UserId,
                    Email = _currentUserService.UserId,
                    Message = "Upload",
                    Description = request
                });


                var webRoot = _env.WebRootPath+"\\UploadedImages";
                var Url = _httpcontext.HttpContext.Request.Scheme + "://" + _httpcontext.HttpContext.Request.Host.Value;
                var PathWithFolderName = System.IO.Path.Combine(webRoot, DateTime.Now.ToString("dd-MM-yyyy"));
                if (!Directory.Exists(PathWithFolderName))
                {
                    DirectoryInfo di = Directory.CreateDirectory(PathWithFolderName);
                }
                var uploadpath = "";
                var temFileName = "";
                var captureType = "";
                if (request.IsAttachment)
                {
                    captureType = "Attachment";
                       var fileName = ContentDispositionHeaderValue.Parse(request.AttachedImage.ContentDisposition).FileName.Trim('"');

                    var extention = Path.GetExtension(fileName);

                     temFileName = "AI-"+request.DocumentNo.Trim()+"-"+System.Guid.NewGuid().ToString() + extention;
                    //var uploadpath = PathWithFolderName + "/" + temFileName;
                    var fullPath = Path.Combine(PathWithFolderName, temFileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        request.AttachedImage.CopyTo(stream);
                    }

                }
                else
                {
                    captureType = "Capture";
                    byte[] bytes = Convert.FromBase64String(request.CapturedImage);
                     temFileName = "CI-"+ request.DocumentNo.Trim()+"-"+System.Guid.NewGuid().ToString() + ".jpg";
                    //var uploadpath = PathWithFolderName + "/" + temFileName;
                    var fullPath = Path.Combine(PathWithFolderName, temFileName);
                    File.WriteAllBytes(fullPath, bytes);
                }

                 uploadpath = Url+ "/UploadedImages/"+ DateTime.Now.ToString("dd-MM-yyyy") + "/" + temFileName;

                var docEntity = new DocumentData {RSLPeriodId=request.RSLPeriodId, DocumentAmount = request.DocumentAmount, Remark = request.Remark, IsActive = true, RSLPeriod = request.RSLPeriod, DocumentNo = request.DocumentNo.Trim(), DocumentType = request.DocumentType, DocumentUrl = DateTime.Now.ToString("dd-MM-yyyy") + "/" + temFileName,RootUrl= Url + "/UploadedImages/",IsChequeIssue=request.IsChequeIssue,Folder= DateTime.Now.ToString("dd-MM-yyyy"),CaptureType= captureType,UserId= _currentUserService.UserId,HierachyID=_currentUserService.HierachyID,EmployeeType=_currentUserService.EmployeeType,Department=_currentUserService.Department,BranchName=_currentUserService.BranchName,BranchID=_currentUserService.BranchID,BranchCode=_currentUserService.BranchCode
                };

                _context.DocumentData.Add(docEntity);

                await _context.SaveChangesAsync(cancellationToken);


            }
            catch (Exception)
            {
                await _logger.Log(LogLevel.Error, new LogFormat
                {
                    Request = requestname,
                    RequestType = "POST",
                    UserID = _currentUserService.UserId,
                    Email = _currentUserService.UserId,
                    Message = "Upload",
                    Description = request
                });
                throw;
            }

            return 1;
        }
    }
}
