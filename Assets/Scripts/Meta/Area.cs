using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static PlayerController;

public class Area : MonoBehaviour
{
    public bool inside = false;
    public bool isSafe = false;

    static PlayerController player;
    static Wendigo wendigo;
    static GameObject wendigo_go;
    static GameObject wendigo_instance;

    const float inside_area_radius = 0.95f;

    public float wendigo_anger = 0.5f;
    public float wendigo_stealth;
    public float wendigo_thirst;
    public float wendigo_health;

    // Start is called before the first frame update
    void Start()
    {
        if(Area.player == null)
        {
            Area.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

            if(Area.player == null)
            {
                Debug.LogError("Object tagged Player containing PlayerController script not found");
            }
        }
        if(Area.wendigo == null)
        {
            Area.wendigo = GameObject.FindGameObjectWithTag("Wendigo").GetComponent<Wendigo>();

            if(Area.wendigo == null)
            {
                Debug.LogError("Object tagged Wendigo containing Wendigo script not found");
            }
        }
        if(Area.wendigo_go == null){
            Area.wendigo_go = GameObject.FindGameObjectWithTag("Wendigo");

            if(Area.wendigo_go == null)
            {
                Debug.LogError("Object tagged Wendigo not found");
            }
        }

        if(! this.inside){
            // Instance a copy of self, set it to be a child of the parent
            // and scale it down in the x and z directions.
            GameObject clone = Instantiate(gameObject, transform.parent);

            // Get the Area component, and set inside as true
            Area area = clone.GetComponent<Area>();
            area.inside = true;

            clone.transform.localScale = new Vector3(
                    clone.transform.localScale.x * inside_area_radius,
                    clone.transform.localScale.y,
                    clone.transform.localScale.z * inside_area_radius
                );  
        }
    }

    // On collision with the player
    private void OnTriggerEnter(Collider _)
    {
        if(this.inside)
        {
            Area.player.area = gameObject.name;
            Area.player.time_in_area = 0;

            // Generate the wendigo, and set the wendigo's area to the player's area
            // randomly
            Area.wendigo.area = this;
            // Instance the wendigo
            if(Area.wendigo_instance == null)
            {
                Area.wendigo_instance = Instantiate(Area.wendigo_go, transform.parent);
                // Set the wendigo_instance position to the center of the area
                Area.wendigo_instance.transform.position = transform.position;

                // Get the Wendigo component, and set the area to the player's area
                Wendigo current_component = Area.wendigo_instance.GetComponent<Wendigo>();

                current_component.area = this; 

                current_component.add_stimulus(this.wendigo_anger, this.wendigo_stealth, this.wendigo_thirst, this.wendigo_health);
            }

            print("Entered inside area " + gameObject.name);
        } else {
            Area.player.area = "";

            print("Entered outside area");
        }
    }

    private void OnTriggerExit(Collider _)
    {
        if(this.inside)
        {
            // Generate the wendigo, and set the wendigo's area to the player's area
            // randomly

            print("Exit inside area " + gameObject.name);
        } else {
            Area.player.area = "";

            print("Exit outside area");
        }
    }


    //

    private void OnMouseEnter() {
        if(Spheres == null){
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = Color.blue;
            Vector3[] vertices = GetComponent<MeshFilter>().mesh.vertices;
            Vector3[] verts = removeDuplicates(vertices);
            Spheres = new GameObject[verts.Length];
            
            drawSpheres(verts);
        } else {
            foreach (GameObject sphere in Spheres)
            {
                sphere.SetActive(true);
            }
        }
    }

    private void OnMouseExit() {
        foreach (GameObject sphere in Spheres)
        {
            sphere.SetActive(false);
        }
    }

    private Vector3[] removeDuplicates(Vector3[] dupArray) {
        Vector3[] newArray = new Vector3[8];  //change 8 to a variable dependent on shape
        bool isDup = false;
        int newArrayIndex = 0;
        for (int i = 0; i < dupArray.Length; i++) {
            for (int j = 0; j < newArray.Length; j++) {
                if (dupArray[i] == newArray[j]) {
                    isDup = true;
                }
            }
            if (!isDup) {
                newArray[newArrayIndex] = dupArray[i];
                newArrayIndex++;
                isDup = false;
            }
        }
        return newArray;
    }

    GameObject[] Spheres;
    private void drawSpheres(Vector3[] verts) {
        for (int i = 0; i < verts.Length; i++) {
            Spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Spheres[i].transform.position = verts[i];
            Spheres[i].transform.localScale -= new Vector3(0.8F, 0.8F, 0.8F);
        }
    }
}
