using UnityEngine;

[System.Serializable]
public class TestStoryNode : StoryNode {

    public override StoryAction OnEnter() {
        return StoryAction.Show(new TextStoryModel {
            description = "Your ship is doing great, still cruising along."
        }, a => StoryAction.Push(new ShopNode(), Test));
    }

    public StoryAction Test() {
        return StoryAction.Show(new TextStoryModel {
            description = "You feel happy with this exchange"
        }, choice => null);
    }
}

public class ShopNode : StoryNode {

    public override StoryAction OnEnter() {
        return StoryAction.Show(new TextStoryModel {
            description = "What looked like a shop from far away ended up being a shop when up close."
        }, choice => Shop(), new TestStatus());
    }

    public StoryAction Shop() {
        return StoryAction.Show(new ChoiceStoryModel {
            description = "The shop owner tells you to take a look at his merchandise.",
            choiceDescription = "What do you wish to buy?",

            duration = 0,

            effects = null,

            choices = new ChoiceModel[] {
                new ChoiceModel {
                    id = 0,
                    action = "buy combustible", requirementOrEffect = "1 credit",
                    requirement = true, locked = !GetSystem<TestSystem>().CanBuyCombustible()
                },
                new ChoiceModel { id = 1, action = "buy nothing" }
            },
        }, Choice);
    }

    StoryAction Choice(int choice) {
        if(choice == 0) {
            GetSystem<TestSystem>().BuyCombustible();

            return Shop();
        } else {
            return TimedChoice();
        }
    }

    StoryAction TimedChoice() {
        return StoryAction.Show(new ChoiceStoryModel {
            description = "Quick! Make a choice!",

            choiceDescription = "What will it be?",

            duration = 100,

            choices = new ChoiceModel[] {
                new ChoiceModel { id = 0, action = "choice 1", locked = true },
                new ChoiceModel { id = 1, action = "choice 2" },
                new ChoiceModel { id = 2, action = "choice 3" },
                new ChoiceModel { id = 3, action = "choice 4", subChoices = new ChoiceModel[] {
                    new ChoiceModel { id = 4, action = "sub choice 1" },
                    new ChoiceModel { id = 5, action = "sub choice 2" }
                } }
            }
        }, choice => End());
    }

    StoryAction End() {
        return StoryAction.Show(new TextStoryModel {
            description = "You leave",

            effects = new string[] { "you bought some stuff" }
        }, a => StoryAction.Pop());
    }
}