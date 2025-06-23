using System;

public interface IEventBindingPP<T> { 
    public Action<T> OnEvent {  get; set; }
    public Action OnEventNoArgs { get; set; }
}

public class EventBindingPP<T> : IEventBindingPP<T> where T : IEventPP
{
    Action<T> onEvent = _ => { };
    Action onEventNoArgs = () => { };

    Action<T> IEventBindingPP<T>.OnEvent
    {
        get => onEvent;
        set => onEvent = value;
    }

    Action IEventBindingPP<T>.OnEventNoArgs
    {
        get => onEventNoArgs;
        set => onEventNoArgs = value;
    }

    public EventBindingPP(Action<T> onEvent) => this.onEvent = onEvent;
    public EventBindingPP(Action onEventNoArgs) => this.onEventNoArgs = onEventNoArgs;

    public void Add(Action onEvent) => onEventNoArgs += onEvent;
    public void Remove(Action onEvent) => onEventNoArgs -= onEvent;

    public void Add(Action<T> onEvent) => this.onEvent += onEvent;
    public void Remove(Action<T> onEvent) => this.onEvent -= onEvent;
}