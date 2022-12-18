using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Utils {

    public static class CollectionExt
    {

        // Swaps 2 items in list
        public static IList<T> Swap<T>(this IList<T> list, T obja, T objb)
        {
            int a = list.IndexOf(obja);
            int b = list.IndexOf(objb);

            T tmp = list[a];
            list[a] = list[b];
            list[b] = tmp;
            return list;
        }

        // Excludes one list from master list
        public static List<T> Exclude<T>(List<T> master, List<T> excluded) {

            //return master.Where(i => !excluded.Any(e => i.Contains(e))).ToList();
            return master.Except(excluded).ToList();
        }


        // Get random item form list
        public static T GetRandomItem<T>(List<T> list)
        {
            if (list.Count == 0)
                throw new System.Exception("List is empty");

            var i = Random.Range(0, list.Count);
            return list[i];
        }
    }

}


