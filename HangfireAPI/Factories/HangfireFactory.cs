using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireAPI.Factories
{
    public abstract class HangfireFactory
    {
        public abstract void FireAndForget(string message);

        public abstract void RecurringJobs(string message);

        public abstract void Batches(string batchId, string message);

        public abstract void DelayedJobs(string message);

        public abstract void Continuations(string jobId, string message);

        public abstract void BatchContinuations(string batchId, string message);

        public abstract void CustomBatches(string message);
    }
}
