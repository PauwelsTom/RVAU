using Photon.Pun;
using UnityEngine;

public class CochonnetController : MonoBehaviourPun, IPunObservable
{
    public bool pickedUp = false;   // Indique si le cochonnet a �t� ramass�
    public bool launched = false;     // Indique si le cochonnet a �t� lanc�
    public float launchForce = 200f;  // Force appliqu�e lors du lancement

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Aucun Rigidbody trouv� sur " + gameObject.name);
        }
    }

    // M�thode appel�e pour ramasser le cochonnet.
    // Ici, on utilise la m�me logique que pour les boules.
    public void PickUp(Vector3 pickupPosition)
    {
        if (pickedUp) return;
        pickedUp = true;
        launched = false;
        Debug.Log("Cochonnet ramass� !");
        // On peut repositionner le cochonnet si n�cessaire (par exemple, dans un inventaire virtuel)
        transform.position = pickupPosition;

        // Assurez-vous que photonView n'est pas null (le prefab doit avoir un PhotonView)
        if (photonView == null)
        {
            Debug.LogError("PhotonView est null sur ce GameObject !");
            return;
        }
        // Appel de l'RPC pour d�sactiver le cochonnet sur tous les clients.
        photonView.RPC("PickUpRPC", RpcTarget.AllBuffered);
    }

    // RPC qui d�sactive le cochonnet pour tous les clients.
    [PunRPC]
    void PickUpRPC()
    {
        gameObject.SetActive(false);
        Debug.Log("PickUpRPC: Cochonnet d�sactiv� (ramass�) sur tous les clients.");
    }

    // M�thode appel�e pour lancer le cochonnet.
    public void Launch(Transform spawnPoint)
    {
        if (launched) return;
        launched = true;
        pickedUp = false; 
        // Réactive la boule pour le propriétaire afin qu'elle réapparaisse lors du lancement.
        gameObject.SetActive(true);
        Debug.Log("Launch() appelé, la boule est réactivée et lancée.");
        
        if (rb != null)
        {
            // Initialise la position de la boule à la position de spawnPoint
            transform.position = spawnPoint.position;
            // transform.rotation = spawnPoint.rotation;

            // Mettre la vitesse à 0
            rb.linearVelocity = Vector3.zero;

            // Applique une force dans la direction "forward" de spawnPoint
            rb.AddForce(spawnPoint.forward * launchForce);
        } else {
            Debug.Log("Y'a pas le rigidBody mon gate");
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
