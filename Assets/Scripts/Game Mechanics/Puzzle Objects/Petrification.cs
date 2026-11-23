using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Petrification : MonoBehaviour
{
    [Header("Core Elements")]
    public Transform tablet, snake;
    public GameObject glowyEyes;
    public TextMeshProUGUI countdownText;
    public int countdown = 3;

    [Header("Visuals")]
    public ParticleSystem tabletBreaks;

    Player player;

    public void Initiate(Player player)
    {
        this.player = player;

        UpdateText();
        player.engine.ExpandBody += Countdown;
    }

    void Countdown(List<Vector2Int> newPositions)
    {
        if (countdown == 0) return;

        countdown--;

        UpdateText();

        if (countdown == 0) Petrify(newPositions);
    }

    void UpdateText()
    {
        countdownText.text = countdown.ToString();
        tablet.DOShakePosition(.1f, .3f);
    }

    void Petrify(List<Vector2Int> newPositions)
    {
        glowyEyes.SetActive(true);
        snake.DOShakePosition(.3f, .5f);
        tabletBreaks.Play();

        newPositions.ForEach(position => DoPetrify(position));
    }

    void DoPetrify(Vector2Int position)
    {
        tablet.gameObject.SetActive(false);

        var block = player.engine.GetBlock(position);

        if (block == null) return;

        if (block.state != 0) return;

        block.state = BlockState.Petrified;

        player.engine.SetBlock(position, block);

        var visuals = block.transform.GetComponent<SquareVisualManager>();

        if (visuals.IsUnityNull()) return;

        visuals.Petrify();
    }
}
