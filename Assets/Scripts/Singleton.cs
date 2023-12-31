using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance {
        get {
            if (_instance == null){
                GameObject obj = new()
                {
                    name = typeof(T).Name,
                    // hideFlags = HideFlags.HideAndDontSave
                };
                _instance = obj.AddComponent<T>();
            }
            return _instance;
        }
    }

    private void OnDestroy() {
        if (_instance == this) {
            _instance = null;
        }
    }
}

public class SingletonPersistent<T> : MonoBehaviour where T : Component
{ 
    private static T _instance;

    public static T Instance {
        get {
            if (_instance == null){
                Scene activeScene = SceneManager.GetActiveScene();
                Debug.Log("Setting Managers as Active Scene");
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("Managers"));
                GameObject obj = new()
                {
                    name = typeof(T).Name
                    // hideFlags = HideFlags.HideAndDontSave
                };
                _instance = obj.AddComponent<T>();
                Debug.LogWarning("Manager created: " + obj.name + " on " + obj.scene.name);
                Debug.Log("Setting (" + activeScene.name + ") as Active Scene");
                SceneManager.SetActiveScene(activeScene);
            }
            return _instance;
        }
    }

    private void OnDestroy() {
        if (_instance == this) {
            _instance = null;
        }
    }

}