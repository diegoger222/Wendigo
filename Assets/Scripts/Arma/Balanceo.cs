using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balanceo : MonoBehaviour
{
    // Start is called before the first frame update
    private Quaternion startRotation;

    public float swayAmount = 8;
    void Start()
    {
        startRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        Sway();
    }

    private void Sway()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Quaternion xAngle = Quaternion.AngleAxis(mouseX * -1.5f, Vector3.up);

        Quaternion yAngle = Quaternion.AngleAxis(mouseY * 1.5f, Vector3.right);

        Quaternion targetRotation = startRotation * xAngle * yAngle;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * swayAmount);
    }
}
