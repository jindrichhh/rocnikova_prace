using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlotController : MonoBehaviour
{

    readonly string NL = System.Environment.NewLine;

    [SerializeField]
    TextMeshProUGUI LevelText;

    [SerializeField]
    Image RenderingImage;

    public InventoryController InvCtrl;

    public InventoryItem.SlotType Type = InventoryItem.SlotType.Normal;
    public bool FreeSlot = true;
    public InventoryItem Item = null;
    public Sprite EmptySprite;

    MasterController Master;
    


    private void Awake()
    {
        bool lvl_show = Item != null;
        if (lvl_show)
            lvl_show = Item.Level > 0;

        LevelText.gameObject.SetActive(lvl_show);
        //EmptySprite = RenderingImage.sprite;
    }

    //void Start()
    //{

    //}


    //void Update()
    //{

    //}

    // Triggered on mouse enter
    public void MouseEnter() {

        if (!FreeSlot)
        {

            var dmg = Item.Damage;
            var armor = Item.Armor;
            var heal = Item.Heal;

            var text = string.Empty;
            text += Utils.EnumHelper.GetDescription(Item.Type);
            if (Item.Level > 0) {

                text += " urovne " + Item.Level;
            }

            text += NL;

            if (dmg != null)
            {

                text += string.Format("{0}: {1}-{2}{3}",
                    "Utok", dmg.MinEffect, dmg.MaxEffect, NL);
            }
            if (armor != null)
            {

                text += string.Format("{0}: {1}{2}",
                    "Obrana", armor.BakedValue, NL);
            }
            if (heal != null)
            {

                text += string.Format("{0}: {1}-{2}{3}",
                    "Leci", heal.MinEffect, heal.MaxEffect, NL);
            }

            text += NL;
            text += "Popis predmetu" + NL;
            text += Item.Text;

            InvCtrl.SetTooltipText(Item.ShowName, text);
        }
        else {

            InvCtrl.SetTooltipText("Prazdny slot", "");
        }
            
    }

    // Triggered on mouse exit
    public void MouseExit()
    {
        InvCtrl.SetTooltipText("", "");
    }

    // Assigns item to slot
    public void AssignItem(InventoryItem item) {

        Master = MasterController.Singleton;

        if (!FreeSlot) {

            throw new System.Exception("Inventory error, item assigment");
        }

        Item = item;
        FreeSlot = false;
        LevelText.text = item.Level + "";
        LevelText.gameObject.SetActive(Item.Level > 0);

        RenderingImage.sprite = Master.GetSprite(Item.Id);
    }

    // Removes item from inventory
    public void RemoveItem() {

        if (FreeSlot)
        {
            throw new System.Exception("Inventory error, item removement");
        }

        Item = null;
        FreeSlot = true;
        LevelText.gameObject.SetActive(false);
        RenderingImage.sprite = EmptySprite;

    }


    // Take action on click
    public void TakeAction() {

        if (FreeSlot)
            return;

        InvCtrl.ParentInventory.TakeAction(Item, Type == InventoryItem.SlotType.Normal);

        var player = PlayerController.Singleton;
        var att = player.Hud.AttributePanel.GetComponent<AttributePanelController>();
        var stats = player.Pawn.Stats;
        att.Combat.RefreshData(stats.HitPoints, stats.Defence, stats.Attack);
    }

}
