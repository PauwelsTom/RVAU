using Photon.Pun;
using UnityEngine;

public class BouleSync : MonoBehaviourPun, IPunObservable
{
    // Cette m�thode RPC sera appel�e pour d�placer la boule
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
            // Le propri�taire envoie la position et la rotation
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Les autres clients re�oivent et mettent � jour
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
