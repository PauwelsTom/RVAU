using Photon.Pun;
using UnityEngine;

public class BouleSync : MonoBehaviourPun, IPunObservable
{
    // Cette méthode RPC sera appelée pour déplacer la boule
    [PunRPC]
    public void MoveBoule(Vector3 newPos)
    {
        transform.position = newPos;
    }

    // Synchronisation de la position et rotation
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Le propriétaire envoie la position et la rotation
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Les autres clients reçoivent et mettent à jour
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
