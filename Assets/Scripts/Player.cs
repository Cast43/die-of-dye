using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movimentacao")]
    //primeiramente define a variável de movimento utilizada(nesse caso um vetor de dimensão 2)
    public Vector2 MoveInput; // variável em que irá armazenar os valores dos inputs (variam de 1 e -1)
    Vector2 mousePos;   // Vetor que irá armazenar a posição do mouse na tela;
    public float velocidade; // variável que multiplicará o vetor
    float fakeSpeed;    // Variável usada para manter o valor de velocidade
    public bool rbVelocity;// parametros booleanos para mudar o tipo de funcao de movimentação
    public bool rbMovePosit;
    [Header("Dash Attack")]
    public bool dashAttack = false; // Bool (true ou falso) para verificar se o player está em uma ação de dash
    public bool prepareAttack = false; // bool apra verificar se o player está carregando o ataque (será usado para calcular o tempo que ficou no prepare)
    public bool canDashAtk = true;  // Variável bool usada para deixar o player Dashar depois de um tempo (cooldown)
    public float distanceLimit = 0; // Limitador para o quanto o DashAttack pode ir longe no dash
    public float cooldownDash = 0;  // tempo para canDashAtk ficar true
    public float velDash;   // Rapides em que o dashAttack terá
    float dashAttackTime = 0; // variável de temporizador para saber quanto tempo o jogador ficou segurando o botão direito


    public Rigidbody2D rb;// definindo onde sera armazenado o componente rb do game object(nesse caso o rb do Player)

    void Start()
    {
        fakeSpeed = velocidade;    
        rb = GetComponent<Rigidbody2D>(); // pegando o componente rb do GameObject
        canDashAtk = true; 
    }

    void FixedUpdate()
    {
        MoveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        //nessa linha peguei os inputs horizontais e verticais, e atribui eles no X do vetor e no Y respectivamente
        // o .normalized faz com que o vetor tenha tamanho 1 sempre(normalizacao do vetor). Normalizando mantemos a direção do vetor
        // Pois se andarmos na diagonal sem normalizar teremos o X e o Y de MoveInput valendo 1 ou -1 fazendo com que
        // o player ande mais rapido nas diagonais
        if (rbVelocity && !dashAttack) // verifica se está no meio de um dash Attack(não deixa o jogador se mexer com W,A,S,D no meio de um dash)
        {
            rb.velocity = MoveInput * velocidade;// como o modulo do vetor vale 1 para aumentar a velocidade multiplica-se por velocidade
        }
        else if (rbMovePosit)
        {
            rb.MovePosition(rb.position + (MoveInput * velocidade) * Time.deltaTime);
        }
        else if (dashAttack) // Se estiver no meio de um DashAttack executará a função
        {

            Vector3 dashPos = (mousePos.normalized) * (dashAttackTime * 0.5f + 1);  
            // Utilizo a posição do mouse na tela normalizada e somada com a posição atual do player para gerar o ponto que o dash deve ir
            // e após isso multiplico pelo tempo em que o jogado ficou segurando o botão direito do mouse (dashAttackTime)
            // somadao com 1 para dar sempre um vailor maior
            print(dashPos);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + dashPos, Time.deltaTime * velDash);
            // aqui defino que a posição do player é uma nova posição gerada pela função MoveTowards a qual vai mover da posição atual do player
            // até a posição do player até a posição final do dash (criada baseada na posição do mouse em relação a tela), o ultimo requisição da função 
            // é a velocidade em que isso acontece 
        }
        if (prepareAttack) // função para verificar quanto tempo o jogador segurou o botão direito
        {
            DashDistance();
        }
    }
    void Update()
    {

        if (!dashAttack && canDashAtk)  // Se não estiver em um dash e ja tiver passado o cooldown pode dashar
        {
            if (Input.GetMouseButton(1) && !prepareAttack)// Verifica se o botão direito do mouse foi apertado
            {
                velocidade = fakeSpeed / 2; //Diminui a velocidade do player enquanto ele tiver com o botão apertado
                if (!prepareAttack)   // Isso é um gatinho para garantir que  a função seja rodada apenas 1 vez enquanto o botão Direto estiver segurado 
                {
                    prepareAttack = true; // faz com que afunção rode apenas 1 vez
                                          // e além disso permite que o contador da função DashDistance no FixedUpdate começe a contagem;
                    Debug.Log("Prepare");
                }
            }
            if (Input.GetMouseButtonUp(1) && prepareAttack)// verifica se o botão do mouse foi solto
            {
                prepareAttack = false;  // Faz com que o contador No FixedUpdate pare
                rb.velocity = Vector2.zero; // zera a velocidade dada pelo moveInput para não inteferir no dash
                Debug.Log("Release");
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; 
                // pega a posição do mouse com uma função da prórpia camera (pego a camera usada na cena e pego a posição da tela para um ponto no jogo)
                // subtraio da posição do player para gerar um vetor direção
                StartCoroutine(DashAttack());
                //starto uma coroutina para difinir os tempos do dash

            }
        }

    }
    public IEnumerator DashAttack() // Função que conta o tempo (nesse caso é o tempo de coldown).
    {
        dashAttack = true;      // antes do Yield return colocamos ações que devem ser executadas antes do temporizador rolar.
        canDashAtk = false;     
        MoveInput = Vector2.zero;// zero o move input para que não interfira no dash
        yield return new WaitForSeconds(dashAttackTime * 0.75f); //

        velocidade = fakeSpeed; //redefino a velocidade normal do player
        dashAttackTime = 0;     // reseto o temporizado para não interferir na próxima contagem
        dashAttack = false;     
        yield return new WaitForSeconds(cooldownDash);  // contador do cooldown do dash
        canDashAtk = true;                              // permito o dash


    }
    public void DashDistance()      
    {
        dashAttackTime += Time.deltaTime;   // Somo na variável dashAttackTime o tempo 
                                            //(essa parte eu copiei do vídeo de um cara, não sei qual a difereça desses tempos)
                                            // chuto que deve se que o Time.deltaTime é a diferença do tempo ele sempre da menor que 1

        if (dashAttackTime > distanceLimit) // verifico se o contador passou do limite e redefino el igual ao limite para que a distância mude
        {
            dashAttackTime = distanceLimit;
        }
        // A função DashDistance controla tanto o tempo de dash como a velocidade de dash
        // interessante observar que na função FixedUpdate() estou mexendo com o vetor velocidade em que o dash terá (rapidez do dash), 
        // e na Corotina DashAttack() há o controle do tempo em que o dash irá ter (distância do dash)
    }

}
