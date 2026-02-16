using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

public class GridSystem : MonoBehaviour
{
    public List<GameObject> gridObjects;

    void Awake()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>(true);
        List<GameObject> childrenGameObjects = transforms.Where(t => t != this.transform).Select(t => t.gameObject).ToList();
        gridObjects = childrenGameObjects;
        
        for(int i = 0; i < gridObjects.Count; i++)
        {
            gridObjects[i].AddComponent<GridBlocks>();
            gridObjects[i].GetComponent<GridBlocks>().defaultSprite = gridObjects[i].GetComponent<ProceduralImage>().sprite;
            gridObjects[i].GetComponent<GridBlocks>().isAllowed = UnityEngine.Random.value > 0.5f;
        }
    }

    void Start()
    {
        foreach(var objects in gridObjects)
        {
            objects.GetComponent<ProceduralImage>().sprite = null;
        }

        gridObjects.Clear();
    }
}
