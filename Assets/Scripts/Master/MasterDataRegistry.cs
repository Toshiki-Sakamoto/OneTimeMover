using System.Collections.Generic;
using OneTripMover.Master;

namespace OneTripMover.Master
{
    public abstract class MasterDataRegistry<TMaster> : IMasterDataRegistry<TMaster> 
        where TMaster : IMasterData<TMaster>
    {
        protected readonly Dictionary<MasterId<TMaster>, TMaster> _masters = new ();
        
        public void Register(TMaster master)
        {
            _masters[master.Id] = master;
            RegisterCore(master);
        }

        public void Clear()
        {
            _masters.Clear();
            ClearCore();
        }

        public bool TryGetValue(MasterId<TMaster> id, out TMaster master)
        {
            return _masters.TryGetValue(id, out master);
        }
        
        public IEnumerable<TMaster> GetAll()
        {
            return _masters.Values;
        }

        protected virtual void RegisterCore(TMaster master) {}
        protected virtual void ClearCore() { }
    }
}