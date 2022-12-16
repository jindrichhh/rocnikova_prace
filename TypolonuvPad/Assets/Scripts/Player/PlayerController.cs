using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Singleton;

    public PlayerInputs Inputs;
    public PawnController Pawn;

    public HudPanelController Hud;

    public Vector3Int Coords;
    


    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;

        Pawn.Stats.HitPoints = new PawnStat.Stat(5, 20);
        Pawn.Stats.Defence = new PawnStat.Stat(5, 5);
        Pawn.Stats.Attack = new PawnStat.Stat(25, 10);
        Pawn.Stats.ActionPoints = new PawnStat.Stat(5, 1);

        Pawn.Stats.TakeDamage(15, true);
    }

    //void Start()
    //{


    //}


    //void Update()
    //{

    //}


}
