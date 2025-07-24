using UnityEngine;

namespace GolemKinGames.Vacumn
{
    
    public class VacuumAffector : MonoBehaviour
    {
        private Rigidbody rb;
        private Rigidbody2D rb2D;

        private void Awake()
        {
            // Get the Rigidbody component
            rb = GetComponent<Rigidbody>();
            rb2D = GetComponent<Rigidbody2D>();
          
        }

        /// <summary>
        /// Apply force to the object based on vacuum interaction.
        /// </summary>
        /// <param name="force">The force vector to apply.</param>
        public void ApplyForce(Vector3 force)
        {
            if (rb != null)
            {
                // Apply force to the Rigidbody
                rb.AddForce(force, ForceMode.Acceleration);
                
            }
            else
            {
                Debug.LogWarning("No Rigidbody found on " + gameObject.name);
            }
        }
        
        // Apply force for 2D physics
        public void ApplyForce2D(Vector3 force)
        {
            Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
            if (rb2D != null)
            {
                rb2D.AddForce(new Vector2(force.x, force.y));
            }
        }

        /// <summary>
        /// Get the object's current velocity.
        /// </summary>
        /// <returns>Current velocity vector.</returns>
        public Vector3 GetVelocity()
        {
            return rb != null ? rb.linearVelocity : Vector3.zero;
        }
    }
}