using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Utils {

    public static class FileOps
    {
        // Check if path is valid - file
        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        // Deserialize file with type
        public static T Deserialize<T>(string path) {

            if (!Exists(path))
                throw new System.Exception("File not exists: " + path);

            try
            {

                TextReader reader = new StreamReader(path);
                return Utils.Serial.Deserialize<T>(reader);
            }
            catch (System.Exception e)
            {

                Debug.LogError(e.Message);
                Debug.LogError(e.GetBaseException());
            }

            throw new System.Exception("File deser. error");
        }


        // Get all resource file pahts recursively
        public static string[] ResourcePathsRec(string dir, bool resource = true)
        {

            dir = Path.Combine(Application.dataPath, "Resources", dir);

            List<string> paths = new List<string>();
            paths.Add(dir);

            // recuresively calls getting subdirs
            GetSubdirs(dir, paths);

            List<string> files = new List<string>();
            foreach (var p in paths)
            {

                files.AddRange(ResourcePaths(p, resource));
            }

            return files.ToArray();
        }

        // Get all resource file paths
        public static string[] ResourcePaths(string subdir, bool resource = false)
        {

            string path = Path.Combine(Application.dataPath, "Resources", subdir);

            var files = Directory.GetFiles(path);
            var list = new List<string>();
            foreach (var f in files)
            {

                var ext = Path.GetExtension(f);
                if (ext != ".meta")
                {

                    if (resource)
                    {

                        var resource_path = GetPathWithoutExt(f);
                        var asset_path = Application.dataPath + "\\Resources\\";
                        resource_path = resource_path.Replace(asset_path, "");

                        list.Add(resource_path);
                        continue;
                    }

                    list.Add(f);
                }
            }

            return list.ToArray();
        }

        // Get all subdirs
        public static void GetSubdirs(string dir, List<string> subdirs)
        {

            foreach (var d in Directory.GetDirectories(dir))
            {

                subdirs.Add(d);
                GetSubdirs(d, subdirs);
            }
        }

        // Get path without file extention
        public static string GetPathWithoutExt(string path)
        {

            return Path.ChangeExtension(path, null);
        }
    }
}


