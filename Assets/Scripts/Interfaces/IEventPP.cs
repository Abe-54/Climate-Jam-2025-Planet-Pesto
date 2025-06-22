using System;
using System.Collections.Generic;

public interface IEventPP { }

public class EventsPP :IEventPP { }

public static class EventBusPP<T> where T : IEventPP {
    static readonly HashSet<IEventBindingPP<T>> bindings = new HashSet<IEventBindingPP<T>>();

    public static void Register(EventBindingPP<T> binding) => bindings.Add(binding);
    public static void Deregister(EventBindingPP<T> binding) => bindings.Remove(binding);

    public static void Raise(T @event)
    {
        foreach (var binding in bindings)
        {
            binding.OnEvent.Invoke(@event);
            binding.OnEventNoArgs.Invoke();
        }
    }
}