using UnityEngine;
using System.Collections;

public class RoomLighter : MonoBehaviour {
    public Material defaultMaterial;
    public Material diffuseMaterial;

    // Entrance
    bool entranceLit = false;
    SpriteRenderer EntranceRenderer;
    SpriteRenderer EntranceDrawerRenderer;
    public Sprite EntranceDark;
    public Sprite EntranceLivingRoomLit;
    public Sprite EntranceKitchenLit;
    public Sprite EntranceLivAndKitLit;

    // Living Room
    SpriteRenderer livingCouchRenderer;
    SpriteRenderer livingTableRenderer;
    SpriteRenderer livingRenderer;
    bool livingLit = false;
    public Sprite LivingRoomDark;
    public Sprite LivingRoomEntranceLit;

    // Kitchen
    SpriteRenderer kitchenRenderer;
    bool kitchenLit = false;
    public Sprite KitchenDark;
    public Sprite KitchenEntranceLit;

    void Start() {
        EntranceRenderer = GameObject.Find("Entrance_Background").GetComponent<SpriteRenderer>();
        EntranceDrawerRenderer = GameObject.Find("Entrance_Byra").GetComponent<SpriteRenderer>();
        livingCouchRenderer = GameObject.Find("Livingroom_Couch").GetComponent<SpriteRenderer>();
        livingTableRenderer = GameObject.Find("Livingroom_Table").GetComponent<SpriteRenderer>();
        livingRenderer = GameObject.Find("Livingroom_Background").GetComponent<SpriteRenderer>();
        kitchenRenderer = GameObject.Find("Kitchen_Background").GetComponent<SpriteRenderer>();
    }

    public void switchKitchenBool() {
        kitchenLit = !kitchenLit;
        if (kitchenLit == false) {
            kitchenRenderer.material = diffuseMaterial;
        } 
        else {
            kitchenRenderer.material = defaultMaterial;
        }
    }

    public void switchLivingBool() {
        livingLit = !livingLit;
        if (livingLit == false) {
            livingCouchRenderer.material = diffuseMaterial;
            livingRenderer.material = diffuseMaterial;
            livingTableRenderer.material = diffuseMaterial;
        }
        else {
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
    void changeRoomLightning() {
        if (entranceLit == true && livingLit == true && kitchenLit == true) {
            EntranceRenderer.sprite = EntranceLivAndKitLit;
            livingRenderer.sprite = LivingRoomEntranceLit;
            kitchenRenderer.sprite = KitchenEntranceLit;
        }
        else if (entranceLit == true && livingLit == true && kitchenLit == false) {
            EntranceRenderer.sprite = EntranceLivingRoomLit;
            livingRenderer.sprite = LivingRoomEntranceLit;
            kitchenRenderer.sprite = KitchenEntranceLit;
        } 
        else if (entranceLit == true && livingLit == false && kitchenLit == true) {
            EntranceRenderer.sprite = EntranceKitchenLit;
            livingRenderer.sprite = LivingRoomEntranceLit;
            kitchenRenderer.sprite = KitchenEntranceLit;
        }
        else if (entranceLit == false && livingLit == true && kitchenLit == true) {
            EntranceRenderer.sprite = EntranceLivAndKitLit;
            livingRenderer.sprite = LivingRoomDark;
            kitchenRenderer.sprite = KitchenDark;
        }
        else if (entranceLit == true && livingLit == false && kitchenLit == false) {
            EntranceRenderer.sprite = EntranceDark;
            livingRenderer.sprite = LivingRoomEntranceLit;
            kitchenRenderer.sprite = KitchenEntranceLit;
        }
        else if (entranceLit == false && livingLit == true && kitchenLit == false) {
            EntranceRenderer.sprite = EntranceLivingRoomLit;
            livingRenderer.sprite = LivingRoomDark;
            kitchenRenderer.sprite = KitchenDark;
        }
        else if (entranceLit == false && livingLit == false && kitchenLit == true) {
            EntranceRenderer.sprite = EntranceKitchenLit;
            livingRenderer.sprite = LivingRoomDark;
            kitchenRenderer.sprite = KitchenDark;
        }
        else if (entranceLit == false && livingLit == false && kitchenLit == false) {
            EntranceRenderer.sprite = EntranceDark;
            livingRenderer.sprite = LivingRoomDark;
            kitchenRenderer.sprite = KitchenDark;
        }
    }

    void Update() {
        changeRoomLightning();
    }
}