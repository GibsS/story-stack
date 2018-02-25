using UnityEngine;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;

public class Serializer<X> where X : class {

    string path;

    public Serializer(string name) {
        path = Application.persistentDataPath + "/" + name + ".dat";
    }

    public static X Load(string name) {
        return new Serializer<X>(name).Load();
    }
    public static void Save(string name, X x) {
        new Serializer<X>(name).Save(x);
    }

    public X Load() {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);

        X element = null;

        try {
            element = bf.Deserialize(file) as X;
        } catch {
            Debug.LogError("[CACHE - " + path + "] Failed to load cache.");
        }

        file.Close();
        return element;
    }

    public void Save(X element) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        bf.Serialize(file, element);
        file.Close();
    }

    public void ClearCache() {
        File.Delete(path);
    }
}
