using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLibrary : MonoBehaviour
{

    public enum LibraryType { 
    
        Items,
        Graphics,
    }


    public class Library {

        public bool Loaded = false;
        public LibraryType LibType;
        public System.Type Type;
        public string[] Paths;

        Dictionary<string, object> Objects;

        public Library(LibraryType lt) {

            LibType = lt;
            Type = AssetLibrary.ByLt(lt);
        }

        // Load objects to library
        public IEnumerator LoadAsync() {

            Loaded = false;

            Objects = new Dictionary<string, object>();
            foreach (var path in Paths)
            {
                ResourceRequest rr;
                object o;
                Object asset;
                switch (LibType) {

                    case LibraryType.Items:

                        o = InventoryItem.Deserialize(path);
                        var item = (InventoryItem)o;
                        if (!Objects.ContainsKey(item.Id)) {

                            Objects.Add(item.Id, item);
                        }

                        //Debug.Log("Resources: Added item: " + item.Id);
                        break;

                    case LibraryType.Graphics:

                        rr = Resources.LoadAsync<Sprite>(path);
                        while (!rr.isDone) {

                            yield return null;
                        }

                        asset = rr.asset;
                        if (asset == null)
                            continue;

                        Objects.Add(asset.name, asset as Sprite);

                        //Debug.Log("Resources: Added graphics: " + asset.name);
                        break;

                    default:
                        continue;
                }

                yield return null;
            }

            Loaded = true;
            yield return null;
        }

        // Get object from library
        public T Get<T>(string key) {

            try {

                if (Objects.TryGetValue(key, out object o))
                {
                    var cnt = o.GetType().GetConstructors();
                    if (cnt.Length > 0) // classic object
                    {
                        return Utils.Reflect.Clone<T>(o);
                    }
                    else { // Unity object

                        var inst = GameObject.Instantiate(o as Object);
                        return (T)System.Convert.ChangeType(inst, typeof(T));
                    }

                }

            } catch (System.Exception e) {

                Debug.LogError(e.Message);
                throw e;
            }

            throw new System.Exception("Non-existing key: " + key);
        }
    }



    public bool Ready = false;
    public Dictionary<LibraryType, Library> Lib;


    // Load resource data to libraries
    public void LoadAsync() {

        if (!Ready) {

            Lib = new Dictionary<LibraryType, Library>();

            InitLib(LibraryType.Items, 
                Utils.FileOps.ResourcePaths("Items"));
            InitLib(LibraryType.Graphics,
                Utils.FileOps.ResourcePathsRec("Graphics"));
        }

        StartCoroutine(LoadAssets());
    }

    // Creates instance of object in library
    public T Get<T>(LibraryType lt, string key) {

        return Lib[lt].Get<T>(key);
    }

    // Inits basic things in library
    private void InitLib(LibraryType lt, string[] paths) {

        Lib.Add(lt, new Library(lt) { Paths = paths });
    }

    // Start loading assets to library
    private IEnumerator LoadAssets() {

        foreach (var lib in Lib.Values) {

            StartCoroutine(lib.LoadAsync());
            while (!lib.Loaded) {

                yield return null;
            }
        }

        Ready = true;
        yield return null;
    }


    // Pair type of library with object type
    public static System.Type ByLt(LibraryType lt) {

        switch (lt) {

            case LibraryType.Items:
                return typeof(InventoryItem);

            case LibraryType.Graphics:
                return typeof(Sprite);

            default:
                return typeof(object);
        }
    }
}
