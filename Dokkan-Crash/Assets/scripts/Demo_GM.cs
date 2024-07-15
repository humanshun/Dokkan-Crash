using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demo_GM : MonoBehaviour {

    // グローバルアクセス用の静的インスタンス
    public static Demo_GM Gm;

    // UI画像の配列
    public Image[] UIImage;

    // 初期化処理
    void Awake () {
        // フルスクリーンを無効にする
        Screen.fullScreen = false;

        // このインスタンスを静的インスタンスに設定
        Gm = this;
    }
	
    // 毎フレーム更新処理
    void Update () {
        KeyUPDownchange();
    }

    // 画像の色を初期化するメソッド
    void InitColor()
    {
        for (int i = 0; i < UIImage.Length; i++)
        {
            // 画像の色を白に設定
            UIImage[i].color = new Color(255, 255, 255);
        }
    }

    // キーの押下状態に応じて画像の色を変更するメソッド
    public void KeyUPDownchange()
    {
        // 'A'キーが離された時
        if (Input.GetKeyUp(KeyCode.A))
        {
            Color myColor = new Color32(255, 255, 255, 255);
            Demo_GM.Gm.UIImage[2].color = myColor;
        }
        // 'D'キーが離された時
        if (Input.GetKeyUp(KeyCode.D))
        {
            Color myColor = new Color32(255, 255, 255, 255);
            Demo_GM.Gm.UIImage[3].color = myColor;
        }
        // 'W'キーが離された時
        if (Input.GetKeyUp(KeyCode.W))
        {
            Color myColor = new Color32(255, 255, 255, 255);
            Demo_GM.Gm.UIImage[0].color = myColor;
        }
        // 'S'キーが離された時
        if (Input.GetKeyUp(KeyCode.S))
        {
            Color myColor = new Color32(255, 255, 255, 255);
            Demo_GM.Gm.UIImage[1].color = myColor;
        }
        // 'A'キーが押された時
        if (Input.GetKeyDown(KeyCode.A))
        {
            Color myColor = new Color32(180, 180, 180, 255);
            Demo_GM.Gm.UIImage[2].color = myColor;
        }
        // 'D'キーが押された時
        if (Input.GetKeyDown(KeyCode.D))
        {
            Color myColor = new Color32(180, 180, 180, 255);
            Demo_GM.Gm.UIImage[3].color = myColor;
        }
        // 'W'キーが押された時
        if (Input.GetKeyDown(KeyCode.W))
        {
            Color myColor = new Color32(180, 180, 180, 255);
            Demo_GM.Gm.UIImage[0].color = myColor;
        }
        // 'S'キーが押された時
        if (Input.GetKeyDown(KeyCode.S))
        {
            Color myColor = new Color32(180, 180, 180, 255);
            Demo_GM.Gm.UIImage[1].color = myColor;
        }
        // マウスの左ボタンが離された時
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Color myColor = new Color32(255, 255, 255, 255);
            Demo_GM.Gm.UIImage[4].color = myColor;
        }
        // マウスの右ボタンが離された時
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Color myColor = new Color32(255, 255, 255, 255);
            Demo_GM.Gm.UIImage[5].color = myColor;
        }
        // マウスの左ボタンが押された時
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Color myColor = new Color32(180, 180, 180, 255);
            Demo_GM.Gm.UIImage[4].color = myColor;
        }
        // マウスの右ボタンが押された時
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Color myColor = new Color32(180, 180, 180, 255);
            Demo_GM.Gm.UIImage[5].color = myColor;
        }
        // '1'キーが押された時
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Color myColor = new Color32(180, 180, 180, 255);
            Demo_GM.Gm.UIImage[6].color = myColor;
        }
        // '1'キーが離された時
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            Color myColor = new Color32(255, 255, 255, 255);
            Demo_GM.Gm.UIImage[6].color = myColor;
        }
        // スペースキーが押された時
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Color myColor = new Color32(180, 180, 180, 255);
            Demo_GM.Gm.UIImage[7].color = myColor;
        }
        // スペースキーが離された時
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Color myColor = new Color32(255, 255, 255, 255);
            Demo_GM.Gm.UIImage[7].color = myColor;
        }
    }
}
