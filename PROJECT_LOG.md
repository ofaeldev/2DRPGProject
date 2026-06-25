# RPG2D - Diario do Projeto

Ultima atualizacao: 2026-06-24

## Acordo de trabalho

- O projeto sera construido em conjunto, com foco em aprendizado.
- Codex atua como professor: explica conceitos, faz perguntas e revisa o trabalho.
- Codex nao escreve codigo pronto sem um pedido explicito.
- Sugestoes de melhoria devem explicar o problema, o motivo e as alternativas.
- Antes de cada nova funcionalidade, definimos um objetivo pequeno e verificavel.
- Este arquivo deve ser atualizado quando houver uma decisao, aprendizado ou marco relevante.

## Estado atual

- Unity: `6000.3.10f1`
- Render pipeline: Universal Render Pipeline
- Input: Input System `1.18.0`
- Cena atual: `Assets/Scenes/SampleScene.unity`
- Git: `branch main` - estruturado e pronto para registrar o estado funcional
- **22 scripts implementados** em 4 sistemas principais
- **Arquitetura estável** com componentes bem definidos
- Console: sem erros reportados no overview de 2026-06-20

## Sistemas implementados (revisao 2026-06-20)

### 1. Player System (4 componentes)
- `PlayerController` - Orquestrador
- `PlayerMovement` - Fisica com lock de estado
- `PlayerInteraction` - Deteccao de interativos via Physics2D overlap
- `PlayerAnimator` - Parametros do animator

### 2. Input System
- `PlayerInputSource` - Bridge com Input System
- `PlayerInputReader` - Estado centralizado
- `InputSystem_Actions` - Gerado automaticamente

### 3. UI System (3 subsistemas)
- **Dialogue**: `DialogueService` + `DialogueController` + `DialoguePanelUI` + dados (`DialogueNode`, `DialogueOption`)
- **PopUp**: `PopUpService` + `PopUpController` + `PopUpManagerUI`
- **Feedback**: `FeedbackService` + `FeedbackController` + `FeedbackPanelUI`

### 4. World System
- `IInteraction` - Interface para interativos
- `NpcInteractable` - Objetos com diálogo
- `FeedbackInteractable` - Objetos com mensagem

### 5. Services (4 static services)
- `GameplayStateService` - Multi-lock system (Dialogue, Inventory, Cutscene, Menu, Loading, Feedback)
- `DialogueService` - Event broadcaster para diálogos
- `PopUpService` - Event broadcaster para hints
- `FeedbackService` - Event broadcaster para mensagens

## Arquitetura atual do jogador

O fluxo de movimento esta dividido em responsabilidades:

1. `PlayerInputSource` recebe eventos do Input System.
2. `PlayerInputReader` guarda a direcao atual e a ultima direcao valida.
3. `PlayerMovement` aplica a velocidade no `Rigidbody2D` durante `FixedUpdate`.
4. `PlayerAnimator` atualiza os parametros do Animator.
5. `PlayerController` cria o leitor e conecta os componentes.

Essa divisao e uma boa base para aprender responsabilidade unica e reduzir o
acoplamento entre entrada, fisica e apresentacao.

## Pontos para estudar

- Ciclo de vida da Unity: `Awake`, `OnEnable`, `Update`, `FixedUpdate` e `OnDisable`.
- Diferenca entre entrada, estado do jogador e movimento fisico.
- Referencias serializadas e riscos de referencias ausentes no Inspector.
- Encapsulamento: decidir o que deve ser publico, privado ou serializado.
- Ciclo de inscricao e remocao de eventos do Input System.
- Diferenca entre scripts autorais e codigo gerado automaticamente.
- Ordem de inicializacao entre componentes e como criar conexoes idempotentes.
- Diferenca entre campos, propriedades C# e dados serializados pela Unity.
- Vetores, magnitude e normalizacao aplicados a movimento e animacao.

## Observacoes atuais

- `InputSystem_Actions.cs` e gerado automaticamente e nao deve ser editado manualmente.
- `PlayerInputSource` remove os eventos em `OnDisable`; precisamos estudar o que
  acontece ao reativar o objeto.
- As referencias serializadas do `PlayerController` e do `PlayerAnimator` dependem
  de configuracao correta no Inspector.
- `PlayerMovement` normaliza a entrada antes de aplicar velocidade, mantendo a
  mesma velocidade no movimento diagonal.
