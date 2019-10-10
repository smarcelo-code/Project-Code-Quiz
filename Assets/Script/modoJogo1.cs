using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class modoJogo1 : MonoBehaviour
{
    [Header("Configuração dos textos")]
    public      Text            nomeTematxt;
    public      Text            perguntatxt;
    public      Image           perguntaConfigimg;
    public      Text            infoRespostastxt;
    public      Text            notaFinaltxt;
    //config das msg de texto de parabéns
    public      Text            msgUmtxt;
    public      Text            msgDoistxt;

    [Header("Configuração dos Textos Alternativas")]
    public      Text            altAtxt;
    public      Text            altBtxt;
    public      Text            altCtxt;
    public      Text            altDtxt;

    [Header("Configuração das imagens Alternativas")]
    public      Image           altAimg;
    public      Image           altBimg;
    public      Image           altCimg;
    public      Image           altDimg;

    [Header("Configuração das barras")]
    public      GameObject      barraProgresso;
    public      GameObject      barraTempo;

    [Header("Configuração dos botões")]
    public      Button[]        botoes;
    public      Color           corAcerto, corErro;

    [Header("Configuração do modo de jogo")]
    public      bool            perguntasComImg;
    public      bool            perguntasaAleatorias;
    public      bool            jogarComTempo;
    public      float           tempoResponder;
    public      bool            mostrarCorreta;
    public      int             qtdePiscar;
    // utlizar um bool para indicar se as alts vão ou não ser utilizadas(modo de jogo)
    public      bool            utilizarAlternativas;
    public      bool            utilizarAlternativasIMG;

    [Header("Configuração das Perguntas")]
    public      string[]        perguntas;
    public      Sprite[]        perguntasimg; //sprite é o tipo de gráfico d vamos usar nesse array
    public      string[]        correta; // aqui decide ql é a CORRETA, SE FOR MULTIPLAESCOLHA
    public      int             qtdePerguntas; // qnts vou add dentro da lista
    public      List<int>       listaPerguntas; /*jogar o id das perguntas aqui e respondar.....onde está usando "perguntas" tem que mudar para lista de perguntas*/

    [Header("Configuração das Alternativas")]
    public      string[]        alternativasA;
    public      string[]        alternativasB;
    public      string[]        alternativasC;
    public      string[]        alternativasD;

    [Header("Configuração das Alternativas IMG")]
    public      Sprite[]        alternativasAimg;
    public      Sprite[]        alternativasBimg;
    public      Sprite[]        alternativasCimg;
    public      Sprite[]        alternativasDimg;

    [Header("Configuração dos Painéis")]
    public      GameObject[]    paineis;
    public      GameObject[]    estrela;
    public      GameObject      painelExplicativo;

    [Header("Configuração das Mensagens")]
    public      string[]        mensagem1; //msg
    public      string[]        mensagem2; //sub msg (que fica na parte de baixo)
    public      Color[]         corMensagem;


    //------------------------------------------------------------------------------//

    private     int         idResponder, qtdeAcertos, notaMin1Estrela, notaMin2Estrela, nEstrelas, idTema, idbtnCorreta;
    private     float       percProgresso, percTempo, qtdeRespondida, notaFinal, valorQuestao, TempoPassou;
    private     bool        exibindoCorreta, vinhetaParada;

    private soundController soundController; //o script criado
    
    //------------------------------------------------------------------------------//

    // Start is called before the first frame update
    void Start()
    {
        soundController = FindObjectOfType(typeof(soundController)) as soundController;

        //carregar os valores que trouxemos do tema
        //trazendo esses valores do sistema que a configuramos
        idTema = PlayerPrefs.GetInt("idTema");
        notaMin1Estrela = PlayerPrefs.GetInt("notaMin1Estrela");
        notaMin2Estrela = PlayerPrefs.GetInt("notaMin2Estrela");

        nomeTematxt.text = PlayerPrefs.GetString("nomeTema");

        barraTempo.SetActive(false);

        if (perguntasComImg) {
            montarListaPerguntasIMG();
        }
        else {
            montarListaPerguntas();
        }        

        //p/ barra começar com 0 || SCALE É UM VETOR
        progressaoBarra();
        controleBarraTempo();

        

        //qnd o jogo COMEÇA, ele inicia com o painel de responder 
        //e qnd ele termina(classe ProximaPergunta)
        paineis[0].SetActive(true);
        paineis[1].SetActive(false);                
    }

    // Update is called once per frame
    void Update()
    {
        if (jogarComTempo == true && exibindoCorreta == false && vinhetaParada == false) {
            TempoPassou += Time.deltaTime; //qnt um frame passou entre um fps e outro

            print("controle da barra do tempo chamada?");
            controleBarraTempo();

            if (TempoPassou >= tempoResponder) {
                //se o tempo acabou numa pergunta, ele chama o método PROXIMA PERGUNTA
                proximaPergunta();
            }
        }

    }

    
    public  void    montarListaPerguntas() {

        //faz a validação da qtde d perguntas para o teste em relação a qtde existente
        if (qtdePerguntas > perguntas.Length) {

            //vai garantir que não dê erro no sis. caso coloque mais perguntas que o config.
            //config de segurança para não DESTRUIR o projeto, Kappa

            qtdePerguntas = perguntas.Length;
        }
        valorQuestao = 10 / (float)qtdePerguntas; //perguntas n é float - ocorreu bug por não colocar float  

        //modo de jogo d perguntas random
        if (perguntasaAleatorias) {

            bool addPergunta = true;  
                               

            //verificar ENQUANTO NÃO tiver lista completa, continuar sorteando
            // e adicionando na lista
            while (listaPerguntas.Count < qtdePerguntas) {
                addPergunta = true; //para ter valor inicial
                                                             
                int rand = Random.Range(0, perguntas.Length);              
               
                //verificar se a pergunta está ou NÃO na lista

                foreach (int idPer in listaPerguntas) {
                    //se tiver um valor dentro da lista de perguntas que for 
                    //igual ao valor sorteado, ele recebe falso e NÃO add e reinicia o ciclo de volta
                    if (idPer == rand) {
                        addPergunta = false;
                    }
                }

                //sortear o valor e ANTES de ADD verificar se é true
                if (addPergunta) {
                    listaPerguntas.Add(rand);
                }
            }
            
        }
        //modo de jogo onde as perguntas não são randons

        else {            
                //caso a lista de perguntas NÃO seja aleatória      
                for (int i = 0; i < qtdePerguntas; i++) {
                    listaPerguntas.Add(i);
                }
        }                     
                   
         perguntatxt.text = perguntas[listaPerguntas[idResponder]];
                

        //verificar se é para alterar os txt dos botões alternativos
        if (utilizarAlternativas == true && utilizarAlternativasIMG == false) {
            altAtxt.text = alternativasA[listaPerguntas[idResponder]];
            altBtxt.text = alternativasB[listaPerguntas[idResponder]];
            altCtxt.text = alternativasC[listaPerguntas[idResponder]];
            altDtxt.text = alternativasD[listaPerguntas[idResponder]];
        }
        else if (utilizarAlternativas == true && utilizarAlternativasIMG == true) {//setar true nos 2 bools
            altAimg.sprite = alternativasAimg[listaPerguntas[idResponder]];
            altBimg.sprite = alternativasBimg[listaPerguntas[idResponder]];
            altCimg.sprite = alternativasCimg[listaPerguntas[idResponder]];
            altDimg.sprite = alternativasDimg[listaPerguntas[idResponder]];
        }
    }

    //f(x) responsável de montar list de perguntas cm img
    public void montarListaPerguntasIMG() {

        //faz a validação da qtde d perguntas para o teste em relação a qtde existente
        if (qtdePerguntas > perguntasimg.Length) {
            qtdePerguntas = perguntasimg.Length;
        }

        valorQuestao = 10 / (float)qtdePerguntas;

        //modo de jogo d perguntas random
        if (perguntasaAleatorias) {

            bool addPergunta = true;
           
            //verificar ENQUANTO NÃO tiver lista completa, continuar sorteando
            // e adicionando na lista
            while (listaPerguntas.Count < qtdePerguntas) {
                addPergunta = true; //para ter valor inicial
                               
                int rand = Random.Range(0, perguntasimg.Length);             
                
                //verificar se a pergunta está ou NÃO na lista

                foreach (int idPer in listaPerguntas) {
                    //se tiver um valor dentro da lista de perguntas que for 
                    //igual ao valor sorteado, ele recebe falso e NÃO add e reinicia o ciclo de volta
                    if (idPer == rand) {
                        addPergunta = false;
                    }
                }

                //sortear o valor e ANTES de ADD verificar se é true
                if (addPergunta) {
                    listaPerguntas.Add(rand);
                }
            }

        }
        //modo de jogo onde as perguntas não são randons

        else {            
               for (int i = 0; i < qtdePerguntas; i++) {
                    listaPerguntas.Add(i);
               }           
            
        }
        
        perguntaConfigimg.sprite = perguntasimg[listaPerguntas[idResponder]];

        //verificar se é para alterar os txt dos botões alternativos
        if (utilizarAlternativas == true && utilizarAlternativasIMG == false) {
            altAtxt.text = alternativasA[listaPerguntas[idResponder]];
            altBtxt.text = alternativasB[listaPerguntas[idResponder]];
            altCtxt.text = alternativasC[listaPerguntas[idResponder]];
            altDtxt.text = alternativasD[listaPerguntas[idResponder]];
        }
        else if (utilizarAlternativas == true && utilizarAlternativasIMG == true) {//setar true nos 2 bools
            altAimg.sprite = alternativasAimg[listaPerguntas[idResponder]];
            altBimg.sprite = alternativasBimg[listaPerguntas[idResponder]];
            altCimg.sprite = alternativasCimg[listaPerguntas[idResponder]];
            altDimg.sprite = alternativasDimg[listaPerguntas[idResponder]];
        }
    }    
    
    //f(x) responsável por processar a resp. dada pelo usuário
    public      void        responder (string alternativa) {
        
        //verifica se no modo de jogo está setado as alts.Corretas
        if (exibindoCorreta) {
            return;
        }

        qtdeRespondida += 1;
        progressaoBarra(); //foi retirado a chamada na proxima pergunta, pq ele só aumentava a barra qnd passava para a próxima, ao invés de responder
        
        
        //pegar o id que está na lista e verifica se a resposta está certa ou n
        if (correta[listaPerguntas[idResponder]] == alternativa) {
            
            
            qtdeAcertos += 1;
            soundController.playAcerto(); // qnd acertar, ele toca          
            //painelExplicativo.SetActive(true);
        }
        else {
            
            /* aqui tem que entrar o popup explicativo e pedir um tempo de loadscene para próx pergunta
              tentando aplicar o popup explicativo qnd o usuário responder a pergunta errada, mas tendo alguns problemas com leitura
              a chamada da "próxima pergunta" tem que entrar na nova função do popup para dar certo, colocar tempo ou btn para finalizar o futuro painel explicativo*/
            soundController.playErro();
            
            //if (painelExplicativo != null) {
            //    painelExplicativo.SetActive(true);
            //}

        }
        
        //se colocar dentro do IF /\ ele vai mostrar apenas se tiver certa

        //em caso de modo d jogo cm a exibição da correta, indica ql btn tem a resp correta
        switch (correta[listaPerguntas[idResponder]]) {
            case "A":
                idbtnCorreta = 0;
                break;
            case "B":
                idbtnCorreta = 1;
                break;
            case "C":
                idbtnCorreta = 2;
                print("testeC");
                break;
            case "D":
                idbtnCorreta = 3;
                print("testeD");
                break;
                
        }

        //antes de chamar a prox, indicar se está correta ou n
        //chamada da f(x) de piscar
        if (mostrarCorreta == true) {
            // antes de chamar a corrotina, colocar todos os botões de cor errado
            foreach (Button b in botoes) {
                b.image.color = corErro;
            }
            //impedir de clicar no botão enquanto estiver exibindo o pisca
            exibindoCorreta = true;
            botoes[idbtnCorreta].image.color = corAcerto;
            StartCoroutine("mostrarAlternativaCorreta");
        }
        else { //caso o modo d jogo não seja exibir a correta, chama a prox pergunta

            exibindoCorreta = true; //só para parar o tempo
            StartCoroutine("aguardarProxima"); //aguardar meio segundo antes de ir para prox

            //proximaPergunta();    
        }                    
        
    }


    //faz a chamada d uma nova pergunta ou finaliza o teste
    public      void        proximaPergunta() {
        idResponder += 1;

        TempoPassou = 0; //toda vez q tiver uma nova questão, volta a 0

        //qtdeRespondida += 1;
        //progressaoBarra();

        
        //fazer validação p/saber se tm mais perguntas ou se terminou
        if (idResponder < listaPerguntas.Count) {

            //saber se a pergunta é cm img ou n
            if (perguntasComImg) {
                perguntaConfigimg.sprite = perguntasimg[listaPerguntas[idResponder]];
            }
            else {
                perguntatxt.text = perguntas[listaPerguntas[idResponder]];
            }


            //se utilizar alternativa, ele altera o text dos botões
            if (utilizarAlternativas == true && utilizarAlternativasIMG == false) {
                altAtxt.text = alternativasA[listaPerguntas[idResponder]];
                altBtxt.text = alternativasB[listaPerguntas[idResponder]];
                altCtxt.text = alternativasC[listaPerguntas[idResponder]];
                altDtxt.text = alternativasD[listaPerguntas[idResponder]];
            }
            else if (utilizarAlternativas == true && utilizarAlternativasIMG == true) {//setar true nos 2 bools
                altAimg.sprite = alternativasAimg[listaPerguntas[idResponder]];
                altBimg.sprite = alternativasBimg[listaPerguntas[idResponder]];
                altCimg.sprite = alternativasCimg[listaPerguntas[idResponder]];
                altDimg.sprite = alternativasDimg[listaPerguntas[idResponder]];
            }
        }
        else {//caso o tst tenha sido finalizado, chama a f(x) calc final
            calcularNotaFinal();
        }
    }

    //f(x) que controla barra de progresso
    void progressaoBarra() {
        //att a resposta "1 de 25" || usar concatenar para colocar variáveis e ele ler os números
        infoRespostastxt.text = "Respondeu " + (qtdeRespondida) + " de " + listaPerguntas.Count + " perguntas";


        // a barra de progresso vai aumentando e lendo a array de perguntas para saber em qnts % está
        percProgresso = qtdeRespondida / listaPerguntas.Count;
        barraProgresso.transform.localScale = new Vector3(percProgresso, 1, 1);
    }

    //f(x) controla barra do tempo, em caso de modo d jogo cm o tempo
    void controleBarraTempo() {
        if (jogarComTempo) {
            barraTempo.SetActive(true); // se marcar o jogo com o tempo, ele exibe a barra

            //progressão da barra ao contrário, com o tempo acabando
            percTempo = ((TempoPassou - tempoResponder) / tempoResponder) * -1;

            //a barra está diminuindo para o -INFINITO, fazer controle para não passar de 0
            if (percTempo < 0) {
                percTempo = 0;
            }
            
            //usar o vetor para marcar o X com o valor da variável
            barraTempo.transform.localScale = new Vector3(percTempo, 1, 1);
        }        
    }

    //f(x) que calcula e grava nota final
    void calcularNotaFinal() {

        vinhetaParada = true; //entra true aqui, porque é onde importa, já que é aqui q as 3 estrelas vão ser dadas
        //arredondar a nota
        notaFinal = Mathf.RoundToInt(valorQuestao * qtdeAcertos);

        //se a nota for > a nota é gravada, se não vale o maior resultado
        if (notaFinal > PlayerPrefs.GetInt("notaFinal_" + idTema.ToString())) {
            //gravar a nota passada no tema
            PlayerPrefs.SetInt("notaFinal_" + idTema.ToString(), (int)notaFinal);
        }
                

        //a Label do nota final está recebendo a variável notaFinal que foi
        //feita antes, por isso ele recebe a pontuação
        notaFinaltxt.text = notaFinal.ToString();
        
        if (notaFinal == 10) {
            nEstrelas = 3;
            soundController.playVinheta();
        }
        else if (notaFinal >= notaMin2Estrela) {
            nEstrelas = 2;
        }
        else if (notaFinal >= notaMin1Estrela) {
            nEstrelas = 1;
        }

        notaFinaltxt.color = corMensagem[nEstrelas]; //cor nº nota
        //msg 1 text recebe o msg1 com a qntidade de estrelas ganhas
        msgUmtxt.text = mensagem1[nEstrelas];
        msgUmtxt.color = corMensagem[nEstrelas];    
        //sub msg
        msgDoistxt.text = mensagem2[nEstrelas];

        //f(x) para zerar todas as estrelas
        foreach (GameObject s in estrela) {

            s.SetActive(false);
        }

        //f(x) para ver a qtde de estrelas e ativa-las
        for (int i = 0; i < nEstrelas; i++) {
            estrela[i].SetActive(true);
        }


        //inverte os paineis agora-
        paineis[0].SetActive(false);
        paineis[1].SetActive(true);
    }


    IEnumerator aguardarProxima() {
        yield return new WaitForSeconds(1); //esperar meio segundo antes de chamar a próxima
        exibindoCorreta = false; //qnd terminar de esperar, recebe false
        proximaPergunta();
    }



    /*f(x) que pode pausar e voltar durante a execução (corrotina), toda
    // corrotina deve ter um retorno
    se quiser chamar essa f(x) em outro lugar, 
    tm q usar "startCorroutine("nome da corrotina") */

    //corroutine que faz a animação de piscar o botão da alt. correta
    IEnumerator mostrarAlternativaCorreta() {

        //definir qnts vezes ele vai piscar antes de ir para prox quest
        for (int i = 0; i < qtdePiscar; i++) {
            botoes[idbtnCorreta].image.color = corAcerto;
            yield return new WaitForSeconds(0.2f);            
            botoes[idbtnCorreta].image.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            //depois de passar esse tempo, vai mudar apra a cor branca q é a origin.
        }

        //no final, para garantir que todos vão voltar ao normal

        foreach (Button b in botoes) {
            b.image.color = Color.white;
        }
        exibindoCorreta = false;
        proximaPergunta();
        
        /* tm q colocar uma comando para ESPERAR p/ 
        NÃO ir a prox. quest, enquanto estiver fazendo esse proc.*/
    }


}
