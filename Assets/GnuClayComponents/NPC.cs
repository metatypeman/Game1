using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEditor;
using System.IO;
using GnuClay;

[AddComponentMenu("GnuClay/NPC")]
//[HelpURL("http://example.com/docs/MyComponent.html")]
public class NPC : MonoBehaviour
{
    public TextAsset App;

    private Engine mEngine;

    // Use this for initialization
    void Start()
    {
        if(App != null)
        {
            var path = AssetDatabase.GetAssetPath(App);

#if DEBUG
            Debug.Log($"Start path = {path}");
#endif
            var localPath = Directory.GetParent(UnityEngine.Windows.Directory.localFolder).FullName;

#if DEBUG
            Debug.Log($"localPath = {localPath}");
#endif
            var fullPath = Path.Combine(localPath, path);

#if DEBUG
            Debug.Log($"fullPath = {fullPath}");
#endif

            mEngine = new Engine(new EngineOptions() {
                AppFileName = fullPath
            });
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
