using UnityEngine;

public class LevelMemoryManager
{
    const string levelNameListLocation = "ListOfLevelNames", levelNamePrefix = "CustomLevel_";

    public static LevelData GetLevelData(int levelNumber)
    {
        var levelLocation = GetLevelName(levelNumber);

        if (!PlayerPrefs.HasKey(levelLocation))
        {
            Debug.LogWarning("Level Data does not exist in this location");
            return null;
        }

        var levelData = JsonUtility.FromJson<LevelData>(PlayerPrefs.GetString(levelLocation));

        if (levelData == null)
        {
            Debug.LogWarning("Level Data was null or unreadable");
            return null;
        }

        return levelData;
    }

    public static LevelSaveLocationData GetLocationData()
    {
        if (!PlayerPrefs.HasKey(levelNameListLocation))
        {
            Debug.LogWarning("Could not find Level List");
            return null;
        }

        var jsonList = PlayerPrefs.GetString(levelNameListLocation);
        var levelSaveData = JsonUtility.FromJson<LevelSaveLocationData>(jsonList.ToString());

        return levelSaveData;
    }

    public static void SetLocationData(LevelSaveLocationData locationData)
    {
        var newJsonList = JsonUtility.ToJson(locationData);

        PlayerPrefs.SetString(levelNameListLocation, newJsonList.ToString());
    }

    public static void InitLevelList()
    {
        var levelList = new LevelSaveLocationData();
        var jsonList = JsonUtility.ToJson(levelList);

        PlayerPrefs.SetString(levelNameListLocation, jsonList.ToString());
    }

    public static string RegisterLevel(string levelName)
    {
        if (!PlayerPrefs.HasKey(levelNameListLocation))
        {
            InitLevelList();
        }

        var levelSaveData = GetLocationData();

        var levelNumber = 0;
        while (levelSaveData.SaveLocations.Contains(levelNumber)) levelNumber++;
        levelSaveData.SaveLocations.Add(levelNumber);

        SetLocationData(levelSaveData);
        return GetLevelName(levelNumber);
    }

    public static void DeleteLevel(int levelNumber)
    {
        PlayerPrefs.DeleteKey(GetLevelName(levelNumber));
    }

    public static string GetLevelName(int levelNumber) => $"{levelNamePrefix}{levelNumber}";
}
