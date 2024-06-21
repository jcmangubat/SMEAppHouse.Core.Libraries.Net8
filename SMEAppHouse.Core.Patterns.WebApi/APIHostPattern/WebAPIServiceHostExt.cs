using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SMEAppHouse.Core.Patterns.WebApi.APIHostPattern
{
    public abstract class WebApiServiceHostExt : ControllerBase
    {
        protected abstract Task<IActionResult> ExecuteAsync(Func<Task<IActionResult>> executeActionAsync);
    }
}