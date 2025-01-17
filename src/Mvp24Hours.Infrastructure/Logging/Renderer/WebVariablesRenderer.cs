//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.AspNetCore.Http.Features;
using Mvp24Hours.Infrastructure.Helpers;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;
using System.Globalization;
using System.Text;

namespace Mvp24Hours.Infrastructure.Logging.Renderer
{
    /// <summary>
    /// 
    /// </summary>
    [LayoutRenderer("web_variables")]
    public class WebVariablesRenderer : LayoutRenderer
    {
        private const string HttpHostKey = "HTTP_HOST";
        private const string UrlKey = "URL";

        public WebVariablesRenderer()
        {
            Format = "";
            Culture = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// 
        /// </summary>
        protected int GetEstimatedBufferSize(LogEventInfo ev)
        {
            if (ev.Level == LogLevel.Error)
            {
                return 0;
            }

            return 10000;
        }

        /// <summary>
        /// 
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultParameter]
        public string Format { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var serverVariables = HttpContextHelper.GetContext()?.Features?.Get<IServerVariablesFeature>();

            if (serverVariables == null)
            {
                return;
            }

            if (serverVariables[HttpHostKey] == null || serverVariables[UrlKey] == null)
            {
                return;
            }

            var xml = string.Format("url: {0}{1}", serverVariables[HttpHostKey], serverVariables[UrlKey]);

            builder.Append(xml);
        }
    }
}

