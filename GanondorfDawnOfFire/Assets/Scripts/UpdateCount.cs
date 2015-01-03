using UnityEngine;
using System.Collections;

public class UpdateCount : MonoBehaviour, IObserver
{
    private UILabel unitCount;

	// Use this for initialization
	void Start () 
    {
        unitCount = gameObject.GetComponent<UILabel>();

        Subscribe();
	}

    private void handleMessage(Telegram telegram)
    {
        if (telegram is InterfaceTelegram)
        {
            InterfaceTelegram interfaceTelegram = telegram as InterfaceTelegram;
            switch (interfaceTelegram._TargetText)
            {
                case TargetText.Ally:
                    if(unitCount.name == "AllyCount")
                        unitCount.text = interfaceTelegram.Text;
                    break;
                case TargetText.Enemy:
                    if(unitCount.name == "EnemyCount")
                        unitCount.text = interfaceTelegram.Text;
                    break;
                default:
                    break;
            }
        }
    }

    public void Subscribe()
    {
        MessageDispatcher.Instance.SendMessage += new MessageDispatcher.SendMessageHandler(handleMessage);
    }

    public void UnSubscribe()
    {
        MessageDispatcher.Instance.SendMessage -= new MessageDispatcher.SendMessageHandler(handleMessage);
    }

    public void SubscribeGlobal()
    {
        throw new System.NotImplementedException();
    }

    public void UnSubscribeGlobal()
    {
        throw new System.NotImplementedException();
    }
}
