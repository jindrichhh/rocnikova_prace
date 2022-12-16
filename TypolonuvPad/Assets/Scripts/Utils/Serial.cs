using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;


namespace Utils {

    public static class Serial
    {

        // Deserialization of xml file with given type
        public static T Deserialize<T>(System.IO.TextReader stream)
        {

            try
            {

                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                object obj = deserializer.Deserialize(stream);
                stream.Close();

                return (T)obj;

            }
            catch (System.Exception e)
            {

                Debug.LogError(e.Message);
                Debug.LogError(e.GetBaseException());
            }

            throw new System.Exception("Problem here with deser.");
        }

    }
}


