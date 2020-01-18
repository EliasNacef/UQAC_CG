using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private Tilemap selectionMap;
    public TileBase blueTile;
    public TileBase oldTile;
    private Grid grid;

    [SerializeField]
    private Trap[] traps;
    public KillTrap newTrap;

    
    private GameObject player;
    private GameObject spectate;
    [SerializeField]
    private GameObject player1; // GameObject du joueur
    [SerializeField]
    private GameObject player2; // GameObject du joueur
    private int whoPlay;

    private Vector3 spawnPosition;
    private Vector3 spectatePosition;
    [SerializeField]
    private Vector3 currentCell; // Cellule où est le joueur
    [SerializeField]
    private Vector3 frontCell; // Cellule devant le joueur
    private Vector3Int currentCellInt;
    private Vector3Int frontCellInt;

    [SerializeField]
    private GameObject victoryPanel;

    private bool endRound;

    private void Start()
    {
        whoPlay = 1;
        player = player1;
        spectate = player2;
        endRound = false;
        spawnPosition = GameObject.Find("SpawnPosition").transform.position;
        spectatePosition = GameObject.Find("SpectatePosition").transform.position;
        UpdatePositions();
        UpdateAbilities();


        //Notre map est la GameArea defini par la TileMap GameArea
        grid = tilemap.layoutGrid;
        //Liste des pièges
        traps = GameObject.Find("Traps").GetComponentsInChildren<Trap>();


        // Cellule du joueur et devant lui
        frontCell = grid.WorldToCell(player.transform.position); // Cellule devant le joueur (car le transform du player est une case au dessus)
        currentCell = frontCell + new Vector3(0f, -1f, 0f); // Cellule où est le joueur
        frontCellInt = new Vector3Int(Mathf.FloorToInt(frontCell.x), Mathf.FloorToInt(frontCell.y), Mathf.FloorToInt(frontCell.z));
        currentCellInt = new Vector3Int(Mathf.FloorToInt(currentCell.x), Mathf.FloorToInt(currentCell.y), Mathf.FloorToInt(currentCell.z));
        oldTile = selectionMap.GetTile(frontCellInt); // Ancienne Tile
    }



    // Update is called once per frame
    void Update()
    {
        Player p = player.GetComponent<Player>();
        Player s = spectate.GetComponent<Player>();
        if (p.Life <= 0 || s.Life <= 0)
        {
            victoryPanel.SetActive(true);
        }
        if (endRound)
        {
            EndRound();
        }
        else
        {
            // La case de selection se deplace avec le joueur.
            SetSelectionTile();
            if (grid.WorldToCell(player.transform.position).y > 4)
            {
                endRound = true;
            }
            else if (Input.GetButtonDown("Jump"))
            {
                TryToSetTrap();
            }
        }
    }


    void SetSelectionTile()
    {
        if (!endRound)
        {
            selectionMap.SetTile(frontCellInt, oldTile);
            frontCell = grid.WorldToCell(player.transform.position); // Cellule devant le joueur (car le transform du player est une case au dessus)
            currentCell = frontCell + new Vector3(0f, -1f, 0f); // Cellule où est le joueur
            frontCellInt = new Vector3Int(Mathf.FloorToInt(frontCell.x), Mathf.FloorToInt(frontCell.y), Mathf.FloorToInt(frontCell.z));
            currentCellInt = new Vector3Int(Mathf.FloorToInt(currentCell.x), Mathf.FloorToInt(currentCell.y), Mathf.FloorToInt(currentCell.z));
            oldTile = selectionMap.GetTile(frontCellInt);
            selectionMap.SetTile(frontCellInt, blueTile);
        }
    }


    void TryToSetTrap()
    {
        bool canSetTrap = true;
        Vector3 positionFutureTrap = tilemap.GetCellCenterLocal(frontCellInt);
        traps = GameObject.Find("Traps").GetComponentsInChildren<Trap>();
        foreach (Trap trap in traps)
        {
            if (positionFutureTrap == tilemap.WorldToLocal(trap.transform.position))
            {
                canSetTrap = false;
            }
        }
        if (canSetTrap && !endRound) // Si je peux poser un piege et que ce n'est pas la fin de la manche
        {
            Debug.Log("Je pose le piège");
            Instantiate(newTrap, tilemap.GetCellCenterLocal(frontCellInt), Quaternion.identity, GameObject.Find("Traps").transform);
            Debug.Log("Fin de manche");
            endRound = true;
            selectionMap.SetTile(frontCellInt, oldTile); // On retire la case de selection pour voir le piege se poser
        }
        else // Je ne peux pas poser de piege pour le moment
        {
            Debug.Log("Je ne peux pas poser de piège");
        }
    }

    void SwitchPlayer()
    {
        if (whoPlay == 1)
        {
            player = player2;
            spectate = player1;
            whoPlay = 2;
        }
        else if(whoPlay == 2)
        {
            player = player1;
            spectate = player2;
            whoPlay = 1;
        }
        else
        {
            Debug.Log("whoPlay a une valeur différente de 1 ou 2 !");
        }
    }

    public void UpdatePositions()
    {
        Player p;
        p = player.GetComponent<Player>();
        //Si on s'est mange un piege, on perd une seule vie
        if (p.hasLost)
        {
            p.Life--;
            p.hasLost = false;
        }
        //Mise a jour des positions
        player.transform.position = spawnPosition;
        spectate.transform.position = spectatePosition;
        CharacterController controllerSpectate = spectate.GetComponent<CharacterController>();
        if (!controllerSpectate.m_FacingRight)
        {
            controllerSpectate.Flip();
        }
    }

    void UpdateAbilities()
    {
        player.GetComponent<PlayerMovement>().canMove = true;
        spectate.GetComponent<PlayerMovement>().canMove = false;
    }


    public void EndRound()
    {
        SwitchPlayer();
        UpdateAbilities();
        UpdatePositions();
        endRound = false;
    }
}
