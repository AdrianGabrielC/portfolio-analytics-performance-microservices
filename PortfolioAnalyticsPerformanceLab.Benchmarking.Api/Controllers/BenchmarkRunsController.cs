using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Commands.CreateBenchmarkRun;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Queries.GetBenchmarkScenarios;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public partial class BenchmarkRunsController: ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public BenchmarkRunsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [HttpPost("benchmark-runs")]
    [ProducesResponseType(typeof(CreateBenchmarkRunCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateBenchmarkRunCommandResult>> GetTestDataAsync([FromBody] CreateBenchmarkRunCommandOptions request, CancellationToken cancellationToken) => 
        await _mediator.Send(new CreateBenchmarkRunCommand(request), cancellationToken);
    
    [HttpGet("benchmark-scenarios")]
    [ProducesResponseType(typeof(GetBenchmarkScenariosQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetBenchmarkScenariosQueryResult>> GetBenchmarkScenariosAsync(CancellationToken cancellationToken) =>
        await _mediator.Send(new GetBenchmarkScenariosQuery(new GetBenchmarkScenariosQueryOptions()), cancellationToken);
}