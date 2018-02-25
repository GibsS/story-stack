using UnityEngine;
using System.Collections;

public class TestStatus : StoryStatus {

    public override StoryStatusModel GetStatus() {
        StoryStatusEntryModel[] entries = new StoryStatusEntryModel[Random.Range(0, 3)];

        TestPlayerModel player = GetModel<TestPlayerModel>();
        TestShipModel ship = GetModel<TestShipModel>();

        for (int i = 0; i < entries.Length; i++) {
            entries[i] = new StoryStatusEntryModel {
                name = "Test",
                description = "This is a test entry",
                status = "all is well"
            };
        }

        StoryStatusStatModel[] stats = {
            new StoryStatusStatModel {
                id = "credit",

                name = "credit",
                description = "amout of credit your crew owns",
                status = "That's a lot of dough",
                quantity = player.creditCount,

                color = Color.black,
                type = StoryStatusStatType.HEX
            },
            new StoryStatusStatModel {
                id = "combustible",

                name = "combustible",
                description = "fuel your ship uses to move",
                status = ship.combustible > 0 ? "sufficient stock" : "more required to travel",
                quantity = ship.combustible,

                color = Color.black,
                type = StoryStatusStatType.SQUARE
            }
        };

        return new StoryStatusModel {
            entries = entries,
            stats = stats
        };
    }
}
