using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public enum SlotType { 
    
        Normal,
        Weapon,
        Armor,
    }

    public enum ItemType { 
    
        [Description("Zbran")]
        Weapon,
        [Description("Brneni")]
        Armor,
        [Description("Material")]
        Material,
        [Description("Pozivatelny predmet")]
        Consumable,
    }

    [System.Serializable]
    public class ItemEffect {

        [XmlAttribute("Base")]
        public int Base;
        [XmlAttribute("Max")]
        public int Max;
        [XmlAttribute("Mult")]
        public float Multiplier;

        public int BakedValue = 0;
        public int MinEffect = 0;
        public int MaxEffect = 0;

        // Initialize min-max range
        public void InitMinMax(int lvl) {

            MinEffect = (int)(Base * lvl * Multiplier);
            MaxEffect = (int)((Base + Max) * lvl * Multiplier);
        }

        // Initialize one-shot randomness
        public void InitBake(int lvl) {

            BakedValue = (int)(Base * lvl * Multiplier + Random.Range(0, Max));
        }

        // Take effect due to level and randomness
        public int TakeEffect(int lvl) {

            return Random.Range(MinEffect, MaxEffect);
        }


    }


    [XmlAttribute("Id")]
    public string Id;
    [XmlAttribute("ShowName")]
    public string ShowName;
    [XmlAttribute("Type")]
    public ItemType Type;

    [XmlElement("Damage")]
    public ItemEffect Damage;
    [XmlElement("Armor")]
    public ItemEffect Armor;
    [XmlElement("Heal")]
    public ItemEffect Heal;

    [XmlElement("Text")]
    public string Text;

    public int Level = 0;

    // Inits item
    public void Init(int level = 0) {

        Level = level;
        if (Level != 0 && Type != ItemType.Material) {

            if (Damage != null) {

                Damage.InitMinMax(Level);
            }

            if (Armor != null)
            {

                Armor.InitBake(Level);
            }

            if (Heal != null)
            {

                Heal.InitMinMax(Level);
            }
        }
    }

    // Deserialize item from file
    public static InventoryItem Deserialize(string path) {

        return Utils.FileOps.Deserialize<InventoryItem>(path);
    }
}
