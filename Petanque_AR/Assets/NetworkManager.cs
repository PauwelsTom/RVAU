using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    //public GameObject BacPetanque;
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

            //PhotonNetwork.Instantiate("BacPetanque", Vector3.zero, Quaternion.identity);

            //PhotonNetwork.Instantiate("Cochonnet", new Vector3(0, 0, 1), Quaternion.identity);

            //PhotonNetwork.Instantiate("Boule_1", new Vector3(-1, 0, 2), Quaternion.identity);
            //PhotonNetwork.Instantiate("Boule_2", new Vector3(1, 0, 2), Quaternion.identity);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Déconnecté de Photon pour cause {0}", cause);
    }

    void Update()
    {
        
    }
}
