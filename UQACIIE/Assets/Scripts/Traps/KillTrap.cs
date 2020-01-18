using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrap : Trap
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    override public void Activate(Player player)
    {
        isActivated = true;
        animator.SetBool("isActivated", isActivated);
        player.Hurt();
        StartCoroutine(Hitting());
    }
}
