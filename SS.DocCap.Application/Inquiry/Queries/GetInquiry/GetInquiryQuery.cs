using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SS.DocCap.Application.Common.Interfaces;
using SS.DocCap.Application.Common.Models;
using SS.DocCap.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SS.DocCap.Application.Inquiry.Queries.GetInquiry
{
    public class  GetInquiryQuery : IRequest<List<DocumentData>>
    {
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string DocumentType { get; set; }
        public string RSLPeriod { get; set; }
        public string RSLPeriodId { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentAmount { get; set; }
        public bool IsChequeIssue { get; set; }
    }
    public class GetVisitorDetailsByDateHandler : IRequestHandler<GetInquiryQuery, List<DocumentData>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IOptions<ConfigurationManager> _configurationService;
        private readonly IMapper _mapper;


        public GetVisitorDetailsByDateHandler(IApplicationDbContext context, IMapper mapper,  ICurrentUserService currentUserService, IOptions<ConfigurationManager> configurationService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _configurationService = configurationService;
        }



        public async Task<List<DocumentData>> Handle(GetInquiryQuery request, CancellationToken cancellationToken)
        {
            var result = new List<DocumentData>();

            if (_currentUserService.BranchID == string.Empty)
            {
                result = await _context.DocumentData.Where(x => (request.fromDate != "" ? x.Created >= Convert.ToDateTime(request.fromDate) : true)
                && (request.toDate != "" ? x.Created <= Convert.ToDateTime(request.toDate) : true

                ) && (request.DocumentType != "" ? x.DocumentType.ToLower().Equals(request.DocumentType.ToLower()) : true
                ) && (request.RSLPeriod != "" ? x.RSLPeriod.ToLower().Contains(request.RSLPeriod.ToLower()) : true
                && (request.RSLPeriodId != "" ? x.RSLPeriodId.ToLower().Equals(request.RSLPeriodId.ToLower()) : true
                )
                ) && (request.DocumentAmount != "" ? x.DocumentAmount == Convert.ToDecimal(request.DocumentAmount) : true
                )
                && (request.DocumentNo != "" ? x.DocumentNo.ToLower().Contains(request.DocumentNo.ToLower()) : true
                )
                && (request.IsChequeIssue != false ? x.IsChequeIssue == true : true)).OrderByDescending(x => x.Id).ToListAsync();
            }
            else
            {
                result = await _context.DocumentData.Where(x => (request.fromDate != "" ? x.Created >= Convert.ToDateTime(request.fromDate) : true)
                && (request.toDate != "" ? x.Created <= Convert.ToDateTime(request.toDate) : true

                ) && (request.DocumentType != "" ? x.DocumentType.ToLower().Equals(request.DocumentType.ToLower()) : true
                ) && (request.RSLPeriod != "" ? x.RSLPeriod.ToLower().Contains(request.RSLPeriod.ToLower()) : true
                && (request.RSLPeriodId != "" ? x.RSLPeriodId.ToLower().Equals(request.RSLPeriodId.ToLower()) : true
                )
                ) && (request.DocumentAmount != "" ? x.DocumentAmount == Convert.ToDecimal(request.DocumentAmount) : true
                )
                && (request.DocumentNo != "" ? x.DocumentNo.ToLower().Contains(request.DocumentNo.ToLower()) : true
                )
                && (request.IsChequeIssue != false ? x.IsChequeIssue == true : true) && x.BranchID==_currentUserService.BranchID).OrderByDescending(x => x.Id).ToListAsync();
            }
            //x.CreatedBy == _currentUserService.CurrentUserId -- 21-08-2021 UA - Change to Branch Wise view
            return result;

        }
    }
}
