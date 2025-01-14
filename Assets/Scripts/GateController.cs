﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GateController : MonoBehaviour
{
    public TMP_Text _gateNumberText = null;
    public enum GateType
    {
        PositiveGate,
        NegativeGate
    }

    public GateType _gateType;

    public int _gateNumber;

    public int GetGateNumber()
    {
        return _gateNumber;
    }
 
    void Start()
    {
        RandomGateNumber();
    }   

    private void RandomGateNumber()
    {
        switch (_gateType)
        {
            case GateType.PositiveGate: _gateNumber = Random.Range(1, 10);
                                        _gateNumberText.text = _gateNumber.ToString();
                                        break;
            case GateType.NegativeGate: _gateNumber = Random.Range(-1, -10);
                                        _gateNumberText.text = _gateNumber.ToString();
                                        break;
        }
    }

}
