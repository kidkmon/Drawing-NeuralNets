using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrawer : MonoBehaviour {

	public Camera camera;
	
	public float speed = 5;
	private Vector3 mousePosition;

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0)){
			Vector3 temp = Input.mousePosition;
			temp.z = 10;
			transform.position = Camera.main.ScreenToWorldPoint(temp);
			GetComponent<TrailRenderer>().enabled = true;
		}
		if(Input.GetMouseButtonUp(0)){
			StartCoroutine("WriteAndSendPng");
		}
	}


	IEnumerator WriteAndSendPng(){
		RenderTexture currentRT = RenderTexture.active;
		var rTex = new RenderTexture(camera.pixelHeight, camera.pixelHeight, 16);
		camera.targetTexture = rTex;
		RenderTexture.active = camera.targetTexture;
		camera.Render();
		Texture2D tex = new Texture2D(camera.targetTexture.width, camera.targetTexture.height, TextureFormat.RGB24, false);
		tex.ReadPixels(new Rect(0,0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
		tex.Apply(false);
		RenderTexture.active = currentRT;
		byte[] bytes = tex.EncodeToPNG();
		Destroy(tex);
		System.IO.File.WriteAllBytes("test.png", bytes);

		yield return new WaitForSeconds(1f);
	}
}
