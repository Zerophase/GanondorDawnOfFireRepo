using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Summoned
{
    List<Unit> heldUnitType = new List<Unit>();
    public List<Unit> HeldUnitType { get { return heldUnitType; } }

    private Summoned() 
    {
        Subscribe();
    }

    private static Summoned instance;

    public static Summoned Instance
    {
        get { return instance ?? (instance = new Summoned()); }
    }

    public void HandleMessage(Telegram telegram)
    {
        if (telegram is SummonTelegram)
        {
            SummonTelegram summonTelegram = telegram as SummonTelegram;

            switch (summonTelegram.ActionToTake)
            {
                case MessagePurpose.ADD:
                    heldUnitType.Add(summonTelegram.Summon);
                    break;
                case MessagePurpose.REMOVE:
                    heldUnitType.Remove(summonTelegram.Summon);
                    break;
                default:
                    break;
            }
            
        }
    }

    public void Subscribe()
    {
        MessageDispatcher.Instance.SendMessage += new MessageDispatcher.SendMessageHandler(HandleMessage);
    }

    public void UnSubscribe()
    {
        MessageDispatcher.Instance.SendMessage -= new MessageDispatcher.SendMessageHandler(HandleMessage);
    }
}
