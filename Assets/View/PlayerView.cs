using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.ComponentModel;

public class PlayerView : MonoBehaviour
{
    //PlayerAttackStream _playerAttackStream;
    public AnimationCurve speedCurve;　//アニメーションカーブ
    float moveTime;             //移動時間
    float speed;                //速度
    const float maxSpeed = 5f;  //最高速度
    Rigidbody2D _rigidbody2D;         //Rigidbody

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _rigidbody2D.velocity = new Vector2(speed, _rigidbody2D.velocity.y);

        moveTime += Time.deltaTime / 2;
        speed = speedCurve.Evaluate(moveTime) * maxSpeed;

        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    _playerAttackStream.OnAttackSignal.Value = true;
        //}
        //if (Input.GetKeyUp(KeyCode.D))
        //{
        //    _playerAttackStream.OnAttackSignal.Value = false;
        //}
    }
}
