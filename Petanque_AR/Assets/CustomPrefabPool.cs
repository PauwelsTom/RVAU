using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;

public class CustomPrefabPool : MonoBehaviour, IPunPrefabPool
{
    [Tooltip("Liste des prefabs à gérer par le pool (leurs noms doivent correspondre aux chaînes passées à PhotonNetwork.Instantiate)")]
    public List<GameObject> prefabs;

    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

    void Awake()
    {
        // Remplit le dictionnaire avec les prefabs assignés dans l'inspecteur.
        foreach (GameObject prefab in prefabs)
        {
            if (prefab != null)
            {
                // Utilise prefab.name pour la clé.
                prefabDictionary[prefab.name] = prefab;
            }
        }
        PhotonNetwork.PrefabPool = this;
    }

    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        if (prefabDictionary.TryGetValue(prefabId, out GameObject prefab))
        {
            GameObject instance = Instantiate(prefab, position, rotation);
            // On désactive l'objet avant de le retourner pour respecter les exigences de Photon.
            instance.SetActive(false);
            return instance;
        }
        Debug.LogError("CustomPrefabPool: Prefab not found: " + prefabId);
        return null;
    }

    public void Destroy(GameObject gameObject)
    {
        UnityEngine.Object.Destroy(gameObject);
    }
}
