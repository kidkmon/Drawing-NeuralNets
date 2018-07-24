using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSphereScript : MonoBehaviour {
	//Prefab da esfera
	public GameObject SpherePrefab;
	public GameObject BatPrefab;
	//Objeto base onde vai ser posicionado a nova esfera
	public GameObject Placer;
	//Verifica se pode colocar a esfera
	bool CanPlace;
	
	// Update is called once per frame
	void Update () {
		// //Se o botão correspondente ao toque no touchpad do controle direito for pressionade
		// if(Input.GetKeyDown("joystick button 18")){
		// 	//Ativa o objeto placer
		// 	CanPlace = true;
		// 	Placer.SetActive(true);
		// }
		
		// //Se o botão correspondete ao toque no touchpad for solto
		// if(Input.GetKeyUp("joystick button 18")){
		// 	//Desativa o objeto placer
		// 	CanPlace = false;
		// 	Placer.SetActive(false);
		// }

		//Se o touchpad for pressionado
		// if(Input.GetKeyDown("joystick button 16")){
		// 	if(CanPlace){
		// 		//Cria uma nova esfera
		// 		Instantiate(SpherePrefab, Placer.transform.position, Quaternion.identity);
		// 	}
		// }

		if(CursorDrawer.ballInstiate && Input.GetKeyDown("joystick button 16") ){
			Instantiate(SpherePrefab, Placer.transform.position, Quaternion.identity);
			CursorDrawer.ballInstiate = false;
		}

		if(CursorDrawer.batInstatiate && Input.GetKeyDown("joystick button 17") ){
			BatPrefab.SetActive(true);
		}
		
		if(CursorDrawer.batInstatiate && Input.GetKeyDown("joystick button 5") ){
			BatPrefab.SetActive(false);
			CursorDrawer.batInstatiate = false;
		}
		
	}
}
