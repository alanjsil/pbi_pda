// ============================================================
// PASSO 0 — Parâmetro
// Criar parâmetro pra evitar de tar atualizando local de arquivos
// ============================================================
"C:\Users\alan_\OneDrive\Projetos VS Code\Verena\PDA_Competências.xlsx" meta [IsParameterQuery=true, List={"C:\Users\User\OneDrive\Projetos VS Code\Verena\PDA_Competências.xlsx", "C:\Users\alan_\OneDrive\Projetos VS Code\Verena\PDA_Competências.xlsx"}, DefaultValue=..., Type="Text", IsParameterQueryRequired=true]

// ============================================================
// PASSO 1 — Tabela base (PDA_Base)
// Carregue o CSV, renomeie a query para "PDA_Base"
// Delimitador: ponto e vírgula (;)
// ============================================================
let
    Fonte = Excel.Workbook(File.Contents(Local), null, true),

    Planilha = Table.SelectRows(Fonte, each [Name] = "PDA"),

    Colunas = Table.ColumnNames(Planilha{0}[Data]),

    Expandido = Table.ExpandTableColumn(
        Planilha,
        "Data",
        Colunas,
        Colunas
    ),

    Cabecalhos = Table.PromoteHeaders(Expandido, [PromoteAllScalars=true]),

    Limpeza = Table.RemoveColumns(
        Cabecalhos,
        {"PDA_1", "Sheet", "false", "PDA"}
    ),

  // Tipos — agora com TD_adaptado, IP_adaptado e TF
  Tipos = Table.TransformColumnTypes(Cabecalhos, {
    {"id_colaborador",  Int64.Type},
    {"nome",            type text},
    {"area",            type text},
    {"cargo",           type text},
    {"R_natural",       Int64.Type},
    {"E_natural",       Int64.Type},
    {"P_natural",       Int64.Type},
    {"N_natural",       Int64.Type},
    {"A_natural",       Int64.Type},
    {"IE_R_natural",    Int64.Type},
    {"IE_E_natural",    Int64.Type},
    {"IE_P_natural",    Int64.Type},
    {"IE_N_natural",    Int64.Type},
    {"IE_A_natural",    Int64.Type},
    {"R_adaptado",      Int64.Type},
    {"E_adaptado",      Int64.Type},
    {"P_adaptado",      Int64.Type},
    {"N_adaptado",      Int64.Type},
    {"A_adaptado",      Int64.Type},
    {"IE_R_adaptado",   Int64.Type},
    {"IE_E_adaptado",   Int64.Type},
    {"IE_P_adaptado",   Int64.Type},
    {"IE_N_adaptado",   Int64.Type},
    {"IE_A_adaptado",   Int64.Type},
    {"TD_natural",      Int64.Type},
    {"IP_natural",      Int64.Type},
    {"NE_natural",      Int64.Type},
    {"EE",              Int64.Type},
    {"MP",              Int64.Type},
    {"Consistencia",    type text},
    {"TD_adaptado",     Int64.Type},
    {"IP_adaptado",     Int64.Type},
    {"TF",              Int64.Type}
  }),

  // ------------------------------------------------------------
  // CLASSIFICAÇÕES — Natural
  // ------------------------------------------------------------

  ClassTD_N = Table.AddColumn(Tipos, "Classificacao_TD_Natural", each
    if [TD_natural] > 80      then "Arriscado"
    else if [TD_natural] > 50 then "Tensão (arriscado compensado)"
    else if [TD_natural] = 50 then "Tensão confirmada"
    else if [TD_natural] >= 20 then "Tensão (cauteloso compensado)"
    else "Cauteloso"
  , type text),

  ClassTD_A = Table.AddColumn(ClassTD_N, "Classificacao_TD_Adaptado", each
    if [TD_adaptado] > 80      then "Arriscado"
    else if [TD_adaptado] > 50 then "Tensão (arriscado compensado)"
    else if [TD_adaptado] = 50 then "Tensão confirmada"
    else if [TD_adaptado] >= 20 then "Tensão (cauteloso compensado)"
    else "Cauteloso"
  , type text),

  // Mudança de TD entre Natural e Adaptado
  MudancaTD = Table.AddColumn(ClassTD_A, "Mudanca_TD", each
    if [Classificacao_TD_Natural] = [Classificacao_TD_Adaptado]
    then "Sem mudança"
    else "Mudou estilo de decisão"
  , type text),

  ClassIP_N = Table.AddColumn(MudancaTD, "Classificacao_IP_Natural", each
    if [IP_natural] > 80      then "Muito rígido"
    else if [IP_natural] > 65 then "Perfil muito evidente"
    else if [IP_natural] >= 31 then "Flexibilidade normal"
    else if [IP_natural] >= 20 then "Perfil pouco evidente"
    else "Flexibilidade extrema"
  , type text),

  ClassIP_A = Table.AddColumn(ClassIP_N, "Classificacao_IP_Adaptado", each
    if [IP_adaptado] > 80      then "Muito rígido"
    else if [IP_adaptado] > 65 then "Perfil muito evidente"
    else if [IP_adaptado] >= 31 then "Flexibilidade normal"
    else if [IP_adaptado] >= 20 then "Perfil pouco evidente"
    else "Flexibilidade extrema"
  , type text),

  ClassNE = Table.AddColumn(ClassIP_A, "Classificacao_NE", each
    if [NE_natural] > 80      then "Energia extrema"
    else if [NE_natural] >= 50 then "Alta energia"
    else if [NE_natural] >= 30 then "Energia adequada"
    else if [NE_natural] >= 20 then "Energia baixa"
    else "Muito baixa energia"
  , type text),

  ClassEE = Table.AddColumn(ClassNE, "Classificacao_EE", each
    if [EE] > 80      then "Sobrecarga extrema"
    else if [EE] > 60 then "Possivelmente estressado"
    else if [EE] >= 40 then "Motivado / equilibrado"
    else if [EE] >= 20 then "Possivelmente desmotivado"
    else "Muito abaixo do exigido"
  , type text),

  ClassMP = Table.AddColumn(ClassEE, "Classificacao_MP", each
    if [MP] > 80      then "Não modifica (possível inconsistência)"
    else if [MP] >= 70 then "Modifica pouco"
    else if [MP] >= 20 then "Modifica consideravelmente"
    else "Modifica muito (possível inconsistência)"
  , type text),

  // Classificação do Tempo do Formulário
  ClassTF = Table.AddColumn(ClassMP, "Classificacao_TF", each
    if [TF] <= 3       then "Abaixo do tempo normal (possível inconsistência)"
    else if [TF] <= 4  then "Limite do tempo normal"
    else if [TF] <= 50 then "Dentro do tempo normal"
    else "Excedeu o tempo (possível inconsistência)"
  , type text),

  // ------------------------------------------------------------
  // GAPS Natural x Adaptado
  // ------------------------------------------------------------
  GapR = Table.AddColumn(ClassTF, "Gap_R", each [R_adaptado] - [R_natural], Int64.Type),
  GapE = Table.AddColumn(GapR,    "Gap_E", each [E_adaptado] - [E_natural], Int64.Type),
  GapP = Table.AddColumn(GapE,    "Gap_P", each [P_adaptado] - [P_natural], Int64.Type),
  GapN = Table.AddColumn(GapP,    "Gap_N", each [N_adaptado] - [N_natural], Int64.Type),
  GapA = Table.AddColumn(GapN,    "Gap_A", each [A_adaptado] - [A_natural], Int64.Type),

  GapTotal = Table.AddColumn(GapA, "Gap_Total_Absoluto", each
    Number.Abs([Gap_R]) + Number.Abs([Gap_E]) +
    Number.Abs([Gap_P]) + Number.Abs([Gap_N]) + Number.Abs([Gap_A])
  , Int64.Type),

  // ------------------------------------------------------------
  // FLAGS de eixos extremados — Natural (IE < 20 ou > 80)
  // ------------------------------------------------------------
  FlagR = Table.AddColumn(GapTotal, "Risco_Extremado",
    each [IE_R_natural] > 80 or [IE_R_natural] < 20, type logical),

  FlagE = Table.AddColumn(FlagR, "Extroversao_Extremada",
    each [IE_E_natural] > 80 or [IE_E_natural] < 20, type logical),

  FlagP = Table.AddColumn(FlagE, "Paciencia_Extremada",
    each [IE_P_natural] > 80 or [IE_P_natural] < 20, type logical),

  FlagN = Table.AddColumn(FlagP, "Normas_Extremada",
    each [IE_N_natural] > 80 or [IE_N_natural] < 20, type logical),

  FlagA = Table.AddColumn(FlagN, "Autocontrole_Extremado",
    each [IE_A_natural] > 80 or [IE_A_natural] < 20, type logical),

  QtdExtremados = Table.AddColumn(FlagA, "Qtd_Eixos_Extremados", each
    (if [Risco_Extremado]        then 1 else 0) +
    (if [Extroversao_Extremada]  then 1 else 0) +
    (if [Paciencia_Extremada]    then 1 else 0) +
    (if [Normas_Extremada]       then 1 else 0) +
    (if [Autocontrole_Extremado] then 1 else 0)
  , Int64.Type),

  AlertaCombinado = Table.AddColumn(QtdExtremados, "Alerta_Perfil_Intenso",
    each [Autocontrole_Extremado] = true and [Qtd_Eixos_Extremados] >= 2
  , type logical),

  // Score de risco de inconsistência (0 a 4 — baseado nos gatilhos do manual pág. 51)
  ScoreInconsistencia = Table.AddColumn(AlertaCombinado, "Score_Risco_Inconsistencia", each
    (if [NE_natural] > 80 or [NE_natural] < 20 then 1 else 0) +
    (if [EE] > 80 or [EE] < 20 then 1 else 0) +
    (if [IP_natural] > 80 or [IP_natural] < 20 then 1 else 0) +
    (if [MP] > 80 or [MP] < 20 then 1 else 0) +
    (if [TF] <= 3 or [TF] > 50 then 1 else 0)
  , Int64.Type),

  // Rótulo do score
  RotuloScore = Table.AddColumn(ScoreInconsistencia, "Alerta_Consistencia", each
    if [Score_Risco_Inconsistencia] = 0 then "Sem alertas"
    else if [Score_Risco_Inconsistencia] <= 2 then "Atenção"
    else "Alto risco de inconsistência"
  , type text),

  TendenciaA = Table.AddColumn(RotuloScore, "Tendencia_Autocontrole", each
    if [A_natural] >= 67 then "Alta (racional)"
    else if [A_natural] >= 34 then "Situacional (equilíbrio ideal)"
    else "Baixa (emocional)"
  , type text),

  // Eixo dominante (maior entre R, E, P, N)
  EixoDominante = Table.AddColumn(TendenciaA, "Eixo_Dominante", each
    let
      valores  = {{"Risco",[R_natural]},{"Extroversão",[E_natural]},
        {"Paciência",[P_natural]},{"Normas",[N_natural]}},
      ordenado = List.Sort(valores, (a,b) => Value.Compare(b{1},a{1}))
    in ordenado{0}{0}
  , type text),

  // Quadrante Marston baseado em R e E (para o scatter)
  // Usa 50 como ponto de corte visual conforme discussão
  QuadranteMarston = Table.AddColumn(EixoDominante, "Quadrante_Marston", each
    if [R_natural] >= 50 and [E_natural] >= 50 then "Proativo (R↑ E↑)"
    else if [R_natural] >= 50 and [E_natural] < 50 then "Dominante (R↑ E↓)"
    else if [R_natural] < 50 and [E_natural] >= 50 then "Influente (R↓ E↑)"
    else "Receptivo (R↓ E↓)"
  , type text)

