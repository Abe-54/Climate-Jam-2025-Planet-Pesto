using UnityEngine;

public struct ConversationEndEvent : IEventPP {
    public string eventName;
}

public struct ConversationStartEvent : IEventPP { }

public struct ScannerOnEvent : IEventPP { }

public struct AlinaConversationEvent : IEventPP { }

public struct SteamEmptyEvent : IEventPP { }