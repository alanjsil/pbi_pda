// ============================================================
// PASSO 1 — Tabela base (PDA_Base)
// Carregue o CSV e renomeie a query para "PDA_Base"
// Esta tabela mantém UMA LINHA POR COLABORADOR
// com todos os indicadores derivados e classificações
// ============================================================

let
    Fonte = Csv.Document(
        File.Contents("C:\PDA\pda_exemplo.csv"),
        [Delimiter=",", Encoding=65001, QuoteStyle=QuoteStyle.None]
    ),

    // Promover cabeçalhos
    Cabecalhos = Table.PromoteHeaders(Fonte, [PromoteAllScalars=true]),

    // Tipos corretos por coluna
    Tipos = Table.TransformColumnTypes(Cabecalhos, {
        {"id_colaborador",         Int64.Type},
        {"nome",                   type text},
        {"area",                   type text},
        {"cargo",                  type text},
        {"R_natural",              Int64.Type},
        {"E_natural",              Int64.Type},
        {"P_natural",              Int64.Type},
        {"N_natural",              Int64.Type},
        {"A_natural",              Int64.Type},
        {"IE_R_natural",           Int64.Type},
        {"IE_E_natural",           Int64.Type},
        {"IE_P_natural",           Int64.Type},
        {"IE_N_natural",           Int64.Type},
        {"IE_A_natural",           Int64.Type},
        {"R_adaptado",             Int64.Type},
        {"E_adaptado",             Int64.Type},
        {"P_adaptado",             Int64.Type},
        {"N_adaptado",             Int64.Type},
        {"A_adaptado",             Int64.Type},
        {"IE_R_adaptado",          Int64.Type},
        {"IE_E_adaptado",          Int64.Type},
        {"IE_P_adaptado",          Int64.Type},
        {"IE_N_adaptado",          Int64.Type},
        {"IE_A_adaptado",          Int64.Type},
        {"TD_natural",             Int64.Type},
        {"IP_natural",             Int64.Type},
        {"NE_natural",             Int64.Type},
        {"EE",                     Int64.Type},
        {"MP",                     Int64.Type},
        {"TD_adaptado",            Int64.Type},
        {"IP_adaptado",            Int64.Type},
        {"TF",                     Int64.Type},
        {"Consistencia",           type text}
    }),

    // ------------------------------------------------------------
    // COLUNAS CALCULADAS — Classificações (equivalentes ao DAX,
    // feitas aqui no PQ para reduzir carga no modelo)
    // ------------------------------------------------------------

    // Classificação de Tomada de Decisão — manual pág. 38
    ClassTD = Table.AddColumn(Tipos, "Classificacao_TD", each
        if [TD_natural] > 80 then "Arriscado"
        else if [TD_natural] > 50 then "Tensão (arriscado compensado)"
        else if [TD_natural] = 50 then "Tensão confirmada"
        else if [TD_natural] >= 20 then "Tensão (cauteloso compensado)"
        else "Cauteloso"
    , type text),

    // Classificação de Intensidade do Perfil
    ClassIP = Table.AddColumn(ClassTD, "Classificacao_IP", each
        if [IP_natural] > 80 then "Muito rígido"
        else if [IP_natural] > 65 then "Perfil muito evidente"
        else if [IP_natural] >= 31 then "Flexibilidade normal"
        else if [IP_natural] >= 20 then "Perfil pouco evidente"
        else "Flexibilidade extrema"
    , type text),

    // Classificação de Nível de Energia
    ClassNE = Table.AddColumn(ClassIP, "Classificacao_NE", each
        if [NE_natural] > 80 then "Energia extrema"
        else if [NE_natural] >= 50 then "Alta energia"
        else if [NE_natural] >= 30 then "Energia adequada"
        else if [NE_natural] >= 20 then "Energia baixa"
        else "Muito baixa energia"
    , type text),

    // Classificação de Equilíbrio de Energia
    ClassEE = Table.AddColumn(ClassNE, "Classificacao_EE", each
        if [EE] > 80 then "Sobrecarga extrema"
        else if [EE] > 60 then "Possivelmente estressado"
        else if [EE] >= 40 then "Motivado / equilibrado"
        else if [EE] >= 20 then "Possivelmente desmotivado"
        else "Muito abaixo do exigido"
    , type text),

    // Classificação de Modificação do Perfil
    ClassMP = Table.AddColumn(ClassEE, "Classificacao_MP", each
        if [MP] > 80 then "Não modifica (possível inconsistência)"
        else if [MP] >= 70 then "Modifica pouco"
        else if [MP] >= 20 then "Modifica consideravelmente"
        else "Modifica muito (possível inconsistência)"
    , type text),

    // ------------------------------------------------------------
    // GAPS entre Natural e Adaptado (valor e intensidade)
    // ------------------------------------------------------------
    GapR  = Table.AddColumn(ClassMP,  "Gap_R", each [R_adaptado]  - [R_natural],  Int64.Type),
    GapE  = Table.AddColumn(GapR,     "Gap_E", each [E_adaptado]  - [E_natural],  Int64.Type),
    GapP  = Table.AddColumn(GapE,     "Gap_P", each [P_adaptado]  - [P_natural],  Int64.Type),
    GapN  = Table.AddColumn(GapP,     "Gap_N", each [N_adaptado]  - [N_natural],  Int64.Type),
    GapA  = Table.AddColumn(GapN,     "Gap_A", each [A_adaptado]  - [A_natural],  Int64.Type),

    GapTotal = Table.AddColumn(GapA, "Gap_Total_Absoluto", each
        Number.Abs([Gap_R]) + Number.Abs([Gap_E]) +
        Number.Abs([Gap_P]) + Number.Abs([Gap_N]) + Number.Abs([Gap_A])
    , Int64.Type),

    // ------------------------------------------------------------
    // FLAGS de eixos extremados (IE < 20 ou IE > 80)
    // ------------------------------------------------------------
    FlagR = Table.AddColumn(GapTotal, "Risco_Extremado", each
        [IE_R_natural] > 80 or [IE_R_natural] < 20
    , type logical),

    FlagE = Table.AddColumn(FlagR, "Extroversao_Extremada", each
        [IE_E_natural] > 80 or [IE_E_natural] < 20
    , type logical),

    FlagP = Table.AddColumn(FlagE, "Paciencia_Extremada", each
        [IE_P_natural] > 80 or [IE_P_natural] < 20
    , type logical),

    FlagN = Table.AddColumn(FlagP, "Normas_Extremada", each
        [IE_N_natural] > 80 or [IE_N_natural] < 20
    , type logical),

    FlagA = Table.AddColumn(FlagN, "Autocontrole_Extremado", each
        [IE_A_natural] > 80 or [IE_A_natural] < 20
    , type logical),

    // Contagem de eixos extremados
    QtdExtremados = Table.AddColumn(FlagA, "Qtd_Eixos_Extremados", each
        (if [Risco_Extremado]         then 1 else 0) +
        (if [Extroversao_Extremada]   then 1 else 0) +
        (if [Paciencia_Extremada]     then 1 else 0) +
        (if [Normas_Extremada]        then 1 else 0) +
        (if [Autocontrole_Extremado]  then 1 else 0)
    , Int64.Type),

    // Flag de alerta combinado: Autocontrole extremado + outro eixo extremado
    AlertaCombinado = Table.AddColumn(QtdExtremados, "Alerta_Perfil_Intenso", each
        [Autocontrole_Extremado] = true and [Qtd_Eixos_Extremados] >= 2
    , type logical),

    // Tendência do eixo de Autocontrole (pág. 25 — único onde Situacional é ideal)
    TendenciaA = Table.AddColumn(AlertaCombinado, "Tendencia_Autocontrole", each
        if [A_natural] >= 67 then "Alta (racional)"
        else if [A_natural] >= 34 then "Situacional (equilíbrio ideal)"
        else "Baixa (emocional)"
    , type text),

    // Perfil dominante (eixo mais alto entre R, E, P, N — exclui Autocontrole)
    PerfilDominante = Table.AddColumn(TendenciaA, "Eixo_Dominante", each
        let
            valores = {
                {"R", [R_natural]},
                {"E", [E_natural]},
                {"P", [P_natural]},
                {"N", [N_natural]}
            },
            ordenado = List.Sort(valores, (a, b) => Value.Compare(b{1}, a{1}))
        in
            ordenado{0}{0}
    , type text)

