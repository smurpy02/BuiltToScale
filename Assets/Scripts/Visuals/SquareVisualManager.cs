using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

public class SquareVisualManager : MonoBehaviour
{
    public Sprite defaultSprite, surroundedSprite;
    public SpriteRenderer playerRenderer, backdropRenderer, speckRenderer;
    public SpriteAtlas playerAtlas, speckAtlas;
    public Texture2D playerTexture;

    List<SquareSprite> squareSprites = new List<SquareSprite>();
    List<Vector2Int> blockDirections = new List<Vector2Int>();

    static Dictionary<SpriteAtlas, List<SquareSprite>> processedSquareSprites = new();

    private List<Vector2Int> directions = new List<Vector2Int>
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.right,
        Vector2Int.left,
    };

    void Start()
    {
        CreateSpawnSprites();
        RandomiseSpeck();
        UpdateSprite();

        playerRenderer.transform.DOShakeScale(10, 0.08f, 2, 80, false, ShakeRandomnessMode.Harmonic).SetLoops(-1);
    }

    void RandomiseSpeck()
    {
        var sprites = GetSprites(speckAtlas);

        if (sprites.Count == 0) return;

        speckRenderer.sprite = sprites[Random.Range(0, sprites.Count)];
    }

    void CreateSpawnSprites()
    {
        if (processedSquareSprites.ContainsKey(playerAtlas))
        {
            squareSprites = processedSquareSprites[playerAtlas];
            return;
        }

        var sprites = GetSprites(playerAtlas);

        if (sprites.Count == 0) return;

        squareSprites.Clear();
        var jointColour = GetJointColor();

        foreach(var sprite in sprites)
        {
            var squareSprite = new SquareSprite(sprite);

            foreach(var direction in directions)
            {
                var color = GetPixelColor(sprite, direction);

                if (color.Equals(jointColour)) squareSprite.blockDirections.Add(direction);
            }

            squareSprites.Add(squareSprite);
        }

        processedSquareSprites.Add(playerAtlas, squareSprites);
    }

    Color GetPixelColor(Sprite sprite, Vector2Int direction)
    {
        var rect = sprite.rect.center;
        var texture = playerTexture;

        if (direction.x < 0) rect.x -= 8;
        if (direction.x > 0) rect.x += 7;

        if (direction.y < 0) rect.y -= 8;
        if (direction.y > 0) rect.y += 7;

        return texture.GetPixel((int)rect.x, (int)rect.y);
    }

    Color GetJointColor()
    {
        return GetPixelColor(surroundedSprite, Vector2Int.up);
    }

    List<Sprite> GetSprites(SpriteAtlas atlas)
    {
        Sprite[] sprites = new Sprite[atlas.spriteCount];

        atlas.GetSprites(sprites);

        return sprites.ToList();
    }

    public void BlockRemoved(Vector2Int myPosition, Vector2Int blockPosition)
    {
        Vector2Int blockDirection = blockPosition - myPosition;
        if (!directions.Contains(blockDirection)) return;
        if (blockDirections.Contains(blockDirection)) blockDirections.Remove(blockDirection);
        UpdateSprite();
    }

    public void BlockAdded(Vector2Int myPosition, Vector2Int blockPosition)
    {
        Vector2Int blockDirection = blockPosition - myPosition;
        if (!directions.Contains(blockDirection)) return;
        if (!blockDirections.Contains(blockDirection)) blockDirections.Add(blockDirection);
        UpdateSprite();
    }

    void UpdateSprite()
    {
        foreach (var squareSprite in squareSprites)
        {
            if (SpriteStrictlyPasses(squareSprite))
            {
                playerRenderer.sprite = squareSprite.sprite;
                backdropRenderer.sprite = squareSprite.sprite;
                return;
            }
        }

        playerRenderer.sprite = defaultSprite;
        backdropRenderer.sprite = defaultSprite;
    }

    bool SpriteStrictlyPasses(SquareSprite sprite)
    {
        if (
            sprite.blockDirections.All(direction => blockDirections.Contains(direction)) &&
            blockDirections.All(direction => sprite.blockDirections.Contains(direction))
            )
        {
            return true;
        }

        return false;
    }
}

[Serializable]
class SquareSprite
{
    public Sprite sprite;
    public List<Vector2Int> blockDirections = new();

    public SquareSprite(Sprite sprite)
    {
        this.sprite = sprite;
    }
}