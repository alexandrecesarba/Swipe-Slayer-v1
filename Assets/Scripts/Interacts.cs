using UnityEngine;

public class Interacts : MonoBehaviour {

    #region Variables
    MovingObject movingObject;
    #endregion

    #region Unity Methods

    void OnEnable()
    {
        movingObject = GetComponent<MovingObject>();
        movingObject.OnHit += Interact;
    }

    void OnDisable()
    {
        movingObject.OnHit -= Interact;
    }
    private void Start()
    {
    }

    public void Interact(GameObject targetObject)
    {
        if (targetObject != null)
        {
            IInteractable interactableComponent = targetObject.GetComponent<IInteractable>();
            if (interactableComponent != null)
            {
                interactableComponent.Interact(gameObject);
                Debug.Log(targetObject.name + " foi interagido!");
            }
            else
            {
                Debug.Log("Erro: Não foi possível acessar o script IInteractable");
            }
        }
    }

    #endregion
}
