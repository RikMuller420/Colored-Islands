using System;
using SlimeGround.Data.Saves;
using SlimeGround.Integration.Metrics;
using SlimeGround.Menu.Extensions.Windows;
using UnityEngine;

namespace SlimeGround.Menu.Windows.Customization
{
	public class CustomizationWindow : MenuWindow
	{
		[SerializeField] private PlayerDataProvider _dataProvider;

		private DateTime _lastOpenTime;

		public override void Open()
	    {
	        if (IsOpened)
	        {
	            return;
			}

	        base.Open();

			if (_dataProvider.IsCustomizationWindowWasOpened == false)
			{
				_dataProvider.SetCustomizationWindowWasOpened();
				_dataProvider.Save();
			}

			_lastOpenTime = DateTime.Now;
			MetricSaver.OpenCustomizationWindow();
	    }

	    public override void Close()
	    {
	        if (IsOpened == false)
	        {
	            return;
	        }

	        base.Close();

			float spendedSeconds = (float)(DateTime.Now - _lastOpenTime).TotalSeconds;
	        MetricSaver.CloseCustomizationWindow(spendedSeconds);
	    }
	}
}
