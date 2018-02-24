[System.Serializable]
public class TestPlayerModel : StoryModel {

    public TestPlayerModel() {
        creditCount = 200;
    }

    public int creditCount;
}

[System.Serializable]
public class TestShipModel : StoryModel {

    public TestShipModel() {
        combustible = 100;
    }

    public int combustible;
}