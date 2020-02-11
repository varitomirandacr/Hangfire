using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireAPI.Base
{
    public abstract class BaseProcess
    {
        public string Message { get; set; }

        public virtual void ValidateAndExecute(Action action)
        {
            if (string.IsNullOrEmpty(Message))
                throw new Exception("Invalid Message");

            action?.Invoke();
        }
    }
}
