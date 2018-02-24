using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabStore : MonoBehaviour {

    public static PrefabStore store { get; private set; }

    public void Initialize() {
        if(store != null) {
            Destroy(this);
            return;
        }

        store = this;
    }

    public GameObject icon;

    public GameObject choiceStoryPrefab;
    public GameObject textStoryPrefab;
}
