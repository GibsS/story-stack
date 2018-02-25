using UnityEngine;

using StoryStack;

[System.Serializable]
public class TestStatus : StoryStatus {

    public override StoryStatusModel GetStatus() {
        var status = CreateModel();

        TestPlayerModel player = GetModel<TestPlayerModel>();
        TestShipModel ship = GetModel<TestShipModel>();

        for (int i = 0, count = Random.Range(0, 3); i < count; i++) {
            status.AddEntry("Test", "This is a test entry", "all is well");
        }

        status.AddStat(
            "credit", "credit", player.creditCount, 
            "amount of credit your crew owns", "Thats a lot of dough", 
            StoryStatusStatType.HEX, Color.black
        );

        status.AddStat(
            "combustible", "combustible", ship.combustible,
            "fuel your ship uses to move", ship.combustible > 0 ? "sufficient stock" : "more required to travel",
            StoryStatusStatType.SQUARE, Color.black
        );

        return status;
    }
}
