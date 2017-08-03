using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PumpController : MonoBehaviour , IPointerUpHandler , IPointerDownHandler , IDragHandler {

	Animator anim;
	AnimatorStateInfo pumpState;
	public Slider pumpGauge;
	public float heatDownTime = 2f;
	public float gaugeSpeed = 2.5f;
	public int gaugeVolumeMax = 10;

	private int gaugeVolumeCurrent;
	private float gaugeImageMin = 0.061f;
	private float gaugeImageMax = 0.939f;
	private float gaugeImageCurrent;
	private float gaugeImageTarget;
	private float gaugeImageUnit;
	private bool gaugeCallback;
	private float heatDownCount;

	void Awake ()
	{
		anim = GetComponent <Animator> ();
		pumpGauge.value = gaugeImageMin;
		gaugeImageUnit = (gaugeImageMax - gaugeImageMin) / (gaugeVolumeMax-1);
		gaugeVolumeCurrent = 0;
		gaugeImageCurrent = gaugeImageMin;
	}

	public void OnDrag (PointerEventData data)
	{
	}

	public void OnPointerUp (PointerEventData data)
	{
	}

	public void OnPointerDown (PointerEventData data)
	{
		pumpState = anim.GetCurrentAnimatorStateInfo(0);
		if(pumpState.IsName("PumpHandleReady"))
		{
			anim.SetTrigger("PumpPressed");
			if(gaugeVolumeCurrent < gaugeVolumeMax)
			{
				gaugeVolumeCurrent++;
			}
			heatDownCount = 0;
		}
	}

	void HeatDown()
	{
		if(heatDownCount > heatDownTime)
		{
			if(gaugeVolumeCurrent != 0)
			{
				gaugeVolumeCurrent--;
			}
			heatDownCount = 0;
		}
		heatDownCount += Time.deltaTime;
	}

	void Update()
	{
		gaugeImageTarget = gaugeVolumeCurrent * gaugeImageUnit;
		gaugeImageCurrent = gaugeImageCurrent + ((gaugeImageTarget - gaugeImageCurrent) * gaugeSpeed * Time.deltaTime);
		pumpGauge.value = gaugeImageCurrent;
		HeatDown();
	}
}
