using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HangfireAPI.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace HangfireAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
        private readonly IProcessService _service;

        public ProcessController(IProcessService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Post([FromBody]string type)
        {
            switch (type.ToLower())
            {
                default:
                case "fire":
                    _service.Message = "Firing";
                    _service.Fire();
                    break;
                case "delay":
                    _service.Message = "Delay";
                    _service.Delay();
                    break;
                case "recurrent":
                    _service.Message = "Recurrent";
                    _service.Recurrent();
                    break;
                case "custom":
                    _service.Message = "Custom";
                    _service.CustomBatch();
                    break;
            }            
        }
    }
}