in
  QuadranteMarston


// ============================================================
// PASSO 2 — Tabela longa para gráfico de barras comparativo
// (Natural x Adaptado por eixo)
// Renomeie para "PDA_Eixos_Longo"
// ============================================================
let
  Base = PDA_Base,

  Selecionadas = Table.SelectColumns(Base, {
    "id_colaborador", "nome", "area", "cargo", "Consistencia",
    "Quadrante_Marston", "Eixo_Dominante",
    "R_natural","E_natural","P_natural","N_natural","A_natural",
    "R_adaptado","E_adaptado","P_adaptado","N_adaptado","A_adaptado"
  }),

  Unpivot = Table.UnpivotOtherColumns(Selecionadas,
    {"id_colaborador","nome","area","cargo","Consistencia",
      "Quadrante_Marston","Eixo_Dominante"},
    "Eixo_Perfil", "Valor"
  ),

  // "R_natural" → Eixo="R", Perfil="natural"
  // Problema: "E_natural" vira Eixo="E" e Perfil="natural" — correto
  // Mas "A_natural" vira Eixo="A" e Perfil="natural" — correto também
  SplitEixo = Table.SplitColumn(
    Unpivot, "Eixo_Perfil",
    Splitter.SplitTextByDelimiter("_", QuoteStyle.Csv),
    {"Eixo_Sigla", "Perfil"}
  ),

  NomeEixo = Table.AddColumn(SplitEixo, "Eixo", each
    if [Eixo_Sigla] = "R" then "Risco"
    else if [Eixo_Sigla] = "E" then "Extroversão"
    else if [Eixo_Sigla] = "P" then "Paciência"
    else if [Eixo_Sigla] = "N" then "Normas"
    else if [Eixo_Sigla] = "A" then "Autocontrole"
    else [Eixo_Sigla]
  , type text),

  NomePerfil = Table.AddColumn(NomeEixo, "Perfil_Label", each
    if [Perfil] = "natural" then "Natural"
    else if [Perfil] = "adaptado" then "Adaptado"
    else [Perfil]
  , type text),

  Tendencia = Table.AddColumn(NomePerfil, "Tendencia", each
    if [Valor] <= 33 then "Baixa"
    else if [Valor] <= 66 then "Situacional"
    else "Alta"
  , type text),

  OrdemEixo = Table.AddColumn(Tendencia, "Ordem_Eixo", each
    if [Eixo] = "Risco"          then 1
    else if [Eixo] = "Extroversão"  then 2
    else if [Eixo] = "Paciência"    then 3
    else if [Eixo] = "Normas"       then 4
    else if [Eixo] = "Autocontrole" then 5
    else 99
  , Int64.Type),

  // Remover coluna auxiliar de sigla
  Limpo = Table.RemoveColumns(OrdemEixo, {"Eixo_Sigla", "Perfil"}),

  Tipos = Table.TransformColumnTypes(Limpo, {{"Valor", Int64.Type}})

