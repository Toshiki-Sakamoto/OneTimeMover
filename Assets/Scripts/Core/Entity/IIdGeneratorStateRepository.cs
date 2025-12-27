using System.Collections.Generic;
using Core.Common;

namespace OneTripMover.Core.Entity
{
    /// <summary>
    /// ID生成器の保存管理
    /// </summary>
    public interface IIdGeneratorStateRepository
    {
        IReadOnlyDictionary<TypeToken, long> GetStates();
        
        void SetStates(IReadOnlyDictionary<TypeToken, long> state);
    }
}
