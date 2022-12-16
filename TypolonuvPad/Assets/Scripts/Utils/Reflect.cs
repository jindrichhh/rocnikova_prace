using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Utils {

    public static class Reflect
    {
        // Clone object (memory copy -> model vs new instance)
        public static T Clone<T>(object o)
        {
            object clone = null;
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, o);
                stream.Seek(0, SeekOrigin.Begin);
                clone = formatter.Deserialize(stream);
            }

            return (T)clone;
        }
    }
}

