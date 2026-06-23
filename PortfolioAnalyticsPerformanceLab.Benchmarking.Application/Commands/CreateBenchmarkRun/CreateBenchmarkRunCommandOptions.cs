using System.Text.Json;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Enums;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Commands.CreateBenchmarkRun;


public abstract class CreateBenchmarkRunCommandOptions
{
    public ScenarioType ScenarioType { get; set; }
    public JsonElement Configuration { get; set; }
}


/*
 
 For the scenario which is in JsonElement, we can use something like this:
 public sealed class CpuValuationScenario : IBenchmarkScenario
   {
       public string Id => "cpu-valuation";
   
       public string Name => "CPU-bound Portfolio Valuation";
   
       public string Description =>
           "Tests CPU usage, batching, worker count, and parallel execution strategies.";
   
       public BenchmarkScenarioDetailsDto GetDetails()
       {
           return new BenchmarkScenarioDetailsDto
           {
               Id = Id,
               Name = Name,
               Description = Description,
               ConfigurationSchema = CpuValuationSchema.Build(),
               Presets = CpuValuationPresets.All
           };
       }
   
       public object DeserializeOptions(JsonElement options)
       {
           var typedOptions = options.Deserialize<CpuValuationOptions>();
   
           if (typedOptions is null)
               throw new ValidationException("Invalid CPU valuation options.");
   
           Validate(typedOptions);
   
           return typedOptions;
       }
   
       private static void Validate(CpuValuationOptions options)
       {
           if (options.PortfolioCount < 1 || options.PortfolioCount > 100)
               throw new ValidationException("PortfolioCount must be between 1 and 100.");
   
           if (options.WorkerCount < 1 || options.WorkerCount > 32)
               throw new ValidationException("WorkerCount must be between 1 and 32.");
       }
   }

*/