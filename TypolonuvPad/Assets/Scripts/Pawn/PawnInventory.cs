using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PawnInventory 
{
    const int INV_SIZE = 18;

    InventoryController InvCtrl;
    MasterController Master;

    PawnStat Stats;

    List<InventoryItem> Items; // 20
    InventoryItem Weapon, Armor;


    public PawnInventory(PawnSetup model, PawnStat stats, InventoryController gui = null) {

        Stats = stats;
        InvCtrl = gui;
        Master = MasterController.Singleton;

        Items = new List<InventoryItem>(INV_SIZE + 2);

        foreach (var i in model.Items) {

            Debug.Log("item i: " + i.Id);
            i.RollChances();
            if (i.Confirmed) {

                var lvl = Random.Range(0.4f, 1.0f) * stats.Leveling.CurrentLevel;
                lvl += 1;
                var spawn = Master.SpawnItem(i.Id);
                if (spawn.Type != InventoryItem.ItemType.Material) {

                    spawn.Init((int)lvl);
                }
                
                AddItem(spawn);

                if (i.Equip) {

                    TakeAction(spawn, true);
                }
            }
        }

        //var weapon = Master.SpawnItem("WoodenSword", Random.Range(1,3));
        //AddItem(weapon);

        //var armor = Master.SpawnItem("LeatherArmor", Random.Range(1, 3));
        //AddItem(armor);

        //AddItem(Master.SpawnItem("Meat"));
        //AddItem(Master.SpawnItem("Firewood"));
        //AddItem(Master.SpawnItem("Firewood"));
        //AddItem(Master.SpawnItem("Potion", Random.Range(1, 3)));

        
        if (InvCtrl)
        {

            gui.ParentInventory = this;

            InvCtrl.SetBasicInfoText("Levym tlacitkem mysi pouzijete nebo vymenite predmety");
            InvCtrl.SetInvSizeText(INV_SIZE, 0);
            InvCtrl.SetTooltipText("", "");
        }
    }


    // return all items
    public InventoryItem[] GetItems() {

        return Items.ToArray();
    }

    // Finds first item of id
    public InventoryItem FindItem(string id) {

        return Items.Where(x => x.Id.ToLower() == id.ToLower()).FirstOrDefault();
    }

    // Add item to inventory
    public bool AddItem(InventoryItem item) {

        if (Items.Capacity > Items.Count) {

            if (InvCtrl) {

                InvCtrl.AddItem(item);
            }

            Items.Add(item);
            return true;
        }

        return false;
    }

    // Remove item from inventory
    public void RemoveItem(InventoryItem item) {

        Items.Remove(item);
        if (InvCtrl)
            InvCtrl.RemoveItem(item);
    }

    // Manipulation with inventory
    public void TakeAction(InventoryItem item, bool normal) {

        switch (item.Type) {

            case InventoryItem.ItemType.Armor:
                TakeAction_Armor(item, normal);
                break;

            case InventoryItem.ItemType.Consumable:
                TakeAction_Consum(item);
                break;

            case InventoryItem.ItemType.Weapon:
                TakeAction_Weapon(item, normal);
                break;

            case InventoryItem.ItemType.Material:
                break;

            default:
                Debug.LogWarning("Unimplemented type click " + item.Type.ToString());
                break;
        }

    }

    // Clicked on weapon in inventory
    public void TakeAction_Weapon(InventoryItem item, bool from_grid) {

        Stats.Attack.RemoveBonus(Weapon);
        Stats.Defence.RemoveBonus(Weapon);
        Weapon = item;
        if (item.Damage != null)
            Stats.Attack.AddBonus(Weapon, item.Damage.BakedValue);
        if (item.Armor != null)
            Stats.Defence.AddBonus(Weapon, item.Armor.BakedValue);

        if (!InvCtrl) { // no gui

            return;
        }
            

        if (from_grid)
        {
            InvCtrl.SwapWeapon(item);
        }
        else {

            Stats.Attack.RemoveBonus(Weapon);
            Stats.Defence.RemoveBonus(Weapon);
            InvCtrl.SwapWeapon();
            Weapon = null;
        }
    }

    // Clicked on armor in inventory
    public void TakeAction_Armor(InventoryItem item, bool from_grid)
    {
        Stats.Defence.RemoveBonus(Armor);
        Stats.Attack.RemoveBonus(Armor);
        Armor = item;
        if (item.Armor != null)
            Stats.Defence.AddBonus(Armor, item.Armor.BakedValue);
        if(item.Damage != null)
            Stats.Attack.AddBonus(Armor, item.Damage.BakedValue);

        if (!InvCtrl)
        { // no gui

            return;
        }


        if (from_grid)
        {
            InvCtrl.SwapArmor(item);
        }
        else
        {
            Stats.Defence.RemoveBonus(Armor);
            Stats.Attack.RemoveBonus(Armor);
            InvCtrl.SwapArmor();
            Armor = null;
        }
    }

    // Clicked on consumable in inventory
    public void TakeAction_Consum(InventoryItem item)
    {
        Stats.Heal(item.Heal.TakeEffect(item.Level));

        if (!InvCtrl)
        { // no gui

            return;
        }

        RemoveItem(item);

    }

}
