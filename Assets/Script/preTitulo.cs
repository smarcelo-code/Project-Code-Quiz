using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class preTitulo : MonoBehaviour {

    private         fade    fade;

    public          int     tempoEspera;

    // Start is called before the first frame update
    void Start() {
        fade = FindObjectOfType(typeof(fade)) as fade;

        StartCoroutine("esperar");
        //se quiser colocar animação, colocara aqui
        //colocar tbm alguma apresentação no ínicio
    }

    IEnumerator esperar() {
        yield return new WaitForSeconds(tempoEspera);

        //ao invés de ir para prox tela, ele vai chamar:
        fade.fadeIn(); //script.f(x)

        //pause o comando -- espere enquanto > 
        yield return new WaitWhile(() => fade.fumeTransi.color.a < 0.9f); //0.9 é uma transição aproximada do alfa da cor, estipulado esse valor na Unity, Canvas Transição - Step
        SceneManager.LoadScene("Titulo");
    }
}