- Ainda nao existem testes automatizados.

## Atualizacao 2026-06-20

Sistema de interacao e UI foi completamente implementado. Analise do projeto revelou:

- **Pontos fortes**: Arquitetura muito bem organizada, separacao de responsabilidades clara,
  uso de events para desacoplamento, interface IInteraction extensivel.
  
- **Pronto para evoluir**: Sistema de estado (GameplayStateService) ja pode lidar com
  multiplos locks. PopUp e Feedback funcionam. Estrutura suporta novos interativos facilmente.
  
- **Gaps identificados**:
  1. PlayerInteraction nao prioriza alvos multiplos (pega qualquer um do overlap)
  2. Sem visualizacao de raio de deteccao na Scene View
  3. NPCs nao tem estado (ocupado/trancado) e sempre sao interagiveis
  4. Sem feedback visual no NPC selecionado (apenas popup)
  5. Sem testes automatizados

- **Git**: Projeto ainda nao em repositorio formalizado. Pronto para registrar estado
  atual em commits pequenos e significativos.

## Mapa de aprendizado

Conceitos que ja foram bem compreendidos:

- Fluxo do input desde a action ate o `Rigidbody2D`.
- Uso de uma unica instancia compartilhada de `PlayerInputReader`.
- Papel de `Update` e `FixedUpdate`.
- Motivo para manter a ultima direcao de movimento.
- Diferenca entre componentes no objeto raiz e no filho `Avatar`.
- Diferenca entre corpos `Dynamic`, `Kinematic` e `Static`.
- Papel de massa, friccao e damping na simulacao fisica.
- Diferenca entre o Prefab Asset e suas instancias nas cenas.

Conceitos que exigiram mais exploracao e devem reaparecer nos proximos exercicios:

- Ordem entre `OnEnable`, `Awake` e inicializacao feita por outro componente.
- Evitar inscricoes duplicadas em eventos e restaurar conexoes ao reativar objetos.
- Escolher entre `GetComponent`, referencias serializadas e `RequireComponent`.
- Entender que propriedades C# nao sao normalmente serializadas pela Unity.
- Diferenciar vetor original, vetor normalizado e magnitude.
- Traduzir problemas espaciais em operacoes com vetores antes de escolher metodos da Unity.
- Diferenciar soma de vetores, multiplicacao por distancia, `Lerp`, `Distance` e `MoveTowards`.
- Validar comportamento em Play Mode, alem de apenas verificar compilacao.

Abordagem de ensino para matematica:

- Primeiro definir em palavras qual valor queremos descobrir.
- Listar os dados conhecidos e seus tipos ou unidades.
- Desenhar ou montar um exemplo numerico pequeno.
- Escolher a operacao matematica antes de procurar um metodo da Unity.
- Validar visualmente o resultado com gizmos ou linhas de depuracao quando possivel.

## Proximo marco sugerido

**FASE 1: Revisar e consolidar o Sistema de Interacao**

Objetivo: Entender como o sistema atual funciona antes de evoluir.

Desafios pedagogicos:

1. **Challenge 1.1** - Rastreamento visual
   - Questao: Quando o jogador esta perto de um interativo, como ele sabe disso?
   - Explore: PlayerInteraction.Update(), Physics2D.OverlapCircleAll, IInteraction.GetPopUpText()
   - Desafio: Adicionar um Gizmo que desenhe o raio de deteccao em Scene View

2. **Challenge 1.2** - Multiplos alvos
   - Questao: O que acontece se houver 2+ NPCs dentro do raio?
   - Explore: Como a lista eh mantida? Qual eh priorizado?
   - Desafio: Implementar selecao do alvo mais proximo (distance) em vez de qualquer um

3. **Challenge 1.3** - Feedback visual do alvo selecionado
   - Questao: Como o usuario sabe qual alvo esta selecionado?
   - Explore: PopUpManager - como posiciona a popup? Qual offset usar?
   - Desafio: Adicionar um outline/highlight no sprite do NPC selecionado

4. **Challenge 1.4** - Estados do interativo
   - Questao: Um NPC pode estar ocupado? Uma porta pode estar trancada?
   - Explore: Extensao da interface IInteraction com metodos de estado
   - Desafio: Adicionar CanInteract() e desabilitar popup quando false

