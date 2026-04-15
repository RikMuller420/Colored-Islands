using SlimeGround.Data.Saves;
using SlimeGround.Integration.DeviceInfo;
using SlimeGround.Menu.Extensions.Controls;
using UnityEngine;

namespace SlimeGround.Menu.Windows.Settings
{
	public class ShadowsModeChanger : MonoBehaviour
	{
		[SerializeField] private PlayerDataProvider _playerData;
		[SerializeField] private ToggleButton _toggle;

		private Integration.DeviceInfo.DeviceType _deviceType;

		private void OnEnable()
		{
			_toggle.ValueChanged += ChangeShadowsMode;
		}

		private void OnDisable()
		{
			_toggle.ValueChanged -= ChangeShadowsMode;
		}

		public void Initialize()
		{
			var deviceInfoProvider = new DeviceInfoProvider();
			_deviceType = deviceInfoProvider.GetDeviceType();

			if (_deviceType == Integration.DeviceInfo.DeviceType.Mobile)
			{
				_toggle.SetToggle(_playerData.IsShadowActiveOnMobile);
			}
			else
			{
				_toggle.SetToggle(_playerData.IsShadowActiveOnDesktop);
			}

			enabled = true;
		}

		private void ChangeShadowsMode(bool isOn)
		{
			if (_deviceType == Integration.DeviceInfo.DeviceType.Mobile)
			{
				_playerData.SetIsShadowActiveOnMobile(isOn);
			}
			else
			{
				_playerData.SetIsShadowActiveOnDesktop(isOn);
			}

			_playerData.Save();
		}
	}
}