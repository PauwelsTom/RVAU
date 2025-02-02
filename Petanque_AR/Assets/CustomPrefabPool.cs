using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;

public class CustomPrefabPool : MonoBehaviour, IPunPrefabPool
{
    // Liste des prefabs que tu souhaites gérer. 
    // Tu pourras les assigner directement dans l'inspecteur.
    [Tooltip("Liste des prefabs à gérer par le pool (leurs noms doivent correspondre à ceux utilisés dans PhotonNetwork.Instantiate)")]
    public List<GameObject> prefabs;

    // Dictionnaire pour accéder rapidement aux prefabs par leur nom.
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

    void Awake()
    {
        // Remplit le dictionnaire avec les prefabs assignés.
        foreach (GameObject prefab in prefabs)
        {
            if (prefab != null)
            {
                // Assure-toi que le nom du prefab correspond à celui que tu vas utiliser dans PhotonNetwork.Instantiate.
                prefabDictionary[prefab.name] = prefab;
            }
        }

        // Assigne ce pool personnalisé à PhotonNetwork.
        PhotonNetwork.PrefabPool = this;
    }

    /// <summary>
    /// Instancie le prefab correspondant à prefabId à la position et rotation indiquées.
    /// </summary>
    /// <param name="prefabId">Le nom du prefab à instancier.</param>
    /// <param name="position">La position d'instanciation.</param>
    /// <param name="rotation">La rotation d'instanciation.</param>
    /// <returns>La GameObject instanciée.</returns>
    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        if (prefabDictionary.TryGetValue(prefabId, out GameObject prefab))
        {
            // Instancie le prefab à la position et rotation souhaitées
            GameObject instance = GameObject.Instantiate(prefab, position, rotation);
            // Désactive l'objet pour respecter l'exigence de Photon
            instance.SetActive(false);
            return instance;
        }
        Debug.LogError("CustomPrefabPool: Prefab not found: " + prefabId);
        return null;
    }

    /// <summary>
    /// Détruit l'objet passé en paramètre.
    /// </summary>
    /// <param name="gameObject">La GameObject à détruire.</param>
    public void Destroy(GameObject gameObject)
    {
        UnityEngine.Object.Destroy(gameObject);
    }
}
