using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributePanelController : PanelController
{

    public ExpPanelController Exp;
    public CombatAttributesPanelController Combat;


    //private void Awake()
    //{
        
    //}

    //void Start()
    //{
        
    //}

    //void Update()
    //{
        
    //}


    // Toggles panel and refresh data
    public override void TogglePanel()
    {
        base.TogglePanel();
        RefreshData(Player.Pawn.Stats);
    }


    // Refresh UI
    public void RefreshData(PawnStat stats) {

        Exp.RefreshData(stats.Leveling);
        Combat.RefreshData(stats.HitPoints, stats.Defence, stats.Attack);
    }
}
