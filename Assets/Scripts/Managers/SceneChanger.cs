using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance { get; private set; }

    public string StartLevelName = "Hideout";

    public string ActiveGameScene = "Hideout";
    public string SceneToLoad = "Hideout";
    public string StartMenu = "StartMenu";
    public string Managers = "Managers";
    public string[] ScenesPriorityActivation = {"Louvre","BuildSite", "Office","Hideout","StartMenu"};

    [SerializeField] private GameObject playerObjectAlsoIngamCamera; 
    [SerializeField] private GameObject InGameUI; 
    [SerializeField] private GameObject InGameEventSystem;

    public static Action SceneChanged;

    private void Start()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;


#if UNITY_EDITOR
        Resources.UnloadUnusedAssets();
        CheckedForScenes();
#else      
            StartCoroutine(InitScenes());            
            // Make game display in second monitor

#endif
    }


    public IEnumerator InitScenes()
    {
        yield return null;
        
        Scene managers = SceneManager.GetSceneByName("Managers");

        if (managers == null || !managers.isLoaded)
            yield return SceneManager.LoadSceneAsync(managers.name, LoadSceneMode.Single);
        
        yield return null;

        yield return SceneManager.LoadSceneAsync(StartMenu, LoadSceneMode.Additive);

        yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(StartMenu));

        Debug.Log("InitScenes, runs on Builds not unity play mode");

        UpdateActiveSceneUIAndCamera("StartMenu");

        // Store the name of the Scene to change Into
        ActiveGameScene = StartMenu;
    }

    private void CheckedForScenes()
    {
        bool UnloadRest = false;
        // Editor version of starting the game - Check for the highest priority scene that is open and make that the active one
        foreach(var sceneName in ScenesPriorityActivation) {
            if(SceneManager.GetSceneByName(sceneName).IsValid() && SceneManager.GetSceneByName(sceneName).isLoaded) {
                Debug.Log("Scenemanagement: Setting "+sceneName+" as Active");
                if (!UnloadRest) {
                    UnloadRest = true;
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                    UpdateActiveSceneUIAndCamera(sceneName);
                }
                else {
                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
                }
            }
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


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        //SceneManager.sceneUnloaded += OnSceneUnloaded;

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        //SceneManager.sceneUnloaded -= OnSceneUnloaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Handles Cameras and UI Visibility
        bool showIngame = scene.name != "StartMenu";

        Debug.Log("Loaded Scene "+scene.name+" Show the Ingame UI and Camera: "+showIngame);
        // Hide the In-Game Camera And UI when in the StartMenu
        InGameCamera.SetActive(showIngame);
        InGameUI.SetActive(showIngame);

    }

    */

    private bool restartedGame = false;
    public void ChangeScene(string name, bool additive = true, bool restartTimer = false, bool useTransitionDarkening = true)
    {
        // Store the name of the Scene to change Into
        ActiveGameScene = SceneManager.GetActiveScene().name;

        if (name != StartMenu) {
            // Do not save when exiting game
            SaveScene(ActiveGameScene);
        }
        PoliceTimer.Instance?.Reset();

        SceneToLoad = name;
        Debug.Log("Scene: Unloading Scene: " + ActiveGameScene +" And Loading Scene "+SceneToLoad);
        //{"Office","Market","Buildsite","Louvre" }; 
        // Do the Darkening?

        restartedGame = restartTimer;

        if (useTransitionDarkening && ActiveGameScene != StartMenu)
            TransitionScreen.Instance.Darken(ChangeSceneWhenDark, 0.6f);
        else
            StartCoroutine(ChangeSceneCO());
    }
    public void ChangeSceneWhenDark()
    {
        StartCoroutine(ChangeSceneCO());
    }

    private void SaveScene(string activeGameScene)
    {
        Debug.Log("Saving state of scene "+activeGameScene);
        SceneStateLoader.Instance.Save(activeGameScene);
    }
    
    private void LoadSceneState(string sceneName)
    {
        Debug.Log("Load state of scene "+ sceneName);
        bool didLoad = SceneStateLoader.Instance.TryLoad(sceneName);
        Debug.Log("Loaded state " + sceneName +": " + didLoad);

    }

    private IEnumerator ChangeSceneCO()
    {
        // Short Wait
        yield return null;

        bool comingFromStartMenu = ActiveGameScene == StartMenu;
        bool comingFromManagers = ActiveGameScene == Managers;

        
        // Unload
        Debug.Log("Scene: Unloading Scene: " + ActiveGameScene);
        if(!comingFromManagers) // Never Unload Managers
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(ActiveGameScene));
        
        yield return null;

        // Load the Player, Camera, UI and Eventlistener
        UpdateActiveSceneUIAndCamera(SceneToLoad);

        Debug.Log("Scene: Loading Scene: " + SceneToLoad);
        yield return SceneManager.LoadSceneAsync(SceneToLoad, LoadSceneMode.Additive);

        PlayerController.Instance?.FindAndPlaceAtLevelsStartPosition();

        yield return null;

        Scene scene = SceneManager.GetSceneByName(SceneToLoad);

        // Load state Data
        if(!comingFromStartMenu)
            LoadSceneState(SceneToLoad);

        Debug.Log("Scene: Activating Scene: " + SceneToLoad + " Valid: " + scene.IsValid() + " Loaded: " + scene.isLoaded);
        
        SceneManager.SetActiveScene(scene);
        /*
        if (restartedGame) {
            Stats.Instance.Restart();
            restartedGame = false;
        }*/



        yield return null;
        

        SceneChanged?.Invoke();
    }

    private void UpdateActiveSceneUIAndCamera(string sceneToLoad)
    {
        if (sceneToLoad == "Managers") {
            Debug.Log("Loaded Scene " + sceneToLoad);
            return;
        }

        ActiveGameScene = SceneToLoad;

        bool showIngame = sceneToLoad != "StartMenu";

        
        Debug.Log("Loaded Scene " + sceneToLoad + " Show the Ingame UI and Camera: " + showIngame);
        
        // Show Player Gameobject with Ingame Camera  (Hidden while in Start Menu)
        playerObjectAlsoIngamCamera.SetActive(showIngame);

        // Show UI
        InGameUI.SetActive(showIngame);

        // Enable Eventlistener - To interact with new UI
        InGameEventSystem.SetActive(showIngame);
    }

    /*
    private void OnSceneUnloaded(UnityEngine.SceneManagement.Scene scene)
    {

        // We Now Have it Unloaded, Load the New Scene after:
        Debug.Log("Scene: Scene Unloaded: " + scene.name);
        StartCoroutine(DelayedLoad());
    }

    private IEnumerator DelayedLoad()
    {
        // Short Wait
        yield return null;

        // Unload
        Debug.Log("Scene: Loading Scene: " + SceneToLoad);
        SceneManager.LoadSceneAsync(SceneToLoad, LoadSceneMode.Additive);
    }
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene: Scene Loaded: " + scene.name);
        StartCoroutine(DelayedActivate(scene.name));

    }

    private IEnumerator DelayedActivate(string name)
    {
        yield return null;
        //yield return new WaitForSeconds(2f);
        Debug.Log("Scene: Setting Scene Active: " + name);

        Debug.Log("Scene: List Of Scenes at this point: ");
        Debug.Log("Scene: ------------------------------");
        foreach(var scene in SceneManager.GetAllScenes()) {
            Debug.Log("Scene: "+scene.name+" Valid: "+scene.IsValid()+" Loaded: "+scene.isLoaded);
            if(scene.name == name) {
                while (!scene.isLoaded) {
                    yield return null;
                    Debug.Log("Scene: ---------------waiting until its Loaded");
                }
                    Debug.Log("Scene: ---------------                  Loaded");
                SceneManager.SetActiveScene(scene);
            }
        }
        Debug.Log("Scene: -----------------\n");

        // Stor this as the Active One
        ActiveGameScene = name;
    }*/

    /*
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
    }*/
}
