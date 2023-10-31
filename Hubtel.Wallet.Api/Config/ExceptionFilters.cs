using HandySquad.Exceptions;
using Hubtel.Wallet.Api.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hubtel.Wallet.Api.Config;

public class NotFound404NotFoundException: IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is not NotFound404Exception) return;
        context.Result = new NotFoundObjectResult(new MessageResponseDto
            {Status = "failure", Message = context.Exception.Message});
        context.ExceptionHandled = true;
    }
}

public class Duplicate404ConflictException : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is not Duplicate409Exception) return;
        context.Result = new ConflictObjectResult(new MessageResponseDto
            {Status = "failure", Message = context.Exception.Message});
        context.ExceptionHandled = true;
    }
}

public class BadRequest400BadRequestException: IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is not BadRequest400Exception) return;
        context.Result = new BadRequestObjectResult(new MessageResponseDto
            {Status = "failure", Message = context.Exception.Message}
        );
    }
}