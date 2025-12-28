using System.Collections.Generic;

namespace Views.Cargo
{
    public interface ICargoViewRegistry
    {
        void AddCurrent(CargoView view);
        void RemoveCurrent(CargoView view);
        void AddDropped(CargoView view);
        IReadOnlyList<CargoView> GetCurrentViews();
        IReadOnlyList<CargoView> GetDroppedViews();
        CargoView GetTopCurrentView();
        void ClearAll();
    }
}
