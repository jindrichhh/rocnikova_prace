using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnController : MonoBehaviour
{

    [SerializeField]
    InventoryController InvCtrl;

    public PawnStat Stats;
    public PawnInventory Inventory;

    public PawnSetup Model;
    public string PawnName;
    public string PawnId;
    public bool Dead = false;
    public bool IsPlayer;


    // Inits pawn by id 
    public void Init(string id) {

        Model = MasterController.Singleton.Library.Get<PawnSetup>(AssetLibrary.LibraryType.Pawns, id);

        if(Model == null)
            throw new System.Exception("Unknown PAWN id: " + id);

        PawnId = id;
        PawnName = Model.ShowName;
        IsPlayer = id == "Player";

        if (Stats == null)
            Stats = new PawnStat();
        Inventory = new PawnInventory(Model, Stats, InvCtrl);

    }

}
