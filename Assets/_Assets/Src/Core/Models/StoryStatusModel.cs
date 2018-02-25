using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace StoryStack {

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

        public StoryStatusModel() {

        }

        public StoryStatusModel(StoryStatusEntryModel[] entries, StoryStatusStatModel[] stats) {
            this.entries = entries;
            this.stats = stats;
        }

        public StoryStatusEntryModel AddEntry(string name, string description, string status) {
            StoryStatusEntryModel entry = new StoryStatusEntryModel {
                name = name,
                description = description,
                status = status
            };

            if (entries == null) {
                entries = new StoryStatusEntryModel[] { entry };
            } else {
                List<StoryStatusEntryModel> newEntries = new List<StoryStatusEntryModel>(entries);

                newEntries.Add(entry);

                entries = newEntries.ToArray();
            }

            return entry;
        }

        public StoryStatusStatModel AddStat(string id, string name, int quantity, string description, string status, Sprite icon, Color color) {
            return AddStat(id, name, quantity, description, status, icon, StoryStatusStatType.CIRCLE, color);
        }

        public StoryStatusStatModel AddStat(string id, string name, int quantity, string description, string status, StoryStatusStatType type, Color color) {
            return AddStat(id, name, quantity, description, status, null, type, color);
        }

        StoryStatusStatModel AddStat(string id, string name, int quantity, string description, string status, Sprite icon, StoryStatusStatType type, Color color) {
            StoryStatusStatModel stat = new StoryStatusStatModel {
                id = id,

                icon = icon,
                type = type,
                color = color,

                name = name,
                quantity = quantity,
                description = description,
                status = status
            };

            if (stats == null) {
                stats = new StoryStatusStatModel[] { stat };
            } else {
                List<StoryStatusStatModel> newStats = new List<StoryStatusStatModel>(stats);

                newStats.Add(stat);

                stats = newStats.ToArray();
            }

            return stat;
        }

        public void Validate() {
            if (entries != null) {
                foreach (var entry in entries) {
                    entry.Validate();
                }
            }

            if (stats != null) {
                foreach (var stat in stats) {
                    stat.Validate();
                }

                if (stats.Length != stats.ToList().ConvertAll(s => s.id).Distinct().Count()) {
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
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(status)) {
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
            if (string.IsNullOrEmpty(id)) {
                Debug.LogWarning("[StoryStatusStatModel] ID can't be null or empty");
            }

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(status)) {
                Debug.LogWarning("[StoryStatusStatModel] No string field can be null or empty");
            }
        }
    }
}