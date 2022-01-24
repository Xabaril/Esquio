using Esquio.UI.Api.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Infrastructure.Behaviors
{
    public class LoggerMediatRBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest: IRequest<TResponse>
    {
        private readonly ILoggerFactory _loggerFactory;

        public LoggerMediatRBehavior(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
           var logger =  _loggerFactory.CreateLogger<TRequest>();

            Log.ExecutingCommand(logger, typeof(TRequest).Name);

            var response = await next();

            Log.ExecutedCommand(logger, typeof(TRequest).Name);

            return response;
        }
    }
}
