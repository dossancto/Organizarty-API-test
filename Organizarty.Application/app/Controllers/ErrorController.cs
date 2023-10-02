using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Organizarty.Domain.Exceptions;

namespace Organizarty.Application.app.Controllers;

[ApiController]
public class ErrorController : ControllerBase
{
    [HttpGet("/Error")]
    public ActionResult OnGetError()
    {
        return ShowError();
    }

    [HttpPost("/Error")]
    public ActionResult OnPostError()
    {
        return ShowError();
    }

    private ActionResult ShowError()
    {
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        var error = exceptionHandlerPathFeature?.Error;

        if (error is NotFoundException)
        {
            var ex = (NotFoundException)error;
            return StatusCode(404, ex.Message);
        }

        if (error is ValidationFailException)
        {
            var ex = (ExpiredDataException)error;
            return StatusCode(498, ex.Message);
        }

        if (error is ValidationFailException)
        {
            var ex = (ValidationFailException)error;
            return StatusCode(400, ex.Message);
        }

        if (error is DbUpdateException)
        {
            var ex = (DbUpdateException)error;
            return StatusCode(400, ex.Message);
        }

        if (error is Exception)
        {
            return StatusCode(500, "Something went wrong");
        }

        return Ok("Everything is okay");
    }

}
