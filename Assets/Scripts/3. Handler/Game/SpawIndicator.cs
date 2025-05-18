using UnityEngine;

public class SpawIndicator : MonoBehaviour
{
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private Color _drawColor = Color.red;


    private void OnDrawGizmos()
    {
        Gizmos.color = _drawColor;

        Vector3 size = _boxCollider.size;
        size.y *= transform.localScale.y;
        size.x *= transform.localScale.x;
        size.z *= transform.localScale.z;

        Gizmos.DrawCube(transform.position, size);
    }
}
