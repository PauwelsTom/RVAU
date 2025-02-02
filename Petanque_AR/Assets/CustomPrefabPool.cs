using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;

public class CustomPrefabPool : MonoBehaviour, IPunPrefabPool
{
    // Liste des prefabs que tu souhaites g�rer. 
    // Tu pourras les assigner directement dans l'inspecteur.
    [Tooltip("Liste des prefabs � g�rer par le pool (leurs noms doivent correspondre � ceux utilis�s dans PhotonNetwork.Instantiate)")]
    public List<GameObject> prefabs;

    // Dictionnaire pour acc�der rapidement aux prefabs par leur nom.
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

    void Awake()
    {
        // Remplit le dictionnaire avec les prefabs assign�s.
        foreach (GameObject prefab in prefabs)
        {
            if (prefab != null)
            {
                // Assure-toi que le nom du prefab correspond � celui que tu vas utiliser dans PhotonNetwork.Instantiate.
                prefabDictionary[prefab.name] = prefab;
            }
        }

        // Assigne ce pool personnalis� � PhotonNetwork.
        PhotonNetwork.PrefabPool = this;
    }

    /// <summary>
    /// Instancie le prefab correspondant � prefabId � la position et rotation indiqu�es.
    /// </summary>
    /// <param name="prefabId">Le nom du prefab � instancier.</param>
    /// <param name="position">La position d'instanciation.</param>
    /// <param name="rotation">La rotation d'instanciation.</param>
    /// <returns>La GameObject instanci�e.</returns>
    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        if (prefabDictionary.TryGetValue(prefabId, out GameObject prefab))
        {
            // Instancie le prefab � la position et rotation souhait�es
            GameObject instance = GameObject.Instantiate(prefab, position, rotation);
            // D�sactive l'objet pour respecter l'exigence de Photon
            instance.SetActive(false);
            return instance;
        }
        Debug.LogError("CustomPrefabPool: Prefab not found: " + prefabId);
        return null;
    }

    /// <summary>
    /// D�truit l'objet pass� en param�tre.
    /// </summary>
    /// <param name="gameObject">La GameObject � d�truire.</param>
    public void Destroy(GameObject gameObject)
    {
        UnityEngine.Object.Destroy(gameObject);
    }
}
