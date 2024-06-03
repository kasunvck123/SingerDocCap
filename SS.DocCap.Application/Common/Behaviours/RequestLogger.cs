using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using SS.DocCap.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SS.DocCap.Application.Common.Behaviours
{
    public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        private readonly ICurrentUserService _currentUserService;



        public RequestLogger(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;

        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var re = request;
            var userId = _currentUserService.CurrentUserId ?? string.Empty;

            _logger.LogInformation("SS DocCap Request: {Name} {@UserId} {@UserName} {@Request}",
                requestName, userId, userId, request.ToString());


        }
    }
}
