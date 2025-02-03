using Photon.Pun;
using UnityEngine;

public class BallController : MonoBehaviourPun, IPunObservable
{
    public bool pickedUp = false;   // Indique si la boule a été ramassée
    public bool launched = false;     // Indique si la boule a été lancée
    public float launchForce = 500f;  // Force appliquée lors du lancement

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Méthode appelée pour ramasser la boule.
    // pickupPosition est la position à laquelle la boule est "stockée" (dans un inventaire virtuel par exemple).
    public void PickUp(Vector3 pickupPosition)
    {
        if (pickedUp) return;  // Si déjà ramassée, ne rien faire.
        pickedUp = true;
        Debug.Log("PickUp() appelé, pickedUp = " + pickedUp);
        // Optionnel : repositionner la boule (si vous souhaitez la stocker à un endroit fixe)
        transform.position = pickupPosition;
        // Appel de l'RPC qui désactive la boule sur tous les clients.
        photonView.RPC("PickUpRPC", RpcTarget.AllBuffered);
    }

    // RPC qui désactive la boule pour tout le monde.
    [PunRPC]
    void PickUpRPC()
    {
        gameObject.SetActive(false);
        Debug.Log("PickUpRPC: la boule est désactivée (ramassée).");
    }

    // Méthode appelée pour lancer la boule.
    public void Launch()
    {
        if (launched) return;
        launched = true;
        // Réactive la boule pour le propriétaire afin qu'elle réapparaisse lors du lancement.
        // Ici, on active systématiquement pour l'exemple.
        gameObject.SetActive(true);
        Debug.Log("Launch() appelé, la boule est réactivée et lancée.");

        if (rb != null)
        {
            // Applique une force dans la direction "forward" de l'objet.
            rb.AddForce(transform.forward * launchForce);
        }
    }

    // Synchronisation de la position et de la rotation via Photon.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
