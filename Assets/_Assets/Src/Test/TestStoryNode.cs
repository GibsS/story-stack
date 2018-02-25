using System;

[Serializable]
public class TestStoryNode : StoryNode {

    public override StoryAction OnEnter() {
        return Text("Your ship is doing great, still cruising along.", HandleOpenShop);
    }

    StoryAction HandleOpenShop(int _) {
        return Push<ShopNode>(Test);
    }

    public StoryAction Test() {
        return Text("You feel happy with this exchange", EndStory, "+1 morale", "you ok now");
    }
}

[Serializable]
public class ShopNode : StoryNode {

    public override StoryAction OnEnter() {
        return Text("What looked like a shop from far away ended up being a shop when up close.", Shop, new TestStatus());
    }

    StoryAction Shop(int choice) {
        var action = Choice("The shop owner tells you to take a look at his merchandise.", "What do you wish to buy?", ShopChoice);

        action.choice.duration = 0;

        // 0
        var buyChoice = action.AddChoice("buy combustible", "1 credit");
        buyChoice.requirement = true;
        buyChoice.locked = !GetSystem<TestSystem>().CanBuyCombustible();

        // 1
        var buyNothingChoice = action.AddChoice("buy nothing");

        // 2
        var backChoice = action.AddChoice("go to the back");

        return action;
    }

    StoryAction ShopChoice(int choice) {
        if(choice == 0) {
            GetSystem<TestSystem>().BuyCombustible();

            StoryAction action = Shop(0);

            action.story.preventFade = true;

            return action;
        } else if(choice == 1) {
            return TimedChoice();
        } else if(choice == 2) {
            return Push(new ShopBackendNode(), TimedChoice);
        } else {
            return TimedChoice();
        }
    }

    StoryAction TimedChoice() {
        var action = Choice("Quick! Make a choice!", "What will it be?", End);

        action.choice.duration = 100;

        action.AddChoice(0, "choice 1").locked = true;
        action.AddChoice(1, "choice 2");
        action.AddChoice(2, "choice 3");

        var choice4 = action.AddChoice(3, "choice 4");
        choice4.AddChoice(4, "sub choice 1");
        choice4.AddChoice(5, "sub choice 1");

        return action;
    }

    StoryAction End(int _) {
        return Text("You leave.", Pop, "you bought some stuff");
    }
}

[Serializable]
public class ShopBackendNode : StoryNode {

    public override StoryAction OnEnter() {
        return Text("you check out the back of the store", End);
    }

    StoryAction End(int _) {
        return Text("You go back to the main area", Pop);
    }
}