namespace Views.Cargo
{
    public interface ICargoComponent
    {
        /// <summary>
        /// プレビュー時の利用か
        /// </summary>
        bool IsPreview { get; }

        void UpdateCargoView(CargoView view, float delta);
    }
}