using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CochonnetController cochonnetController;

    public Transform camera;

    void Start()
    {
        if (cochonnetController == null)
        {
            cochonnetController = FindObjectOfType<CochonnetController>();
            if (cochonnetController == null)
            {
                Debug.LogError("Aucun CochonnetController n'a �t� trouv� dans la sc�ne.");
            }
            else
            {
                Debug.Log("CochonnetController trouv� automatiquement : " + cochonnetController.gameObject.name);
            }
        }
    }

    // M�thode appel�e par le bouton UI pour ramasser le cochonnet.
    public void RamasserCochonnet()
    {
        if (cochonnetController != null)
        {
            cochonnetController.PickUp(new Vector3(10, 0, 10));
        }
        else
        {
            Debug.LogError("R�f�rence au CochonnetController manquante.");
        }
    }

    // M�thode pour lancer le cochonnet
    public void LancerCochonnet()
    {
        if (cochonnetController != null)
        {
            cochonnetController.Launch(camera);
        }
        else
        {
            Debug.LogError("R�f�rence au CochonnetController manquante.");
        }
    }
}
