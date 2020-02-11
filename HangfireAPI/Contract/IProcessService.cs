using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireAPI.Contract
{
    public interface IProcessService
    {
        string Message { get; set; }

        void Fire();

        void Delay();

        void Recurrent();

        void CustomBatch();
    }
}
