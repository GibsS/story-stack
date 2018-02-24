using System;
using System.Collections.Generic;
using UnityEngine;

public class StoryNodeStack {

    GameState state;

    public Stack<StoryNode> stack;
    public Stack<Func<StoryAction>> popCallbacks;
    public Stack<StoryStatus> status;

    Func<int, StoryAction> choiceCallback;

    public StoryNodeStack(GameState state) {
        this.state = state;

        stack = new Stack<StoryNode>();
        popCallbacks = new Stack<Func<StoryAction>>();
        status = new Stack<StoryStatus>();
    }

    public StoryViewModel Start(StoryNode node) {
        stack.Clear();

        stack.Push(node);
        popCallbacks.Push(() => {
            Debug.Log("story is over");
            return null;
        });
        status.Push(null);

        node._Inject(state);

        return ProcessAction(node.OnEnter());
    }

    StoryViewModel ProcessAction(StoryAction action) {
        if(action == null) {
            Debug.Log("story is over");
            return null;
        }

        if(action.type == StoryActionType.POP_STORY) {
            stack.Pop();

            Func<StoryAction> callback = popCallbacks.Pop();

            status.Pop();

            return ProcessAction(callback());
        } else if(action.type == StoryActionType.PUSH_STORY) {
            stack.Push(action.node);
            popCallbacks.Push(action.popCallback);

            if (action.status != null) {
                status.Pop();
                status.Push(action.status);

                action.status._Inject(state);
            }

            status.Push(null);

            action.node._Inject(state);

            return ProcessAction(action.node.OnEnter());
        } else if(action.type == StoryActionType.SHOW_STORY) {
            choiceCallback = action.choiceCallback;

            if (action.status != null) {
                status.Pop();
                status.Push(action.status);

                action.status._Inject(state);
            }

            return action.story;
        }

        return null;
    }

    public StoryViewModel MakeChoice(int choice) {
        return ProcessAction(choiceCallback(choice));
    }

    public StoryStatusModel GetStatus() {
        StoryStatus[] statuses = status.ToArray();

        for(int i = statuses.Length - 1; i >= 0; i--) {
            if(statuses[i] != null) {
                return statuses[i].GetStatus();
            }
        }

        return null;
    }
}
