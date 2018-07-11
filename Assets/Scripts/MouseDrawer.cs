using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MouseDrawer : MonoBehaviour {

	private new Camera camera;
	
	public GameObject cube;
	public float depth = 5;
	public float speed = 5;
	private Vector3 mousePosition;

	// Use this for initialization
	void Start () {
		camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0)){
			mousePosition = GetMouseCameraPoint();
			cube.transform.position = Vector3.MoveTowards(cube.transform.position, mousePosition, speed * Time.deltaTime);
		
		}
		if(Input.GetMouseButtonUp(0)){
			StartCoroutine("WriteAndSendPng");
		}
	}

	private Vector3 GetMouseCameraPoint(){
		var ray = camera.ScreenPointToRay(Input.mousePosition);
		return ray.origin + ray.direction * depth;
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
