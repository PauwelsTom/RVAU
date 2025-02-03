using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CochonnetController cochonnetController;

    void Start()
    {
        if (cochonnetController == null)
        {
            cochonnetController = FindObjectOfType<CochonnetController>();
            if (cochonnetController == null)
            {
                Debug.LogError("Aucun CochonnetController n'a été trouvé dans la scène.");
            }
            else
            {
                Debug.Log("CochonnetController trouvé automatiquement : " + cochonnetController.gameObject.name);
            }
        }
    }

    // Méthode appelée par le bouton UI pour ramasser le cochonnet.
    public void RamasserCochonnet()
    {
        if (cochonnetController != null)
        {
            cochonnetController.PickUp(new Vector3(10, 0, 10));
        }
        else
        {
            Debug.LogError("Référence au CochonnetController manquante.");
        }
    }

    // Méthode pour lancer le cochonnet
    public void LancerCochonnet()
    {
        if (cochonnetController != null)
        {
            cochonnetController.Launch();
        }
        else
        {
            Debug.LogError("Référence au CochonnetController manquante.");
        }
    }
}
