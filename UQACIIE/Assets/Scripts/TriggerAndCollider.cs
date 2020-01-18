using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAndCollider : MonoBehaviour
{
    [SerializeField]
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Le trap qui va s'enclencher
        Trap trap = collision.gameObject.GetComponent<Trap>();

        // Pour activer l'animation de destruction du trap.
        collision.gameObject.GetComponent<SpriteRenderer>().sprite = null;

        // Activation du trap
        trap.Activate(player);

        // Debug
        Debug.Log("Boom, le piège s'est activé");
    }


}
