﻿using UnityEngine;
using System.Collections;

namespace managers
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        SubMenu[] menus;

        [SerializeField]
        GameObject play, pause;

        public static bool paused = false;
        public delegate void PauseAction();
        public static event PauseAction OnPause;


        void Start()
        {
            disableAllMenus();
            Play();
        }

        void Update()
        {
            if (Input.GetButtonDown(Axis.Cancel))
            {
                MenuManager.paused = !MenuManager.paused;
                if (MenuManager.paused)
                    Pause();
                else
                    Play();
            }
        }

        void disableAllMenus()
        {
            foreach (SubMenu m in menus)
            {
                if (m == null)
                    return;
                m.gameObject.SetActive(false);
            }
        }

        public void Play()
        {
            print("playing");
            play.SetActive(true);
            pause.SetActive(false);
            MenuManager.paused = false;
            FireEvents();
        }

        public void Pause()
        {
            print("pause");
            play.SetActive(false);
            pause.SetActive(true);
            MenuManager.paused = true;
            OpenMenu(0);
            FireEvents();
        }

        void FireEvents()
        {
            print("paused :" + MenuManager.paused);
            if (OnPause != null)
                OnPause();
        }

        public void OpenMenu(int id)
        {
            disableAllMenus();
            menus[id].gameObject.SetActive(true);
        }

        public void OpenLevel(int id)
        {
            Application.LoadLevel(id);
        }

        [System.Serializable]
        class SubMenu
        {
            public string name;
            public GameObject gameObject;
        }
    }
}