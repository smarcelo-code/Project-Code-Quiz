using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fade : MonoBehaviour
{

    public GameObject painelTransicao;
    public Image fumeTransi;

    public Color[] corTransi;
    public float step; // vel da transição

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void fadeIn() {
        //escurecer
        painelTransicao.SetActive(true);
        StartCoroutine("fadeI");
    }

    //pq chamar a coroutine aqui? e não direto? pq com o script do btn, 
    //vai chamar as f(x) fadeIn e Out e isso facilita as chamadas 
    public void fadeOut() {
        
        StartCoroutine("fadeO");
    }


    IEnumerator fadeI() {
        for (float i = 0; i < 1; i+= step) {
            fumeTransi.color = Color.Lerp(corTransi[0], corTransi[1], i);
            //Lerp é justamente a f(x) que interpola as cores por um determinado tempo (entre cor 'a' e 'b')
            yield return new WaitForEndOfFrame(); //termina o frame e daew volta para o começo do loop
        }
    }


    IEnumerator fadeO() {

        for (float i = 0; i < 1; i += step) {
            fumeTransi.color = Color.Lerp(corTransi[1], corTransi[0], i);

            //print(fumeTransi.color.a); //verificar a cor em alfa, é daqui que visualiza a 
            //aproximação para o 0.9 estipuplado na tela de espera do PreTitulo

            //Lerp é justamente a f(x) que interpola as cores por um determinado tempo (entre cor 'a' e 'b')
            yield return new WaitForEndOfFrame(); //termina o frame e daew volta para o começo do loop
        }


        painelTransicao.SetActive(false); // qnd fica totalmente claro, ele desativa, por isso está aqui na corroutine e não na f(x)
        
    }
}
