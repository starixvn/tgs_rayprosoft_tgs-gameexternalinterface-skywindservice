using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tgs.GameExternalInterface.SkywindService.Common.Protos;
using Tgs.GameExternalInterface.SkywindService.Core.Exceptions;
using Tgs.GameExternalInterface.SkywindService.ViewModels;

namespace Tgs.GameExternalInterface.SkywindService.Attributes;

public class ExceptionWrapperAttribute : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var errorCode = context.Exception switch
        {
            PlayerNotFoundException => (int)GameErrorCode.PlayerNotFound,
            _ => (int)GameErrorCode.MerchantInternalError
        };

        var result = new ErrorCodeResponseViewModel
        {
            ErrorCode = errorCode
        };
        context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
        context.Result = new ObjectResult(result);
    }
}
