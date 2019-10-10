using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundController : MonoBehaviour
{
    //futura alteração de volumes
    public      AudioSource         audioMusic, audioFx;
    public      AudioClip           somAcerto, somErro, somBotao, vinheta3Estrelas;
    public      AudioClip[]         musicas;

    void Awake() {
        DontDestroyOnLoad(this.gameObject);//não vai "destruir" entre os downloads e vai manter entre todas as cenas
    }

    // Start is called before the first frame update
    void Start()
    {
        carregarPrefs(); // antes do audio, para saber as configs da music

        audioMusic.clip = musicas[0];
        audioMusic.Play(); //trocou o audio clip e dps deu play
        //AudioS = GetComponent<AudioSource>();
    }

    
    public void playAcerto() {
        //audioFx.volume = 0.6f;
        //audioFx.PlayOneShot(somAcerto); //dps o playoneshot, ele vai tocar, sem interferir na anterior, no msm volume que já estiver

        audioFx.clip = somAcerto;
        audioFx.Play();
    }


    public void playErro() {
        //audioFx.volume = 1;
        //audioFx.PlayOneShot(somErro); 
        audioFx.clip = somErro;
        audioFx.Play();
    }


    public void playButton() {
        //audioFx.volume = 1;
        //audioFx.PlayOneShot(somBotao); isso foi comentado, porque antes o volume não estava setado com o players prefs, logo precisava setar
        audioFx.clip = somBotao;
        audioFx.Play();
    }


    public void playVinheta() {
        audioFx.clip = vinheta3Estrelas; // vai tocar por cima da música msm, qnd der 3 stars
        audioFx.Play();
    }


    void carregarPrefs() {
        //método para qnd rodar o game pela 1º vez, ele vai 
        //setar alguns valores default - vai ter que ser valor inteiro, 
        //porque n da para gravar bool, logo utilizaremos 1 e 0

        // -------------------("aqui lê o valor") == aqui faz a comparação)
        if (PlayerPrefs.GetInt("valoresDefault") == 0) {
           
            PlayerPrefs.SetInt("valoresDefault", 1); // se tiver como 0(ou seja, eles n existem), ele recebe 1
            PlayerPrefs.SetInt("onOffMusic", 1);
            PlayerPrefs.SetInt("onOffEfects", 1);
            PlayerPrefs.SetFloat("volMusic", 1);
            PlayerPrefs.SetFloat("volEffects", 1);
            // se na primeira vez q rodar, n tiver setado valores, ele recebe os que foram passados agr
        }
        //carrega os values de config de sons
        int onOffMusicaSoundCtrl = PlayerPrefs.GetInt("onOffMusic");
        int onOffEfectsSoundCtrl = PlayerPrefs.GetInt("onOffEfects");
        float volMusic = PlayerPrefs.GetFloat("volMusic");
        float volEffects = PlayerPrefs.GetFloat("volEffects"); //verificar se ele está chamando a classe ou o que está especificado como método/parametro

        //print(onOffMusicaSoundCtrl);
        //print(onOffEfectsSoundCtrl);
        //print(volMusic);
        //print(volEffects);

        bool tocarMusic = false;
        bool tocarEffects = false;

        if (onOffMusicaSoundCtrl == 1) {
            tocarMusic = true;
        }
        if (onOffEfectsSoundCtrl == 1) {
            tocarEffects = true;
        }     


        audioMusic.mute = !tocarMusic; 
        audioFx.mute = !tocarEffects;

        audioMusic.volume = volMusic;
        audioFx.volume = volEffects;
    }
}
