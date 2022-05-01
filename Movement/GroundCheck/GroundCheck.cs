using UnityEngine;

public static class GroundCheck
{
    /// <summary>
    /// Performs a <see cref="Physics.CheckSphere(Vector3, float, int, QueryTriggerInteraction)"/> below the object, 
    /// using a given position and radius.
    /// </summary>
    public static bool Grounded(Vector3 position, float sphereRadius, float groundedOffset, LayerMask groundLayer)
    {
        Vector3 spherePosition = new Vector3(position.x, position.y + groundedOffset, position.z);
        return Physics.CheckSphere(spherePosition, sphereRadius, groundLayer, QueryTriggerInteraction.Ignore);
    }

    /// <summary>
    /// Casts a sphere under the character position to return a <see cref="Vector3"/> with the normal of the surface below the player
    /// </summary>
    public static Vector3 GetFloorNormal(Transform playerTransform, float playerRadius, float playerHeight)
    {
        if (Physics.SphereCast(playerTransform.position, playerRadius, -playerTransform.up, out RaycastHit hit, playerHeight + 0.5f))
        {
            return hit.normal;
        }
        return Vector3.up;
    }
}
