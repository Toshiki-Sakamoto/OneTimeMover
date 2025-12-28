using System.Collections.Generic;
using System.Linq;

namespace Views.Cargo
{
    public class CargoViewRegistry : ICargoViewRegistry
    {
        private readonly List<CargoView> _current = new();
        private readonly List<CargoView> _dropped = new();

        public void AddCurrent(CargoView view)
        {
            if (view == null) return;
            if (_current.Contains(view)) return;
            _current.Add(view);
        }

        public void RemoveCurrent(CargoView view)
        {
            if (view == null) return;
            _current.Remove(view);
        }

        public void AddDropped(CargoView view)
        {
            if (view == null) return;
            if (_dropped.Contains(view)) return;
            _dropped.Add(view);
        }

        public IReadOnlyList<CargoView> GetCurrentViews() => _current;
        public IReadOnlyList<CargoView> GetDroppedViews() => _dropped;

        public CargoView GetTopCurrentView()
        {
            if (_current.Count == 0) return null;
            return _current.OrderBy(v => v.transform.position.y).LastOrDefault();
        }

        public void ClearAll()
        {
            _current.Clear();
            _dropped.Clear();
        }
    }
}
