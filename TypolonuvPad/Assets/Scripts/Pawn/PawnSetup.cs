using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class PawnSetup
{

    public enum EnemyType {

        Player,
        Wild,
        Roman,
    }

    public enum EnemyRank {

        Player,
        Pawn,
        Elite,
        Boss,
    }


    [System.Serializable]
    public class Stat {

        [XmlAttribute("Base")]
        public int Base;
        [XmlAttribute("Max")]
        public int Max;
        [XmlAttribute("Mult")]
        public float Multiplier;
    }

    [System.Serializable]
    public class Item {

        [XmlAttribute("Id")]
        public string Id;
        [XmlAttribute("Poss")]
        public float PossesionChance;
        [XmlAttribute("Equip")]
        public float EquippedChance;
        [XmlAttribute("Drop")]
        public float DropChance;


        public bool Confirmed;
        public bool Equip;

        public void RollChances() {

            Confirmed = Random.Range(0.0f, 1.0f) < PossesionChance;
            if (Confirmed) {

                Equip = Random.Range(0.0f, 1.0f) < EquippedChance;
            }
        }
    }


    [XmlElement("Hp")]
    public Stat HitPoints;
    [XmlElement("Attack")]
    public Stat Attack;
    [XmlElement("Armor")]
    public Stat Armor;

    [XmlElement("Exp")]
    public Stat Exp;

    [XmlElement("Item")]
    public List<Item> Items;

    [XmlAttribute("Id")]
    public string Id;
    [XmlAttribute("ShowName")]
    public string ShowName;
    [XmlAttribute("Type")]
    public int TypeInt;
    [XmlAttribute("Rank")]
    public int RankInt;
    [XmlAttribute("Model")]
    public string Model;

    public EnemyRank Rank;
    public EnemyType Type;
    public int ExpReward;

    public PawnSetup() {

        
    }

    // Post-processing
    private void Init() {

        Rank = (EnemyRank)RankInt;
        Type = (EnemyType)TypeInt;
    }

    // Deserialize from file
    public static PawnSetup Deserialize(string path)
    {

        var obj = Utils.FileOps.Deserialize<PawnSetup>(path);
        obj.Init();

        return obj;
    }
}
