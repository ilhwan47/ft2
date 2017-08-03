using UnityEngine;
using System.Collections;

public class WaffleStandWaffleController : MonoBehaviour {

	public int waffleImageWidth = 22;
	public int waffleStandCapacity = 18;

	private RectTransform waffleStandWaffleImage;

	public int waffleQuantity;

	// Use this for initialization
	void Start ()
	{
		waffleQuantity = 0;
		waffleStandWaffleImage = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		waffleStandWaffleImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, waffleImageWidth * waffleQuantity);
	}

	public bool IsWaffleStandNotFull()
	{
		if(waffleQuantity < waffleStandCapacity) return(true);
		else return(false);
	}

	public bool IsWaffleStandNotEmpty()
	{
		if(waffleQuantity > 0) return(true);
		else return(false);
	}

	public void PutWaffleToStand()
	{
		waffleQuantity++;
	}

	public void GetWaffleFromStand()
	{
		waffleQuantity--;
	}
}
