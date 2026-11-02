using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Core.Input
{
    // Exemple de lecture des contrôles : tir continu, attaque chargée, boutons
    public class InputTester : MonoBehaviour
    {
        [SerializeField] private InputActionReference _moveAction;
        [SerializeField] private InputActionReference _attackAction;
        [SerializeField] private InputActionReference _pauseAction;
        [SerializeField] private InputActionReference _godModeAction;

        private void OnEnable()
        {
            _pauseAction.action.performed += OnPausePerformed;
            _godModeAction.action.performed += OnGodModePerformed;

            // Méthode 2 (attaque chargée) : nécessite l'interaction Hold sur Attack dans l'asset,
            // et de retirer le test IsPressed() dans Update.
            // _attackAction.action.started += OnAttackStarted;
            // _attackAction.action.performed += OnAttackPerformed;
            // _attackAction.action.canceled += OnAttackCanceled;
        }

        private void OnDisable()
        {
            _pauseAction.action.performed -= OnPausePerformed;
            _godModeAction.action.performed -= OnGodModePerformed;

            // Méthode 2 (attaque chargée)
            // _attackAction.action.started -= OnAttackStarted;
            // _attackAction.action.performed -= OnAttackPerformed;
            // _attackAction.action.canceled -= OnAttackCanceled;
        }

        private void Update()
        {
            Vector2 move = _moveAction.action.ReadValue<Vector2>();
            if (move != Vector2.zero)
            {
                Debug.Log($"Move : {move}");
            }

            // Méthode 1 (tir continu) : on attaque tant que le bouton est enfoncé.
            // La cadence sera gérée par le composant Attaque (SCRUM-173).
            if (_attackAction.action.IsPressed())
            {
                Debug.Log("Attack");
            }
        }

        private void OnPausePerformed(InputAction.CallbackContext context) => Debug.Log("Pause");
        private void OnGodModePerformed(InputAction.CallbackContext context) => Debug.Log("GodMode");

        // Méthode 2 (attaque chargée)
        // started : le bouton vient d'être enfoncé, la charge commence.
        // private void OnAttackStarted(InputAction.CallbackContext context) => Debug.Log("Attack : début de la charge");

        // performed : le bouton a été maintenu pendant toute la Hold Time, l'attaque part.
        // private void OnAttackPerformed(InputAction.CallbackContext context) => Debug.Log("Attack : attaque chargée lancée");

        // canceled : le bouton a été relâché (avant la fin de la charge, ou après l'attaque).
        // private void OnAttackCanceled(InputAction.CallbackContext context) => Debug.Log("Attack : bouton relâché");
    }
}
