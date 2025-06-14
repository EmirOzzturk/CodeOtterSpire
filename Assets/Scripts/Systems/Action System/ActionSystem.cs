using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Action_System
{
    public class ActionSystem : Singleton<ActionSystem>
    {
        private List<GameAction> reactions = null;
        public bool IsPreforming { get; private set; } = false;
        private static Dictionary<Type, List<Action<GameAction>>> preSubs = new();
        private static Dictionary<Type, List<Action<GameAction>>> postSubs = new();
        private static Dictionary<Type, Func<GameAction, IEnumerator>> performers = new();

        public void Perform(GameAction action, System.Action OnPerformFinished = null)
        {
            if (IsPreforming) return;
            IsPreforming = true;
            StartCoroutine(Flow(action, () => 
            {
                IsPreforming = false;
                OnPerformFinished?.Invoke();
            }));
        }

        public void AddReaction(GameAction action)
        {
            reactions?.Add(action);
        }

        private IEnumerator Flow(GameAction action, Action onFlowFinished = null)
        {
            reactions = action.PreReactions;
            PerformSubscribers(action, preSubs);
            yield return PerformReactions();

            reactions = action.PerformReactions;
            yield return PerformPerformer(action);
            yield return PerformReactions();
            
            reactions = action.PostReactions;
            PerformSubscribers(action, postSubs);
            yield return PerformReactions();
            
            onFlowFinished?.Invoke();
        }

        private void PerformSubscribers(GameAction action, Dictionary<Type, List<Action<GameAction>>> subs)
        {
            Type type = action.GetType();
            if (subs.ContainsKey(type))
            {
                foreach (var sub in subs[type])
                {
                    sub(action);
                }
            }
        }

        private IEnumerator PerformReactions()
        {
            foreach (var reaction in reactions)
            {
                yield return Flow(reaction);
            }
        }

        private IEnumerator PerformPerformer(GameAction action)
        {
            Type type = action.GetType();
            if (performers.ContainsKey(type))
            {
                yield return performers[type](action);
            }
        }
        
        public static void AttachPerformer<T>(Func<T, IEnumerator> performer) where T : GameAction
        {
            Type type = typeof(T);
            IEnumerator WrappedPerformer(GameAction action) => performer((T)action);
            if (performers.ContainsKey(type)) performers[type] = WrappedPerformer;
            else performers.Add(type, WrappedPerformer);
        }

        public static void DetachPerformer<T>() where T : GameAction
        {
            Type type = typeof(T);
            if (performers.ContainsKey(type)) performers.Remove(type);
        }

        public static void SubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
        {
            Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
            void WrappedReaction(GameAction action) => reaction((T)action);
            
            if (subs.ContainsKey(typeof(T)))
            {
                subs[typeof(T)].Add(WrappedReaction);
            }
            else
            {
                subs.Add(typeof(T), new());
                subs[typeof(T)].Add(WrappedReaction);
            }
        }

        public static void UnsubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
        {
            Dictionary<Type, List<Action<GameAction>>> subs =  timing == ReactionTiming.PRE ? preSubs : postSubs;

            if (subs.ContainsKey(typeof(T)))
            {
                void WrappedReaction(GameAction action) => reaction((T)action);
                subs[typeof(T)].Remove(WrappedReaction);
            }
        }
    }
}