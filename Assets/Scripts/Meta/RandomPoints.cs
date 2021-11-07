using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPoints : MonoBehaviour
{
    static Vector3[] mesh;
    static int verticesCount = 0;

    void Start() {
        if(RandomPoints.mesh == null){
            // In case any other class tries to access mesh
            // before it is generated
            RandomPoints.mesh = new Vector3[0];
            Vector3[] vertices = GetComponent<MeshFilter>().mesh.vertices;

            RandomPoints.mesh = removeDuplicates(vertices).ToArray();
        }

        
    }

    private List<Vector3> removeDuplicates(Vector3[] dupArray) {
        List<Vector3> newArray = new List<Vector3>();  //change 8 to a variable dependent on shape
        bool isDup = false;
        for (int i = 0; i < dupArray.Length; i++) {
            for (int j = 0; j < newArray.Count; j++) {
                if (dupArray[i] == newArray[j]) {
                    isDup = true;
                }
            }
            if (!isDup) {
                newArray.Add(dupArray[i]);
                isDup = false;
            }
        }

        RandomPoints.verticesCount = newArray.Count;
        return newArray;
    }
}