in
  Tipos


// ============================================================
// PASSO 3 — Tabela auxiliar de faixas (referência visual)
// Renomeie para "PDA_Faixas"
// ============================================================
let
  Tabela = #table(
    type table [
      Faixa       = text,
      Min         = Int64.Type,
      Max         = Int64.Type,
      Cor         = text,
      Ordem       = Int64.Type
    ],
    {
      {"Baixa",       0,  33, "#E74C3C", 1},
      {"Situacional", 34, 66, "#F39C12", 2},
      {"Alta",        67, 100,"#27AE60", 3}
    }
  )
in
  Tabela


// ============================================================
// PASSO 4 — Tabela auxiliar de referencias de perfil
// Renomeie para "PDA_Perfis_Referencia"
// ============================================================
let
  Tabela = #table(
    {"Perfil","R","E","P","N"},
    {
      {"Proativo",100,100,0,0}, {"Decidido",100,50,50,0}, {"Dinâmico",100,50,0,50},
      {"Dominante",100,40,20,40}, {"Investigador",100,0,100,0}, {"Determinado",100,0,50,50},
      {"Resolutivo",100,0,0,100}, {"Intuitivo",60,80,60,0}, {"Inquieto",60,80,0,60},
      {"Analítico",60,0,80,60}, {"Convincente",50,100,50,0}, {"Influente",50,100,0,50},
      {"Diplomático",50,50,100,0}, {"Exigente",50,50,0,100}, {"Lógico",50,0,100,50},
      {"Cético",50,0,50,100}, {"Promotor",40,100,40,20}, {"Calmo",40,20,100,40},
      {"Normativo",40,20,40,100}, {"Orientado para Pessoas",0,100,100,0},
      {"Amável",0,100,50,50}, {"Cativante",0,100,0,100}, {"Colaborador",0,60,80,60},
      {"Amigável",0,50,100,50}, {"Preciso",0,50,50,100}, {"Receptivo",0,0,100,100}
    }
  )
