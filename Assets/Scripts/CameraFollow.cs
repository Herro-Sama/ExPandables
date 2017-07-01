﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject Player;
    [SerializeField]
    private float FollowSpeed = 3f;
    private Vector3 offset;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - Player.transform.position;
    }

    void LateUpdate()
    {
        //transform.position = Player.transform.position + offset;

        Vector3 newPosition = Player.transform.position;
        newPosition.z = -10;
        transform.position = Vector3.Slerp(transform.position, newPosition, FollowSpeed * Time.deltaTime);
    }
}