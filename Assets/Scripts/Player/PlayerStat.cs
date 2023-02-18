using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    private float baseSpeed = 8f;
    private float baseJumpForce = 16f;

    [HideInInspector] public float currentSpeed;
    [HideInInspector] public float currentJumpForce;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = baseSpeed;
        currentJumpForce = baseJumpForce;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
