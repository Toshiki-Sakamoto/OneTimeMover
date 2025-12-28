using System.Collections.Generic;
using System.Linq;

namespace Views.Cargo
{
    public class CargoViewRegistry : ICargoViewRegistry
    {
        private readonly List<CargoView> _current = new();
        private readonly List<CargoView> _dropped = new();
        private CargoView _top;

        public void AddCurrent(CargoView view)
        {
            if (view == null) return;
            if (_current.Contains(view)) return;
            _current.Add(view);
            _top = view;
        }

        public void RemoveCurrent(CargoView view)
        {
            if (view == null) return;
            _current.Remove(view);
            
            if (_top == view)
            {
                _top = _current.Count > 0 ? _current[^1] : null;
            }
        }

        public void AddDropped(CargoView view)
        {
            if (view == null) return;
            if (_dropped.Contains(view)) return;
            _dropped.Add(view);
        }

        public IReadOnlyList<CargoView> GetCurrentViews() => _current;
        public IReadOnlyList<CargoView> GetDroppedViews() => _dropped;
        public CargoView GetTop() => _top;

        public void ClearAll()
        {
            _current.Clear();
            _dropped.Clear();
            _top = null;
        }
    }
}
