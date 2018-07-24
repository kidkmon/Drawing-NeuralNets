using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBaseballBat : MonoBehaviour {
	//Prefab da esfera
	public GameObject batPrefab;
	//Objeto base onde vai ser posicionado a nova esfera
	public GameObject Placer;
	//Verifica se pode colocar a esfera
	bool CanPlace;
	
	// Update is called once per frame
	void Update () {
		//Se o botão correspondente ao toque no touchpad do controle direito for pressionade
		if(Input.GetKeyDown("joystick button 19")){
			//Ativa o objeto placer
			CanPlace = true;
			Placer.SetActive(true);
		}
		
		//Se o botão correspondete ao toque no touchpad for solto
		if(Input.GetKeyUp("joystick button 19")){
			//Desativa o objeto placer
			CanPlace = false;
			Placer.SetActive(false);
		}

		//Se o touchpad for pressionado
		if(Input.GetKeyDown("joystick button 17")){
			if(CanPlace){
				//Cria uma nova esfera
				Instantiate(batPrefab, Placer.transform.position, Quaternion.identity);
			}
		}
	}
}
