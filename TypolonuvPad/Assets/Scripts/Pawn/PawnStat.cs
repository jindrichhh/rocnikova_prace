using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PawnStat {

    public class Stat
    {
        public class Level
        {

            const float LVL_DIFF_RATIO = 2.3f;
            const int BASE_EXP = 100;

            public int CurrentLevel = 1;
            public int PointsRemaining = 5;
            public int TotalExpPoints; // total amount of exp
            public int ExpPoints; // current points between levels
            public int ExpNeeded; // to next level
            public int NextLevel; // current to next level

            public Level()
            {

                ExpNeeded = BASE_EXP;
                NextLevel = ExpNeeded;
                //AddExp(Random.Range(20, 80));
            }


            // Add exp recurs.
            public void AddExp(int exp)
            {

                ExpPoints += exp;
                TotalExpPoints += exp;
                NextLevel = ExpNeeded - ExpPoints;

                if (ExpPoints >= ExpNeeded)
                {

                    var overflow = ExpPoints - ExpNeeded;

                    ExpNeeded = (int)(LVL_DIFF_RATIO * ExpNeeded);
                    //Debug.Log(ExpNeeded);

                    AddLevel();
                    AddExp(overflow);
                }

            }

            // New level
            private void AddLevel()
            {

                CurrentLevel++;
                PointsRemaining += 5;

                ExpPoints = 0;
            }
        }


        public int Base;
        public int Points = 1;
        public int PointStrength;
        public int Bonus;
        public int Current;

        private Dictionary<object, int> BonusEffects;

        public Stat(int b, int ps)
        {

            Base = b;
            PointStrength = ps;
            Current = Capacity();

            BonusEffects = new Dictionary<object, int>();
        }


        // Calc capacity of stat
        public int Capacity()
        {
            return Base + Bonus + Points * PointStrength;
        }

        // Add to current val (current / capacity)
        public void Add(int val)
        {

            IsZero(val);

            //if(Current == Capacity())

        }

        // Check if stat is zero
        public bool IsZero(int change)
        {

            Current = Mathf.Clamp(Current + change, 0, Capacity());

            return Current == 0;
        }

        // Add skill point to stat
        public void AddPoint()
        {

            var diff = Capacity() - Current;
            Points++;
            Current = Capacity() - diff;
        }

        // Register bonus value and source
        public void AddBonus(object o, int val)
        {

            if (o == null)
                return;

            if (!BonusEffects.ContainsKey(o))
            {

                BonusEffects.Add(o, val);
                Bonus += val;
            }
        }

        // Remove registered bonus value
        public void RemoveBonus(object o)
        {
            if (o == null)
                return;

            if (BonusEffects.ContainsKey(o))
            {
                Bonus -= BonusEffects[o];
                BonusEffects.Remove(o);
            }
        }

        // Renew to capacity
        public void Renew()
        {

            Current = Capacity();
        }
    }


    public Stat.Level Leveling;

    public Stat HitPoints;
    public Stat Defence;
    public Stat Attack;

    public Stat ActionPoints;

    public PawnStat()
    {
        Leveling = new Stat.Level();
    }



    public void Heal(int hp)
    {
        var lasthp = HitPoints.Current;
        HitPoints.Add(hp);
        GameControlller.Singleton.PosLog("Léèení: +" + (HitPoints.Current - lasthp));
    }

    public void Heal(float perc)
    {

        var hp = HitPoints.Capacity() * perc;
        Heal((int)hp);
    }

    public void TakeDamage(int damage, bool ignore_armor = false) {

        if (!ignore_armor) {

            damage -= Defence.Capacity();
            damage = Mathf.Clamp(damage, 5, 200);
        }

        HitPoints.Add(-damage);
    }  
}


