public class CustomizationWindow : MenuWindow
{
    public override void Open()
    {
        if (IsOpened)
        {
            return;
        }

        base.Open();
        MetricSaver.OpenCustomizationWindow();
    }

    public override void Close()
    {
        if (IsOpened == false)
        {
            return;
        }

        base.Close();
        MetricSaver.CloseCustomizationWindow();
    }
}
