using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Reflection;
using System.ComponentModel;

namespace Utils {

    public static class EnumHelper {

        // Get all values of enumeration type, will ignore negatives if are defined
        public static List<T> GetValues<T>(bool ignore = false) {

            var a = System.Enum.GetValues(typeof(T));
            var list = a.OfType<T>().ToList();

            if (!ignore)
                return list;

            return list.Where(x => System.Convert.ToInt32(x) >= 0).ToList();
        }

        // Return enums as list of strings
        public static List<string> GetStrings<T>(bool ignore = false) {

            var list = GetValues<T>(ignore);

            return list.Select(x => x.ToString()).ToList();
        }

        // Return enum by string
        public static T ParseEnum<T>(string item, T def = default(T), bool is_int = false) where T : System.Enum
        {

            var list = GetValues<T>();
            var names = list.Select(x => x.ToString()).ToList();

            try
            {

                if (is_int) {

                    //item = ValueHelper.ConvertTo<T>(int.Parse(item)).ToString();
                    item = ((T)System.Enum.Parse(typeof(T), item, true)).ToString();
                }

                if (names.Contains(item))
                {

                    return list[names.IndexOf(item)];
                }


            }
            catch (System.Exception exc)
            {

                Debug.LogError(exc);
                //throw new System.Exception("Item not found in " + typeof(T) + " val: " + item);
            }

            return def;
        }

        // Returns description of object
        public static string GetDescription<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }
    }

}

