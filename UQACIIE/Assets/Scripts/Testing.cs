using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{

    public  GridMap grid; // La grille de jeu associee a la tilemap de la game area
    public GameObject finder;


    // Start is called before the first frame update
    void Start()
    {
        grid = new GridMap(7, 10, 1f, new Vector3(-3f, -5f, 0f)); // TODO Adapter automatiquement
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posFinder = finder.transform.position;
        Vector3 center = grid.GetLocalPosition(posFinder);
        grid.CheckGrid(Mathf.FloorToInt(posFinder.x), Mathf.FloorToInt(posFinder.y));

        //        Debug.DrawLine(new Vector3(v.x - 0.5f, v.y - 0.5f, 0), new Vector3(v.x + 0.5f, v.y + 0.5f, 0), Color.blue, 100f);

    }
}