**FASE 2: Evoluir para feedback visual avancado** (apos concluir Fase 1)
- Transicoes visuais do interativo (hover, ativo, desabilitado)
- Sistema de proximidade com escala/fade da popup
- Queue de interacoes (o que fazer se player interage durante diálogo?)

## Disciplina de commits

- Fazer commits pequenos, completos e executaveis.
- Cada Challenge = 1 commit com mensagem clara
- Formato: `Challenge 1.1: Adicionar gizmo de raio de deteccao`
- Separar mudancas de configuracao, funcionalidades e correcoes quando fizer sentido.
- Antes do commit: conferir `git diff`, testar na Unity e verificar o Console.
- Usar mensagens no imperativo, por exemplo:
  - `docs: registra estado inicial do projeto`
  - `feat(player): adiciona movimento basico`
  - `fix(input): restaura eventos ao reativar jogador`
- Nao misturar arquivos gerados ou mudancas acidentais sem revisar sua necessidade.

## Diario

### 2026-06-10 - Inicio do acompanhamento

- Mapeamos os scripts atuais e o fluxo de movimento do jogador.
- Confirmamos que a Unity esta conectada e sem erros ou avisos no Console.
- Definimos o modelo de colaboracao focado em ensino, revisao e decisoes conjuntas.
- Identificamos que o primeiro passo deve ser consolidar e compreender o estado atual.

### 2026-06-10 - Movimento basico validado

- Revisamos as responsabilidades de source, reader, movement, animator e controller.
- Corrigimos conceitualmente o ciclo de conexao do Input System ao reativar o jogador.
- Normalizamos os vetores usados pelo movimento e pela animacao.
- Definimos quando usar `GetComponent`, referencias serializadas e `RequireComponent`.
- Corrigimos a referencia perdida do `Animator`.
- Definimos a velocidade constante inicial como `3`.
- Validamos o projeto em Play Mode sem erros ou avisos no Console.
- Consideramos concluida a primeira etapa da estrutura de movimento.

### 2026-06-10 - Fundacao fisica validada

- Transformamos o jogador em um prefab reutilizavel usado pela cena.
- Mantivemos a raiz `Player` responsavel pela logica e o filho `Avatar` pelo visual.
- Adicionamos um `CapsuleCollider2D` na regiao dos pes do jogador.
- Validamos o jogador como corpo dinamico com gravidade e rotacao desabilitadas.
- Criamos uma cena laboratorio com paredes e pedras estaticas.
- Validamos uma pedra dinamica empurravel com massa e damping adequados.
- Aplicamos uma superficie sem friccao ao jogador para deslizar pelas paredes.
- Confirmamos manualmente que a pedra desacelera e para.
- Confirmamos manualmente que o jogador nao perde velocidade ao tocar paredes.
- Consideramos concluida a fundacao reutilizavel e fisica do jogador.

### 2026-06-11 - Interacao direcionada validada

- Consolidamos movimento e interacao em um unico Input Actions Asset.
- Representamos o pedido pontual de interacao por um evento no `PlayerInputReader`.
- Criamos o contrato `IInteraction` sem acoplar o jogador a objetos concretos.
- Calculamos a area de deteccao com posicao, direcao normalizada e distancia.
- Filtramos candidatos por uma layer `Interactable`.
- Criamos uma placa simples que implementa o contrato e responde a interacao.
- Adicionamos gizmos para visualizar ponto, alcance e direcao da consulta.
- Definimos uma direcao inicial para permitir interacao antes do primeiro movimento.
- Validamos a inicializacao em Play Mode sem erros ou avisos do jogo.
- Consideramos concluida a primeira etapa do sistema de interacao.

### 2026-06-14 - Feedback visual de interacao validado

- Evoluimos `IInteraction` para expor texto de feedback e ponto visual no mundo.
- Separamos deteccao e execucao no `PlayerInteraction` de apresentacao visual na UI.
- Adicionamos evento de mudanca de interagivel para a UI reagir apenas quando o alvo muda.
- Refinamos a selecao de candidato usando `Collider2D.ClosestPoint`.
- Criamos um manager de UI para posicionar o feedback sobre o ponto do interagivel.
- Adicionamos `CanvasGroup` para controlar visibilidade do feedback.
- Adicionamos pulso visual no objeto raiz do feedback usando `Mathf.Sin` e `Mathf.Lerp`.
- Validamos em Play Mode sem erros ou avisos no Console.
- Aprendizado reforcado: diferenciar posicao, escala, alpha e texto como responsabilidades separadas.

