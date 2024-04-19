using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirCamera : MonoBehaviour
{
    private Vector3 camFollow;
    private Transform Bola, Win;

    void Awake()
    {
        Bola = FindObjectOfType<Ball>().transform;  // Garante que Ball é um tipo definido e acessível
    }

    void Update()
    {
        if (Win == null)
            Win = GameObject.Find("Win(Clone)").GetComponent<Transform>();

        if (transform.position.y > Bola.transform.position.y && transform.position.y > Win.position.y + 4f)
            camFollow = new Vector3(transform.position.x, Bola.position.y, transform.position.z);

        transform.position = new Vector3(transform.position.x, camFollow.y, -5); 
    }
}

