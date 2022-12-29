using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    [Header("Component References")]                 //inspector panelindeki değişkenleri gruplamak için kullanıldı (Empty_Tile  butonunun inspector panelinde bulunuyor)
    public GameStateController gameController;                       // gameController a referans
    public Button interactiveButton;                                 // buton a referans
    public Text internalText;                                        // text e referans (X, O yazımı için)

    // Butona her bastığımızda çağrılır,  kutucuğun durumunu güncelleriz.
    // Gerekli oyuncu için metin bileşeni ve düğmeyi devre dışı bırakın
    //GetPlayersTurn(),GetPlayerSprite(),EndTurn() diğer sınıftan çağrılacak duruma göre Tile'lar güncellenecek.
    public void UpdateTile()
    {
        internalText.text = gameController.GetPlayersTurn(); //Oyuncu değişimi yapılır
        interactiveButton.image.sprite = gameController.GetPlayerSprite();
        interactiveButton.interactable = false; //buton özelliğini devre dışı bırakılır
        gameController.EndTurn();
    }

    // Döşeme özelliklerini sıfırlar (metin bileşenlerini yani X, O stringlerini , düğme görüntüsünü yani X ve O nun image lerini)
   
    public void ResetTile()
    {
        internalText.text = "";
        interactiveButton.image.sprite = gameController.tileEmpty;
    }
}