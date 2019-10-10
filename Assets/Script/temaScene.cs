using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class temaScene : MonoBehaviour
{
    private         fade            fade;
    private         soundController soundController;

    public          Text            nomeTematxt;
    public          Button          btnJogar;


    [Header("Configuração das paginas")]
    public          GameObject[]    btnPaginacao;
    public          GameObject[]    painelTemas;
    private         bool            ativarBtnPagina;
    private         int             idPagina;           

    // Start is called before the first frame update
    void Start() {

        fade = FindObjectOfType(typeof(fade)) as fade;
        soundController = FindObjectOfType(typeof(soundController)) as soundController;
        //ele vai começar qnd tiver um tema selecionado
        
        onOffButtonsAndPannel();
    }
    

    void onOffButtonsAndPannel() {
        //se os tema ddo painel passarem d 1 pg, os botões dir. e esq. são ativados
        btnJogar.interactable = false;

        foreach (GameObject p in painelTemas) {
            p.SetActive(false);
        }

        painelTemas[0].SetActive(true);

        if (painelTemas.Length > 1) {

            ativarBtnPagina = true;

        }
        else {
            ativarBtnPagina = false;
        }

        foreach (GameObject b in btnPaginacao) {
            b.SetActive(ativarBtnPagina);
        }
    }


    //criar a f(x) do botão play
    public void Jogar() {

        soundController.playButton();
        soundController.audioMusic.clip = soundController.musicas[1];
        soundController.audioMusic.Play();

        //fazer uma validação primeiro
        int idCena = PlayerPrefs.GetInt("idTema");
        if (idCena !=0) {
            //se a cena existir (for !=0) o nome da cena é int e vai para string
            //SceneManager.LoadScene(idCena.ToString());
            StartCoroutine("transicao", idCena.ToString());
        }
    }


    //como fazer paginação?
    public void btnPagina(int i) {

        soundController.playButton();

        idPagina += i;
        if (idPagina < 0) {
            idPagina = painelTemas.Length - 1; //temos 2 pag, quando vai para primeira pag a esquerda (negativa), ele na vdd pula para última pagina
        }
        else if (idPagina >= painelTemas.Length) {
            idPagina = 0; //ficar pulando só entre as pg que existem
        }

        btnJogar.interactable = false;
        nomeTematxt.color = Color.white;
        nomeTematxt.text = "Selecione um tema!"; // toda vez q mudar d pg, desativar o play

        foreach (GameObject p in painelTemas) {
            p.SetActive(false);
        }

        painelTemas[idPagina].SetActive(true); //p/ ativar apenas a pag atual que estiver e desativar o resto
    }


    IEnumerator transicao(string nomeCena) {
        fade.fadeIn();
        yield return new WaitWhile(() => fade.fumeTransi.color.a < 0.9f);
        SceneManager.LoadScene(nomeCena);
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