### 2026-06-21 - Refatoracao de UI de dialogo e otimizacao do controlador

- Refatorei `DialoguePanelUI` para suportar opcoes dinamicas via prefab de botao.
  - `ShowOptions(DialogueOption[] options, Action<int> onOptionSelected)` agora instancia N botoes
  - `optionsContainer` e `optionButtonPrefab` foram adicionados para configuracao no Inspector
  - `HideOptions()` remove os filhos antigos para evitar vazamentos de UI

- Otimizei `DialogueController` para usar um `Dictionary<int, DialogueNode>` (`validateId`) como lookup O(1):
  - O dicionario e preenchido em `OnDialogueStarted` a partir de `currentNodes`
  - `FindNodeById` agora usa `TryGetValue` em vez de um loop O(n)
  - Logs de warning adicionados quando `nextId` ou `option.nextId` sao invalidos

- Explicacao e boas praticas sobre `Dictionary` foram discutidas: limpar com `Clear()`, detectar duplicatas,
  e usar `TryGetValue` para consultas seguras. Recomenda-se `validateId.Clear()` em `OnDialogueEnded()`.

- Conceitos importantes, como callbacks, foram documentados no log para servir de referencia futura.

### 2026-06-24 - Dialogo ramificado por estado do mundo validado

- Consolidamos `DialogueSession` como pacote de conversa de um NPC:
  - `firstTimeNodes` representa o caminho padrao/inicial.
  - `worldStateBranches` representa variacoes condicionadas por flags do mundo.

- Criamos `DialogueBranch` para ligar uma condicao de mundo a uma lista de falas:
  - `requiredFlag` define qual flag precisa existir no `WorldStateService`.
  - `nodes` define qual fluxo de dialogo sera usado quando a flag estiver ativa.

- Refatoramos `DialogueController` para escolher os nodes em um metodo dedicado:
  - Comeca sempre com `firstTimeNodes`.
  - Ignora branches incompletas ou invalidas.
  - Usa a primeira branch valida cuja flag exista no `WorldStateService`.
  - Mantem fallback seguro para o dialogo inicial.

- Validamos em Play Mode o primeiro dialogo completo:
  - Ash inicia uma conversa com opcoes.
  - A opcao de aceitar ajuda salva a flag `accepted_quest`.
  - Ao falar novamente com Ash, a fala muda para a branch condicionada por `accepted_quest`.
  - Um segundo NPC (`guard_npc`) foi configurado conceitualmente para reagir a mesma flag:
    antes de falar com Ash, tem uma fala simples; depois de aceitar a ajuda, muda sua fala.

- Aprendizado reforcado:
  - Diferenca entre configuracao (`DialogueSession`), variacao condicional (`DialogueBranch`) e estado em execucao (`currentNodes`/`currentNode`).
  - Uso de fallback para dados serializados incompletos.
  - `continue` significa ignorar a branch atual e testar a proxima; `break` significa encerrar a busca porque uma branch vencedora foi encontrada.
  - Abstracoes devem ser validadas por casos reais antes de ganhar mais complexidade.

- Decisao de arquitetura:
  - O dialogo ramificado minimo esta satisfatorio para a fase atual.
  - Nao vamos adicionar validadores avancados, ScriptableObjects, prioridade complexa ou sistema de persistencia agora.
  - Proximo sistema sugerido: quest simples, conectando dialogo, flags de mundo e conclusao de objetivo.

## Acoes recomendadas/pendentes

- Criar validacao tambem para os `nodes` dentro de `DialogueBranch`.
- Revisar `DialogueOption` para decidir se `flagOptions` deve continuar como enum ou virar string configuravel.
- Corrigir configuracoes pontuais de `startNodeId` quando o id nao corresponder a nenhum node.
- Iniciar um sistema de quest simples, mantendo o mesmo modelo de aprendizado: objetivo pequeno, verificavel e integrado ao estado do mundo.

## Commits recentes

- `docs`: atualizou `PROJECT_LOG.md` com resumo das alteracoes de 2026-06-21
- `docs`: atualizou `PROJECT_LOG.md` com explicacao de callbacks e compromisso de documentar conceitos importantes (2026-06-22)
