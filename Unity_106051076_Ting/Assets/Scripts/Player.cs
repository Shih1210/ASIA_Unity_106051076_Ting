using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region 欄位區域
    //宣告變數(定義欄位 Field)
    //修飾詞 欄位類型 欄位名稱(指定 數值)結束；
    //私人 Private ；公開 Public
    [Header("移動速度")]
    [Range(1, 2000)]
    public int speed = 10;
    [Header("旋轉速度")]
    [Tooltip("角色旋轉速度"), Range(1.5f, 200f)]
    //說明文字 Range(最小值,最大值)→拉桿
    public float turn = 20.5f;
    [Header("玩家名稱")]
    public string Name = "Player ";
    #endregion

    [Header("撿東西放的位置")]
    public Rigidbody rigCatch;

    public Transform tran;
    public Rigidbody rig;
    public Animator ani;

    private void Update()
    {
        Turn();
        Run();
        Attack();
    }

    //觸發碰撞時持續執行(1秒約執行60次) 碰撞物件資訊
    private void OnTriggerStay(Collider other)
    {
        print(other.name);

        //如果碰撞物件的名稱 為 雞腿 撥放攻擊動畫
        if (other.name == "藥水" && ani.GetCurrentAnimatorStateInfo(0).IsName("攻擊"))
        {
            //物理.忽略碰撞(A碰撞，B碰撞)
            Physics.IgnoreCollision(other, GetComponent<Collider>());
            //碰撞物件.取得元件<泛型>
            other.GetComponent<HingeJoint>().connectedBody = rigCatch;
        }
    }

    #region 方法區域
    ///<summary>
    ///跑步
    ///</summary>
    private void Run()
    {
        //如果動畫為 撿東西 就 跳出
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("攻擊")) return;

        float v = Input.GetAxis("Vertical");    //W上 1，S下 -1 ，沒有 0
        //rig.AddForce(0, 0, speed * v);                //世界座標
        //tran.forward 區域座標Z軸
        //tran.right   區域座標X軸
        //tran.up      區域座標Y軸
        rig.AddForce(tran.forward * speed * v* Time.deltaTime);       //區域座標

        ani.SetBool("走路開關", v != 0);
    }
    ///<summary>
    ///旋轉
    ///</summary>
    private void Turn()
    {
        float h = Input.GetAxis("Horizontal");    //A左 -1，D右 1 ，沒有 0
        tran.Rotate(0, turn * h * Time.deltaTime, 0);
    }
    ///<summary>
    ///攻擊
    ///</summary>
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //按下左鍵攻擊/撿東西
            ani.SetTrigger("攻擊觸發器");
        }
    }
    #endregion
}
