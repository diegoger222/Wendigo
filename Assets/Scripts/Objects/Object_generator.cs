using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Object_creation;

public class Object_generator : MonoBehaviour
{
    // All possible instances
    static public List<GameObject> instances;
    
    GameObject instance;

    static public string instances_name = "Object_instances";
    
    // Start is called before the first frame update
    void Start()
    {
        // If instances do not exist yet, get all
        // children instances of GameObject called
        // "Object_instances"
        if(instances == null)
        {
            instances = new List<GameObject>();

            GameObject instance_parent = GameObject.Find(instances_name);

            foreach (Transform child in instance_parent.transform)
            {
                instances.Add(child.gameObject);
            }

            if(instance_parent == null)
            {
                Debug.LogError("Object_instances not found");
            } else {
                print("Found " + instances.Count + " instances");
            }
        }

    }

    // Add an object 
    public void generate_object(){
        // Get a random instance
        int rnd = Object_creation.random.Next(0, instances.Count-1);

        this.instance = instances[rnd];

        // Generate an instance of this.instance as a
        // child of this GameObject
        this.instance = Instantiate(this.instance,
                                    this.transform.position,
                                    this.transform.rotation);
    }
}
