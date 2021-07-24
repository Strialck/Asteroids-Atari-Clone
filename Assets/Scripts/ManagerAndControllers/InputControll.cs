using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AstroidCrasher;

namespace AstroidCrasher
{
    public enum InputType
    {
        Keyboard,
        KeyboardAndMouse
    }
}
public class InputControll : MonoBehaviour
{
    public GameEvent ThrustingChanged = delegate { };

    ShipController _shipController;
    public InputType DefalutInputType;


    [SerializeField]
    string _onlyKeyboardFireAxis;

    [SerializeField]
    string _keyboardMouseFireAxis;

    string _rotateAxis = "Horizontal";
    string _forwardAxis = "Vertical";
    string _fireAxis;

    delegate void Processing();
    Processing inputProcessing = delegate { };

    [SerializeField]
    bool _fireButtonPressed;

    bool _isThrusting;
    public bool isThrusting
    {
        get
        {
            
            return _isThrusting;
        }
        private set
        {
            _isThrusting = value;
            ThrustingChanged();
        }
    }

    void Start()
    {
        isThrusting = false;
        _fireButtonPressed = false;
        _shipController = GetComponent<ShipController>();
        SetInputType(DefalutInputType);
    }

    void FixedUpdate()
    {
        if (!_fireButtonPressed && Input.GetAxisRaw(_fireAxis) !=0)
        {
            _fireButtonPressed = true;
            _shipController.Fire();
        }
        if ( Input.GetAxisRaw(_fireAxis) == 0)
            _fireButtonPressed = false;

        inputProcessing();
    }

    void KeyboardInput()
    {
        float input = Input.GetAxisRaw(_rotateAxis);
        if (input != 0)
        {
            _shipController.Rotate(input,Time.deltaTime);
        }

        if (Input.GetAxis(_forwardAxis) > 0)
        {
            _shipController.MoveForward(Time.deltaTime);
            isThrusting = true;
        }

        else
            isThrusting = false;
    }

    void MouseAndKeyboardInput()
    {
        _shipController.RotateAt(Camera.main.ScreenToWorldPoint(Input.mousePosition), Time.deltaTime);
        if (Input.GetAxis(_forwardAxis) > 0)
        {
            _shipController.MoveForward(Time.deltaTime);
            isThrusting = true;
        }
            
        else
            isThrusting = false;
    }

    public void SetInputType(InputType inputType)
    {
        switch (inputType)
        {
            case InputType.Keyboard:
                {
                    _fireAxis = _onlyKeyboardFireAxis;
                    inputProcessing = KeyboardInput;
                    break;
                }
                
            case InputType.KeyboardAndMouse:
                {
                    _fireAxis = _keyboardMouseFireAxis;
                    inputProcessing = MouseAndKeyboardInput;
                    break;
                }
            default:
                break;
        }
    }
}
