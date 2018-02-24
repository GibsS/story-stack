using UnityEngine;
using System.Collections;

public enum StoryStatusStatType {
    CIRCLE,
    SQUARE,
    DIAMOND,
    HEX,
    TRIANGLE
}

public class StoryStatusModel {

    public StoryStatusEntryModel[] entries;
    public StoryStatusStatModel[] stats;
}

public class StoryStatusEntryModel {

    public string name;
    public string description;
    public string status;
}

public class StoryStatusStatModel {

    public string id;

    public Sprite icon = null;
    public StoryStatusStatType type = StoryStatusStatType.CIRCLE;
    public Color color = Color.black;

    public int quantity;
    public string description;
    public string status;
}