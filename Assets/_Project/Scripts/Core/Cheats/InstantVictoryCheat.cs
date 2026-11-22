using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Core.Cheats
{
    public class InstantVictoryCheat : MonoBehaviour
    {
        [Tooltip("Touche de victoire instantanée")]
        [SerializeField] private InputActionReference _instantVictoryAction;

        private void OnEnable()
        {
            _instantVictoryAction.action.performed += OnInstantVictoryPerformed;
        }
        
        private void OnDisable()
        {
            _instantVictoryAction.action.performed -= OnInstantVictoryPerformed;
        }

        private void OnInstantVictoryPerformed(InputAction.CallbackContext context)
        {
            GameManager gameManager = GameManager.Instance;

            if (gameManager == null || gameManager.IsPaused)
            {
                return;
            }

            gameManager.TriggerVictory();
        }
    }
}