using Hangfire;
using HangfireAPI.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireAPI.Model
{
    public class ProcessFactory : HangfireFactory
    {
        public override void BatchContinuations(string batchId, string message)
        {
            // PRO version
            //BatchJob.ContinueWith(batchId, x =>
            //{
            //    x.Enqueue(() => Console.WriteLine("Last Job"));
            //});
        }

        public override void Batches(string batchId, string message)
        {
            // PRO
            //var batchId = BatchJob.StartNew(x =>
            //{
            //    x.Enqueue(() => Console.WriteLine("Job 1"));
            //    x.Enqueue(() => Console.WriteLine("Job 2"));
            //});
        }

        public override void Continuations(string jobId, string message)
        {
            BackgroundJob.ContinueJobWith(
                jobId,
                () => Console.WriteLine(message));
        }

        public override void DelayedJobs(string message)
        {
            var jobId = BackgroundJob.Schedule(
                () => Console.WriteLine(message),
                TimeSpan.FromSeconds(10));

            Console.WriteLine(jobId);
        }

        public override void FireAndForget(string message)
        {
            BackgroundJob.Enqueue(() => Console.WriteLine(message));
        }

        public override void CustomBatches(string message)
        {
            var jobId = BackgroundJob.Enqueue(
                () => Console.WriteLine(message));

            Console.WriteLine(jobId);

            Continuations(jobId, $"{message} - No. 2");
        }

        public override void RecurringJobs(string message)
        {
            RecurringJob.AddOrUpdate(
                () => Console.WriteLine(message),
                Cron.Minutely);
        }
    }
}
