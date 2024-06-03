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

namespace SS.DocCap.Application.Inquiry.Queries.GetDocCount
{
    public class GetDocCountQuery : IRequest<IQueryable<object>>
    {
       
        public string DocumentType { get; set; }
        public string RSLPeriod { get; set; }
        public string RSLPeriodId { get; set; }
        public string BranchCode { get; set; }
    }
    public class GetVisitorDetailsByDateHandler : IRequestHandler<GetDocCountQuery, IQueryable<object> >
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IOptions<ConfigurationManager> _configurationService;
        private readonly IMapper _mapper;


        public GetVisitorDetailsByDateHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService, IOptions<ConfigurationManager> configurationService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _configurationService = configurationService;
        }

       
        public async Task<IQueryable<object>> Handle(GetDocCountQuery request, CancellationToken cancellationToken)
        {
            IQueryable<object> result = null;

            if (request.DocumentType == "" && request.BranchCode == "")
            {
                var result1 = _context.DocumentData.Where(x => x.RSLPeriodId == request.RSLPeriodId).Select(x =>
                      new
                      {
                          x.RSLPeriod,
                          x.BranchCode,
                          x.BranchName,
                          x.DocumentType,
                          x.Id


                      }).ToArray().GroupBy(g => new
                      {
                          g.BranchCode,
                          g.DocumentType
                      }).Select(x => new
                      {
                          RSLPERIOD = x.OrderByDescending(y => y.Id > 0).First().RSLPeriod,
                          SITECODE = x.OrderByDescending(y => y.Id > 0).First().BranchCode,
                          SITENAME = x.OrderByDescending(y => y.Id > 0).First().BranchName,
                          PAYMENTMODE = x.OrderByDescending(y => y.Id > 0).First().DocumentType,
                          NOOFDOCUMENTS = x.OrderByDescending(y => y.Id > 0).Count(),
                          NOOFCBENTRIES = 0
                      }).ToList();
                result = result1.AsQueryable();
            }
            else if (request.DocumentType != "" && request.BranchCode == "")
            {
                var result1 = _context.DocumentData.Where(x => x.RSLPeriodId == request.RSLPeriodId && x.DocumentType == request.DocumentType ).Select(x =>
                       new
                       {
                           x.RSLPeriod,
                           x.BranchCode,
                           x.BranchName,
                           x.DocumentType,
                           x.Id


                       }).ToArray().GroupBy(g => new
                       {
                           g.BranchCode,
                           g.DocumentType
                       }).Select(x => new
                       {
                           RSLPERIOD = x.OrderByDescending(y => y.Id > 0).First().RSLPeriod,
                           SITECODE = x.OrderByDescending(y => y.Id > 0).First().BranchCode,
                           SITENAME = x.OrderByDescending(y => y.Id > 0).First().BranchName,
                           PAYMENTMODE = x.OrderByDescending(y => y.Id > 0).First().DocumentType,
                           NOOFDOCUMENTS = x.OrderByDescending(y => y.Id > 0).Count(),
                           NOOFCBENTRIES = 0
                       }).ToList();
                result =  result1.AsQueryable();
            }
            else if (request.DocumentType == "" && request.BranchCode != "")
            {
                var result1 = _context.DocumentData.Where(x => x.RSLPeriodId == request.RSLPeriodId && x.BranchCode == request.BranchCode).Select(x =>
                     new
                     {
                         x.RSLPeriod,
                         x.BranchCode,
                         x.BranchName,
                         x.DocumentType,
                         x.Id


                     }).ToArray().GroupBy(g => new
                     {
                         g.BranchCode,
                         g.DocumentType
                     }).Select(x => new
                     {
                         RSLPERIOD = x.OrderByDescending(y => y.Id > 0).First().RSLPeriod,
                         SITECODE = x.OrderByDescending(y => y.Id > 0).First().BranchCode,
                         SITENAME = x.OrderByDescending(y => y.Id > 0).First().BranchName,
                         PAYMENTMODE = x.OrderByDescending(y => y.Id > 0).First().DocumentType,
                         NOOFDOCUMENTS = x.OrderByDescending(y => y.Id > 0).Count(),
                         NOOFCBENTRIES = 0
                     }).ToList();
                result = result1.AsQueryable();
            }
            else if (request.DocumentType != "" && request.BranchCode != "")
            {
                var result1 = _context.DocumentData.Where(x => x.RSLPeriodId == request.RSLPeriodId && x.BranchCode == request.BranchCode && x.DocumentType == request.DocumentType).Select(x =>
                     new
                     {
                         x.RSLPeriod,
                         x.BranchCode,
                         x.BranchName,
                         x.DocumentType,
                         x.Id


                     }).ToArray().GroupBy(g => new
                     {
                         g.BranchCode,
                         g.DocumentType
                     }).Select(x => new
                     {
                         RSLPERIOD = x.OrderByDescending(y => y.Id > 0).First().RSLPeriod,
                         SITECODE = x.OrderByDescending(y => y.Id > 0).First().BranchCode,
                         SITENAME = x.OrderByDescending(y => y.Id > 0).First().BranchName,
                         PAYMENTMODE = x.OrderByDescending(y => y.Id > 0).First().DocumentType,
                         NOOFDOCUMENTS = x.OrderByDescending(y => y.Id > 0).Count(),
                         NOOFCBENTRIES = 0
                     }).ToList();
                result = result1.AsQueryable();
            }
            else if (request.RSLPeriod == null)
            {
                var result1 = await _context.DocumentData.Where(x => x.RSLPeriodId == request.RSLPeriodId).ToListAsync();
                result = result1.AsQueryable();
            }
            return result;

        }
       
      
    }
}
