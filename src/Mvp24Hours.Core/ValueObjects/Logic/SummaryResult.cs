//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mvp24Hours.Core.ValueObjects.Logic
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.ISummaryResult"/>
    /// </summary>
    [DataContract, Serializable]
    public class SummaryResult : BaseVO, ISummaryResult
    {
        #region [ Ctor ]
        public SummaryResult(int totalCount, int totalPages)
        {
            TotalCount = totalCount;
            TotalPages = totalPages;
        }
        #endregion

        #region [ Properties ]
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.ISummaryResult.TotalCount"/>
        /// </summary>
        [DataMember]
        public int TotalCount { get; }
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.ValueObjects.Logic.ISummaryResult.TotalPages"/>
        /// </summary>
        [DataMember]
        public int TotalPages { get; }
        #endregion

        #region [ Methods ]
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TotalCount;
            yield return TotalPages;
        }
        #endregion
    }
}
