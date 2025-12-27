using System;

namespace OneTripMover.Master
{
    /// <summary>
    /// マスタデータ基本
    /// </summary>
    public interface IMasterData<TMaster> : IEquatable<IMasterData<TMaster>>
    {
        /// <summary>
        /// マスタID
        /// </summary>
        MasterId<TMaster> Id { get; }
    }
}