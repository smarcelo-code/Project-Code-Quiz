using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //ter acesso aos botões, textos...

public class temaInfo : MonoBehaviour
{
    private soundController soundController;
    private temaScene temaScene;

    [Header("Configuração do Tema")]// a primeira variável, no caso idTema, tem q ser isolada, não pode ser declarada 2 variáveis na msm linha (1º)
    public      int              idTema;
    public      string           nomeTema;
    public      Color            corTema;
    public      bool             requerNotaMin;
    public      int              notaMinimaNecess;

    [Header("Configuração das Estrelas")]
    public      int              notaMin1Estrela;
    public      int              notaMin2Estrela;

    [Header("Configuração do Botão")] //só funciona se tiver uma variável em baixo
    public      Text            idTematxt;
    public      GameObject[]    estrela; //array que vai  receber a qntidade de figuras/estrelas

    private      int            notaFinal;
    private      Button         btnTema;

    // Start is called before the first frame update    
    void Start()
    {
        soundController = FindObjectOfType(typeof(soundController)) as soundController;

        notaFinal = PlayerPrefs.GetInt("notaFinal_" + idTema.ToString());

        temaScene = FindObjectOfType(typeof(temaScene)) as temaScene; //typeof = do tipo  pegar ele como ele msm (temascene)

        idTematxt.text = idTema.ToString();

        estrelas();
        //desligar as "estrelas" que estão sobrepostas.... s = star

        //coloca nates de chamar o método, se não ele não seta o botão para false
        btnTema = GetComponent<Button>(); //para ter acesso ao botão, az nessa variável o próprio botão

        print("chamada da nota minima ta indo?");
        verificarNotaMin();       
    }

    void verificarNotaMin() {
        //tm que desabilitar o botão no START e nessa f(x) vai habilitar
        btnTema.interactable = false;

        //fazer a validação
        if (requerNotaMin) {
            int notaTemaAnterior = PlayerPrefs.GetInt("notaFinal_" + (idTema - 1).ToString()); //pegar receber nota do tema anterior e verificar
            if (notaTemaAnterior >= notaMinimaNecess) {
                btnTema.interactable = true;
            }
        }
        else {
            btnTema.interactable = true;
            
        }
    }

   
    // Update is called once per frame
    void Update()
    {
        
    }

    public void selecionarTema() {

        soundController.playButton();

        temaScene.nomeTematxt.text = nomeTema;
        temaScene.nomeTematxt.color = corTema;

        //gravar o ID o entre "" é a variável que vai ser criada no mobile
        // e o sem "" é a variável local da unity
        PlayerPrefs.SetInt("idTema", idTema);
        //gravar nome
        PlayerPrefs.SetString("nomeTema", nomeTema);
        PlayerPrefs.SetInt("notaMin1Estrela", notaMin1Estrela);
        PlayerPrefs.SetInt("notaMin2Estrela", notaMin2Estrela);

        //a partir do momento que selecionar um tema, o btn é ativado
        temaScene.btnJogar.interactable = true;

        /*OBS: Essas f(x) são POR tema, o valor dessas variáveis muda por tema*/
    }


    public void estrelas() {
       
        foreach (GameObject s in estrela) {

            s.SetActive(false);
        }

        int nEstrelas = 0;

        if(notaFinal == 10) {
            nEstrelas = 3;
        }
        else if (notaFinal >= notaMin2Estrela) {
            nEstrelas = 2;
        }
        else if (notaFinal >= notaMin1Estrela) {
            nEstrelas = 1;
        }
        
        for(int i = 0; i<nEstrelas; i++) {
            estrela[i].SetActive(true);
        }

    }

}
