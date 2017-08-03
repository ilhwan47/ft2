using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WafflePanController : MonoBehaviour , IPointerUpHandler , IPointerDownHandler , IDragHandler {

	public WaffleStandWaffleController waffleStandWaffle;
	public GameObject waffleStand;
	public RectTransform waffle;

	Animator anim;
	AnimatorStateInfo panState;

	private Vector3 startPos;


	void Awake ()
	{
		anim = GetComponent <Animator> ();
	}

	void Satrt()
	{
		startPos = waffle.position;
	}

	public void OnDrag (PointerEventData data)
	{
		if(waffleStandWaffle.IsWaffleStandNotFull() && IsDone())
		{
			Vector3 newPos = data.position;
			waffle.position = newPos;
		}
	}

	public void OnPointerDown (PointerEventData data)
	{
	}

	public void OnPointerUp (PointerEventData data)
	{
		anim.SetTrigger("Pressed");
		if(waffleStandWaffle.IsWaffleStandNotFull() && IsDone())
		{
			if(data.pointerCurrentRaycast.gameObject == waffleStand)
			{
				waffleStandWaffle.PutWaffleToStand();
			}
			waffle.position = startPos;
		}

		panState = anim.GetCurrentAnimatorStateInfo(0);
		if(panState.IsName("Boil") || panState.IsName("Ready"))
		{
			startPos = waffle.position;
		}
	}

	bool IsDone()
	{
		panState = anim.GetCurrentAnimatorStateInfo(0);
		if(panState.IsName("Done") || panState.IsName("BurnWarning")) return(true);
		else return(false);
	}

}
