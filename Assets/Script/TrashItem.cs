using UnityEngine;
using UnityEngine.EventSystems;
public enum TrashType
{
    Paper,
    Plastic
}

public class TrashItem : MonoBehaviour
{
    [SerializeField] private TrashType trashType;
    [SerializeField] private GameObject PaperField;
    [SerializeField] private GameObject PlasticField;

    void Start()
    {

    }

  // Update is called once per frame
    void Update()
    {
        
    }

}