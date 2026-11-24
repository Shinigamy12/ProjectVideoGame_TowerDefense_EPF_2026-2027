using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.Core.Placement;

namespace _Project.Scripts.Grid
{
    public class GridComponent : MonoBehaviour
    {
        [Header("Dimensions")]
        private bool[,] _buildable;
        private bool[,] _walkable;
        private IGridOccupant[,] _occupants;
        
        [Tooltip("Nombre de cases sur l'axe X, bordure cachée comprise")]
        [SerializeField, Range(10,200)] private int _width = 92;
        [Tooltip("Nombre de cases sur l'axe Z, bordure cachée comprise")]
        [SerializeField, Range(10, 200)] private int _height = 60;

        private readonly Dictionary<IGridOccupant, List<Vector2Int>> _occupiedCells = new();
        
        /// <summary>
        /// Au démarage du script, remplis les variables _buildable et _walkable de true et laisse
        /// _occupants remplie de Null. 
        /// </summary>
        private void Awake()
        {
            _buildable = new bool[_width, _height];
            _walkable = new bool[_width, _height];
            _occupants = new IGridOccupant[_width, _height];
            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _height; z++)
                {
                    _buildable[x, z] = true;
                    _walkable[x, z] = true;
                }
            }
        }

        /// <summary>
        /// Déssine à l'initialisation du script les cases qui serviront
        /// a voir où placer les défenses
        /// </summary>
        private void OnDrawGizmos()
        {
            Vector3 center = transform.position;
            Vector3 size = new Vector3(_width, 0f, _height);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(center, size);

            for (int i = 0; i <= _width; i++)
            {
                float x = i - _width / 2;
                Vector3 start = transform.position + new Vector3(x, 0f, -_height / 2);
                Vector3 end = transform.position + new Vector3(x, 0f, +_height / 2);
                Gizmos.color = Color.grey;
                Gizmos.DrawLine(start, end);
            }
            for (int  i = 0;  i <= _height;  i++)
            {
                float z = i - _height / 2;
                Vector3 start = transform.position + new Vector3(-_width/2, 0f, z);
                Vector3 end = transform.position + new Vector3(_width / 2, 0f, z);
                Gizmos.color = Color.grey;
                Gizmos.DrawLine(start, end);
            }
        }

        /// <summary>
        /// Donne la céllule en fonction d'une coordonner dans le monde.
        /// </summary>
        /// <param name="worldPosition">Position en x,y,z dand le monde</param>
        /// <returns>La position de la céllule dans les tableaux en 2D</returns>
        public Vector2Int WorldToCell(Vector3 worldPosition)
        {
            Vector3 localPosition = worldPosition - transform.position;
            int x = Mathf.FloorToInt(localPosition.x) + _width / 2;
            int z = Mathf.FloorToInt(localPosition.z) + _height / 2;
            return new Vector2Int(x, z);
        }

        /// <summary>
        /// L'inverse de WordToCell, donne la position d'une céllule
        /// en position dans le monde.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public Vector3 CellToWorld(Vector2Int cell)
        {
            float x = cell.x - _width / 2 + 0.5f;
            float z = cell.y - _height / 2 + 0.5f;
            return transform.position + new Vector3(x, 0f, z);
        }

        public bool IsInside(Vector2Int cell)
        {
            return cell.x >= 0 && cell.x < _width && cell.y >= 0 && cell.y < _height;
        }

        public bool IsBuildable(Vector2Int cell)
        {
            return IsInside(cell) && _buildable[cell.x, cell.y] && 
                   _occupants[cell.x, cell.y] == null;
        }

        public bool IsWalkable(Vector2Int cell)
        {
            IGridOccupant occupant = _occupants[cell.x, cell.y];
            return IsInside(cell) && _walkable[cell.x, cell.y] && 
                   (occupant == null || !occupant.IsBlockingPath);
        }
        
        [ContextMenu("Tester les conversions")]
        private void TestConversions()
        {
            Debug.Log(WorldToCell(new Vector3(0.3f, 0f, 0.3f)));
            Debug.Log(WorldToCell(new Vector3(-46f, 0f, -30f)));
            Debug.Log(CellToWorld(new Vector2Int(46, 30)));
            Debug.Log(IsInside(new Vector2Int(0,0)));
            Debug.Log(IsInside(new Vector2Int(91,59)));
            Debug.Log(IsInside(new Vector2Int(92,0)));
            Debug.Log(IsInside(new Vector2Int(-1,59)));
        }
        
    }
}
