using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class btnComandos : MonoBehaviour
{
    private         fade                fade;
    private         soundController     soundController;
    
    void Start() {
        soundController = FindObjectOfType(typeof(soundController)) as soundController;
        fade = FindObjectOfType(typeof(fade)) as fade;
    }


    
    
    public void irParaCena(string nomeCena) {
        soundController.playButton();

        //evitar que qnd troca do titulo p/tema ou vice versa, não mude a música, só cenas externas(gameplay)
        if (SceneManager.GetActiveScene().name != "Titulo" && SceneManager.GetActiveScene().name != "temas") {
            soundController.audioMusic.clip = soundController.musicas[0];
            soundController.audioMusic.Play();
        }
       
        StartCoroutine("transicao", nomeCena);

    }


    
    public void sair() {
        Application.Quit();
    }

    
    public void jogarNovamente() {

        soundController.playButton();
        //validar a cena e ler a cena gravada

        int idCena = PlayerPrefs.GetInt("idTema");

        if (idCena != 0) {
            
            SceneManager.LoadScene(idCena.ToString());
        }
    }

    //escurecer a tela
    IEnumerator transicao(string nomeCena) {
        fade.fadeIn();
        yield return new WaitWhile(() => fade.fumeTransi.color.a < 0.9f); 
        SceneManager.LoadScene(nomeCena);
    }

    //public void configs(bool onOff) {//parâmetro pesquisar
    //                                 //se as configs tiverem on, abre o painel de option
    //    soundController.playButton();
    //    painel1.SetActive(!onOff);
    //    painelOption.SetActive(onOff);
    //    //qnd clicar no options, passa o true do painel1, logo ele recebe false e o option recebe true sem o not, lógica interruptor 
        
    //}


    //public void zerarJogo() {
    //    soundController.playButton();
    //    PlayerPrefs.DeleteAll(); //vai zerar td, inclusive configs
    //    //colocar aqui um painel para abrir um novo, perguntando realmente se deseja fazeer isso
    //}

    //a merda é q tem varias cenas e ter q colocar várias cenas(config o btn de todas elas) para cada música 
    //public void voltarTitulo() {
    //    soundController.audioMusic.clip = soundController.musicas[0];
    //    soundController.audioMusic.Play();
    //    SceneManager.LoadScene("Titulo");
    //}


    //public void irTemas() {
    //    soundController.audioMusic.clip = soundController.musicas[0];
    //    soundController.audioMusic.Play();
    //    SceneManager.LoadScene("temas");
    //}
}
