using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classe gestionnaire de déplacement du joueur.
/// </summary>
public class PlayerMovement : MonoBehaviour {

    public Animator animator; // Animator de deplacement
    private MapManager mapManager;

    private float mapXMin; // Limite min en X ou le joueur peut aller
    private float mapYMin; // Limite min en Y ou le joueur peut aller
    private float mapXMax; // Limite max en X ou le joueur peut aller
    private float mapYMax; // Limite max en Y ou le joueur peut aller

    public bool canMove; // Le player peut-il bouger ?
    private float movingX; // Le player doit bouger selon X : movingX indique la direction avec son signe.
    private float movingY; // Le player doit bouger selon Y : movingY indique la direction avec son signe.
    private Vector3 currentPosition; // Position actuelle du joueur
    private Vector3 futurePosition; // Position où le joueur doit aller

    public bool m_FacingRight = true;  // Le player regarde vers la droite initialement
    public float runSpeed = 1f; // Vitesse de déplacement
	float horizontalMove = 0f; // Mouvement selon X
    float verticalMove = 0f; // Mouvement selon Y


    private void Start()
    {
        mapManager = GameObject.Find("Tiles").GetComponent<MapManager>(); // On load le MapManager
        mapXMin = mapManager.startTilemap.x;
        mapYMin = mapManager.startTilemap.y;
        mapXMax = mapManager.endTilemap.x;
        mapYMax = mapManager.endTilemap.y;

        // Initialement, les joueurs sont immobiles
        canMove = false; 
        movingX = 0;
        movingY = 0;
        // Position actuelle et position où dois aller le joueur : nulle part.
        currentPosition = this.transform.position;
        futurePosition = this.transform.position;
        // Lancement de la coroutine qui va gérer le déplacement du joueur
        StartCoroutine(ChangeCell());
    }


    void Update () {
        if (canMove) // Si le joueur peut bouger
        {
            runSpeed = 1f; // Vitesse définie
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed; // Vitesse selon X
            verticalMove = Input.GetAxisRaw("Vertical") * runSpeed; // Vitesse selon Y
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove) + Mathf.Abs(verticalMove)); // Si le joueur bouge, l'animation doit s'activer
        }
        else // Si le joueur ne peut pas bouger
        {
            horizontalMove = 0f; // Mouvement en X nul
            verticalMove = 0f; // Mouvement en X nul
            animator.SetFloat("Speed", 0f); // Pas d'animation de déplacement
        }

	}



    /// <summary>
    /// Provoque un changement de cellule du joueur actuel si un input le demande et si le deplacement est possible
    /// </summary>
    /// <returns> Permet d'attendre la fin de frame avant de retourner en debut de boucle </returns>
    public IEnumerator ChangeCell()
    {
        while(true) // Boucle infinie
        {
            var move = new Vector2(horizontalMove, verticalMove); // Vecteur de deplacement selon X et Y
            currentPosition = this.transform.position;   // Actualisation de la position actuelle

            if ((move.x > 0) && !m_FacingRight) // Regarder a droite si on doit bouger vers la droite
            {
                Flip();
            }
            else if ((move.x < 0) && m_FacingRight) // Regarder a gauche si on doit bouger vers la gauche
            {
                Flip();
            }

            if (move.x != 0) // Si on souhaite se déplacer selon X
            {
                movingX = Mathf.Sign(move.x); // Selon +X => 1 ; Selon -X => -1
                if (currentPosition.x + movingX < mapXMax && currentPosition.x + movingX > mapXMin)
                {
                    Vector3 futurePosition = new Vector3(currentPosition.x + movingX, currentPosition.y, currentPosition.z);
                    Vector3Int cellPosition = mapManager.grid.GetLocalPosition(futurePosition - new Vector3(0.3f, 0.3f, 0f));
                    Entity entity = mapManager.grid.GetValue(cellPosition.x, cellPosition.y);
                    if (entity == null || !(entity is Block))
                        transform.position = futurePosition; // Déplacement sur la future cellule
                }


                movingX = 0;
                yield return new WaitForSeconds(0.15f);
            }
            else if (move.y != 0) // Sinon si on souhaite se deplacer selon Y
            {
                movingY = Mathf.Sign(move.y); // Selon +Y => 1 ; Selon -Y => -1
                if (currentPosition.y-0.3f + movingY < mapYMax && currentPosition.y-0.3f + movingY > mapYMin)
                {
                    Vector3 futurePosition = new Vector3(currentPosition.x, currentPosition.y + movingY, currentPosition.z); // Déplacement sur la future cellule
                    Vector3Int cellPosition = mapManager.grid.GetLocalPosition(futurePosition - new Vector3(0.3f, 0.3f, 0f));
                    Entity entity = mapManager.grid.GetValue(cellPosition.x, cellPosition.y);
                    if (entity == null || !(entity is Block))
                        transform.position = futurePosition; // Déplacement sur la future cellule
                }
                movingY = 0;
                yield return new WaitForSeconds(0.15f);
            }
            yield return new WaitForEndOfFrame(); // Boucle fonctionne frame par frame
        }
    }

    public void Flip()
    {
        // Va regarder dans l'autre sens
        m_FacingRight = !m_FacingRight;

        // Regarde dans le bon sens
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
