using UnityEngine;
using System.Collections;

public class RoomLighter : MonoBehaviour {
    public Material defaultMaterial;
    public Material diffuseMaterial;
    //SpriteRenderer LivingRenderer;
    //SpriteRenderer KitchenRenderer;
    //SpriteRenderer ÖvreRenderer;
   // // Entrance
    bool entranceLit = false;
    SpriteRenderer EntranceRenderer;
    SpriteRenderer EntranceDrawerRenderer;
    //public Sprite EntranceDark;
    //public Sprite EntranceLivingRoomLit;
    //public Sprite EntranceKitchenLit;
    //public Sprite EntranceÖvreEntranceLit;
    //public Sprite EntranceLivAndKitLit;
    //public Sprite EntranceÖvrAndKitLit;
    //public Sprite EntranceÖvrAndLivLit;
    //public Sprite EntranceAllThreeLit;

   // // Living Room
    SpriteRenderer livingCouchRenderer;
    SpriteRenderer livingTableRenderer;
    SpriteRenderer livingRenderer;
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

    void Start() {
        EntranceRenderer = GameObject.Find("Entrance_Background").GetComponent<SpriteRenderer>();
        EntranceDrawerRenderer = GameObject.Find("Entrance_Byra").GetComponent<SpriteRenderer>();
        livingCouchRenderer = GameObject.Find("Livingroom_Couch").GetComponent<SpriteRenderer>();
        livingTableRenderer = GameObject.Find("Livingroom_Table").GetComponent<SpriteRenderer>();
        livingRenderer = GameObject.Find("Livingroom_Background").GetComponent<SpriteRenderer>();
    }

    public void switchKitchenBool() {
        kitchenLit = !kitchenLit;
    }

    public void switchLivingBool() {
        livingLit = !livingLit;
        if (livingLit == false) {
            livingCouchRenderer.material = diffuseMaterial;
            livingRenderer.material = diffuseMaterial;
            livingTableRenderer.material = diffuseMaterial;
        } else {
            livingCouchRenderer.material = defaultMaterial;
            livingRenderer.material = defaultMaterial;
            livingTableRenderer.material = defaultMaterial;
        }
    }

    public void switchEntranceBool() {
        entranceLit = !entranceLit;
        if (entranceLit == false) {
            EntranceRenderer.material = diffuseMaterial;
            EntranceDrawerRenderer.material = diffuseMaterial;
        } 
        else{
            EntranceRenderer.material = defaultMaterial;
            EntranceDrawerRenderer.material = defaultMaterial;
        }
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
        changeRoomLightning();
    }
}