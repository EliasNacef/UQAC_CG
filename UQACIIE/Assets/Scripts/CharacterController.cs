using UnityEngine;


/// <summary>
/// TODO : INUTILE ?
/// </summary>
public class CharacterController : MonoBehaviour
{
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // Rendre plus smooth le mouvement

    private Rigidbody2D m_Rigidbody2D; // Le RigidBody2D du player
    public bool m_FacingRight = true;  // Le player regarde vers la droite initialement
    private Vector3 velocity = Vector3.zero; // Velocite nulle au debut

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>(); // Le RigidBody2D du player
    }




    public void Move(Vector2 move)
    {
        /* 
        // Bouge le player en trouvant sa cible de velocite
        Vector3 targetVelocity = 5f * move;
        // Rend plus smooth le mouvement et l'applique au joueur
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

        // Regarder dans la bonne direction
        if ((move.x > 0) && !m_FacingRight)
        {
            Flip();
        }
        else if ((move.x < 0) && m_FacingRight)
        {
            Flip();
        }
        */
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