using System.Collections;
using System.IO;
using UnityEngine;
using TMPro;

// a data manager, like the one used in Lysteria
// apperently this is what makes saving/loading possible:
// where tf is this even used?
public class DataManager : MonoBehaviour
{
    #region - Variables     
    public static DataManager instance;             // makes this a singleton

    [Header("File Settings")]
    public string saveFileName = "GameData";        // this creates a file? yes, .json (line 42)
    public string folderName = "SaveData";          // where tf is this folder?

    [Header("Data Settings")]
    public DefaultData gameData = new DefaultData();    // where is gameData instance assigned?

    string defaultPath;
    string fileName;
    #endregion

    // SAVING CRAP:
    [Header("Saving Crap")]
    [SerializeField] Canvas savingTextObject;  // the parent object containing the savingText. We pass the panelCanvas here. Not sure if necessary???

    // [SerializeField] TextMeshProUGUI savingText;        // must be carried over across scenes
    [SerializeField] GameObject savingText;        // must be carried over across scenes

    [SerializeField] float savingTime = 3f;

    #region - Unity Base Methods
    void Awake()
    {
        // Create a static instance of the data manager & destroy a new one if it exists in the scene
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(savingTextObject);
    }

    void Start()
    {
        // Set the folder and file names
        defaultPath = Application.persistentDataPath + "/" + folderName;
        fileName = defaultPath + "/" + saveFileName + ".json";

        // Check if the folder exists if not create it
        if (!FolderExists(defaultPath))
        {
            Directory.CreateDirectory(defaultPath);
        }

        // Load the data
        LoadGameData();
    }
    #endregion

    #region User Methods
    bool FolderExists(string folderPath)
    {
        return Directory.Exists(folderPath);
    }

    public void LoadGameData()
    {
        // Check if the file exists if not create it, if it does exist load the data
        if (File.Exists(fileName))
        {
            string saveData = File.ReadAllText(fileName);
            gameData = JsonUtility.FromJson<DefaultData>(saveData);
        }
        else
        {
            // SaveGameData();     // runs if the save file does not exist
            TriggerSaveData();
        }
    }

    public void SaveGameData()
    {
        // Save the data
        string saveData = JsonUtility.ToJson(gameData);
        File.WriteAllText(fileName, saveData);
        savingText.SetActive(false);
        Debug.Log("Data Saved!");
    }

    // executed AFTER returning to level select from a level:
    public void TriggerSaveData() {
        Debug.Log("Trigger Saving Data...");
        savingText.SetActive(true);
        Invoke("SaveGameData", 3f);
    }

    // public IEnumerator SaveGameData()
    // {
    //     // Save the data
    //     savingText.enabled = true;
    //     Debug.Log("Saving Data...");    // this fucking prints twice

    //     yield return new WaitForSeconds(savingTime);
        
    //     string saveData = JsonUtility.ToJson(gameData);
    //     File.WriteAllText(fileName, saveData);
    //     savingText.enabled = false;
    // }
    #endregion
}