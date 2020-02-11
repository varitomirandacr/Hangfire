using HangfireAPI.Base;
using HangfireAPI.Contract;
using HangfireAPI.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireAPI.Services
{
    public class ProcessService : BaseProcess, IProcessService
    {
        private readonly ProcessFactory _factory;
        private readonly ILoggerFactory _loggerFactory;

        public ProcessService(ILoggerFactory loggerFactory)
        {
            _factory = new ProcessFactory();
            _loggerFactory = loggerFactory;
        }

        public void CustomBatch()
        {
            ValidateAndExecute(() => _factory.CustomBatches(Message));
        }

        public void Delay()
        {
            ValidateAndExecute(() => _factory.DelayedJobs(Message));
        }

        public void Fire()
        {
            ValidateAndExecute(() => _factory.FireAndForget(Message));
        }

        public void Recurrent()
        {
            ValidateAndExecute(() => _factory.RecurringJobs(Message));
        }
    }
}
