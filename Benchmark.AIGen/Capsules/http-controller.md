# HTTP Controller Rules

Controllers:
- Must inherit from ControllerBase.
- Must use [ApiController].
- Must use [Route].
- Must be partial.
- Must receive IMediator and IMapper through constructor injection.

Controller methods:
- Must be async.
- Must accept CancellationToken.
- Must call _mapper.Map.
- Must call _mediator.Send.
- Must return ActionResult<T>.