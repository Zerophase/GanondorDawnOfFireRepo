using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameController : Director 
{
    private List<UnitMediator> unitMediators = new List<UnitMediator>();
    public List<UnitMediator> UnitMediators { get { return unitMediators; } }

    private Unit[] units;
    private UnitMediator[] unitMediatorsToAdd;

    private ForwardDirection playerForward;

    [HideInInspector]
    public float TimeSinceCollision;

    void Awake()
    {
        if (Teams.Instance.Team.Count > 0)
            Teams.Instance.Clear();
    }

	void Start ()
    {
         unitMediatorsToAdd = GameObject.FindObjectsOfType<UnitMediator>();
         units = GameObject.FindObjectsOfType<Unit>();
         for (int i = 0; i < unitMediatorsToAdd.Length; i++)
         {
             unitMediatorsToAdd[i].Init(this, units[i]);

             unitMediators.Add(unitMediatorsToAdd[i]);
         }

         Debug.Log("Units on Screen = " + unitMediators.Count);
         playerForward = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<ForwardDirection>();

         subscribe();
    }
	
	void Update () 
    {
        foreach (UnitMediator enemy in unitMediators)
        {
            if (playerForward.Hit().collider == enemy.collider)
            {
                //Debug.Log("Enemy ID = " + enemy.ID);
                enemy.Picked();
                TimeSinceCollision = 0;
            }
        }

        if (playerForward.Hit().collider != null)
        {
            if (playerForward.Hit().collider.GetComponent<UnitMediator>() == null)
            {
                waitToDeselect();
            }
        }
        else if (playerForward.Hit().collider == null)
        {
            waitToDeselect();
        }

        if ((Levels)Application.loadedLevel == Levels.VILLAGELEVEL)
        {
            if (Teams.Instance.Team[Allegiance.ALLY].Count == 0 || Teams.Instance.Team[Allegiance.ENEMY].Count == 0
                || !Teams.Instance.Team[Allegiance.ALLY].OfType<Player>().Any())
            {
                Unit.ResetID();
                UnitGroupManager.ResetKey();
                UnitGroupManager.Instance.Reset();
                Application.LoadLevel((int)Levels.AFTERACTION);
            }
        }
	}

    private void waitToDeselect()
    {
        TimeSinceCollision += Time.deltaTime;
        if (TimeSinceCollision > .5f)
        {
            Picker.Instance.UpdateSpherePos();
            TimeSinceCollision = 0;
        }
    }

    public void HandleMessage(Telegram telegram)
    {
        if (telegram is MediatorTelegram)
        {
            MediatorTelegram mediatorTelegram = telegram as MediatorTelegram;

            unitMediators.Remove(mediatorTelegram.Sender);
        }
    }

    private void subscribe()
    {
        MessageDispatcher.Instance.SendMessage += new MessageDispatcher.SendMessageHandler(HandleMessage);
    }
}
