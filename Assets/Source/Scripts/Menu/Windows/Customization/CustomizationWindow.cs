using SlimeGround.Integration.Metrics;
using SlimeGround.Menu.Extensions.Windows;

namespace SlimeGround.Menu.Windows.Customization
{
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
}
