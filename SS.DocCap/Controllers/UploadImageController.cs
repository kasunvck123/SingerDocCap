using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SS.DocCap.Application.Common.Interfaces;
using SS.DocCap.Application.Common.Models;
using SS.DocCap.Application.ImageCap.Commands.UploadImage;
using SS.DocCap.Application.Inquiry.Queries.GetDocCount;
using SS.DocCap.Application.Inquiry.Queries.GetInquiry;
using SS.DocCap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SS.DocCap.Controllers
{
    public class UploadImageController : ApiController
    {
        private readonly IApplicationLogger _logger;
        private readonly string requestname = "/UploadImage";
        private readonly ICurrentUserService _currentUserService;

        public UploadImageController(IApplicationLogger logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

        [Authorize]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult<int>> Create()
        {

            await _logger.Log(LogLevel.Information, new LogFormat
            {
                Request = requestname,
                RequestType = "POST",
                UserID = _currentUserService.UserId,
                Email = _currentUserService.UserId,
                Message = "POST",
                Description = "Image Upload" 
            });
            IFormFile file1 = null;
            if (Request.Form.Files.Count > 0)
            {
                file1 = Request.Form.Files[0];
            }

            var capturedImage = Request.Form["CapturedImage"];
            var DocumentType = Request.Form["DocumentType"];
            var RSLPeriod = Request.Form["RSLPeriod"];
            var DocumentNo = Request.Form["DocumentNo"];
            var DocumentAmount = Request.Form["DocumentAmount"];
            var Remark = Request.Form["Remark"];
            var isAttachedment = Request.Form["isAttachedment"];
            var IsChequeIssue = Request.Form["chequeIssue"];
            var RSLPeriodId = Request.Form["RSLPeriodId"];

            return await Mediator.Send(new UploadImageCommand { AttachedImage= file1,DocumentAmount=Convert.ToDecimal(DocumentAmount),DocumentNo=DocumentNo,DocumentType=DocumentType,CapturedImage=capturedImage,IsAttachment =Convert.ToBoolean(isAttachedment),Remark=Remark,RSLPeriod=RSLPeriod,IsChequeIssue=Convert.ToBoolean(IsChequeIssue) ,RSLPeriodId=RSLPeriodId});
        }


        [Authorize]
        [HttpPost("Inquiry")]
        public async Task<ActionResult<List<DocumentData>>> Create(GetInquiryQuery command)
        {
            return await Mediator.Send(command);
        }

        [Authorize]
        [HttpPost("GetDocCount")]
        public async Task<IQueryable<object>> Handle(GetDocCountQuery command)
        {

            return await Mediator.Send(command);
        }



    }
}
