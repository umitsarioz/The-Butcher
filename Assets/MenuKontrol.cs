using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuKontrol : MonoBehaviour {

    public void ToGame()
    {
        SesKontrol.Instance.PlaySound(0);
        SceneManager.LoadScene("Game");
    }
}
