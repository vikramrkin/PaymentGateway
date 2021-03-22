using System.Threading.Tasks;
using Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PaymentGateway.Filters
{
    public class ResultFormatterFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult result)
            {
                if (result.Value is ResponseBase responseBase)
                {
                    if (responseBase.IsError)
                    {
                        context.Result = new BadRequestObjectResult(responseBase);
                    }
                    else
                    {
                        context.Result = new OkObjectResult(responseBase);
                    }
                }
            }

            await next();
        }
    }
}
