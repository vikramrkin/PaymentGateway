using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PaymentGateway.Filters
{
    public class ResultFormatterFilter : IAsyncResultFilter
    {
        public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            throw new System.NotImplementedException();
        }
    }
}
