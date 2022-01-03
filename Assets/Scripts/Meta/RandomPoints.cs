using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPoints : MonoBehaviour
{
    static Bounds bounds;

    public static float limit = 1.0f;
    //public static float y_offset = 2.0f;

    // Apuntarlo para restringir el mapa
    // para que no use el mesh
    public static bool set_min_limit = false;
    public static bool set_max_limit = false;
    public static Vector3 min_limit;
    public static Vector3 max_limit;
    public static float ray_length;

    public static Terrain terrain;

    const float offset_y = 0.5f;

    void Start() {
        terrain = GetComponent<Terrain>();
        
        if(RandomPoints.bounds.min.x == RandomPoints.bounds.max.x) {
            // In case any other class tries to access mesh
            // before it is generated
            RandomPoints.bounds = terrain.terrainData.bounds;

            if(!set_min_limit || min_limit == Vector3.zero) {
                // Get the minimum for every xyz
                if(RandomPoints.bounds.min.x < min_limit.x) {
                    min_limit.x = RandomPoints.bounds.min.x;
                }
                if(RandomPoints.bounds.min.y < min_limit.y) {
                    min_limit.y = RandomPoints.bounds.min.y;
                }
                if(RandomPoints.bounds.min.z < min_limit.z) {
                    min_limit.z = RandomPoints.bounds.min.z;
                }
            }
            if(!set_max_limit || max_limit == Vector3.zero) {
                // Get the maximum for every xyz
                if(RandomPoints.bounds.max.x > max_limit.x) {
                    max_limit.x = RandomPoints.bounds.max.x;
                }
                if(RandomPoints.bounds.max.y > max_limit.y) {
                    max_limit.y = RandomPoints.bounds.max.y;
                }
                if(RandomPoints.bounds.max.z > max_limit.z) {
                    max_limit.z = RandomPoints.bounds.max.z;
                }
            }

            // Set the ray length
            ray_length = max_limit.y-min_limit.y+2;

            print("Set random points bounds (" + min_limit.x + ", " + min_limit.z + ") (" + max_limit.x + ", " + max_limit.z + ")");
        }

        
    }

    public static Vector3 random_point(){
        //print("Random point ("+RandomPoints.bounds.min.x+", "+RandomPoints.bounds.min.z+") ("+RandomPoints.bounds.max.x+", "+RandomPoints.bounds.max.z+")");
        Vector3 xz_point = new Vector3(
            Random.Range(RandomPoints.min_limit.x + limit, RandomPoints.max_limit.x - limit),
            RandomPoints.min_limit.y - 1,
            Random.Range(RandomPoints.min_limit.z + limit, RandomPoints.max_limit.z - limit)
        );

        xz_point[1] = terrain.SampleHeight(xz_point) + offset_y;

        Debug.Log("Random point: " + xz_point);
        return xz_point;
    }

    public static Vector3 random_point(Vector3 min, Vector3 max){
        Vector3 xz_point = new Vector3(
            Random.Range(RandomPoints.min_limit.x + limit, RandomPoints.max_limit.x - limit),
            0.0f,
            Random.Range(RandomPoints.min_limit.z + limit, RandomPoints.max_limit.z - limit)
        );

        xz_point[1] = terrain.SampleHeight(xz_point) + offset_y;

        Debug.Log("Random point in area: " + xz_point);
        return xz_point;
    }

    public static Vector3 random_point_circle(Vector3 center, float radius){
        float random_x = Random.Range(-radius, radius);

        Vector3 xz_point = new Vector3(
            center.x + random_x,
            RandomPoints.min_limit.y - 1,
            center.z + Mathf.Sqrt(radius*radius - random_x*random_x)
        );

        xz_point[1] = terrain.SampleHeight(xz_point) + offset_y;

        Debug.Log("Random point in circle: " + xz_point);
        return xz_point;
    }
}