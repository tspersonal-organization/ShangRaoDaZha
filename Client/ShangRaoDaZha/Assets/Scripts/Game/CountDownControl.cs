using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownControl : MonoBehaviour
{
    UILabel m_label;
    float tmpTimer;
    int currentCountDown;
    // Use this for initialization
    void Start()
    {
        m_label = gameObject.GetComponent<UILabel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCountDown > 0)
        {
            m_label.text = currentCountDown.ToString();
            tmpTimer += Time.deltaTime;
            if (tmpTimer >= 1)
            {
                tmpTimer = 0;
                currentCountDown -= 1;
                if (currentCountDown < 0)
                {
                    currentCountDown = 0;
                }
            }
            m_label.text = currentCountDown.ToString();
        }
    }

    public void SetCountDown(int CountDownSeconds)
    {
        m_label = gameObject.GetComponent<UILabel>();
        currentCountDown = CountDownSeconds;
        m_label.gameObject.SetActive(true);
    }
}
