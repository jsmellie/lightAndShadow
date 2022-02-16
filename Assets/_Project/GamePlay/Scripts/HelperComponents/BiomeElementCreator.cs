using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BiomeElementCreator : MonoBehaviour
{
    [SerializeField] private UnityEngine.Object _rootFolder;
    

    #if UNITY_EDITOR
    [ContextMenu("Generate Elements")]
    private void GenerateElements()
    {
        if (_rootFolder != null)
        {
            string rootPath = Application.dataPath + AssetDatabase.GetAssetPath(_rootFolder).Substring("Assets".Length);
            string[] paths = Directory.GetFiles(rootPath);
            
            if (paths != null && paths.Length > 0)
            {
                foreach(string path in paths)
                {
                    string databasePath = path.Substring(path.LastIndexOf("Assets"));
                    Debug.Log(databasePath);
                    if (Path.GetExtension(databasePath).EndsWith("meta"))
                    {
                        continue;
                    }
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(databasePath);

                    GameObject newObject = new GameObject("P" + sprite.name.Substring(1));
                    newObject.transform.parent = this.transform;
                    SpriteRenderer spriteRender = newObject.AddComponent<SpriteRenderer>();
                    spriteRender.sprite = sprite;
                    PolygonCollider2D polygonCollider = newObject.AddComponent<PolygonCollider2D>();
                }
            }
        }
    }
    #endif
}
