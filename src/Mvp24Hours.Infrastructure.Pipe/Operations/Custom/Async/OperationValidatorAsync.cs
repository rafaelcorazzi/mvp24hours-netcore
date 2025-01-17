//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Pipe.Operations.Custom
{
    /// <summary>  
    /// Abstraction of mapping operations
    /// </summary>
    public abstract class OperationValidatorAsync : OperationBaseAsync
    {
        public override async Task<IPipelineMessage> ExecuteAsync(IPipelineMessage input)
        {
            if (!await IsValid(input))
            {
                input.SetLock();
            }

            return input;
        }

        public abstract Task<bool> IsValid(IPipelineMessage input);
    }
}
