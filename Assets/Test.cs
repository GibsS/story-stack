using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public RectGraphic rect;

    private void Update() {
        rect.lineWidth = Time.time / 10;

        rect.Rebuild(UnityEngine.UI.CanvasUpdate.PreRender);
    }
}
