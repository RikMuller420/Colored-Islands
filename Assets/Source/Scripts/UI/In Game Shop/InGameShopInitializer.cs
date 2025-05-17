using System.Collections.Generic;
using UnityEngine;

public class InGameShopInitializer : MonoBehaviour
{
    [SerializeField] private BoostSettings _boostSettings;
    [SerializeField] private List<BoostOfferLine> _boostOfferLines = new ();

    public void Initialize(BoostAmountProvider boostAmountProvider, WalletProvider walletProvider)
    {
        foreach (BoostOfferLine boostOfferLine in _boostOfferLines)
        {
            boostOfferLine.Initialize(boostAmountProvider, _boostSettings, walletProvider);
        }
    }
}
