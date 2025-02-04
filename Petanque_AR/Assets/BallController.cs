using Photon.Pun;
using UnityEngine;

public class BallController : MonoBehaviourPun, IPunObservable
{
    public bool pickedUp = true;   // Indique si la boule a �t� ramass�e
    public bool launched = false;     // Indique si la boule a �t� lanc�e
    public float launchForce = 500f;  // Force appliqu�e lors du lancement

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // M�thode appel�e pour ramasser la boule.
    // pickupPosition est la position � laquelle la boule est "stock�e" (dans un inventaire virtuel par exemple).
    public void PickUp(Vector3 pickupPosition)
    {
        if (pickedUp) return;
        pickedUp = true;
        launched = false;
        Debug.Log("Cochonnet ramass� !");
        transform.position = pickupPosition;

        if (photonView == null)
        {
            Debug.LogError("PhotonView est null sur ce GameObject !");
            return;
        }

        photonView.RPC("PickUpRPC", RpcTarget.AllBuffered);
    }

    // RPC qui d�sactive la boule pour tout le monde.
    [PunRPC]
    void PickUpRPC()
    {
        gameObject.SetActive(false);
        Debug.Log("PickUpRPC: la boule est d�sactiv�e (ramass�e).");
    }

    // M�thode appel�e pour lancer la boule.
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
