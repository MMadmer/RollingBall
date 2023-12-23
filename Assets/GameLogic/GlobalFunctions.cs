using UnityEngine;

namespace GlobalFunc
{
    public class GlobalFunctions : MonoBehaviour
    {
        public static bool OnGround(Collider collider, float extraDist = 0.0f)
        {
            if (!collider) return false;

            bool onGround = false;

            Vector3 direction = Vector3.down;
            float lenght = collider.bounds.extents.y + extraDist;

            for (sbyte i = -2; i < 3; i++)
            {
                Vector3 offset;

                if (i != 0)
                {
                    offset = i % 2 == 0 ? new Vector3(collider.bounds.extents.x * Mathf.Sign(i), 0.0f, 0.0f) : new Vector3(0.0f, 0.0f, collider.bounds.extents.z * Mathf.Sign(i));
                }
                else
                {
                    offset = Vector3.zero;
                }

                Vector3 startLoc = collider.transform.position + offset;

                Ray ray = new(startLoc, direction);

                if (Physics.Raycast(ray, lenght))
                {
                    onGround = true;
                    break;
                }
            }

            return onGround;
        }
    }
}
