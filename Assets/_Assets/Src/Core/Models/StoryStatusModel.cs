using System.Linq;

using UnityEngine;

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

    public void Validate() {
        if(entries != null) {
            foreach(var entry in entries) {
                entry.Validate();
            }
        }

        if(stats != null) {
            foreach(var stat in stats) {
                stat.Validate();
            }

            if(stats.Length != stats.ToList().ConvertAll(s => s.id).Distinct().Count()) {
                Debug.LogWarning("[StoryStatusModel] Stat shouldn't appear more than once");
            }
        }
    }
}

public class StoryStatusEntryModel {

    public string name;
    public string description;
    public string status;

    public void Validate() {
        if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(status)) {
            Debug.LogWarning("[StoryStatusEntryModel] No string field can be null or empty");
        }
    }
}

public class StoryStatusStatModel {

    public string id;

    public Sprite icon = null;
    public StoryStatusStatType type = StoryStatusStatType.CIRCLE;
    public Color color = Color.black;

    public int quantity;
    public string name;
    public string description;
    public string status;

    public void Validate() {
        if(string.IsNullOrEmpty(id)) {
            Debug.LogWarning("[StoryStatusStatModel] ID can't be null or empty");
        }

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(status)) {
            Debug.LogWarning("[StoryStatusStatModel] No string field can be null or empty");
        }
    }
}