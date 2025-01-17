//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================

namespace Mvp24Hours.Infrastructure.Helpers
{
    /// <summary>
    /// Contains pre-loaded configurations
    /// </summary>
    public static class ConfigurationPropertiesHelper
    {
        #region [ Props ]

        private static int? _maxQtyByQueryPage;
        /// <summary>
        /// Maximum limit per page
        /// </summary>
        public static int MaxQtyByQueryPage
        {
            get
            {
                if (_maxQtyByQueryPage == null)
                {
                    if (int.TryParse((ConfigurationHelper.GetSettings("Mvp24Hours:Persistence:MaxQtyByQueryPage") ?? string.Empty), out int result))
                    {
                        _maxQtyByQueryPage = result;
                    }
                    else
                    {
                        _maxQtyByQueryPage = 100;
                    }
                }
                return (int)_maxQtyByQueryPage;
            }
        }

        #endregion
    }
}
