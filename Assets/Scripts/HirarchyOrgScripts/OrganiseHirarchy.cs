using UnityEngine;


[ExecuteInEditMode]
public class OrganiseHirarchy : MonoBehaviour
{

    [SerializeField] string myChildrenTag = "Eneter tag here";
    [SerializeField] bool running = false;

    void Update()
    {

        if (running && Application.isEditor)
        {
            ParentThingsToMe(myChildrenTag);
        }
    }

    void ParentThingsToMe(string childrenTag)
    {
        GameObject[] lostChildren = GameObject.FindGameObjectsWithTag(childrenTag);
        foreach (var lostChild in lostChildren)
        {
            if (lostChild.transform.parent != this.transform)
                lostChild.transform.parent = this.transform;
        }

    }
}
