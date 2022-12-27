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
    public float distanceRayHit = 1; // Distancia que a linha colisora terá para detectar o inimigo
    public float cooldownDash = 0;  // tempo para canDashAtk ficar true
    public float slowTimeDuration = 0;  // tempo em que o tempo vai ficar lento
    public float slowFactor = 0;  // tempo em que o tempo vai ficar lento
    public int dashDmg = 0;  // dano do dash no inimigo
    public float velDash;   // Rapides em que o dashAttack terá
    public float dashAtkTime;   // Tempo em que o dashAttack durará
    float timer = 0; // variável de temporizador para saber quanto tempo o jogador ficou segurando o botão direitoa
    public LayerMask maskPlayer; // variável de temporizador para saber quanto tempo o jogador ficou segurando o botão direitoa

    [Header("BloodTrail")]
    public bool bloodTrail;
    public bool canPaint;
    public GameObject linePrefab;
    public GameObject currentLine;
    public List<GameObject> lines;
    public List<Vector2> linesPos;
    public List<Vector2> bloodPos;
    public LineRenderer lineRenderer;
    public float timeOfTrail;
    public float timeBleeding;
    public GameObject DamageArea;



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

            Vector3 dashPos = (mousePos.normalized) * (timer * 0.5f + 1);
            // Utilizo a posição do mouse na tela normalizada e somada com a posição atual do player para gerar o ponto que o dash deve ir
            // e após isso multiplico pelo tempo em que o jogado ficou segurando o botão direito do mouse (timer)
            // somadao com 1 para dar sempre um vailor maior
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
            if (Input.GetMouseButton(1))// Verifica se o botão direito do mouse foi apertado
            {
                velocidade = fakeSpeed / 2; //Diminui a velocidade do player enquanto ele tiver com o botão apertado
                Mathf.Clamp(velocidade, fakeSpeed / 2, fakeSpeed / 2);
                if (!prepareAttack)   // Isso é um gatinho para garantir que  a função seja rodada apenas 1 vez enquanto o botão Direto estiver segurado 
                {
                    prepareAttack = true; // faz com que afunção rode apenas 1 vez
                                          // e além disso permite que o contador da função DashDistance no FixedUpdate começe a contagem;
                }
            }
            if (Input.GetMouseButtonUp(1) && prepareAttack)// verifica se o botão do mouse foi solto
            {
                prepareAttack = false;  // Faz com que o contador No FixedUpdate pare
                rb.velocity = Vector2.zero; // zera a velocidade dada pelo moveInput para não inteferir no dash
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                // pega a posição do mouse com uma função da prórpia camera (pego a camera usada na cena e pego a posição da tela para um ponto no jogo)
                // subtraio da posição do player para gerar um vetor direção

                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, (mousePos.normalized), (timer * distanceRayHit), ~maskPlayer);
                // nessa linha atribuo uma nova variável de Physics2D que são os raycast. Os raycast são linhas que detectam a colição com outros colisores
                // nesse caso utilizei o RaycastAll pois presiso saber todos os elemento em que o colisor do DashAttack irá acertar e fazer com que o player
                // teleporte para o último inimigo.
                // faço a RAY(linha) da posição do player até a posição final do dash (fiz uma gambiarra que deu certo pra arrumar essa ray).
                // e o último parâmetro é a layer que eu quero ignorar, para que o colisor do player não interaja com a Ray
                Debug.DrawRay(transform.position, (mousePos.normalized) * (timer * distanceRayHit), Color.red, 3);
                // isso é para eu conseguir ver aonde o Ray está (printa a ray no editor)

                int enemysHit = 0; // determina quantos inimigos foram atingidos.
                // usei isso para saber qual função usar. Função MissDashAttack (não acertou nenhum inimigo) ou DashAttackKill (acertou)

                for (int i = 0; i < hits.Length; i++) // Faço um loop para passar por todos os inimigos em que o Ray colidiu
                {                                     // O RaycastAll retorna uma lista com os hits e eu preciso navegar por eles
                    if (hits[i].collider.GetComponent<Life>() == true)
                    {
                        if (hits[i].collider.GetComponent<Life>().currentHealth > 0 && !hits[i].collider.isTrigger)
                        {   // na linha de cima verifico se na vida do objeto colidido a vida atual dele é maior que 0, pois isso verifica pra mim se ele tem esse componente
                            // e se ele ta vivo. Logo após, vejo se o colisor verificado não é trigger (faço isso, pois o inimigo possui um colisor de agro e o raycast vai detectar isso)
                            // e eu quero que a Ray detecte apenas a colisor que está presente no player

                            enemysHit++;    // conto quantos inimigos foram atingidos
                            StartCoroutine(DashAttackKill(hits[i].collider));   // rodo a corotina passando quem foi atingido de acordo com a Ray
                            hits[i].collider.GetComponent<Life>().StartCoroutine("TakeDmg", dashDmg); // pego o componente de vida do que foi atingido e rodo a função de tomar dano da vida
                            Debug.Log("Acertou Enemy");
                        }
                    }
                }
                if (enemysHit > 0) // Aqui verifico se alguém foi atingido, se foi não continuo o resto da função
                {
                    return;
                }

                StartCoroutine(MissDashAttack());// se o ray não colidiu ninguém roda essa função
                Debug.Log("Errou");


            }
        }
        if (Input.GetKeyDown(KeyCode.E) && lines.Count > 2 && canPaint)
        {

            GameObject area = Instantiate(DamageArea, Vector3.zero, Quaternion.identity);
            PolygonCollider2D collider2D = area.GetComponent<PolygonCollider2D>();

            collider2D.SetPath(0, linesPos);
            for (int i = 0; i < linesPos.Count - 1; i++)
            {
                UpdateLine(linesPos[i]);
            }
            lineRenderer.sortingOrder = 2;
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;

            linesPos.Clear();
            for (int i = lines.Count - 2; i > -1; i--)
            {
                Destroy(lines[i]);
                lines.RemoveAt(i);
            }

            StartCoroutine(CleanTrails(area, currentLine));

        }

        if (Time.timeScale != 1)    // Verifico se timeScale ja está normalizado(= 1)
        {
            Time.timeScale += (1f / slowTimeDuration * 1.5f) * Time.unscaledDeltaTime;  // aqui eu faço uma doidera que eu compiei de um vídeo
                                                                                        // como ta no update vai alterar o timeScale até ser igual a 1
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);   // aqui eu limito que o valor máximo de timeScale será 1
        }
        if (bloodTrail && dashAttack)
        {
            UpdateLine(transform.position);
        }
    }
    public IEnumerator MissDashAttack() // Função que conta o tempo (nesse caso é o tempo de coldown) e permite o dash em FixedUpdate.
    {
        dashAttack = true;      // antes do Yield return colocamos ações que devem ser executadas antes do temporizador rolar.
        canDashAtk = false;     // variável relacionada ao cooldown do ataque
        MoveInput = Vector2.zero;// zero o move input para que não interfira no dash

        if (bloodTrail)
        {
            CreateLine();
            yield return new WaitForSeconds(timer * dashAtkTime); //
            linesPos.Add(transform.position);
            velocidade = fakeSpeed; //redefino a velocidade normal do player
            timer = 0;     // reseto o temporizado para não interferir na próxima contagem
            dashAttack = false;
            yield return new WaitForSeconds(cooldownDash);  // contador do cooldown do dash
            canDashAtk = true;
            yield return null;

        }

        yield return new WaitForSeconds(timer * dashAtkTime); //




        velocidade = fakeSpeed; //redefino a velocidade normal do player
        timer = 0;     // reseto o temporizado para não interferir na próxima contagem
        dashAttack = false;
        yield return new WaitForSeconds(cooldownDash);  // contador do cooldown do dash
        canDashAtk = true;                              // permito o dash


    }
    public IEnumerator DashAttackKill(Collider2D hit) // Função que faz com que o player se teleporte para tras do inimigo atingido.
    {
        rb.MovePosition(hit.transform.position - (transform.position - hit.transform.position).normalized);
        CreateLine();
        linesPos.Add(hit.transform.position - (transform.position - hit.transform.position).normalized);

        UpdateLine(hit.transform.position);
        StartCoroutine(BloodTrail());

        // função de movimentação em que eu pego a posição do inimigo atingido e subtraio pela posição do player menos a posição do inimigo atingido normalizada
        // em "transform.position - hit.transform.position" cria-se um vetor, e normalizando esse vetor temos a direção em que o player está dando o dash attack
        // em relação o inimigo
        // com isso tendo esse vetor normalizado, também temos um ponto e subtraindo esse ponto da posição do inimigo temos outro vetor/ponto que da atras do inimigo 
        velocidade = fakeSpeed; //redefino a velocidade normal do player
        timer = 0;     // reseto o temporizado para não interferir na próxima contage

        Time.timeScale = slowFactor;    // Fator de escala de tempo do jogo (Normalmente é 1) pego essa escala e passo ela para a variável
        Time.fixedDeltaTime = Time.timeScale * 0.02f;   // É necessário também arrumar o FixedDeltaTime que é o tempo em que as físicas do jogo funcionam
                                                        // E ele precisa ser arrumado também. São 2 funções de física diferente
        yield return new WaitForSeconds(slowTimeDuration);  // contador do cooldown do dash

    }
    public void DashDistance()
    {
        timer += Time.deltaTime;   // Somo na variável timer o tempo 
                                   //(essa parte eu copiei do vídeo de um cara, não sei qual a difereça desses tempos)
                                   // chuto que deve se que o Time.deltaTime é a diferença do tempo ele sempre da menor que 1

        if (timer > distanceLimit) // verifico se o contador passou do limite e redefino el igual ao limite para que a distância mude
        {
            timer = distanceLimit;
        }
        // A função DashDistance controla tanto o tempo de dash como a velocidade de dash
        // interessante observar que na função FixedUpdate() estou mexendo com o vetor velocidade em que o dash terá (rapidez do dash), 
        // e na Corotina DashAttack() há o controle do tempo em que o dash irá ter (distância do dash)
    }

    public void CreateLine()
    {
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lines.Add(currentLine);
        linesPos.Add(transform.position);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        bloodPos.Clear();
        bloodPos.Add(transform.position);
        bloodPos.Add(transform.position);
        lineRenderer.SetPosition(0, bloodPos[0]);
        lineRenderer.SetPosition(1, bloodPos[1]);

    }
    public void UpdateLine(Vector2 position)
    {
        bloodPos.Add(transform.position);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
    }
    public IEnumerator BloodTrail()
    {
        bloodTrail = true;
        transform.GetComponent<SpriteRenderer>().color = Color.green;
        yield return new WaitForSeconds(timeOfTrail);
        transform.GetComponent<SpriteRenderer>().color = Color.white;
        bloodTrail = false;
    }
    public IEnumerator CleanTrails(GameObject area, GameObject currentLine)
    {
        canPaint = false;
        yield return new WaitForSeconds(timeBleeding);
        Destroy(area);
        lines.Remove(currentLine);
        Destroy(currentLine);

        canPaint = true;
    }

}
