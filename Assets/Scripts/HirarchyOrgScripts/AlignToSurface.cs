using UnityEngine;

[ExecuteInEditMode]
public class AlignToSurface : MonoBehaviour
{
    [SerializeField] float checkDistance = 0.5f;
    [SerializeField] LayerMask mask = ~0;

    private void Update()
    {
        Vector2[] directionValues = new Vector2[]
        { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
        int[] rotationValues = new int[] { 180, 90, 0, 270 };

        int closestIndex = -1;
        float closestDistance = int.MaxValue;

        for (int i = 0; i < 4; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position,
                directionValues[i], checkDistance, mask);
            if (hit)
            {
                if (hit.distance < closestDistance)
                {
                    closestIndex = i;
                    closestDistance = hit.distance;
                }
            }
        }

        if (closestIndex != -1)
        {
            Vector3 euler = transform.rotation.eulerAngles;
            euler.z = rotationValues[closestIndex];
            transform.rotation = Quaternion.Euler(euler);
        }
    }
}