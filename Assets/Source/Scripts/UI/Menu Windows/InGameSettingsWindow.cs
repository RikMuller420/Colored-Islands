using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSettingsWindow : MenuWindow
{
    public override void Open()
    {
        if (IsOpened)
        {
            return;
        }

        Time.timeScale = 0f;
        base.Open();
    }

    public override void Close()
    {
        if (IsOpened == false)
        {
            return;
        }

        Time.timeScale = 1f;
        base.Close();
    }
}
