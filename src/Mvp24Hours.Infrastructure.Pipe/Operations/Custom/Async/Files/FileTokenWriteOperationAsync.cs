//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Infrastructure.Helpers;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Pipe.Operations.Custom.Files
{
    /// <summary>
    /// Operation for writing file log token
    /// </summary>
    public class FileTokenWriteOperationAsync<T> : OperationBaseAsync
    {
        static bool _enable = ConfigurationHelper.GetSettings<bool>("Mvp24Hours:Operation:FileToken:Enable");

        public virtual string FileLogPath => null;

        public override Task<IPipelineMessage> ExecuteAsync(IPipelineMessage input)
        {
            if (_enable)
            {
                var dto = input.GetContent<T>();
                if (dto == null)
                {
                    return Task.FromResult(input);
                }
                FileLogHelper.WriteLogToken(input.Token, typeof(T).Name.ToLower(), dto, FileLogPath);
            }
            return Task.FromResult(input);
        }
    }
}
