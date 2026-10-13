using Lab05_JeremyVizcarra.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Lab05_JeremyVizcarra.Infrastructure.Api.Filters;

public class ManejadorDeExcepciones : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        IActionResult? resultado = context.Exception switch
        {
            RecursoNoEncontradoException => new NotFoundObjectResult(new { message = context.Exception.Message }),
            ReglaNegocioException => new BadRequestObjectResult(new { message = context.Exception.Message }),
            CredencialesInvalidasException => new UnauthorizedObjectResult(new { message = context.Exception.Message }),
            _ => null
        };

        if (resultado == null)
        {
            return;
        }

        context.Result = resultado;
        context.ExceptionHandled = true;
    }
}
