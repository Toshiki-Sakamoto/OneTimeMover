namespace Views.Cargo
{
    public class CargoDropItemTouchedEvent
    {
        public CargoDropItemController DropItem;
        public CargoView CargoView;
        public OneTripMover.Views.Player.PlayerController Player;
        public UnityEngine.Collider2D PlayerCollider;
    }
}
