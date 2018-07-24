using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.WSA.Input;
//using HoloToolkit.Unity.InputModule;

public class GrabObjectScript : MonoBehaviour {
	//Collider do objeto que esta sendo segurado
	Collider Grabable;
	//Variavel que determina qual controle nesse script, esquerdo ou direito
	public bool isLeftController;
	//Forca a ser aplicada no lancamento
	public float ForceApply;
	//Objetos para posicionar o objeto segurado
	public Transform Pivot, ControllerTip;
	//Linha que mostra a direção do controle
	public LineRenderer line;
	//Fila que guarda as velocidades do controle para usar no calculo do lancamento
	public Queue<Vector3> velocities;
	//Audio de teleporte
	//public AudioSource TeleportAudio;
	//Referencia ao script de teleporte
	//public MixedRealityTeleport teleportScript;
	// Use this for initialization
	void Start () {
		velocities = new Queue<Vector3>(10);
		velocities.Enqueue(Vector3.zero);
		//Adiciona ao evento de teleport a função de tocar som
		//teleportScript.OnTeleport += TeleportAudio.Play;
	}
	
	// Update is called once per frame
	void Update () {
		//Verifica qual o controle nesse script
		if(isLeftController){
			//Verifica se o player está colocando o thumbstick esquerdo para frente
			if(Input.GetAxis("Vertical") > 0.9f){				
				//ativa o laser e direciona para frente do controle
				line.enabled = true;
				line.SetPosition(0, ControllerTip.position);
				
				line.SetPosition(1, ControllerTip.position + ControllerTip.up * 3);
			} else {
				line.enabled = false;			
			}
			//se o botão correspondente ao grip esquerdo for pressionado
			if (Input.GetKeyDown("joystick button 4"))
			{	
				//Se tiver algum objeto para segurar
				if(Grabable != null){
					Grab();
				}
			//Se o botão correspondente ao grip for solto
			} else if(Input.GetKeyUp("joystick button 4")){
				//Se tiver algum objeto para soltar
				if(Grabable != null){
					Release();
				}
			}
		} else {
			//Verifica se o player está colocando o thumbstick direito para frente
			if(Input.GetAxis("Vertical2") > 0.9f){
				//ativa o laser e direciona para frente do controle
				line.enabled = true;
				line.SetPosition(0, ControllerTip.position);
				
				line.SetPosition(1, ControllerTip.position + ControllerTip.up * 3);
			} else {
				line.enabled = false;				
			}
			//se o botão correspondente ao grip direito for pressionado
			if (Input.GetKeyDown("joystick button 5"))
			{
				if(Grabable != null){
					Grab();
				}
			//se o botão correspondente ao grip direito for solto
			} else if(Input.GetKeyUp("joystick button 5")){
				if(Grabable != null){
					Release();
				}
			}
		}

		//Verifica a velocidade atual do controle e adiciona a fila de velocidadesç
		Vector3 actVel = GetControllerVelocity(isLeftController);
		velocities.Dequeue();
		velocities.Enqueue(actVel);
	}
	//função para calcular a velocidade a ser aplicada no objeto
	public Vector3 VelocityResult(){
		Vector3 vel = Vector3.zero;
		foreach (Vector3 v in velocities)
		{
			vel += v;
		}

		return vel/velocities.Count;
	}	
	//Pegar a velocidade do controle
	Vector3 GetControllerVelocity(bool isLeft){
		InteractionSourceState[] states = InteractionManager.GetCurrentReading();
		foreach(InteractionSourceState state in states){
			if((isLeft && state.source.handedness == InteractionSourceHandedness.Left)
			|| !isLeft && state.source.handedness == InteractionSourceHandedness.Right){
				Vector3 vel;
				if(state.sourcePose.TryGetVelocity(out vel)){
					return vel;
				} 
			}
		}
		return Vector3.zero;
	}
	//Pegar a velocidade angular do controle
	Vector3 GetControllerAngularVelocity(bool isLeft){
		InteractionSourceState[] states = InteractionManager.GetCurrentReading();
		foreach(InteractionSourceState state in states){
			if((isLeft && state.source.handedness == InteractionSourceHandedness.Left)
			|| !isLeft && state.source.handedness == InteractionSourceHandedness.Right){
				Vector3 vel;
				if(state.sourcePose.TryGetAngularVelocity(out vel)){
					return vel;
				} 
			}
		}
		print("Cant get velocity");
		return Vector3.zero;
	}
	//Segurar um objeto
	void Grab(){
		//Desativa a fisica do objeto
		Grabable.GetComponent<Rigidbody>().isKinematic = true;
		//Coloca ele como parent do controle para fazer segui-lo
		Grabable.transform.parent = Pivot;
	}
	//Solta um objeto
	void Release(){
		//Ativa a fisica no objeto
		Rigidbody body = Grabable.GetComponent<Rigidbody>();
		body.isKinematic = false;
		//Tira o parent para faze-lo para de seguir
		Grabable.transform.parent = null;
		//Se for aplicar uma força
		if(ForceApply > 0){
			//Aplica a velocidade e velocidade angular do controle no objeto
		 	body.angularVelocity = GetControllerAngularVelocity(isLeftController);
			body.AddForce(new Vector3(0,VelocityResult().y * ForceApply * 10, 0));
		}
	}
	//Quando algum objeto colodir com o controle
	void OnTriggerEnter(Collider coll){
		//Se a tag permitir segura-lo
		if(coll.gameObject.CompareTag("Grabable")){
			//Coloca na variavel para poder segurar
			Grabable = coll;
		}
	}
	//Quando sair da colisão de algum objeto
	void OnTriggerExit(Collider coll){
		//Se esse objeto for seguravel, tira ele da variavel para segurar
		if(coll == Grabable){
			Grabable = null;
		}

	}
}
