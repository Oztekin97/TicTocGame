
using UnityEngine;
using UnityEngine.UI;

public class GameStateController : MonoBehaviour
{
    //inspector panelindeki değişkenleri gruplamak için header tag kullanıldı
    //TitleBar References,Misc References,Asset References,GameState Settings tag leri GameController objesinde kullanıldı
    [Header("TitleBar References")]                                  //Bu tag oyun sahnesinde bulunana bileşen referanslarını barındırır
    public Image playerXIcon;                                        // X oyuncusunun simgesi için referans
    public Image playerOIcon;                                        // O oyuncusunun simgesi için referans
    public InputField player1InputField;                             // X oyuncusunun string tipinde veri girişi için referans
    public InputField player2InputField;                             // O oyuncusunun string tipinde veri girişi için referans
    public Text winnerText;                                          // Kazananın adını görüntüleyecek text

    [Header("Misc References")]                                      //Bu tag oyun oyun bitimindeki işlemleri barındırır
    public GameObject endGameState;                                  // Oyun altbilgi kabı + kazanan metnini tutan obje

    [Header("Asset References")]                                     //Bu tag sahnedeki elemanların referans işlemlerini içerir
    public Sprite tilePlayerO;                                       // O sprite için hareketli grafik referansı
    public Sprite tilePlayerX;                                       // X sprite için hareketli grafik referansı
    public Sprite tileEmpty;                                         // Tile_Empty için referans
    public Text[] tileList;                                          // Sahnedeki tüm karoların bir listesini tutar

    [Header("GameState Settings")]    //Bu tag oyun durumunu ayarlama işlemlerini içerir (Sıradaki oyuncunun butonunu aktif et,sırası geçen oyuncunun butonunu inaktif et)
    public Color inactivePlayerColor;                                // Etkin olmayan oyuncu simgesi için görüntülenecek renk
    public Color activePlayerColor;                                  // Etkin oyuncu simgesi için görüntülenecek renk
    public string whoPlaysFirst;                                     // İlk kim oynayacak (X : 0) 

    [Header("Private Variables")]                                    //Bu tag oyuncu değişimi işlemlerini içerir 
    private string playerTurn;                                       // sıranın kimde olduğunu belirleyen metod
    private string player1Name;                                      // 1.Oyuncunun görünen adı
    private string player2Name;                                      // 2.Oyuncunun görünen adı
    private int moveCount;                                           // hareket sayacı




    /// Başlat (aktif frame çağıralacak)

    private void Start()
    {
        // Sıranın kimde olduğuna ilişkin  izleyiciyi ayarlar ve sıranın kimde olduğuna ilişkin UI simgesi bildirimini ayarlar
        playerTurn = whoPlaysFirst;
        if (playerTurn == "X") playerOIcon.color = inactivePlayerColor;
        else playerXIcon.color = inactivePlayerColor;

        //İnputfield lara dinleyici ekledik ve değer değişiminde bu dinleyici çağrılacak
        player1InputField.onValueChanged.AddListener(delegate { OnPlayer1NameChanged(); });
        player2InputField.onValueChanged.AddListener(delegate { OnPlayer2NameChanged(); });

        // Varsayılan değerleri
        player1Name = player1InputField.text;
        player2Name = player2InputField.text;
    }
    //İlk tüm satır,sütun ve çapraz olarak kim kendi stringini elde ederse o kazanır.
    // Kazanma koşullarını kontrol etmek için her turun sonunda GameOver metodu çağrılır
    // Olası tüm kazanma koşulları sabit olarak kodlanmıştır (8)
    // Sadece karoların pozisyonunu alıyoruz ve komşuları kontrol ediyoruz (bir sıra içinde)
    // Döşemeler soldan sağa, satır satır 0..8 olarak numaralandırılmıştır, örnek:
    // [0][1][2]
    // [3][4][5]
    // [6][7][8]

