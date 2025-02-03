using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Liste des boules instanciées pour le joueur local.
    public List<GameObject> myBalls = new List<GameObject>();

    // Le point de spawn pour les boules (à assigner dans l'inspecteur).
    public Transform ballSpawnPoint;

    // Espacement entre les boules (pour qu'elles ne se chevauchent pas lors du spawn).
    public Vector3 ballSpacing = new Vector3(1.5f, 0, 0);

    // Nom du prefab de la boule (celui qui est géré par votre CustomPrefabPool ou placé dans Resources).
    public string ballPrefabName = "Boule_1";

    public GameManager gameManager;

    // Connexion à Photon lors du démarrage.
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connecté à Photon.");
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom("PetanqueRoom", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Salle rejointe.");

        if (PhotonNetwork.IsMasterClient)
        {
            Vector3 cochonnetPosition = new Vector3(0, 0, 3);
            GameObject cochonnet = PhotonNetwork.Instantiate("Cochonnet", cochonnetPosition, Quaternion.identity);

            CochonnetController controller = cochonnet.GetComponent<CochonnetController>();
            if (gameManager != null && controller != null)
            {
                gameManager.cochonnetController = controller;
                Debug.Log("CochonnetController assigné au GameManager.");
            }
        }

        // Lorsqu'un nouveau joueur se connecte, il spawn 3 boules.
        // Le point de spawn et l'espacement vous permettent de positionner les boules.
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPos = ballSpawnPoint.position + i * ballSpacing;
            GameObject ball = PhotonNetwork.Instantiate(ballPrefabName, spawnPos, Quaternion.identity);
            myBalls.Add(ball);
        }
    }

    // Méthode pour lancer la première boule non lancée.
    public void LancerBoule()
    {
        foreach (GameObject ball in myBalls)
        {
            BallController bc = ball.GetComponent<BallController>();
            PhotonView pv = ball.GetComponent<PhotonView>();

            // Affichage des valeurs pour déboguer
            Debug.Log("Boule: pickedUp=" + bc.pickedUp + ", launched=" + bc.launched + ", IsMine=" + pv.IsMine);

            if (bc != null && bc.pickedUp && !bc.launched && pv.IsMine)
            {
                Debug.Log("On va lancer cette boule.");
                bc.Launch();
                return; // Lance la première boule disponible.
            }
        }
        Debug.Log("Toutes les boules ont déjà été lancées.");
    }

    // Méthode pour ramasser (réinitialiser) toutes les boules du joueur.
    public void RamasserBoules()
    {
        foreach (GameObject ball in myBalls)
        {
            BallController bc = ball.GetComponent<BallController>();
            if (bc != null && !bc.pickedUp)
            {
                Debug.Log("RAMASSE.");
                // Transfert de propriété pour que le joueur local puisse gérer la boule
                ball.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                // On appelle ensuite la méthode de ramassage, en passant par exemple ballSpawnPoint.position
                bc.PickUp(ballSpawnPoint.position);

            }
        }
    }
}
