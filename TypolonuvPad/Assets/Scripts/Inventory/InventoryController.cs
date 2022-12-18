using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class InventoryController : MonoBehaviour
{

    public static int Found = 0;

    [SerializeField]
    InventorySlotController[] Slots;
    [SerializeField]
    InventorySlotController WeaponSlot;
    [SerializeField]
    InventorySlotController ArmorSlot;

    [SerializeField]
    TextMeshProUGUI BasicInfoText;

    [SerializeField]
    TextMeshProUGUI TooltipTitleText;
    [SerializeField]
    TextMeshProUGUI TooltipDescText;

    [SerializeField]
    TextMeshProUGUI InvSizeText;

    public PawnInventory ParentInventory;

    int Total;

    private void Start()
    {
        foreach (var slot in Slots) {

            slot.InvCtrl = this;
        }
        WeaponSlot.InvCtrl = this;
        ArmorSlot.InvCtrl = this;

        WeaponSlot.Type = InventoryItem.SlotType.Weapon;
        ArmorSlot.Type = InventoryItem.SlotType.Armor;
    }


    // Set text in help panel
    public void SetBasicInfoText(string text) {

        BasicInfoText.text = text;
    }

    // Fillup tooltip
    public void SetTooltipText(string title, string text)
    {
        TooltipTitleText.text = title;
        TooltipDescText.text = text;
    }

    // Sets size text
    public void SetInvSizeText(int total, int occupied) {

        Total = total;
        InvSizeText.text = string.Format("{0} / {1}", occupied, total);
    }

    // Adds item to inventory
    public void AddItem(InventoryItem item) {

        Found++;

        var freeslots = GetFreeGridSlots();
        if (freeslots.Count == 0) {

            throw new System.Exception("Inventory error");
        }

        var slot = freeslots.First();
        slot.AssignItem(item);

        SetInvSizeText(Total, Total - freeslots.Count + 1);
    }

    // Remove item from inventory
    public void RemoveItem(InventoryItem item) {

        var slot = Utils.CollectionExt.Exclude(Slots.ToList(), GetFreeGridSlots()).Where(x => x.Item == item).First();

        if (slot != null) {

            slot.RemoveItem();
        }
    }

    // Get all free slots
    public List<InventorySlotController> GetFreeGridSlots() {

        return Slots.Where(x => x.FreeSlot).ToList();
    }

    // Swap with weapon slot
    public void SwapWeapon(InventoryItem grid = null) {

        var freeslots = GetFreeGridSlots();
        if (grid == null) // from main slot to gird
        {
            // is free slot in grid
            if (freeslots.Count > 0)
            {

                freeslots.First().AssignItem(WeaponSlot.Item);
                WeaponSlot.RemoveItem();
            }

            SetInvSizeText(Total, Total - freeslots.Count + 1);
        }
        else
        { // from grid to main slot

            if (WeaponSlot.FreeSlot) // set item
            {
                WeaponSlot.AssignItem(grid);
                Slots.Where(x => x.Item == grid).First().RemoveItem();
                SetInvSizeText(Total, Total - freeslots.Count -1);
            }
            else // swap item
            {

                var item = WeaponSlot.Item;
                WeaponSlot.RemoveItem();
                WeaponSlot.AssignItem(grid);
                Slots.Where(x => x.Item == grid).First().RemoveItem();
                freeslots.First().AssignItem(item);
            }
        }
    }

    // Swap with armor slot
    public void SwapArmor(InventoryItem grid = null) {

        var freeslots = GetFreeGridSlots();
        if (grid == null) // from main slot to gird
        {
            // is free slot in grid
            if (freeslots.Count > 0)
            {

                freeslots.First().AssignItem(ArmorSlot.Item);
                ArmorSlot.RemoveItem();
            }

            SetInvSizeText(Total, Total - freeslots.Count + 1);
        }
        else
        { // from grid to main slot

            if (ArmorSlot.FreeSlot) // set item
            {
                ArmorSlot.AssignItem(grid);
                Slots.Where(x => x.Item == grid).First().RemoveItem();
                SetInvSizeText(Total, Total - freeslots.Count -1);
            }
            else // swap item
            {

                var item = ArmorSlot.Item;
                ArmorSlot.RemoveItem();
                ArmorSlot.AssignItem(grid);
                Slots.Where(x => x.Item == grid).First().RemoveItem();
                freeslots.First().AssignItem(item);
            }
        }
    }
}
