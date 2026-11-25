using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Contrôles")]
        [Tooltip("Action Gameplay/Move du fichier GameInputActions")]
        [SerializeField] private InputActionReference _moveAction;

        [Header("Déplacement")]
        [Tooltip("Vitesse de la sorcière, en cases par seconde")]
        [SerializeField, Range(0.5f, 10f)] private float _moveSpeed = 4f;

        private CharacterController _characterController;
        private Vector2 _moveInput;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            _moveInput = _moveAction.action.ReadValue<Vector2>();

            // Le plateau est dans le plan XZ : le haut de l'écran correspond à +Z.
            Vector3 direction = new Vector3(_moveInput.x, 0f, _moveInput.y);

            // Limité à 1 pour que la diagonale ne soit pas plus rapide,
            // tout en gardant un déplacement lent quand le stick est à mi-course.
            direction = Vector3.ClampMagnitude(direction, 1f);

            _characterController.Move(direction * (_moveSpeed * Time.deltaTime));
        }
    }
}