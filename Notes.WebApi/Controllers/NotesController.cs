﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Application.Notes.Commands.DeleteNote;
using Notes.Application.Notes.Queries.GetNoteDetails;
using Notes.Application.Notes.Queries.GetNoteList;
using Notes.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;

namespace Notes.WebApi.Controllers;

[ApiVersion("1.0")]
[ApiVersion("2.0")]
//[ApiVersionNeutral]
[Produces("application/json")]
[ApiController]
[Route("api/{version:apiVersion}/[controller]")]
public class NotesController : BaseController
{
    private readonly IMapper _mapper;

    public NotesController(IMapper mapper) => _mapper = mapper;

    /// <summary>
    /// Gets the list of notes
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// GET: /notes
    /// </remarks>
    /// <returns>Returns NoteListVm</returns>
    /// <response code="200">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<NoteListVm>> GetAll()
    {
        var query = new GetNoteListQuery { UserId = UserId };
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets the note by id
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// GET /note/D34D349E-43B8-429E-BCA4-793C932FD580
    /// </remarks>
    /// <param name="id">Note id (guid)</param>
    /// <returns>Returns NoteDetailsVm</returns>
    /// <response code="200">Success</response>
    /// <response code="401">If the user in unauthorized</response>
    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<NoteDetailsVm>> Get(Guid id)
    {
        var query = new GetNoteDetailsQuery { Id = id, UserId = UserId };
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Creates the note
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// POST /note
    /// {
    ///     title: "note title",
    ///     details: "note details"
    /// }
    /// </remarks>
    /// <param name="createNoteDto">CreateNoteDto object</param>
    /// <returns>Returns id (guid)</returns>
    /// <response code="201">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateNoteDto createNoteDto)
    {
        var command = _mapper.Map<CreateNoteCommand>(createNoteDto);
        command.UserId = UserId;
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Updates the note
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// PUT /note
    /// {
    ///     title: "updated note title"
    /// }
    /// </remarks>
    /// <param name="updateNoteDto">UpdateNoteDto object</param>
    /// <returns>Returns NoContent</returns>
    /// <response code="204">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Update([FromBody] UpdateNoteDto updateNoteDto)
    {
        var command = _mapper.Map<UpdateNoteCommand>(updateNoteDto);
        command.UserId = UserId;
        await Mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes the note by id
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// DELETE /note/88DEB432-062F-43DE-8DCD-8B6EF79073D3
    /// </remarks>
    /// <param name="id">Id of the note (guid)</param>
    /// <returns>Returns NoContent</returns>
    /// <response code="204">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var command = new DeleteNoteCommand { Id = id, UserId = UserId };
        await Mediator.Send(command);
        return NoContent();
    }
}
