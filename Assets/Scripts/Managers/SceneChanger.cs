using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Wolfheat.Inputs;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance { get; private set; }

    public string GameNameString = "Test"; 
    public string ActiveGameScene = "Hideout"; 

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;


#if UNITY_EDITOR
        Resources.UnloadUnusedAssets();        
        CheckedForScenes();
#else      
            ChangeScene("StartMenu");            
            // Make game display in second monitor

#endif
    }


    private void CheckedForScenes()
    {
        
        Debug.Log("** Checking Scenes to Set active. **");
        if (SceneManager.GetSceneByName("StartMenu").IsValid() && SceneManager.GetSceneByName("StartMenu").isLoaded)
        {
            if (SceneManager.GetSceneByName(GameNameString).IsValid() && SceneManager.GetSceneByName(GameNameString).isLoaded)
            {
                Debug.Log("Both Start and DungeonII is loaded");
                // If both Menu and Dungeon is loaded unload the menu
                SceneManager.UnloadSceneAsync("StartMenu");
                if(!SceneManager.GetSceneByName(GameNameString).isLoaded)
                    SceneManager.LoadScene(GameNameString);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(GameNameString));
                return;
            }
            Debug.Log("Only Start is loaded");
            // If only Menu is loaded set it as active
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("StartMenu"));
            Debug.Log("  StartMenu is set as active.");
        }else if (SceneManager.GetSceneByName(GameNameString).IsValid() && SceneManager.GetSceneByName(GameNameString).isLoaded)
        {
            Debug.Log("Only Dungeon II is loaded");
            // If only Dungeon is loaded set it as active            
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(GameNameString));
            Debug.Log("  Dungeon is set as active.");
        }
        Resources.UnloadUnusedAssets();
    }

    /*
    public void ChangeScene(string name)
    {
         
        StartCoroutine(ChangeToSceneAdditive(name)); // Changing this scene to be the active one
    }

    private IEnumerator ChangeToSceneAdditive(string name)
    {
        if (transitionFromscene == "Managers") {
            Debug.Log("ERROR Trying to unload Managers, should never happen");
        }
        else if (SceneManager.GetSceneByName(transitionFromscene).isLoaded) {

            // Unload the previus scene
            var unloadingOperation = SceneManager.UnloadSceneAsync(transitionFromscene);

            // Wait
            while (!unloadingOperation.isDone) {
                yield return null;
            }

            Debug.Log("Unloading Scene " + transitionFromscene + " done");
        }

        yield return null;

        Debug.Log("Scene Change Step 1 ");

        // Load next Scene
        var loadingOperation = SceneManager.LoadSceneAsync(name, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);

        // Wait
        while (!loadingOperation.isDone) {
            yield return null;
            Debug.Log("Scene Change Step 2 ");
        }

        yield return null;


        Debug.Log("Scene Change Step 3 ");
        // Wait


        yield return new WaitForSeconds(0.2f);


        Debug.Log("Scene Change Step 5 ");

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));



    }

    */

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: "+ scene.name);
        SceneManager.SetActiveScene(scene);
    }

    public void ChangeScene(string name, bool additive = true, bool loadFromSaveFile = true)
    {
         
        StartCoroutine(ChangeToActive(name)); // Changing this scene to be the active one
    }

    private IEnumerator ChangeToActive(string name)
    {
                
        Debug.Log("Unloading ActiveScene: "+ ActiveGameScene);
        // Unload the previus scene
        yield return SceneManager.UnloadSceneAsync(ActiveGameScene);


        yield return new WaitForSeconds(0.1f);

        Debug.Log("Loading Scene: "+ name);
        // Load next Scene
        yield return SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

        ActiveGameScene = name;
        Debug.Log("ActiveScene Set to "+ ActiveGameScene);
    }

    private IEnumerator ChangeToActiveOLD(string name, bool additive)
    {        
        string unloadScene = "";
        // Get active scene so it can be unloaded?
        if (SceneManager.GetActiveScene().name == GameNameString)
            unloadScene = GameNameString;
        else if (SceneManager.GetActiveScene().name == "StartMenu")
            unloadScene = "StartMenu";

        yield return SceneManager.LoadSceneAsync(name, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        yield return null;

        if (SceneManager.GetSceneByName(name).IsValid())
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));

        // Unload the previus scene
        UnloadScene(unloadScene);
    }

    internal void UnloadScene(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded) {
            SceneManager.UnloadSceneAsync(sceneName);
            Resources.UnloadUnusedAssets();
        }
    }
}
