using UnityEngine;
using System.Collections;

public class RoomLighter : MonoBehaviour {
    //SpriteRenderer EntranceRenderer;
    //SpriteRenderer LivingRenderer;
    //SpriteRenderer KitchenRenderer;
    //SpriteRenderer ÖvreRenderer;
   // // Entrance
    bool entranceLit = false;
    //public Sprite EntranceDark;
    //public Sprite EntranceLivingRoomLit;
    //public Sprite EntranceKitchenLit;
    //public Sprite EntranceÖvreEntranceLit;
    //public Sprite EntranceLivAndKitLit;
    //public Sprite EntranceÖvrAndKitLit;
    //public Sprite EntranceÖvrAndLivLit;
    //public Sprite EntranceAllThreeLit;

   // // Living Room
    bool livingLit = false;
    //public Sprite LivingRoomDark;
    //public Sprite LivingRoomEntranceLit;

   // // Kitchen
    bool kitchenLit = false;
    //public Sprite KitchenDark;
    //public Sprite KitchenEntranceLit;

    //// Övre Hall
    bool övreLit = false;
    //public Sprite ÖvreEntranceDark;
    //public Sprite ÖvreEntranceUndreLit;

    public void switchKitchenBool() {
        kitchenLit = !kitchenLit;
    }

    public void switchLivingBool() {
        livingLit = !livingLit;
    }

    public void switchEntranceBool() {
        entranceLit = !entranceLit;
    }

    public void switchÖverBool() {
        övreLit = !övreLit;
    }

    void changeRoomLightning() {
    //    if (entranceLit == true && livingLit == true && övreLit == true && kitchenLit == true) {
    //        EntranceRenderer.sprite = EntranceAllThreeLit;
    //        LivingRenderer.sprite = LivingRoomEntranceLit;
    //        ÖvreRenderer.sprite = ÖvreEntranceUndreLit;
    //        KitchenRenderer.sprite = KitchenEntranceLit;         
    //    } 
    //    else if (entranceLit == true && livingLit == true && övreLit == true && kitchenLit == false) {
    //        EntranceRenderer.sprite = EntranceÖvrAndLivLit;
    //        LivingRenderer.sprite = LivingRoomEntranceLit;
    //        ÖvreRenderer.sprite = ÖvreEntranceUndreLit;
    //        KitchenRenderer.sprite = KitchenEntranceLit; 
    //    } 
    //    else if (entranceLit == true && livingLit == true && övreLit == false && kitchenLit == true) {
    //        EntranceRenderer.sprite = EntranceÖvrAndKitLit;
    //        LivingRenderer.sprite = LivingRoomEntranceLit;
    //        ÖvreRenderer.sprite = ÖvreEntranceUndreLit;
    //        KitchenRenderer.sprite = KitchenEntranceLit; 
    //    } 
    //    else if (entranceLit == true && livingLit == false && övreLit == true && kitchenLit == true) {
    //        EntranceRenderer.sprite = EntranceÖvrAndKitLit;
    //        LivingRenderer.sprite = LivingRoomEntranceLit;
    //        ÖvreRenderer.sprite = ÖvreEntranceUndreLit;
    //        KitchenRenderer.sprite = KitchenEntranceLit; 
    //    } 
    //    else if (entranceLit == false && livingLit == true && övreLit == true && kitchenLit == true) {
    //        EntranceRenderer.sprite = EntranceAllThreeLit;
    //        LivingRenderer.sprite = LivingRoomDark;
    //        ÖvreRenderer.sprite = ÖvreEntranceDark;
    //        KitchenRenderer.sprite = KitchenDark; 
    //    } 
    //    else if (entranceLit == true && livingLit == false && övreLit == false && kitchenLit == true) {
    //        EntranceRenderer.sprite = EntranceKitchenLit;
    //        LivingRenderer.sprite = LivingRoomEntranceLit;
    //        ÖvreRenderer.sprite = ÖvreEntranceUndreLit;
    //        KitchenRenderer.sprite = KitchenEntranceLit; 
    //    } 
    //    else if (entranceLit == false && livingLit == false && övreLit == true && kitchenLit == true) {
    //        EntranceRenderer.sprite = EntranceÖvrAndKitLit;
    //        LivingRenderer.sprite = LivingRoomDark;
    //        ÖvreRenderer.sprite = ÖvreEntranceDark;
    //        KitchenRenderer.sprite = KitchenDark; 
    //    }   
    //    else if (entranceLit == true && livingLit == false && övreLit == true && kitchenLit == false) {
    //        EntranceRenderer.sprite = EntranceÖvreEntranceLit;
    //        LivingRenderer.sprite = LivingRoomEntranceLit;
    //        ÖvreRenderer.sprite = ÖvreEntranceUndreLit;
    //        KitchenRenderer.sprite = KitchenEntranceLit; 
    //    } 
    //    else if (entranceLit == false && livingLit == true && övreLit == false && kitchenLit == true) {
    //        EntranceRenderer.sprite = EntranceLivAndKitLit;
    //        LivingRenderer.sprite = LivingRoomDark;
    //        ÖvreRenderer.sprite = ÖvreEntranceDark;
    //        KitchenRenderer.sprite = KitchenDark; 
    //    } 
    //    else if (entranceLit == false && livingLit == true && övreLit == true && kitchenLit == false) {
    //        EntranceRenderer.sprite = EntranceÖvrAndLivLit;
    //        LivingRenderer.sprite = LivingRoomDark;
    //        ÖvreRenderer.sprite = ÖvreEntranceDark;
    //        KitchenRenderer.sprite = KitchenDark; 
    //    } 
    //    else if (entranceLit == true && livingLit == false && övreLit == false && kitchenLit == false) {
    //        EntranceRenderer.sprite = EntranceDark;
    //        LivingRenderer.sprite = LivingRoomEntranceLit;
    //        ÖvreRenderer.sprite = ÖvreEntranceUndreLit;
    //        KitchenRenderer.sprite = KitchenEntranceLit; 
    //    } 
    //    else if (entranceLit == false && livingLit == true && övreLit == false && kitchenLit == false) {
    //        EntranceRenderer.sprite = EntranceLivingRoomLit;
    //        LivingRenderer.sprite = LivingRoomDark;
    //        ÖvreRenderer.sprite = ÖvreEntranceDark;
    //        KitchenRenderer.sprite = KitchenDark; 
    //    } 
    //    else if (entranceLit == false && livingLit == false && övreLit == true && kitchenLit == false) {
    //        EntranceRenderer.sprite = EntranceÖvreEntranceLit;
    //        LivingRenderer.sprite = LivingRoomDark;
    //        ÖvreRenderer.sprite = ÖvreEntranceDark;
    //        KitchenRenderer.sprite = KitchenDark; 
    //    } 
    //    else if (entranceLit == false && livingLit == false && övreLit == false && kitchenLit == true) {
    //        EntranceRenderer.sprite = EntranceKitchenLit;
    //        LivingRenderer.sprite = LivingRoomDark;
    //        ÖvreRenderer.sprite = ÖvreEntranceDark;
    //        KitchenRenderer.sprite = KitchenDark; 
    //    } 
    //    else if (entranceLit == false && livingLit == false && övreLit == false && kitchenLit == false) {
    //        EntranceRenderer.sprite = EntranceDark;
    //        LivingRenderer.sprite = LivingRoomDark;
    //        ÖvreRenderer.sprite = ÖvreEntranceDark;
    //        KitchenRenderer.sprite = KitchenDark; 
    //    } 
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            changeRoomLightning();
        }
    }
}