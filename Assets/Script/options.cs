using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;//para fazer transição de cenas

public class options : MonoBehaviour
{

    private         soundController         soundController;
    public          GameObject              painel1, painelOption;
    public          Toggle                  onOffMusic, onOffEfects;
    public          Slider                  volSliMusic, volSliEfects;
    public          GameObject              mySecondCanvas;
    


    // Start is called before the first frame update
    void Start()
    {
        soundController = FindObjectOfType(typeof(soundController)) as soundController;
        carregarPrefs();
;       painel1.SetActive(true);
        painelOption.SetActive(false);
    }

    public void configuracoes(bool onOff) {
        soundController.playButton();
        painel1.SetActive(!onOff); 
        painelOption.SetActive(onOff);
    }
         

    //public void MyFunction() {
       
    //    mySecondCanvas.SetActive(true);
    //}


    //public void OnClickButton(string choice) {
    //    if (choice == "continue") {
    //        mySecondCanvas.SetActive(false);
    //    }
    //}

    public void zerarProgresso() {

        //mySecondCanvas.SetActive(true);
        //options.FindObjectOfType(OnClickButton(typeof)) soundController = FindObjectOfType(typeof(soundController)) as soundController;
        

        soundController.playButton();
        //sound controller = options
        int onOffMusicaSoundCtrl = PlayerPrefs.GetInt("onOffMusic");
        int onOffEfectsSoundCtrl = PlayerPrefs.GetInt("onOffEfects");
        float volMusic = PlayerPrefs.GetFloat("volMusic");
        float volEffects = PlayerPrefs.GetFloat("volEffects");

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetInt("valoresDefault", 1); // apaga tudo e regrava os valores de preferências de audio
        PlayerPrefs.SetInt("onOffMusic", onOffMusicaSoundCtrl);
        PlayerPrefs.SetInt("onOffEfects", onOffEfectsSoundCtrl);
        PlayerPrefs.SetFloat("volMusic", volMusic);
        PlayerPrefs.SetFloat("volEffects", volEffects);

    }


    public void mutarMusic() {
        //faz a msm coisa que a f(x) de baixo
        soundController.audioMusic.mute = !onOffMusic.isOn; //esse obj/propiedade recebe o valor contrário do onOffMusic / o Mute é referente ao Unity e o onOff é o cod do jogo
        /*
        if (onOffMusic.isOn == false) {
            soundController.audioMusic.mute = true;
            //para que o botão de mute no "music" da unity concilie com o mute do jogo
            //se for mutado no jogo (true) a Unity recebe false
            //se for mutado na Unity (true) o jogo recebe false (Jogo= estrela no config (onOff))
        }
        else {
            soundController.audioMusic.mute = false;
        }*/

        if (onOffMusic.isOn) {
            PlayerPrefs.SetInt("onOffMusic", 1);
        }
        else {
            PlayerPrefs.SetInt("onOffMusic", 0);
        }

    }


    public void mutarEfects() {

        soundController.audioFx.mute = !onOffEfects.isOn;

        if (onOffEfects.isOn) {
            PlayerPrefs.SetInt("onOffEfects", 1);
        }
        else {
            PlayerPrefs.SetInt("onOffEfects", 0);
        }
    }


    public void volMusic() {
        //value é declarado lá na unity 0~1 foi o colocado (on e off)
        soundController.audioMusic.volume = volSliMusic.value; //pegou/recebeu o volmusic
        PlayerPrefs.SetFloat("volMusic", volSliMusic.value);
    }


    public void volEffects() {
        //tanto na f(x) de cima qnt nessa, qnd sai da cena e volta e vai em options, as barras resetam, msm o vol continuando o msm

        soundController.audioFx.volume = volSliEfects.value;
        PlayerPrefs.SetFloat("volEffects", volSliEfects.value);
        print("aaa");
    }

    void carregarPrefs() {

        print("ta entrando no prefs do option?");

        //sound controller = options
        //carrega os values de config de sons
        int onOffMusicaSoundCtrl = PlayerPrefs.GetInt("onOffMusic");
        int onOffEfectsSoundCtrl = PlayerPrefs.GetInt("onOffEfects");
        float volMusic = PlayerPrefs.GetFloat("volMusic");
        float volEffects = PlayerPrefs.GetFloat("volEffects"); //verificar se ele está chamando a classe ou o que está especificado como método/parametro

        bool tocarMusic = false;
        bool tocarEffects = false;

        //print(tocarMusic);
        //print(tocarEffects);

        if (onOffMusicaSoundCtrl == 1) {
            tocarMusic = true;
        }
        if (onOffEfectsSoundCtrl == 1) {
            tocarEffects = true;
        }

        //print(tocarMusic);
        //print(tocarEffects);

        //precisa do toggle aqui = compara com o que recebe do carregar prefs
        onOffMusic.isOn = tocarMusic;
        onOffEfects.isOn = tocarEffects;
        //slider aqui = compara com o que recebe do carregar prefs
        volSliMusic.value = volMusic;
        volSliEfects.value = volEffects;
    }
}
