using UnityEngine;
using System.Collections;

public class ChoiceModel {

    public int id;

    public string action;
    public string requirementOrEffect;

    public bool locked;
    public bool requirement;

    public ChoiceModel[] subChoices;

    public bool isMenu { get { return subChoices != null && subChoices.Length != 0; } }
}
