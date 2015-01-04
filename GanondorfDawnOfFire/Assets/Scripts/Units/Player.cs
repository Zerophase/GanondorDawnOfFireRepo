using UnityEngine;
using System.Collections;

public class Player : BaseUnit 
{
    private UISprite healthBar;

	protected override void Start () 
    {
        health = 100;
        physicalDefense = 15;

        healthBar = GameObject.Find("HealthBar/Health").GetComponent<UISprite>();

        if (Levels.VILLAGELEVEL == (Levels)Application.loadedLevel)
            Teams.Instance.Add(this);

        Subscribe();
	}

    public void HandleMessage(Telegram telegram)
    {
        if (telegram is PlayerTelegram)
        {

            Defend((telegram as PlayerTelegram).Sender.Strength);
        }
    }

    public void Defend(int damage)
    {
        if (physicalDefense >= damage)
            health -= 1;
        else
            health -= damage - physicalDefense;

        healthBar.SetDimensions((int)(160f * ((float)health * .01f)), 156);

        Die();
    }

    private void Die()
    {
        if(health <= 0)
        {
            Teams.Instance.Remove(this);
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
