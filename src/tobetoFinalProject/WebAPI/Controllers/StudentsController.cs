using Application.Features.Students.Commands.Create;
using Application.Features.Students.Commands.Delete;
using Application.Features.Students.Commands.Update;
using Application.Features.Students.Commands.UpdateForPassword;
using Application.Features.Students.Queries.GetById;
using Application.Features.Students.Queries.GetList;
using Application.Features.Users.Commands.UpdateFromAuth;
using Core.Application.Requests;
using Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateStudentCommand createStudentCommand)
    {
        CreatedStudentResponse response = await Mediator.Send(createStudentCommand);

        return Created(uri: "", response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromForm] UpdateStudentCommand updateStudentCommand)
    {
        updateStudentCommand.UserId = getUserIdFromRequest();
        UpdatedStudentResponse response = await Mediator.Send(updateStudentCommand);

        return Ok(response);
    }
    [HttpPut("forPassword")]
    public async Task<IActionResult> UpdateForPassword([FromBody] UpdateStudentForPasswordCommand updateStudentCommand)
    {
        updateStudentCommand.UserId = getUserIdFromRequest();
        UpdatedUserFromAuthResponse response = await Mediator.Send(updateStudentCommand);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        DeletedStudentResponse response = await Mediator.Send(new DeleteStudentCommand { Id = id });

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdStudentResponse response = await Mediator.Send(new GetByIdStudentQuery { Id = id });
        return Ok(response);
    }

    [HttpGet("getByToken")]
    public async Task<IActionResult> GetSingle()
    {
        GetByTokenStudentResponse response = await Mediator.Send(new GetByTokenStudentQuery { UserId=getUserIdFromRequest()});
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListStudentQuery getListStudentQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListStudentListItemDto> response = await Mediator.Send(getListStudentQuery);
        return Ok(response);
    }
}