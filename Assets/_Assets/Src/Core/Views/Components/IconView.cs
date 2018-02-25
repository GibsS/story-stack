using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace StoryStack.Views {

    public class IconView : MonoBehaviour {

        public Image image;
        public HexGraphic hex;

        public void Setup(StoryStatusStatModel model) {
            if (model.icon != null) {
                image.sprite = model.icon;
                image.color = model.color;

                image.gameObject.SetActive(true);
                hex.gameObject.SetActive(false);
            }
            switch (model.type) {
                case StoryStatusStatType.CIRCLE:
                case StoryStatusStatType.DIAMOND:
                case StoryStatusStatType.TRIANGLE:
                    Debug.Log("Not implemented");
                    break;
                case StoryStatusStatType.SQUARE:
                    image.gameObject.SetActive(true);
                    hex.gameObject.SetActive(false);
                    image.color = model.color;
                    break;
                case StoryStatusStatType.HEX:
                    image.gameObject.SetActive(false);
                    hex.gameObject.SetActive(true);
                    hex.color = model.color;
                    break;
            }
        }
    }
}