    public void EndTurn()
    {
        moveCount++;
        if (tileList[0].text == playerTurn && tileList[1].text == playerTurn && tileList[2].text == playerTurn) GameOver(playerTurn); //tüm satır X veya O ise
        else if (tileList[3].text == playerTurn && tileList[4].text == playerTurn && tileList[5].text == playerTurn) GameOver(playerTurn); //tüm satır X veya O ise
        else if (tileList[6].text == playerTurn && tileList[7].text == playerTurn && tileList[8].text == playerTurn) GameOver(playerTurn); //tüm satır X veya O ise
        else if (tileList[0].text == playerTurn && tileList[3].text == playerTurn && tileList[6].text == playerTurn) GameOver(playerTurn); //tüm sütun X veya O ise
        else if (tileList[1].text == playerTurn && tileList[4].text == playerTurn && tileList[7].text == playerTurn) GameOver(playerTurn); //çapraz X veya O ise
        else if (tileList[2].text == playerTurn && tileList[5].text == playerTurn && tileList[8].text == playerTurn) GameOver(playerTurn); //tüm sütun X veya O ise
        else if (tileList[0].text == playerTurn && tileList[4].text == playerTurn && tileList[8].text == playerTurn) GameOver(playerTurn); //çapraz X veya O ise
        else if (tileList[2].text == playerTurn && tileList[4].text == playerTurn && tileList[6].text == playerTurn) GameOver(playerTurn); //çapraz X veya O ise
        else if (moveCount >= 9) GameOver("D");
        else
            ChangeTurn();
    }


    // Sıranın kimde olduğunu belirler ve izleyici değiştirir
    public void ChangeTurn() 
    {
        // Buna, "X"i değerlendiren ve gerçeklere dayalı olarak "O" veya "X" ile sonuçlanan bir Üçlü operatör denir
        // Daha sonra renkler gibi bazı kullanıcı arabirimi geri bildirimlerini değiştiriyoruz.
        playerTurn = (playerTurn == "X") ? "O" : "X";
        if (playerTurn == "X")
        {
            playerXIcon.color = activePlayerColor;
            playerOIcon.color = inactivePlayerColor;
        }
        else
        {
            playerXIcon.color = inactivePlayerColor;
            playerOIcon.color = activePlayerColor;
        }
    }
    // Bir kazanma koşulu veya beraberlik olduğunda bu metod çağrılır
    /// <param name="winningPlayer">X O D</param>
    private void GameOver(string winningPlayer)
    {
        switch (winningPlayer)
        {
            case "D": //Berabere
                winnerText.text = "DRAW";
                break;
            case "X": //X kazandı
                winnerText.text = player1Name;
                break;
            case "O": //O kazandı
                winnerText.text = player2Name;
                break;
        }
        endGameState.SetActive(true);
        ToggleButtonState(false);
    }


    // Oyunu yeniden başlatır

    public void RestartGame()
    {
        // Bazı oyun durumu özelliklerini sıfırlar
        moveCount = 0;
        playerTurn = whoPlaysFirst;
        ToggleButtonState(true);
        endGameState.SetActive(false);

        // Tüm kutucuklarda döngü yapın ve onları sıfırlayın
        for (int i = 0; i < tileList.Length; i++)
        {
            tileList[i].GetComponentInParent<TileController>().ResetTile();
        }
    }


    /// Tüm düğmeleri etkinleştirir veya devre dışı bırakır

    private void ToggleButtonState(bool state)
    {
        for (int i = 0; i < tileList.Length; i++)
        {
            tileList[i].GetComponentInParent<Button>().interactable = state;
        }
    }


    // Mevcut oyuncu sırasını döndürür (X / O)

    public string GetPlayersTurn()
    {
        return playerTurn;
    }


    // Görüntü karakterini döndürür (X / 0)

    public Sprite GetPlayerSprite()
    {
        if (playerTurn == "X") return tilePlayerX;
        else return tilePlayerO;
    }


    // P1_textfield güncellendiğinde geri arama. Sadece Player1 için diziyi güncelliyoruz

    public void OnPlayer1NameChanged()
    {
        player1Name = player1InputField.text;
    }

    
    // P2_textfield güncellendiğinde geri arama. Sadece Oyuncu2 için diziyi güncelliyoruz
   
    public void OnPlayer2NameChanged()
    {
        player2Name = player2InputField.text;
    }
}
