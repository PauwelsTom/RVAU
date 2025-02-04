using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Liste des boules instanci�es pour le joueur local.
    public List<GameObject> myBalls = new List<GameObject>();

    // Le point de spawn pour les boules (� assigner dans l'inspecteur).
    public Transform ballSpawnPoint;

    // Espacement entre les boules (pour qu'elles ne se chevauchent pas lors du spawn).
    public Vector3 ballSpacing = new Vector3(1.5f, 0, 0);

    // Nom du prefab de la boule (celui qui est g�r� par votre CustomPrefabPool ou plac� dans Resources).
    public string ballPrefabName = "Boule_1";

    public GameManager gameManager;

    // Connexion � Photon lors du d�marrage.
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connect� � Photon.");
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom("PetanqueRoom", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Salle rejointe.");

        if (PhotonNetwork.IsMasterClient)
        {
            Vector3 cochonnetPosition = ballSpawnPoint.position;
            GameObject cochonnet = PhotonNetwork.Instantiate("Cochonnet", cochonnetPosition, Quaternion.identity);

            CochonnetController controller = cochonnet.GetComponent<CochonnetController>();
            if (gameManager != null && controller != null)
            {
                gameManager.cochonnetController = controller;
                Debug.Log("CochonnetController assign� au GameManager.");
            }
        }

        // Lorsqu'un nouveau joueur se connecte, il spawn 3 boules.
        // Le point de spawn et l'espacement vous permettent de positionner les boules.
        for (int i = 0; i < 3; i++)
        {
            // Vector3 spawnPos = ballSpawnPoint.position + i * ballSpacing;
            Vector3 spawnPos = ballSpawnPoint.position;
            GameObject ball = PhotonNetwork.Instantiate(ballPrefabName, spawnPos, Quaternion.identity);
            myBalls.Add(ball);
        }
    }

    // M�thode pour lancer la premi�re boule non lanc�e.
    public void LancerBoule()
    {
        foreach (GameObject ball in myBalls)
        {
            BallController bc = ball.GetComponent<BallController>();
            PhotonView pv = ball.GetComponent<PhotonView>();

            // Affichage des valeurs pour d�boguer
            Debug.Log("Boule: pickedUp=" + bc.pickedUp + ", launched=" + bc.launched + ", IsMine=" + pv.IsMine);

            if (bc != null && bc.pickedUp && !bc.launched && pv.IsMine)
            {
                Debug.Log("On va lancer cette boule.");
                bc.Launch(ballSpawnPoint);
                return; // Lance la premi�re boule disponible.
            }
        }
        Debug.Log("Toutes les boules ont d�j� �t� lanc�es.");
    }

    // M�thode pour ramasser (r�initialiser) toutes les boules du joueur.
    public void RamasserBoules()
    {
        foreach (GameObject ball in myBalls)
        {
            BallController bc = ball.GetComponent<BallController>();
            if (bc != null && !bc.pickedUp)
            {
                Debug.Log("RAMASSE.");
                // Transfert de propri�t� pour que le joueur local puisse g�rer la boule
                ball.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                // On appelle ensuite la m�thode de ramassage, en passant par exemple ballSpawnPoint.position
                bc.PickUp(ballSpawnPoint.position);

            }
        }
    }
}
