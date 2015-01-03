using UnityEngine;
using System.Collections;

public class MessageDispatcher
{
    public delegate void SendMessageHandler(Telegram telegram);
    public event SendMessageHandler SendMessage;
    public event SendMessageHandler GlobalMessage;

    private MessageDispatcher() { }
    private static MessageDispatcher instance;

    public static MessageDispatcher Instance
    {
        get { return instance ?? (instance = new MessageDispatcher()); }
    }

    public void DispatchMessage(Telegram telegram)
    {
        // TODO learn how to code a priority que and implement it.

        if (SendMessage != null && telegram.Handler == HandledBy.LOCAL)
            SendMessage(telegram);

        if (GlobalMessage != null && telegram.Handler == HandledBy.GLOBAL)
            GlobalMessage(telegram);
    }
}
