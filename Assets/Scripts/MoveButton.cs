using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField] private Player _player;
    [SerializeField]private float _dir;
    private bool _isHolding;

    private void Awake()
    {
        if (_dir < -1) _dir = -1;
        if (_dir > 1) _dir = 1;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isHolding = true;
    }

    private void Update()
    {
        if(_isHolding) _player.SetMoveInput(_dir);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isHolding = false;
        _player.SetMoveInput(0);
    }
}
