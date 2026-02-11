using UnityEngine;
public class MakeMany : MonoBehaviour
{
    public GameObject block;
    public int numX = 10;
    public int numY = 4;
    public int numZ = 2;

    public float numXOffset = 1;
    public float numYOffset = 1;
    public float numZOffset = 1;
  
    public Vector3 randomOrientation; // = Euler.Identity;
    public Vector3 randomPosition; // = Vector3.Zero;

    public bool disableCollider = false;

    private BoxCollider bc;
    private int blocknumber = 0;
    void Start()
    {
        // Create Many
        for (float z = 0; z < numZ; z++)
        {
            for (float y = 0; y < numY; y++)
            {
                for (float x = 0; x < numX; x++)
                {
                    Vector3 offset = new Vector3(
                        Random.Range(-randomPosition.x, randomPosition.x),
                        Random.Range(-randomPosition.y, randomPosition.y),
                        Random.Range(-randomPosition.z, randomPosition.z)
                    );
                    float px = x * numXOffset;
                    float py = y * numYOffset;
                    float pz = z * numZOffset;
                    Vector3 pos = new Vector3(px, py, pz);
                    pos += offset;
                    Vector3 rotoffset = new Vector3(
                        Random.Range(-randomOrientation.x, randomOrientation.x),
                        Random.Range(-randomOrientation.y, randomOrientation.y),
                        Random.Range(-randomOrientation.z, randomOrientation.z)
                    );
                    Quaternion rot = Quaternion.identity;
                    rot.eulerAngles += rotoffset;
                    GameObject obj = Instantiate(block, this.transform);
                    obj.name = block.name + '_' + x + '_' + y + '_' + z;
                    //print(obj.name +" :MakeMany: x:"+ x + ", y:" + y + ", z:" + z + ", pos = "+pos + ", pz:"+ pz);
                    obj.transform.localPosition = pos;
                    obj.transform.localRotation = rot;
                }
            }
            blocknumber++;
            // block.name = block.name + blocknumber.ToString();
        }
        // return;

        // Create a big merged collider if necessary
        if (disableCollider)
            return;
        else
        {
            bc = GetComponent<BoxCollider>();
            if (bc == null)
                bc = this.gameObject.AddComponent<BoxCollider>();

            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
            Renderer thisRenderer = transform.GetComponent<Renderer>();
            if (thisRenderer != null)
            {
                bounds.Encapsulate(thisRenderer.bounds);
                bc.center = bounds.center - transform.position;
                bc.size = bounds.size * 1.5f;
            }

            var allDescendants = gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform desc in allDescendants)
            {
                Renderer childRenderer = desc.GetComponent<Renderer>();
                if (childRenderer != null)
                {
                    bounds.Encapsulate(childRenderer.bounds);
                    bc.center = bounds.center - transform.position;
                    bc.size = bounds.size * 1.5f;
                }
            }
        }
    }
}