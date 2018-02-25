using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace StoryStack.Core {

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

            if (!node.GetType().IsSerializable) {
                Debug.LogWarning("[StoryNodeStack] StoryNode sub types need to be serializable but " + node.GetType().Name + " isn't.");
            }

            node._Inject(state);

            return ProcessAction(node.OnEnter());
        }

        StoryAction EndStory() {
            return null;
        }

        StoryViewModel ProcessAction(StoryAction action) {
            if (action == null) {
                Debug.Log("story is over");
                return null;
            }

            if (action.type == StoryActionType.POP_STORY) {
                stack.Pop();

                StoryNode node = stack.Peek();

                status.Pop();

                if (node != null) {
                    Type type = node.GetType();

                    MethodInfo methodInfo = GetPopCallbackMethod(type, popCallbacks.Pop());

                    return ProcessAction(methodInfo.Invoke(node, new object[0]) as StoryAction);
                } else {
                    return null;
                }
            } else if (action.type == StoryActionType.PUSH_STORY) {
                if (GetPopCallbackMethod(stack.Peek().GetType(), action.popCallback.Method.Name) == null) {
                    Debug.LogWarning("[StoryNodeStack] " + action.popCallback.Method.Name + " is not a method of " + stack.Peek().GetType().Name);
                }

                stack.Push(action.node);

                if(!action.node.GetType().IsSerializable) {
                    Debug.LogWarning("[StoryNodeStack] StoryNode sub types need to be serializable but " + action.node.GetType().Name + " isn't.");
                }

                popCallbacks.Push(action.popCallback.Method.Name);

                if (action.status != null) {
                    status.Pop();
                    status.Push(action.status);

                    if (action.status != null && !action.status.GetType().IsSerializable) {
                        Debug.LogWarning("[StoryNodeStack] StoryStatus sub types need to be serializable but " + action.status.GetType().Name + " isn't.");
                    }

                    action.status._Inject(state);
                }

                status.Push(null);

                action.node._Inject(state);

                return ProcessAction(action.node.OnEnter());
            } else if (action.type == StoryActionType.SHOW_STORY) {
                choiceCallback = action.choiceCallback.Method.Name;

                if (GetChoiceCallbackMethod(stack.Peek().GetType(), choiceCallback) == null) {
                    Debug.LogWarning("[StoryNodeStack] " + choiceCallback + " is not a method of " + stack.Peek().GetType().Name);
                }

                if (action.status != null) {
                    status.Pop();
                    status.Push(action.status);

                    if(action.status != null && !action.status.GetType().IsSerializable) {
                        Debug.LogWarning("[StoryNodeStack] StoryStatus sub types need to be serializable but " + action.status.GetType().Name + " isn't.");
                    }

                    action.status._Inject(state);
                }

                return action.story;
            }

            return null;
        }

        public StoryViewModel MakeChoice(int choice) {
            StoryNode node = stack.Peek();

            Type type = node.GetType();

            MethodInfo methodInfo = GetChoiceCallbackMethod(type, choiceCallback);

            return ProcessAction(methodInfo.Invoke(node, new object[] { choice }) as StoryAction);
        }

        public StoryStatusModel GetStatus() {
            StoryStatus[] statuses = status.ToArray();

            for (int i = statuses.Length - 1; i >= 0; i--) {
                if (statuses[i] != null) {
                    return statuses[i].GetStatus();
                }
            }

            return null;
        }

        public MethodInfo GetPopCallbackMethod(Type type, string name) {
            return type.GetMethod(
                name,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                null,
                CallingConventions.Any,
                new Type[] { },
                null
            );
        }

        public MethodInfo GetChoiceCallbackMethod(Type type, string name) {
            return type.GetMethod(
                choiceCallback,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                null,
                CallingConventions.Any,
                new Type[] { typeof(int) },
                null
            );
        }
    }
}