in
  Tabela


// ============================================================
// PASSO 5 — Tabela auxiliar de competencias
// Renomeie para "PDA_Competencias_Pesos"
// ============================================================
let
  Tabela = #table(
    {"Competencia", "Peso_R", "Peso_E", "Peso_P", "Peso_N", "Peso_A"},
    {
      {"Comunicação",                0.2,  1.0,  0.0, -0.3,  0.0},
      {"Desenvolvimento de Pessoas", -0.3,  0.6,  0.8,  0.0,  0.0},
      {"Orientação para Resultados",  0.9,  0.2, -0.3,  0.0,  0.2},
      {"Pensamento Estratégico",      0.3,  0.0,  0.0,  0.4,  0.8}
    }
  )
in
  Tabela


// ============================================================
// PASSO 6 — Tabela auxiliar de competencias
// Isto é uma tabela calculada
// ============================================================
PDA_Competencias_Scores =
ADDCOLUMNS(
  ADDCOLUMNS(
    CROSSJOIN(
      SELECTCOLUMNS(
        'PDA_Base',
        "id_colaborador", 'PDA_Base'[id_colaborador],
        "nome",           'PDA_Base'[nome],
        "area",           'PDA_Base'[area],
        "cargo",          'PDA_Base'[cargo],
        "R",              'PDA_Base'[R_natural],
        "E",              'PDA_Base'[E_natural],
        "P",              'PDA_Base'[P_natural],
        "N",              'PDA_Base'[N_natural],
        "A",              'PDA_Base'[A_natural]
      ),
      'PDA_Competencias_Pesos'
    ),
    "Score",
    VAR SomaAbsPesos = ABS([Peso_R]) + ABS([Peso_E]) + ABS([Peso_P]) + ABS([Peso_N]) + ABS([Peso_A])
    VAR ScoreBruto =
      50 + DIVIDE(
        [Peso_R]*([R]-50) + [Peso_E]*([E]-50) + [Peso_P]*([P]-50) + [Peso_N]*([N]-50) + [Peso_A]*([A]-50),
        SomaAbsPesos, 0
      )
    RETURN MAX(0, MIN(100, ScoreBruto))
  ),
  "Classificacao",
  SWITCH(
    TRUE(),
    [Score] >= 80, "Excelente",
    [Score] >= 60, "Muito Boa",
    [Score] >= 40, "Aceitável",
    "Baixa"
  ),
  "Estrelas", ROUND([Score] / 20, 0)
)
