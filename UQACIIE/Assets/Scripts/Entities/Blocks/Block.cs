using UnityEngine;

/// <summary>
/// Classe mere des blocks : les classes filles sont mobiles ou non
/// </summary>
public class Block : Entity
{

    void Start()
    {
        gameManager =FindObjectOfType<GameManager>(); // On load le MapManager
    }
}
