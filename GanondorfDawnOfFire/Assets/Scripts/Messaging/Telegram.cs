using UnityEngine;
using System.Collections;
using System;

public enum HandledBy {GLOBAL, LOCAL};
public class Telegram : EventArgs
{
    public HandledBy Handler;

    public Telegram(HandledBy handler)
    {
        this.Handler = handler;
    }
}

public class UnitTelegram : Telegram
{
    public Unit Sender;
    public Unit Receiver;

    public UnitTelegram(Unit sender, Unit receiver, HandledBy handler)
        : base(handler)
    {
        this.Sender = sender;
        this.Receiver = receiver;
    }
}

public class PlayerTelegram : Telegram
{
    public Unit Sender;
    public Player Receiver;

    public PlayerTelegram(Unit sender, Player receiver, HandledBy handler)
        : base(handler)
    {
        Sender = sender;
        Receiver = receiver;
    }
}

public enum TargetText {Ally, Enemy};
public class InterfaceTelegram : Telegram
{
    public TargetText _TargetText;
    public string Text;

    public InterfaceTelegram(string text, TargetText targetText, HandledBy handler)
        : base(handler)
    {
        Text = text;
        _TargetText = targetText;
    }
}
public enum MessagePurpose { ADD, REMOVE };
public class SummonTelegram : Telegram
{
    public Unit Summon;
    public MessagePurpose ActionToTake;

    public SummonTelegram(Unit summon, MessagePurpose messagePurpose, HandledBy handler)
        : base(handler)
    {
        this.Summon = summon;
        ActionToTake = messagePurpose;
    }
}

public class RockTelegram : UnitTelegram
{
    public Rock RockCollidedWIth;

    public RockTelegram(Unit sender, Unit receiver, Rock rock, HandledBy handler)
        :base(sender, receiver, handler)
    {
        RockCollidedWIth = rock;
    }
}

public class MediatorTelegram : Telegram
{
    public UnitMediator Sender; 
    public MediatorTelegram(UnitMediator sender, HandledBy handler)
        :base(handler)
    {
        this.Sender = sender;
    }
}