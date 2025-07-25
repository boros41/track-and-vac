using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace GolemKinGames.Vacumn
{
    public enum VacuumShape { Cone, Box }
    public enum VacuumMode { Attract, Repel }
    public enum ForceDirectionMode { DirectPull, Vortex }
    public enum VacuumEffect { None, Shrink }
    public enum VacuumObjectEndAction { Destroy, Deactivate } // New enum for destroy/deactivate action
    public enum VacuumDimension { Mode2D, Mode3D }  // New enum to switch between 2D and 3D modes

    [RequireComponent(typeof(Collider))]
    public class VacuumSystem : MonoBehaviour
    {
        [SerializeField] private VacuumDimension vacuumDimension = VacuumDimension.Mode3D;  // 2D or 3D mode
        [SerializeField] private LayerMask affectedLayers;
        [SerializeField] private string[] affectedTags;
        [SerializeField] public VacuumShape vacuumShape = VacuumShape.Cone;
        [SerializeField] private VacuumMode vacuumMode = VacuumMode.Attract;
        [SerializeField] private ForceDirectionMode forceDirectionMode = ForceDirectionMode.DirectPull;
        [SerializeField] private VacuumEffect vacuumEffect = VacuumEffect.None; // Enum to control shrink effect
        [SerializeField] private VacuumObjectEndAction endAction = VacuumObjectEndAction.Destroy; // Action on reaching vacuum point
        [SerializeField] private AnimationCurve forceCurve = AnimationCurve.Linear(0, 1, 1, 0);
        [SerializeField] private AnimationCurve shrinkCurve = AnimationCurve.Linear(0, 1, 1, 0);  // New shrink curve
        [SerializeField] private bool triggerOnStart = true;
        [SerializeField] private float maxForce = 100f;  // Strong force when far from the vacuum point
        [SerializeField] private float minForce = 50f;   // Still a strong force when near
        [SerializeField] public float maxRange = 10f;
        [SerializeField] private float coneAngle = 45f; // Cone angle in degrees
        [SerializeField] public Transform vacuumPoint;
        [SerializeField] private float collectionDistance = 0.5f; // Distance when object is collected
        [SerializeField] private float pulseSpeed = 2.0f;
        [SerializeField] private float pulseIntensity = 0.5f;
        [SerializeField] private ParticleSystem vacuumParticles;
        [SerializeField] private ParticleSystem destroyParticles;
        [SerializeField] private AudioSource vacuumSound;
        [SerializeField] public bool drawGizmos = true;
        [SerializeField] private LayerMask obstacleLayers;
        [SerializeField] private bool velocityAffectsForce = true;
        [SerializeField] private BatteryBarUI batteryBar;

        // Callback for when an object reaches the vacuum point
        public Action<GameObject> OnObjectCollected;

        // Dictionary to store objects and their frustum status
        private Dictionary<VacuumAffector, bool> affectedAffectorsStatus = new Dictionary<VacuumAffector, bool>();

        private static bool isVacuumOn = false;
        private bool previousState = false;

        private float battery = 100;
        //private float drainSpeed = 5.0f / 3.0f; // how much is needed to completely drain the battery in 1 minute. ((drainSpeed * deltaTime) * sixtyFrames/oneSec) * sixtySeconds = 100
        private float drainSpeed = 4f;
        private void Start()
        {
            batteryBar.MaxBattery = battery;

            if (triggerOnStart)
            {
                ActivateVacuum();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && battery > 0)
            {
                isVacuumOn = !isVacuumOn;
            }

            if (isVacuumOn != previousState)
            {
                if (isVacuumOn)
                {
                    VacuumSoundManager.vacuumOffSound.Stop();
                    VacuumSoundManager.vacuumSound.Play();

                    ActivateVacuum();
                }
                else
                {
                    DeactivateVacuum();
                }

                previousState = isVacuumOn;
            }

            DrainBattery();

        }

        private void FixedUpdate()
        {
            if (isVacuumOn)
            {
                UpdateVacuumStatus();
            }
        }

        private void EnableGravity(bool flag)
        {
            foreach (var entry in affectedAffectorsStatus)
            {
                VacuumAffector affector = entry.Key;

                if (affector == null) continue;

                if (flag)
                {
                    affector.GetComponent<Rigidbody>().useGravity = true;
                }
                else
                {
                    affector.GetComponent<Rigidbody>().useGravity = false;
                }

            }
        }

        private void DrainBattery()
        {
            if (battery > 0 && isVacuumOn)
            {
                battery -= drainSpeed * Time.deltaTime;
                batteryBar.Battery = battery;
                print($"BATTERY: {battery:F1}");

                if (battery <= 0) DeactivateVacuum();
            }

        }

        private void DeactivateVacuum()
        {
            VacuumSoundManager.vacuumSound.Stop();
            VacuumSoundManager.vacuumOffSound.Play();

            EnableGravity(true);
            affectedAffectorsStatus.Clear();
        }

        public void ActivateVacuum()
        {
            //Debug.Log($"Total affectors found: {affectedAffectorsStatus.Count}");

            affectedAffectorsStatus.Clear(); // Clear the list before updating

            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (((1 << obj.layer) & affectedLayers) != 0 && IsTagAffected(obj))
                {
                    VacuumAffector affector = obj.GetComponent<VacuumAffector>();
                    if (affector != null && !affectedAffectorsStatus.ContainsKey(affector))
                    {
                        // Ensure the object has a Rigidbody (automatically adds if not present)
                        EnsureRigidbodySetup(affector);

                        bool isWithinFrustum = IsWithinFrustum(affector.transform.position);
                        affectedAffectorsStatus.Add(affector, isWithinFrustum);
                    }
                }
            }
        }

        private void UpdateVacuumStatus()
        {
            // Collect keys to modify after iteration
            List<VacuumAffector> toRemove = new List<VacuumAffector>();
            List<VacuumAffector> toUpdate = new List<VacuumAffector>();

            foreach (var entry in affectedAffectorsStatus)
            {
                VacuumAffector affector = entry.Key;

                if (affector == null) continue;

                bool wasInFrustum = entry.Value;
                bool isInFrustumNow = IsWithinFrustum(affector.transform.position);

                if (wasInFrustum && !isInFrustumNow)
                {
                    // Mark for removal from vacuum effect
                    toRemove.Add(affector);
                    Debug.Log($"Object {affector.gameObject.name} has left the frustum and is no longer vacuumed.");

                    EnableGravity(true);
                }
                else if (!wasInFrustum && isInFrustumNow)
                {
                    // Mark for re-entry into vacuum effect
                    toUpdate.Add(affector);
                    Debug.Log($"Object {affector.gameObject.name} has entered the frustum and is now being vacuumed.");
                    EnableGravity(false);
                }
            }

            // Apply changes after iteration
            foreach (var affector in toRemove)
            {
                affectedAffectorsStatus[affector] = false;
            }

            foreach (var affector in toUpdate)
            {
                affectedAffectorsStatus[affector] = true;
            }

            // Now apply the vacuum force to all that are still in the frustum
            foreach (var entry in affectedAffectorsStatus)
            {
                if (entry.Value) // Only apply force if the object is in the frustum
                {
                    ApplyVacuumForce(entry.Key);
                }
            }
        }


        private bool IsTagAffected(GameObject obj)
        {
            foreach (string tag in affectedTags)
            {
                if (obj.CompareTag(tag)) return true;
            }
            return false;
        }

        private bool IsWithinFrustum(Vector3 objectPosition)
        {
            Vector3 directionToObject = objectPosition - vacuumPoint.position;
            float distanceToObject = directionToObject.magnitude;

            if (distanceToObject > maxRange || distanceToObject < 0.5f)
            {
                return false;
            }

            // If in 3D mode, use regular frustum logic
            if (vacuumDimension == VacuumDimension.Mode3D)
            {
                Vector3 directionToObjectNormalized = directionToObject.normalized;
                float angleToObject = Vector3.Angle(vacuumPoint.forward, directionToObjectNormalized);

                if (angleToObject > coneAngle)
                {
                    return false;
                }

                float frustumRadiusAtDistance = Mathf.Tan(coneAngle * Mathf.Deg2Rad) * distanceToObject;
                Vector3 objectOffset = objectPosition - vacuumPoint.position;
                float lateralDistanceFromCenter = Vector3.ProjectOnPlane(objectOffset, vacuumPoint.forward).magnitude;

                return lateralDistanceFromCenter <= frustumRadiusAtDistance;
            }
            // If in 2D mode, use 2D sector/circular logic
            else if (vacuumDimension == VacuumDimension.Mode2D)
            {
                // Use local right direction for 2D cone calculations
                float angleToObject2D = Vector2.Angle(vacuumPoint.right, directionToObject);
        
                if (angleToObject2D > coneAngle)
                {
                    return false;
                }

                // For box shape in 2D
                if (vacuumShape == VacuumShape.Box)
                {
                    return Mathf.Abs(directionToObject.x) <= maxRange / 2 && Mathf.Abs(directionToObject.y) <= maxRange / 2;
                }
                Debug.LogError(distanceToObject);

                // For cone shape in 2D
                return distanceToObject <= maxRange;
            }

            return false;
        }


        private void ApplyVacuumForce(VacuumAffector affector)
        {
            if (affector == null) return;

            print($"Applying vacuum to: {affector.gameObject.name}");

            Vector3 directionToVacuum = vacuumPoint.position - affector.transform.position;
            float distance = directionToVacuum.magnitude;

            // Check for obstacles between the object and the vacuum point
            if (Physics.Raycast(affector.transform.position, directionToVacuum, distance, obstacleLayers))
            {
                // If an obstacle is detected, do not apply the vacuum force
                Debug.Log($"Obstacle detected between {affector.gameObject.name} and vacuum point, vacuum force blocked.");
                return;
            }

            // Apply the vacuum force as usual if no obstacles are detected
            float force = Mathf.Lerp(maxForce, minForce, forceCurve.Evaluate(distance / maxRange));
            Vector3 forceDirection = directionToVacuum.normalized * force;
            
            if (vacuumMode == VacuumMode.Repel)
            {
                forceDirection *= -1;
            }
            
            if (forceDirectionMode == ForceDirectionMode.Vortex)
            {
                forceDirection = Vector3.Cross(Vector3.up, directionToVacuum).normalized;
            }


            // Apply force based on the vacuum dimension (3D or 2D)
            if (vacuumDimension == VacuumDimension.Mode3D)
            {
                affector.ApplyForce(forceDirection);

                Rigidbody rb = affector.GetComponent<Rigidbody>();
                if (rb != null) rb.useGravity = false;
            }
            else if (vacuumDimension == VacuumDimension.Mode2D)
            {
                affector.ApplyForce2D(forceDirection);

                Rigidbody2D rb2D = affector.GetComponent<Rigidbody2D>();
                if (rb2D != null) rb2D.gravityScale = 0;
            }
            // Instantiate and play particles
            if (vacuumParticles != null)
            {
                
                vacuumParticles.Play();
            }

            // Play audio while pulling the object
            if (vacuumSound != null && !vacuumSound.isPlaying)
            {
                vacuumSound.Play();
            }

            // Shrink the object if the shrink effect is enabled
            if (vacuumEffect == VacuumEffect.Shrink)
            {
                float scale = shrinkCurve.Evaluate(distance / maxRange);
                affector.transform.localScale = Vector3.one * Mathf.Clamp(scale, 0.1f, 1f);
                //affector.transform.localScale *= Mathf.Clamp(scale, 0.1f, 1f);
            }

            // Check if the object has reached the vacuum point (collection distance)
            if (distance <= collectionDistance)
            {
                OnObjectCollected?.Invoke(affector.gameObject);
                DestroyOrDeactivateObject(affector.gameObject);
            }
        }


        private void DestroyOrDeactivateObject(GameObject obj)
        {
            // Decide whether to destroy or deactivate based on the enum
            if (endAction == VacuumObjectEndAction.Destroy)
            {
                VacuumSoundManager.pickUpSound.Play();
                Destroy(obj); // Destroy the object
            }
            else if (endAction == VacuumObjectEndAction.Deactivate)
            {
                obj.SetActive(false); // Deactivate the object
            }
        }
        
        private void EnsureRigidbodySetup(VacuumAffector affector)
        {
            if (vacuumDimension == VacuumDimension.Mode3D)
            {
                Rigidbody rb = affector.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = affector.gameObject.AddComponent<Rigidbody>();
                    rb.useGravity = false;
                    rb.isKinematic = false;
                    Debug.Log($"Rigidbody added to {affector.gameObject.name}");
                }
                else
                {
                    rb.useGravity = false;
                }
            }
            else if (vacuumDimension == VacuumDimension.Mode2D)
            {
                Rigidbody2D rb2D = affector.GetComponent<Rigidbody2D>();
                if (rb2D == null)
                {
                    rb2D = affector.gameObject.AddComponent<Rigidbody2D>();
                    rb2D.gravityScale = 0;
                    rb2D.isKinematic = false;
                    Debug.Log($"Rigidbody2D added to {affector.gameObject.name}");
                }
                else
                {
                    rb2D.gravityScale = 0;
                }
            }
        }
        

        private void OnDrawGizmos()
        {
            if (!drawGizmos || vacuumPoint == null) return;

            Gizmos.color = Color.blue;
            Gizmos.matrix = vacuumPoint.localToWorldMatrix;

            // If in 3D mode, draw the 3D shapes
            if (vacuumDimension == VacuumDimension.Mode3D)
            {
                switch (vacuumShape)
                {
                    case VacuumShape.Cone:
                        Gizmos.DrawFrustum(Vector3.zero, coneAngle * 2f, maxRange, 0.5f, 1.0f);
                        break;
                    case VacuumShape.Box:
                        Gizmos.DrawWireCube(Vector3.zero, new Vector3(maxRange, maxRange, maxRange));
                        break;
                }
            }
            // If in 2D mode, draw 2D shapes using Handles (for arcs)
            else if (vacuumDimension == VacuumDimension.Mode2D)
            {
#if UNITY_EDITOR
                // Draw arc for 2D cone
                if (vacuumShape == VacuumShape.Cone)
                {
                    Handles.color = Color.blue;
                    Handles.DrawWireArc(Vector3.zero, Vector3.forward, Vector3.right, coneAngle * 2, maxRange);
                }

                // Draw wire rectangle for 2D box
                if (vacuumShape == VacuumShape.Box)
                {
                    Gizmos.DrawWireCube(Vector3.zero, new Vector2(maxRange, maxRange));
                }
#endif
            }

            Gizmos.matrix = Matrix4x4.identity;

            // Draw gizmos for affected objects
            foreach (var entry in affectedAffectorsStatus)
            {
                VacuumAffector affector = entry.Key;

                if (affector == null || affector.transform == null)
                {
                    continue;
                }

                bool isInFrustum = entry.Value;

                Gizmos.color = isInFrustum ? Color.green : Color.red;
                Gizmos.DrawWireSphere(affector.transform.position, 0.5f);
            }
        }


    }
}
