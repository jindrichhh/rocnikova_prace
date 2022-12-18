using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyController : MonoBehaviour
{

    public PawnController Pawn;

    [SerializeField]
    Button AttackBtn;
    [SerializeField]
    ProgressBar Health;

    [SerializeField]
    GameObject DeadOverlay;
    [SerializeField]
    Image EnemyImg;
    [SerializeField]
    TextMeshProUGUI TitleText;
    [SerializeField]
    TextMeshProUGUI HpText;
    [SerializeField]
    TextMeshProUGUI AttDefText;

    [SerializeField]
    GameObject InvModel;

    [SerializeField]
    GameObject InvContainer;

    MasterController Master;

    public bool Dead = false;


    private void Awake()
    {
        //Init("Boar", 3);
    }

    //void Start()
    //{

    //}


    //void Update()
    //{

    //}

    public void Init(string id, int lvl) {

        Master = MasterController.Singleton;

        Pawn.Init(id);

        var stats = Pawn.Stats;
        stats.Attack = new PawnStat.Stat(
            Random.Range(Pawn.Model.Attack.Base, Pawn.Model.Attack.Max), (int)(Pawn.Model.Attack.Max * Pawn.Model.Attack.Multiplier));
        stats.Defence = new PawnStat.Stat(
            Random.Range(Pawn.Model.Armor.Base, Pawn.Model.Armor.Max), (int)(Pawn.Model.Armor.Max * Pawn.Model.Armor.Multiplier));
        stats.HitPoints = new PawnStat.Stat(
            Random.Range(Pawn.Model.HitPoints.Base, Pawn.Model.HitPoints.Max), (int)(Pawn.Model.HitPoints.Max * Pawn.Model.HitPoints.Multiplier));
        

        stats.Leveling = new PawnStat.Stat.Level();
        while (stats.Leveling.CurrentLevel < lvl) {

            stats.Leveling.AddExp(100);
        }

        while (stats.Leveling.PointsRemaining > 0) {

            var rnd = Random.Range(0, 3);
            switch (rnd) {

                case 0:
                    stats.HitPoints.AddPoint();
                    break;

                case 1:
                    stats.Defence.AddPoint();
                    break;

                case 2:
                    stats.Attack.AddPoint();
                    break;

                default:
                    throw new System.Exception("Points error at enemy controller");
            }

            stats.Leveling.PointsRemaining--;
        }

        Pawn.Model.ExpReward = (int)(Random.Range(Pawn.Model.Exp.Base, Pawn.Model.Exp.Base + Pawn.Model.Exp.Max) * Pawn.Model.Exp.Multiplier);

        TitleText.text = string.Format("{0} úrovnì {1}", Pawn.PawnName, stats.Leveling.CurrentLevel);

        RefreshStats();
    }


    // Takes damage from player
    public void TakeDamage() {

        var dmg = PlayerController.Singleton.Pawn.Stats.Attack.Current;
        Dead = Pawn.Stats.TakeDamage(dmg);

        GameControlller.Singleton.Log(Pawn.PawnName + " zasažen za " + Pawn.Stats.LastDamage);

        RefreshStats();

        if (Dead)
            Death();

        CombatController.Singleton.EndTurn(PlayerController.Singleton.Pawn);
    }

    // Does damage to player
    public void DoDamage() {

        var stats = PlayerController.Singleton.Pawn.Stats;

        stats.TakeDamage(Pawn.Stats.Attack.Current);
        GameControlller.Singleton.Log(Pawn.PawnName + " zraòuje hráèe za " + stats.LastDamage);
        HudPanelController.Singleton.RefreshData(stats);
    }

    // Dies
    public void Death() {

        Pawn.Dead = true;
        GameControlller.Kills++;

        GameControlller.Singleton.Log(Pawn.PawnName + " zemøel na následky zranìní");
        ToggleAttackButton(false);
        DeadOverlay.SetActive(true);

        var last_lvl = PlayerController.Singleton.Pawn.Stats.Leveling.CurrentLevel;

        PlayerController.Singleton.Pawn.Stats.Leveling.AddExp(Pawn.Model.ExpReward);
        GameControlller.Singleton.PosLog("Zisk " + Pawn.Model.ExpReward + " zkušeností");

        if (PlayerController.Singleton.Pawn.Stats.Leveling.CurrentLevel != last_lvl) {

            GameControlller.Singleton.PosLog("Postup na úroveò " + PlayerController.Singleton.Pawn.Stats.Leveling.CurrentLevel);

            var lvl_diff = PlayerController.Singleton.Pawn.Stats.Leveling.CurrentLevel - last_lvl;
            for (int i = 0; i < lvl_diff; i++) {

                PlayerController.Singleton.Pawn.Stats.ActionPoints.AddPoint();
            }
            
        }
    }

    // Set pawn image
    public void SetImage(string key) {

        EnemyImg.sprite = MasterController.Singleton.GetSprite(key);
    }

    // Refresh stats of pawn
    private void RefreshStats() {

        HpText.text = Pawn.Stats.HitPoints.Current + "";
        AttDefText.text = string.Format("{0}/{1}", Pawn.Stats.Attack.Current, Pawn.Stats.Defence.Current);

        Health.SetProgress(Pawn.Stats.HitPoints.Current, Pawn.Stats.HitPoints.Capacity());
    }

    // Sets inventory visuals
    public void SetupInventory(PawnController pawn) {

        foreach (var item in pawn.Inventory.GetItems()) {

            var inv_item = Instantiate(InvModel);
            inv_item.SetActive(true);
            inv_item.transform.SetParent(InvContainer.transform);

            var image = inv_item.GetComponent<Image>();
            image.sprite = Master.GetSprite(item.Id);

            // equip weapon
            if (item.Type == InventoryItem.ItemType.Weapon) {

                pawn.Inventory.TakeAction_Weapon(item, true);
            }

            // equip armor
            if (item.Type == InventoryItem.ItemType.Armor)
            {
                pawn.Inventory.TakeAction_Armor(item, true);
            }
        }
    }

    // ON/OFF Attack button
    public void ToggleAttackButton(bool show) {

        AttackBtn.gameObject.SetActive(show);
    }
}
