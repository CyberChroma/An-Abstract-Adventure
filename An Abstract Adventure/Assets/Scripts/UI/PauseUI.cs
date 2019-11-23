using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{

    public GameObject pauseMenu;
    public Image[] menuButtons;
    public Color selectedColour;
    public UISlide[] uiSlides;

    private bool paused;
    private int selectedButton;
    private Color[] buttonsColours;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        paused = false;
        pauseMenu.SetActive(false);
        buttonsColours = new Color[4];
        for (int i = 0; i < buttonsColours.Length; i++)
        {
            buttonsColours[i] = menuButtons[i].color;
        }
        selectedButton = 0;
        menuButtons[selectedButton].color = selectedColour;
        foreach (UISlide uiSlide in uiSlides)
        {
            uiSlide.stayActive = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (paused)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (selectedButton == 0)
                {
                    Resume();
                }
                else if (selectedButton == 1)
                {
                    Options();
                }
                else if (selectedButton == 2)
                {
                    Progress();
                }
                else if (selectedButton == 3)
                {
                    ExitLevel();
                }
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                menuButtons[selectedButton].color = buttonsColours[selectedButton];
                if (selectedButton == 2)
                {
                    selectedButton = 0;
                }
                else if (selectedButton == 3)
                {
                    selectedButton = 1;
                }
                menuButtons[selectedButton].color = selectedColour;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                menuButtons[selectedButton].color = buttonsColours[selectedButton];
                if (selectedButton == 0)
                {
                    selectedButton = 2;
                }
                else if (selectedButton == 1)
                {
                    selectedButton = 3;
                }
                menuButtons[selectedButton].color = selectedColour;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                menuButtons[selectedButton].color = buttonsColours[selectedButton];
                if (selectedButton == 0)
                {
                    selectedButton = 1;
                }
                else if (selectedButton == 2)
                {
                    selectedButton = 3;
                }
                menuButtons[selectedButton].color = selectedColour;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                menuButtons[selectedButton].color = buttonsColours[selectedButton];
                if (selectedButton == 1)
                {
                    selectedButton = 0;
                }
                else if (selectedButton == 3)
                {
                    selectedButton = 2;
                }
                menuButtons[selectedButton].color = selectedColour;
            }
        }
    }

    void Pause ()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        paused = true;
        foreach (UISlide uiSlide in uiSlides)
        {
            uiSlide.stayActive = true;
        }
    }

    void Resume ()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        paused = false;
        foreach (UISlide uiSlide in uiSlides)
        {
            uiSlide.stayActive = false;
        }
    }

    void Options ()
    {
        print("Options");
        return;
    }

    void Progress ()
    {
        print("Progress");
        return;
    }

    void ExitLevel ()
    {
        print("Exit level");
        return;
    }
}
