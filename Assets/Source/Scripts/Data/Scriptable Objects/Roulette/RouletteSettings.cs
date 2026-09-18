using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.Roulette
{
	[CreateAssetMenu(fileName = "RouletteSettings", menuName = "Custom/RouletteSettings")]
	public class RouletteSettings : ScriptableObject
	{
		[SerializeField] private int _spinGoldPrice = 200;

		public int SpinGoldPrice => _spinGoldPrice;
	}
}
