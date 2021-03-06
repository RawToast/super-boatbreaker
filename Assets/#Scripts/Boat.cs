﻿using System;
using UnityEngine;
using UnityEngine.Rendering;
using Util;

public class Boat : MonoBehaviour {
    public float speed;

    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private float minXPosition;
    [SerializeField] private float maxXPosition;
    [SerializeField] private bool canMove;

    [SerializeField] private CannonBall ball;

    private float lastFired;

    void FixedUpdate() {
        if (canMove) {
            if (Input.GetKey(KeyCode.LeftArrow)) {
                rig.velocity =
                    new Vector2(-1 * speed * Time.deltaTime, rig.velocity.y);
            }
            else if (Input.GetKey(KeyCode.RightArrow)) {
                rig.velocity =
                    new Vector2(1 * speed * Time.deltaTime, rig.velocity.y);
            }
            else {
                rig.velocity = new Vector2(0, rig.velocity.y);
            }

            transform.position =
                new Vector3(Mathf.Clamp(transform.position.x, minXPosition, maxXPosition),
                    transform.position.y, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (Math.Abs(lastFired - Time.time) > 0.1f) {
                if (Grd.Lives.HasLifes()) {
                    Grd.Lives.TakeLife();

                    lastFired = Time.time;
                    var trans = transform.position;

                    var newBall = Instantiate(ball, new Vector3(trans.x, trans.y + 0.5f), Quaternion.identity);

                    newBall.SetValuesToDefaults();
                    newBall.direction = Vector2.up;
                    newBall.Start();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collidingObj) {
        if (collidingObj.gameObject.CompareTag(Tags.SEA)) {
            if (rig.velocity.y < 0) {
                rig.velocity =
                    new Vector2(rig.velocity.x, rig.velocity.y + 0.15f);
            }
            else {
                rig.velocity =
                    new Vector2(rig.velocity.x, rig.velocity.y + 0.05f);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collidingObj) {
        if (collidingObj.gameObject.CompareTag(Tags.SEA)) {
            if (rig.velocity.y < 0) {
                rig.velocity =
                    new Vector2(rig.velocity.x, rig.velocity.y + 0.15f);
            }
            else {
                rig.velocity =
                    new Vector2(rig.velocity.x, rig.velocity.y + 0.085f);
            }
        }

        if (collidingObj.gameObject.CompareTag(Tags.BLOCK)) {
            Vector2 normalisedDifference = (gameObject.transform.position - collidingObj.transform.position).normalized;
        }
    }

    public void ResetPaddle() {
        transform.position = new Vector3(0, transform.position.y, 0); //Sets the paddle's x position to 0
    }

    public Vector2 velocity() {
        return rig.velocity;
    }
}