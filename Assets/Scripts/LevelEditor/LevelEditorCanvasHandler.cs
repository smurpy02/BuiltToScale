using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelEditorCanvasHandler : MonoBehaviour, IPointerEnterHandler
{
    public RectTransform editorCanvas;
    public float openPosition, closePosition;

    bool editorOpen, busyTransitioning;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (busyTransitioning) return;

        editorOpen = !editorOpen;

        busyTransitioning = true;
        StartCoroutine(ToggleEditor(editorOpen ? openPosition : closePosition));
    }

    IEnumerator ToggleEditor(float position)
    {
        yield return editorCanvas.DOLocalMoveY(position, .5f).WaitForCompletion();

        busyTransitioning = false;
    }
}
