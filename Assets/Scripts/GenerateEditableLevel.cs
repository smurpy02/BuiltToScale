using UnityEngine;
using UnityEngine.SceneManagement;

public class GenerateEditableLevel : GenerateLevelFromSave
{
    public LevelEditorManager levelEditor;

    static bool generatePresetLevel = false;

    protected override void Start()
    {
        if (generatePresetLevel)
        {
            levelEditor.levelNameInput.text = levelData.levelName;
            generatePresetLevel = false;
            base.Start();
        }
    }

    protected override GameObject GetPrefabFromReference(PuzzleComponentPrefabReference prefab) => prefab.editablePrefab;

    protected override void OnComponentSpawned(PuzzleComponentData component, GameObject componentObject)
    {
        levelEditor.AddPuzzleComponent(new PuzzleComponentInstance(component, componentObject.transform));
    }

    public void GenerateLevelEditable(LevelData data)
    {
        levelEditor.levelEditorCanvas.SetActive(false);
        levelData = data;
        generatePresetLevel = true;

        LevelLoader.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}