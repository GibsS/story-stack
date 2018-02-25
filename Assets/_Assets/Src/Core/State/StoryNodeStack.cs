using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class StoryNodeStack {

    GameState state;

    public Stack<StoryNode> stack;
    public Stack<string> popCallbacks;
    public Stack<StoryStatus> status;

    public string choiceCallback;

    public StoryNodeStack(GameState state) {
        this.state = state;

        stack = new Stack<StoryNode>();
        popCallbacks = new Stack<string>();
        status = new Stack<StoryStatus>();
    }

    public StoryViewModel Start(StoryNode node) {
        stack.Clear();

        stack.Push(node);
        Func<StoryAction> action = EndStory;
        popCallbacks.Push(action.Method.Name);
        status.Push(null);

        node._Inject(state);

        return ProcessAction(node.OnEnter());
    }

    StoryAction EndStory() {
        return null;
    }

    StoryViewModel ProcessAction(StoryAction action) {
        if(action == null) {
            Debug.Log("story is over");
            return null;
        }

        if(action.type == StoryActionType.POP_STORY) {
            stack.Pop();

            StoryNode node = stack.Peek();

            status.Pop();

            if (node != null) {
                Type type = node.GetType();

                MethodInfo methodInfo = type.GetMethod(
                    popCallbacks.Pop(),
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                    null,
                    CallingConventions.Any,
                    new Type[] { },
                    null
                );
                
                return ProcessAction(methodInfo.Invoke(node, new object[0]) as StoryAction);
            } else {
                return null;
            }
        } else if(action.type == StoryActionType.PUSH_STORY) {
            stack.Push(action.node);
            popCallbacks.Push(action.popCallback.Method.Name);

            if (action.status != null) {
                status.Pop();
                status.Push(action.status);

                action.status._Inject(state);
            }

            status.Push(null);

            action.node._Inject(state);

            return ProcessAction(action.node.OnEnter());
        } else if(action.type == StoryActionType.SHOW_STORY) {
            choiceCallback = action.choiceCallback.Method.Name;

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
        StoryNode node = stack.Peek();

        Type type = node.GetType();

        MethodInfo methodInfo = type.GetMethod(
            choiceCallback,
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
            null,
            CallingConventions.Any,
            new Type[] { typeof(int) },
            null
        );

        return ProcessAction(methodInfo.Invoke(node, new object[] { choice }) as StoryAction);
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
