﻿using UnityEngine;
using System.Collections;
/*	-	-	-	-	-	-	-	-	-	-	
 
	・Rocketの動きを制御する.
	・各Rocketの操作に対応したジョイスティックの設定をStageConfigが行う -> void SetJoystickNumber();
 
 -	-	-	-	-	-	-	-	-	-	*/
public class R_move : MonoBehaviour {

    Rigidbody2D rb2d;
    float beforeAxis;
    string sHorizontal;
	bool isCameraAreaStay = false;

    public int joyNum = 0; //開発用にpublicにする.

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        sHorizontal = (joyNum == 0 ) ? "Horizontal":"Horizontal" + joyNum;	//テスト用に使うジョイスティック番号で初期化する.
    }
	
	// Update is called once per frame
	void Update () {
        MoveAcceleration();
        MoveRotation();
    }

    /*旋回が行われていない間加速し続けていく処理*/
    void MoveAcceleration()
    {
        //左右入力が解かれた瞬間に加速する,
        if(Input.GetAxisRaw(sHorizontal) == 0.0f && beforeAxis != 0.0f)
        { 
           rb2d.velocity *= 0.5f;
        }
        
        //徐々に加速していく,
        rb2d.AddForce(transform.up * 2.0f);
        
    }


    /*左右入力時回転するが減速する*/
    void MoveRotation()
    {
        //Axisに入力を保持,
        float Axis = -Input.GetAxisRaw(sHorizontal);

        //入力があった場合Rocketと回転させる,
        if (Axis != 0)
        {
            //速度が1.0fを上回っている場合減速させる,
            if (rb2d.velocity.magnitude > 1.0f)
            {
                rb2d.velocity *= 0.97f;
            }
            transform.Rotate(new Vector3(0, 0, 2 * Axis));
        }

        //前回入力を保持
        beforeAxis = Axis;
    }

	/*プレイヤーの番号を外部から指定する. StageConfigで使う.*/
	public void SetJoystickNumber(int n)
	{
		joyNum = n;
		sHorizontal = "Horizontal" + n;
	}
	
	/*プレイヤーの番号を返す. StageConfigで使う.*/
	public int GetJoystickNumber(){return joyNum;}

	/*カメラエリアと重なっている時、trueを返す.
	 * MoveCameraAreaで使用する.*/
	public bool IsCameraAreaStay() {return isCameraAreaStay; }

	/*Trigger2Dと重なり続ける時に呼ばれる関数*/
	void OnTriggerStay2D(Collider2D der2d)
	{
		//カメラエリアに重なっとる時.
		if(der2d.gameObject.tag == "CameraArea")
		{
			isCameraAreaStay = true;
			Debug.Log("(R_move)重なっとるよ！！！！");
		}
		
	}

	void OnTriggerExit2D(Collider2D der2d)
	{
		if(der2d.gameObject.tag == "CameraArea")
		{
			isCameraAreaStay = false;
			Debug.Log("(R_move) 重なってないよ！！");
		}
	}
	
}
