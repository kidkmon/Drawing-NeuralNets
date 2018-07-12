using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;
using System;
using System.IO;

public class MouseDrawer : MonoBehaviour {

	public Camera camera;
	public Text resultText;
	public float speed = 5;
	public GameObject[] baseballPrefabs;

	private Vector3 mousePosition;
	private SocketIOComponent socket;
	private JSONObject dataResult;
	private bool ballInstiate, batInstatiate;

	void Start(){
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();

		socket.On("open", TestOpen);
		socket.On("reply", OnReply);
		socket.On("error", TestError);
		socket.On("close", TestClose);

		resultText.text = "Start";
		StartCoroutine("BeepBoop");
	}

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

		if(dataResult != null){
			if(dataResult.ToString() == "\"ball\"" && !ballInstiate){
				ballInstiate = true;
				Instantiate(baseballPrefabs[0],baseballPrefabs[0].transform.position, baseballPrefabs[0].transform.rotation);
			}	

			if(dataResult.ToString() == "\"baseball bat\"" && !batInstatiate){
				batInstatiate = true;
				Instantiate(baseballPrefabs[1],baseballPrefabs[1].transform.position, baseballPrefabs[1].transform.rotation);
			}
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

		string s =  Convert.ToBase64String(bytes);

		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField("dado", "from c# kid");
		j.AddField("img", s);
		socket.Emit("chat message", j);
		Destroy(tex);
		System.IO.File.WriteAllBytes("test.png", bytes);
		yield return new WaitForSeconds(1f);
	}


	private IEnumerator BeepBoop(){
		// wait 1 seconds and continue
		yield return new WaitForSeconds(1);
		
		socket.Emit("beep");
		
		// wait 3 seconds and continue
		yield return new WaitForSeconds(3);
		
		socket.Emit("beep");
		
		// wait 2 seconds and continue
		yield return new WaitForSeconds(2);
		
		socket.Emit("beep");
		
		// wait ONE FRAME and continue
		yield return null;
		
		socket.Emit("beep");
		socket.Emit("beep");
	}

	public void TestOpen(SocketIOEvent e){
		//Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
	}
	
	public void TestBoop(SocketIOEvent e){
		//Debug.Log("[SocketIO] Boop received: " + e.name + " " + e.data);

		if (e.data == null) { return; }

		// Debug.Log(
		// 	"#####################################################" +
		// 	"THIS: " + e.data.GetField("this").str +
		// 	"#####################################################"
		// );
	}
	
	public void TestError(SocketIOEvent e){
		//Debug.Log("[XXXXXXXX] Error received: " + e.name + " " + e.data);
	}
	
	public void TestClose(SocketIOEvent e){	
		//Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
	}

	public void OnReply(SocketIOEvent e){
		dataResult = e.data[1];
		resultText.text = e.data[1].ToString();
		print(e.data[1].ToString());
	}
}
