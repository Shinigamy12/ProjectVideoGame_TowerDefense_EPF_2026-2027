using UnityEngine;

namespace _Project.Scripts.Grid
{
    public class GridComponent : MonoBehaviour
    {
        [Header("Dimensions")]
        [Tooltip("Nombre de cases sur l'axe X, bordure cachée comprise")]
        [SerializeField, Range(10,200)] private int _width = 92;
        
        [Tooltip("Nombre de cases sur l'axe Z, bordure cachée comprise")]
        [SerializeField, Range(10, 200)] private int _height = 60;

        private void OnDrawGizmos()
        {
            Vector3 center = transform.position;
            Vector3 size = new Vector3(_width, 0f, _height);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(center, size);
        }

        public Vector2Int WorldToCell(Vector3 worldPosition)
        {
            Vector3 localPosition = worldPosition - transform.position;
            int x = Mathf.FloorToInt(localPosition.x) + _width / 2;
            int z = Mathf.FloorToInt(localPosition.z) + _height / 2;
            return new Vector2Int(x, z);
        }

        public Vector3 CellToWorld(Vector2Int cell)
        {
            float x = cell.x - _width / 2 + 0.5f;
            float z = cell.y - _height / 2 + 0.5f;
            return transform.position + new Vector3(x, 0f, z);
        }
    }
}
