using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

public class PlayerVisualElements : MonoBehaviour
{
    public Sprite defaultSprite, surroundedSprite;
    public SpriteRenderer spriteRenderer;
    public SpriteAtlas playerAtlas;
    public bool tryLooseFit = true, usePresetSquareSprites = false;

    List<Sprite> playerSprites = new List<Sprite>();
    [SerializeField] List<SquareSprite> squareSprites = new List<SquareSprite>();
    List<Vector2Int> blockDirections = new List<Vector2Int>();

    static Dictionary<SpriteAtlas, List<SquareSprite>> squareSpritesShared = new();

    private List<Vector2Int> directions = new List<Vector2Int>
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.right,
        Vector2Int.left,
        Vector2Int.up + Vector2Int.left,
        Vector2Int.up + Vector2Int.right,
        Vector2Int.down + Vector2Int.left,
        Vector2Int.down + Vector2Int.right,
    };

    void Start()
    {
        InitSquareSprites();
        UpdateSprite();
    }

    void InitSquareSprites()
    {
        if (usePresetSquareSprites)
        {
            return;
        }

        if (squareSpritesShared.ContainsKey(playerAtlas))
        {
            squareSprites = squareSpritesShared[playerAtlas];
            return;
        }

        InitializePlayerSprites();

        Vector2 textureSize = playerSprites[0].texture.Size();

        Color jointColour = surroundedSprite.texture.GetPixel((int)surroundedSprite.rect.x, (int)surroundedSprite.rect.y);

        int spriteIndex = 0;

        foreach (var sprite in playerSprites)
        {
            spriteIndex++;

            var spriteTexture = sprite.texture;
            var spriteRect = sprite.rect;

            var squareSprite = new SquareSprite(sprite);

            foreach (var direction in directions)
            {
                int textureX = (int)spriteRect.x;

                if (direction.x == 0) textureX += 7;
                if (direction.x > 0) textureX += 15;

                int textureY = (int)spriteRect.y;

                if (direction.y == 0) textureY += 7;
                if (direction.y > 0) textureY += 15;

                var colour = spriteTexture.GetPixel(textureX, textureY);

                if (colour.Equals(jointColour)) squareSprite.blockDirections.Add(direction);
            }

            squareSprites.Add(squareSprite);
        }

        squareSpritesShared.Add(playerAtlas, squareSprites);
    }

    void InitializePlayerSprites()
    {
        Sprite[] spriteArray = new Sprite[playerAtlas.spriteCount];

        playerAtlas.GetSprites(spriteArray);

        playerSprites = spriteArray.ToList();

        Debug.Log(playerSprites.Count);
    }

    public void PopulateSquareSprites()
    {
        Debug.Log("Populating!");

        InitializePlayerSprites();

        squareSprites.Clear();

        foreach (var sprite in playerSprites)
        {
            Debug.Log(sprite);

            squareSprites.Add(new SquareSprite(sprite));
        }
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
                spriteRenderer.sprite = squareSprite.sprite;
                return;
            }
        }

        if (tryLooseFit)
        {
            foreach (var squareSprite in squareSprites)
            {
                if (SpriteLooselyPasses(squareSprite))
                {
                    spriteRenderer.sprite = squareSprite.sprite;
                    return;
                }
            }
        }

        spriteRenderer.sprite = defaultSprite;
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

    bool SpriteLooselyPasses(SquareSprite sprite)
    {
        if
            (
            sprite.blockDirections.All
                (
                direction => (direction.x == 0 || direction.y == 0) &&
                blockDirections.Contains(direction)
                ) &&
            blockDirections.All
                (
                direction => (direction.x != 0 && direction.y != 0) ||
                sprite.blockDirections.Contains(direction)
                )
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