in
    PerfilDominante


// ============================================================
// PASSO 2 — Tabela longa para Radar / Comparativo Natural x Adaptado
// Renomeie esta query para "PDA_Eixos_Longo"
// UMA LINHA POR EIXO POR COLABORADOR (10 pessoas x 5 eixos = 50 linhas natural + 50 adaptado)
// ============================================================

/*
  Crie uma nova query em branco e cole o código abaixo.
  Ela referencia PDA_Base — não carrega o CSV de novo.
*/

let
    Base = PDA_Base,

    // Selecionar apenas id + nome + area + cargo + os 10 eixos (5 natural, 5 adaptado)
    Selecionadas = Table.SelectColumns(Base, {
        "id_colaborador", "nome", "area", "cargo",
        "R_natural","E_natural","P_natural","N_natural","A_natural",
        "R_adaptado","E_adaptado","P_adaptado","N_adaptado","A_adaptado"
    }),

    // Unpivot de todas as colunas de eixo
    Unpivot = Table.UnpivotOtherColumns(Selecionadas,
        {"id_colaborador","nome","area","cargo"},
        "Eixo_Perfil", "Valor"
    ),

    // Separar "R_natural" em duas colunas: Eixo = "R", Perfil = "natural"
    SplitEixo = Table.SplitColumn(
        Unpivot, "Eixo_Perfil",
        Splitter.SplitTextByDelimiter("_", QuoteStyle.Csv),
        {"Eixo", "Perfil"}
    ),

    // Substituir sigla pela descrição completa do eixo
    NomeEixo = Table.ReplaceValue(
        SplitEixo, each [Eixo], each
            if [Eixo] = "R" then "Risco"
            else if [Eixo] = "E" then "Extroversão"
            else if [Eixo] = "P" then "Paciência"
            else if [Eixo] = "N" then "Normas"
            else if [Eixo] = "A" then "Autocontrole"
            else [Eixo],
        Replacer.ReplaceValue, {"Eixo"}
    ),

    // Capitalizar Perfil para exibição
    NomePerfil = Table.ReplaceValue(
        NomeEixo, each [Perfil], each
            if [Perfil] = "natural"  then "Natural"
            else if [Perfil] = "adaptado" then "Adaptado"
            else [Perfil],
        Replacer.ReplaceValue, {"Perfil"}
    ),

    // Adicionar coluna de tendência do eixo (baixo / situacional / alto)
    Tendencia = Table.AddColumn(NomePerfil, "Tendencia", each
        if [Valor] <= 33 then "Baixa"
        else if [Valor] <= 66 then "Situacional"
        else "Alta"
    , type text),

    // Ordenação de exibição para o radar (R → E → P → N → A)
    OrdemEixo = Table.AddColumn(Tendencia, "Ordem_Eixo", each
        if [Eixo] = "Risco"         then 1
        else if [Eixo] = "Extroversão"  then 2
        else if [Eixo] = "Paciência"    then 3
        else if [Eixo] = "Normas"       then 4
        else if [Eixo] = "Autocontrole" then 5
        else 99
    , Int64.Type),

    Tipos = Table.TransformColumnTypes(OrdemEixo, {
        {"Valor", Int64.Type}
    })

in
    Tipos


// ============================================================
// PASSO 3 — Tabela auxiliar de referência: Faixas de Tendência
// Renomeie para "PDA_Faixas"
// Usada em visuais de referência (linhas horizontais, tooltips)
// ============================================================

let
    Tabela = #table(
        type table [
            Faixa = text,
            Min   = Int64.Type,
            Max   = Int64.Type,
            Cor   = text
        ],
        {
            {"Baixa",       0,  33, "#E74C3C"},
            {"Situacional", 34, 66, "#F39C12"},
            {"Alta",        67, 100,"#27AE60"}
        }
    )
in
    Tabela
