using UnityEngine;
using System.Collections;

public class ChoiceModel {

    public int id;

    public string action;
    public string requirementOrEffect;

    public bool available;

    public ChoiceModel[] subChoices;
}
