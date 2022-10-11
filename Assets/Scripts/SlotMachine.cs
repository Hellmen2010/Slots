using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    [SerializeField] Button spinButton;
    [SerializeField] Rigidbody[] wheels;
    [SerializeField] GameObject registrator;
    [SerializeField, Min(0.1f)] float rotationTime;
    [SerializeField, Min(1)] int minSpeed = 1; //set Min value in insp
    [SerializeField] int maxSpeed = 100;

    private readonly int numberOfFacetes = 12;
    private float minRotationStep;

    private void Awake()
    {
        minRotationStep = 360 / numberOfFacetes;
    }
    public void Spin()
    {
        SpinButtonControl();

        foreach (var wheel in wheels)
        {
            var a = Random.Range(minSpeed, maxSpeed);
            var v3end = new Vector3(wheel.transform.eulerAngles.x, wheel.transform.eulerAngles.y, wheel.transform.eulerAngles.z + a * minRotationStep);
            DOTweenModulePhysics.DORotate(wheel, v3end, rotationTime, RotateMode.FastBeyond360);
        }

        CheckCombination();
    }
    private async void CheckCombination()
    {
        await Task.Delay((int)(rotationTime * 1000 + 50));
        for (int i = 0; i < wheels.Length; i++)
        {
            registrator.transform.position = new Vector3(-1.5f, 0, wheels[i].transform.position.z);
            RaycastHit hit;
            Physics.Linecast(registrator.transform.position, wheels[i].transform.position, out hit, 1<<10);
            Debug.Log(hit.collider.name);
        }
    }
    private async void SpinButtonControl()
    {
        spinButton.interactable = false;
        await Task.Delay((int)(rotationTime * 1000 + 100));
        spinButton.interactable = true;
    }
}
