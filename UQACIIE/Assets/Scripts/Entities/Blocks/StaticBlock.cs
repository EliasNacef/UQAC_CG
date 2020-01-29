using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBlock : Block
{

    void Start()
    {
        isStatic = true; // Cette entite est fixe
        name = "Bloc";
    }
    
}
