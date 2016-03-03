﻿
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MobileToggle : MonoBehaviour {
    bool isToggled = false;
    public GameObject optionsMenu;
    public GameObject saveMenu;
    public GameObject loadMenu;
    public GameObject exitMenu;
    public Button warpButton;

    void Start() {
        for (int i = 0; i < transform.childCount; ++i) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void togglePhone() {
        isToggled = !isToggled;
        transform.GetChild(1).gameObject.SetActive(isToggled);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Pathfinding>().setIsActive(!isToggled);
        toggleWarp();
    }

    public void toggleWarp() {
        if (Application.loadedLevelName == "Level01") {
            warpButton.interactable = false;
        } 
        else {
            warpButton.interactable = true;
        }
    }

    void FixedUpdate() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (optionsMenu.activeInHierarchy == true) {
                optionsMenu.SetActive(false);
                togglePhone();
            }
            else if (saveMenu.activeInHierarchy == true) {
                saveMenu.SetActive(false);
                togglePhone();
            }
            else if (loadMenu.activeInHierarchy == true) {
                loadMenu.SetActive(false);
                togglePhone();
            }
            else if (exitMenu.activeInHierarchy == true) {
                exitMenu.SetActive(false);
                togglePhone();
            }
            togglePhone();
        }

    }
}