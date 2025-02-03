using Photon.Pun;
using UnityEngine;

public class CochonnetController : MonoBehaviourPun, IPunObservable
{
    public bool pickedUp = false;   // Indique si le cochonnet a été ramassé
    public bool launched = false;     // Indique si le cochonnet a été lancé
    public float launchForce = 200f;  // Force appliquée lors du lancement

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Aucun Rigidbody trouvé sur " + gameObject.name);
        }
    }

    // Méthode appelée pour ramasser le cochonnet.
    // Ici, on utilise la même logique que pour les boules.
    public void PickUp(Vector3 pickupPosition)
    {
        if (pickedUp) return;
        pickedUp = true;
        Debug.Log("Cochonnet ramassé !");
        // On peut repositionner le cochonnet si nécessaire (par exemple, dans un inventaire virtuel)
        transform.position = pickupPosition;

        // Assurez-vous que photonView n'est pas null (le prefab doit avoir un PhotonView)
        if (photonView == null)
        {
            Debug.LogError("PhotonView est null sur ce GameObject !");
            return;
        }
        // Appel de l'RPC pour désactiver le cochonnet sur tous les clients.
        photonView.RPC("PickUpRPC", RpcTarget.AllBuffered);
    }

    // RPC qui désactive le cochonnet pour tous les clients.
    [PunRPC]
    void PickUpRPC()
    {
        gameObject.SetActive(false);
        Debug.Log("PickUpRPC: Cochonnet désactivé (ramassé) sur tous les clients.");
    }

    // Méthode appelée pour lancer le cochonnet.
    public void Launch()
    {
        if (!pickedUp || launched) return;
        launched = true;
        // Réactive le cochonnet pour le propriétaire afin qu'il réapparaisse lors du lancement.
        gameObject.SetActive(true);
        Debug.Log("Launch() appelé, le cochonnet est réactivé et lancé.");

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
