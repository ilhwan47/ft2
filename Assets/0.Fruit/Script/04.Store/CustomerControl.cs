﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CustomerControl : MonoBehaviour
{
    public Animator _animator;
    public NavMeshAgent _navigation;
    public Image _imageOrder;
    public Image _imageResult;
    
    Vector3 _vecMoveTarget;
    CustomerManager.E_CUSTOMER_STATE _eState;
    CustomerManager.E_CUSTOMER_TYPE _eCustomerType;
    CustomerManager.E_CUSTOMER_ORDER _eCustomerOrder;

    void Start()
    {
        _imageOrder.gameObject.SetActive(false);
        _imageResult.gameObject.SetActive(false);
    }

    void Update()
    {
        if (_eState == CustomerManager.E_CUSTOMER_STATE.DESTORY)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            if (!_navigation.isStopped)
            {
                float fDistance = Vector3.Distance(gameObject.transform.position, _vecMoveTarget);
                if (0.1f > fDistance)
                {
                    _animator.SetInteger("iState", 0);
                    _navigation.isStopped = true;

                    switch (_eState)
                    {
                        case CustomerManager.E_CUSTOMER_STATE.CREATE:
                            {
                                _eState = CustomerManager.E_CUSTOMER_STATE.WAIT;
                                Invoke("WaitOrder", 5f);
                            }
                            break;

                        case CustomerManager.E_CUSTOMER_STATE.MOVE:
                            {
                                _eState = CustomerManager.E_CUSTOMER_STATE.WAIT;
                                CancelInvoke("WaitOrder");
                                Invoke("WaitOrder", 5f);
                            }
                            break;

                        case CustomerManager.E_CUSTOMER_STATE.RESULT_GOOD:
                        case CustomerManager.E_CUSTOMER_STATE.RESULT_BAD:
                        case CustomerManager.E_CUSTOMER_STATE.RESULT_SAD:
                            {
                                _eState = CustomerManager.E_CUSTOMER_STATE.DESTORY;
                            }
                            break;
                    }
                }
            }
        }
    }

    void ShowOrder()
    {
        _imageOrder.gameObject.SetActive(true);

        switch (_eCustomerOrder)
        {
            case CustomerManager.E_CUSTOMER_ORDER.RED: _imageOrder.color = Color.red; break;
            case CustomerManager.E_CUSTOMER_ORDER.YELLOW: _imageOrder.color = Color.yellow; break;
            case CustomerManager.E_CUSTOMER_ORDER.BLUE: _imageOrder.color = Color.blue; break;
        }

        _imageOrder.transform.localScale = Vector3.zero;
        iTween.ScaleTo(_imageOrder.gameObject, iTween.Hash("x", 1f, "y", 1f, "z", 1f, "easeType", iTween.EaseType.easeOutBack, "time", 1f));
    }

    void WaitOrder()
    {
        CustomerManager._this.ButtonJuice(CustomerManager.E_CUSTOMER_ORDER.WAIT_ORDER);
    }

    void ResultError()
    {
        GameObject.Destroy(gameObject);
    }

    public void Create(Vector3 vecTarget, CustomerManager.E_CUSTOMER_TYPE eCustomerType, CustomerManager.E_CUSTOMER_ORDER eCustomerOrder)
    {
        _eCustomerType = eCustomerType;
        _eCustomerOrder = eCustomerOrder;
        SetAction(vecTarget, CustomerManager.E_CUSTOMER_STATE.CREATE);
        Invoke("ShowOrder", 8f);
    }

    public void SetAction(Vector3 vecTarget, CustomerManager.E_CUSTOMER_STATE eState)
    {
        _animator.SetInteger("iState", 1);
        _navigation.SetDestination(vecTarget);
        _navigation.isStopped = false;
        _vecMoveTarget = vecTarget;
        _eState = eState;
        
        switch (_eState)
        {
            case CustomerManager.E_CUSTOMER_STATE.RESULT_GOOD:
                {
                    _imageOrder.gameObject.SetActive(false);
                    _imageResult.gameObject.SetActive(true);
                    _imageResult.sprite = CustomerManager._this.GetSprite(0);
                    _imageResult.transform.localScale = Vector3.zero;
                    iTween.ScaleTo(_imageResult.gameObject, iTween.Hash("x", 1f, "y", 1f, "z", 1f, "easeType", iTween.EaseType.easeOutBack, "time", 1f));
                    Invoke("ResultError", 10f);
                }
                break;

            case CustomerManager.E_CUSTOMER_STATE.RESULT_BAD:
                {
                    _imageOrder.gameObject.SetActive(false);
                    _imageResult.gameObject.SetActive(true);
                    _imageResult.sprite = CustomerManager._this.GetSprite(2);
                    _imageResult.transform.localScale = Vector3.zero;
                    iTween.ScaleTo(_imageResult.gameObject, iTween.Hash("x", 1f, "y", 1f, "z", 1f, "easeType", iTween.EaseType.easeOutBack, "time", 1f));
                    Invoke("ResultError", 10f);
                }
                break;

            case CustomerManager.E_CUSTOMER_STATE.RESULT_SAD:
                {
                    _imageOrder.gameObject.SetActive(false);
                    _imageResult.gameObject.SetActive(true);
                    _imageResult.sprite = CustomerManager._this.GetSprite(1);
                    _imageResult.transform.localScale = Vector3.zero;
                    iTween.ScaleTo(_imageResult.gameObject, iTween.Hash("x", 1f, "y", 1f, "z", 1f, "easeType", iTween.EaseType.easeOutBack, "time", 1f));
                    Invoke("ResultError", 10f);
                }
                break;
        }
    }

    public Vector3 GetMoveTarget()
    {
        return _vecMoveTarget;
    }

    public CustomerManager.E_CUSTOMER_STATE GetState()
    {
        return _eState;
    }

    public CustomerManager.E_CUSTOMER_ORDER GetOrder()
    {
        return _eCustomerOrder;
    }
}