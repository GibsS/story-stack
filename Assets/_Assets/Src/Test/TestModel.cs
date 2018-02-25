[System.Serializable]
public class TestPlayerModel : StoryModel {
    
    public int creditCount;

    public TestPlayerModel() {
        creditCount = 10;
    }

    public override StoryModel Clone() {
        return new TestPlayerModel {
            creditCount = creditCount
        };
    }
}

[System.Serializable]
public class TestShipModel : StoryModel {

    public int combustible;

    public TestShipModel() {
        combustible = 100;
    }

    public override StoryModel Clone() {
        return new TestShipModel {
            combustible = combustible
        };
    }
}