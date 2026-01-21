using YG;

namespace SlimeGround.Integration.DeviceInfo
{
	public class DeviceInfoProvider
	{
	    public DeviceType GetDeviceType()
	    {
	        YG2.Device yandexDeviceType = YG2.envir.device;

	        switch (yandexDeviceType)
	        {
	            case YG2.Device.Desktop:
	                return DeviceType.Desktop;

	            case YG2.Device.Mobile:
	                return DeviceType.Mobile;

	            case YG2.Device.Tablet:
	                return DeviceType.Tablet;

	            default:
	                return DeviceType.Desktop;
	        }
	    }
	}
}
