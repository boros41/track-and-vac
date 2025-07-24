
============================
Vacuum System Asset Documentation
============================

Version: 1.0

### Overview
The Vacuum System is a flexible Unity plugin designed to simulate the effect of a vacuum that pulls or repels objects towards or away from a vacuum point. It supports both 2D and 3D physics, with options for different shapes and force application modes. The system is highly customizable, including features for shrinking objects, applying visual effects, and handling audio feedback.

### Features
- **Dimension Mode (2D/3D):** Supports both 2D and 3D objects with appropriate physics handling.
- **Vacuum Shape:** Choose between cone or box-shaped vacuum fields.
- **Vacuum Modes:** Attract or repel objects towards or away from the vacuum point.
- **Force Direction Modes:** Apply force in a direct pull or vortex-like manner.
- **Shrink Effect:** Objects can shrink as they approach the vacuum point.
- **Obstacle Detection:** Specify layers that act as obstacles and block the vacuum force.
- **Customizable Forces:** Set min and max forces based on distance from the vacuum point.
- **Callbacks:** Trigger custom actions when an object reaches the vacuum point.
- **Visual Feedback:** Play particle effects and audio when objects are affected by the vacuum.
- **Gizmo Support:** Visualize the vacuum shape in the scene view.

### Fields & Properties
- **vacuumDimension:** (Enum) Mode2D, Mode3D - Choose between 2D or 3D mode for the vacuum system.
- **vacuumShape:** (Enum) Cone, Box - Choose the shape of the vacuum field.
- **vacuumMode:** (Enum) Attract, Repel - Whether the vacuum pulls or pushes objects.
- **forceDirectionMode:** (Enum) DirectPull, Vortex - The direction of the vacuum force applied to objects.
- **forceCurve:** (AnimationCurve) Controls how the force is applied over distance.
- **maxForce / minForce:** (float) The maximum and minimum force applied to objects.
- **maxRange:** (float) The maximum range of the vacuum.
- **vacuumPoint:** (Transform) The origin point of the vacuum.
- **collectionDistance:** (float) The distance at which objects are considered collected.
- **obstacleLayers:** (LayerMask) Layers that block the vacuum force.
- **triggerOnStart:** (bool) Automatically trigger the vacuum on start.
- **vacuumEffect:** (Enum) None, Shrink - Apply a shrinking effect to objects as they near the vacuum.
- **shrinkCurve:** (AnimationCurve) Defines the rate of shrinking as objects approach the vacuum.
- **vacuumParticles:** (ParticleSystem) The particle effect to instantiate when an object is vacuumed.
- **vacuumSound:** (AudioSource) The sound to play when an object is vacuumed.
- **drawGizmos:** (bool) Toggle the drawing of scene view gizmos.

### Callbacks
- **OnObjectCollected:** This callback is triggered when an object reaches the vacuum point and is collected (either destroyed or deactivated).

### How to Use
1. **Setup:**
   - Attach the `VacuumSystem` script to a GameObject.
   - Assign a transform to the `vacuumPoint` field (the point where objects are vacuumed towards or away from).
   - Choose the `vacuumShape` (Cone or Box) and the dimension mode (2D or 3D).

2. **Customization:**
   - Adjust the `maxRange`, `maxForce`, and `minForce` values to control the vacuum's strength.
   - Set up the `vacuumParticles` and `vacuumSound` for visual and audio effects.
   - Use `obstacleLayers` to specify which objects block the vacuum effect.

3. **Starting the Vacuum:**
   - The vacuum can start automatically if `triggerOnStart` is enabled. Otherwise, call the `ActivateVacuum()` method in your script.

4. **Obstacle Detection:**
   - Specify layers in `obstacleLayers` to prevent objects behind obstacles from being affected by the vacuum.

5. **Visual Feedback:**
   - Add particle effects and audio clips to give visual and auditory feedback when objects are vacuumed.

### Code Example
```csharp
void Start()
{
    VacuumSystem vacuumSystem = GetComponent<VacuumSystem>();
    vacuumSystem.ActivateVacuum();
}
```

### Notes
- Ensure that objects being affected by the vacuum have a `VacuumAffector` script attached to handle force application.
- For 2D mode, objects should have `Rigidbody2D` components, and for 3D mode, they should have `Rigidbody` components.

### Contact
For issues or feature requests, please contact support at support@golemkingames.com.

