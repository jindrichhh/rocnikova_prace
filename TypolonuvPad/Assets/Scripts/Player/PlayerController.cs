using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Singleton;

    public PlayerInputs Inputs;
    public PawnController Pawn;
    public InventoryController PlayerInventory;

    public HudPanelController Hud;

    public Vector3Int Coords;

    [SerializeField]
    GameObject DeathScreen;


    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;

        Pawn.Stats = new PawnStat();
        Pawn.Stats.HitPoints = new PawnStat.Stat(5, 20);
        Pawn.Stats.Defence = new PawnStat.Stat(5, 5);
        Pawn.Stats.Attack = new PawnStat.Stat(25, 10);
        Pawn.Stats.ActionPoints = new PawnStat.Stat(5, 1); 

        Pawn.Init("Player");

        //Pawn.Stats.TakeDamage(15, true);
        //Pawn.Stats.Leveling.AddExp(10000);

        Hud.RefreshData(Pawn.Stats);
    }

    //void Start()
    //{


    //}


    //void Update()
    //{

    //}

    private void FixedUpdate()
    {
        // death screen on player´s death
        if (Pawn.Stats.HitPoints.Current < 1)
            DeathScreen.gameObject.SetActive(true);
    }

}
