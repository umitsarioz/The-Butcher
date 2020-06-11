using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour
{//teslimden önce sil Urunler=vegetable,products=veggies
    public static GameManager Instance { set; get; }
    private const float REQUIRED_SLICEFORCE=400.0f;
    public GameObject urunlerprefab;
    public Transform trail;
    private bool isPaused;
    private List<Urunler> products = new List<Urunler>();
    
    private float lastSpawn;
    private float deltaSpawn = 1.0f;//atıs sıklıgı;
  
    private Vector3 lastMousePos;
    private Collider2D[] productsCols;
    //UI part of the game thingy
    private int skor;
    private int enyuksekskor;
    private int cansayisi;
    public Text skorText;
    public Text enyuksekskorText;
    public Image[] cansayilari;
    public GameObject PauseMenu;
    public GameObject DeadMenu;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        productsCols =new Collider2D[0];
        newGame();
    }
    public void newGame()
    {
        SesKontrol.Instance.PlaySound(0);
        skor = 0;
        cansayisi = 3;
        PauseMenu.SetActive(false);
        skorText.text =skor.ToString();
        enyuksekskor =PlayerPrefs.GetInt("Skor");
        enyuksekskorText.text ="En İyi Skor:"+enyuksekskor.ToString(); ;
        Time.timeScale = 1;
        isPaused =false;
        foreach (Image i in cansayilari)
            i.enabled =true;
        foreach (Urunler u in products)
            Destroy(u.gameObject);
        products.Clear();
        DeadMenu.SetActive(false);
    }
    private void Update()
    {
        if (isPaused)
            return;
        if (Time.time - lastSpawn > deltaSpawn)
            {
            Urunler u =GetUrunler();
            float randomX = Random.Range(-1.65f,1.65f);//rastgele atış kordinat aralıgı
            u.LaunchUrunler(Random.Range(1.85f,2.75f),randomX,-randomX);
            lastSpawn = Time.time;
        }

        if (Input.GetMouseButton(0))
        {
            
              Vector3 pos=Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            pos.z =-1;
            trail.position = pos;
           Collider2D[] thisFrameProduct=Physics2D.OverlapPointAll(new Vector2(pos.x, pos.y), LayerMask.GetMask("Urunler"));
            
            if ((Input.mousePosition - lastMousePos).sqrMagnitude >REQUIRED_SLICEFORCE)
            {
                foreach (Collider2D c2 in thisFrameProduct)
                {
                    for (int i = 0; i < productsCols.Length; i++)
                    {
                        if (c2 == productsCols[i])
                        {
                            c2.GetComponent<Urunler>().Slice();
                        }
                    }
                }
            }
            lastMousePos = Input.mousePosition;
            productsCols = thisFrameProduct;
        }
    }
    private Urunler GetUrunler()
    {
        Urunler u = products.Find(x=>!x.IsActive);

        if (u == null)
        {
            u =Instantiate(urunlerprefab).GetComponent<Urunler>();
            products.Add(u);
        }
        return u;
    }
    public void Skorarttir(int skormiktari)
    {
        skor +=skormiktari;
        skorText.text =skor.ToString();
        if (skor>enyuksekskor)
        {
            enyuksekskor = skor;
            enyuksekskorText.text ="En İyi Skor:"+enyuksekskor.ToString();
            PlayerPrefs.SetInt("Skor",enyuksekskor);

        }
    }
    public void cankaybi()
    {
        if (cansayisi == 0)
            return;
        cansayisi--;
        cansayilari[cansayisi].enabled=false;
        if (cansayisi ==0)
            olum();
}
    public void olum()
    {
        isPaused =true;
        DeadMenu.SetActive(true);
    }
    public void PauseGame()
    {
        SesKontrol.Instance.PlaySound(0);
        PauseMenu.SetActive(!PauseMenu.activeSelf);
        isPaused =PauseMenu.activeSelf;
        Time.timeScale = (Time.timeScale ==0)?1:0 ;
    }
    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
