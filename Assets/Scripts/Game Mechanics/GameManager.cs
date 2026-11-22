using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Petrification pet;

    void Start()
    {
        pet.Initiate(player);
    }
}
