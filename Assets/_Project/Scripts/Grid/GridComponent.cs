using System;
using UnityEngine;

namespace _Project.Scripts.Grid
{
    public class GridComponent : MonoBehaviour
    {
        [Header("Dimensions")]
        [Tooltip("Attend ça va s'afficher dans les properties Unity ça, mais c'est trop cool!!")]
        [SerializeField, Range(10,200)] private int _width = 92;
        
        [Tooltip("Nombre de cases sur l'axe Z, bordure cachée comprise")]
        [SerializeField, Range(10, 200)] private int _height = 60;

        private void OnDrawGizmos()
        {
            Vector3 centre = transform.position;
            Vector3 taille = new Vector3(_width, 0f, _height);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(centre, taille);
        }
    }
}
