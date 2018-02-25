using StoryStack;

public class TestSystem : StorySystem {

    public bool CanBuyCombustible() {
        TestPlayerModel player = GetModel<TestPlayerModel>();

        return player.creditCount > 0;
    }

    public void BuyCombustible() {
        if(CanBuyCombustible()) {
            TestPlayerModel player = GetModel<TestPlayerModel>();
            TestShipModel ship = GetModel<TestShipModel>();

            player.creditCount--;

            ship.combustible += 10;
        }
    }
}
