using System.Collections.Generic;
using UnityEngine;

public static class LevelMemoryManager
{
    const string levelNameListLocation = "ListOfLevelNames", levelNamePrefix = "CustomLevel_", chapterLevelHolderPath = "ChapterAndLevelDetails";

    static ChapterLevelHolder chapterLevelHolder;

    public static LevelData GetLevelData(int levelNumber)
    {
        return GetLevelData(GetLevelJson(levelNumber));
    }

    public static LevelData GetLevelData(string levelJson)
    {
        var levelData = JsonUtility.FromJson<LevelData>(levelJson);

        if (levelData == null)
        {
            Debug.LogWarning("Level Data was Null or Unreadable");
            return null;
        }

        return levelData;
    }

    public static string GetLevelJson(int levelNumber)
    {
        var levelLocation = GetLevelLocation(levelNumber);

        if (!PlayerPrefs.HasKey(levelLocation))
        {
            Debug.LogWarning("Level Data does not exist in this location");
            return null;
        }

        return PlayerPrefs.GetString(levelLocation);
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
        return GetLevelLocation(levelNumber);
    }

    public static void DeleteLevel(int levelNumber)
    {
        PlayerPrefs.DeleteKey(GetLevelLocation(levelNumber));
    }

    public static string GetLevelLocation(int levelNumber) => $"{levelNamePrefix}{levelNumber}";

    static void LoadChapterLevelHolder()
    {
        if (chapterLevelHolder == null) chapterLevelHolder = Resources.Load<ChapterLevelHolder>(chapterLevelHolderPath);
    }

    public static void SaveLevelToPersistentPath(int levelNumber)
    {
        LoadChapterLevelHolder();

        var levelJson = GetLevelJson(levelNumber);

        chapterLevelHolder.AddLevel("", levelJson);
    }

    public static List<string> GetSavedPersistentLevels()
    {
        LoadChapterLevelHolder();

        var levels = new List<string>();

        chapterLevelHolder.levels.ForEach(level => levels.Add(level.json));

        return levels;
    }

    public static ChapterLevelHolder GetChapterLevelHolder()
    {
        LoadChapterLevelHolder();

        return chapterLevelHolder;
    }
}
