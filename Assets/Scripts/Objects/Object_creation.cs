using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Object_creation : MonoBehaviour
{
    const int max_generation_seeds = 10;
    const float percentage_objects = 1f;

    public static int seed;
    public static System.Random random;

    // Start is called before the first frame update
    void Start()
    {
        if(random == null){
            var rnd = new System.Random();
            Object_creation.seed = rnd.Next(1, Object_creation.max_generation_seeds);

            Object_creation.random = new System.Random(seed);
        }

        // Get all object generators from the map
        GameObject[] object_generators = GameObject.FindGameObjectsWithTag("Generator");
        
        // Shuffle all objects to remove a factor
        Shuffle<GameObject>(ref object_generators);

        // Get only a predefined percentage of objects
        int amount_objects = (int)(object_generators.Length * Object_creation.percentage_objects);
        object_generators = GetRange<GameObject>(object_generators, 0, amount_objects);

        if(object_generators.Length == 0){
            Debug.Log("No object generators (with tag 'Generator') found");
        } else {
            print("Selected " + amount_objects + " objects");
        }

        // Initially generate all objects in the map
        foreach(GameObject generator in object_generators)
        {
            generator.GetComponent<Object_generator>().generate_object();
        }
    }

    
    static T[] GetRange<T>(T[] array, int start, int end)
    {
        T[] result = new T[end - start];

        for(int i = start; i < end; ++i)
        {
            result[i - start] = array[i];
        }

        return result;
    }

    static void Shuffle<T>(ref T[] arr) {
        System.Random random = Object_creation.random;

        for (int i = arr.Length - 1; i > 0; i--)
        {
            int swapIndex = random.Next(i + 1);
            T temp = arr[i];
            arr[i] = arr[swapIndex];
            arr[swapIndex] = temp;
        }
    }    
}
