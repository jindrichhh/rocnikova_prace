using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnController : MonoBehaviour
{

    [SerializeField]
    InventoryController InvCtrl;

    public PawnStat Stats;
    public PawnInventory Inventory;

    public string PawnName;
    public string PawnId;


    private void Awake()
    {
        Stats = new PawnStat();
        Inventory = new PawnInventory(PawnId, Stats, InvCtrl);
        
    }



}
