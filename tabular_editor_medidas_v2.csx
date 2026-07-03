// Tabular Editor C# Script
// Conecte ao Power BI Desktop com o arquivo .pbix aberto
// Execute APÓS o script de colunas calculadas
// Todas as médias analíticas sem filtro fixo de Consistencia
// Controle feito via segmentação na página do relatório

var t = Model.Tables["_Medidas PDA"];

// ============================================================
// 01 — Contagens e qualidade de dados
// Mantém filtro interno — é o objetivo da medida
// ============================================================

t.AddMeasure("Total Avaliações",
    "COUNTROWS('PDA_Base')",
    displayFolder: "01 Contagens");

t.AddMeasure("Qtd Consistentes",
    "CALCULATE(COUNTROWS('PDA_Base'), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "01 Contagens");

t.AddMeasure("Qtd Pouco Consistentes",
    "CALCULATE(COUNTROWS('PDA_Base'), 'PDA_Base'[Consistencia] = \"Pouco_consistente\")",
    displayFolder: "01 Contagens");

t.AddMeasure("Qtd Inválidos",
    "CALCULATE(COUNTROWS('PDA_Base'), 'PDA_Base'[Consistencia] = \"Inválido\")",
    displayFolder: "01 Contagens");

t.AddMeasure("% Consistentes",
    "DIVIDE([Qtd Consistentes], [Total Avaliações], 0)",
    displayFolder: "01 Contagens");

t.AddMeasure("% Pouco Consistentes",
    "DIVIDE([Qtd Pouco Consistentes], [Total Avaliações], 0)",
    displayFolder: "01 Contagens");

t.AddMeasure("% Inválidos",
    "DIVIDE([Qtd Inválidos], [Total Avaliações], 0)",
    displayFolder: "01 Contagens");

t.AddMeasure("Status Qualidade Dados",
    "IF([% Consistentes] >= 0.9, \"✅ Dentro da meta\", \"⚠️ Abaixo da meta\")",
    displayFolder: "01 Contagens");

// ============================================================
// 02 — Médias dos Eixos (Perfil Natural)
// Sem filtro fixo — respeita segmentação da página
// ============================================================

t.AddMeasure("Risco Médio Natural",
    "AVERAGE('PDA_Base'[R_natural])",
    displayFolder: "02 Médias Natural");

t.AddMeasure("Extroversão Média Natural",
    "AVERAGE('PDA_Base'[E_natural])",
    displayFolder: "02 Médias Natural");

t.AddMeasure("Paciência Média Natural",
    "AVERAGE('PDA_Base'[P_natural])",
    displayFolder: "02 Médias Natural");

t.AddMeasure("Normas Média Natural",
    "AVERAGE('PDA_Base'[N_natural])",
    displayFolder: "02 Médias Natural");

t.AddMeasure("Autocontrole Médio Natural",
    "AVERAGE('PDA_Base'[A_natural])",
    displayFolder: "02 Médias Natural");

t.AddMeasure("NE Médio Natural",
    "AVERAGE('PDA_Base'[NE_natural])",
    displayFolder: "02 Médias Natural");

t.AddMeasure("IP Médio Natural",
    "AVERAGE('PDA_Base'[IP_natural])",
    displayFolder: "02 Médias Natural");

t.AddMeasure("TD Médio Natural",
    "AVERAGE('PDA_Base'[TD_natural])",
    displayFolder: "02 Médias Natural");

// ============================================================
// 03 — Médias dos Eixos (Perfil Adaptado)
// Sem filtro fixo — respeita segmentação da página
// ============================================================

t.AddMeasure("Risco Médio Adaptado",
    "AVERAGE('PDA_Base'[R_adaptado])",
    displayFolder: "03 Médias Adaptado");

t.AddMeasure("Extroversão Média Adaptado",
    "AVERAGE('PDA_Base'[E_adaptado])",
    displayFolder: "03 Médias Adaptado");

t.AddMeasure("Paciência Média Adaptado",
    "AVERAGE('PDA_Base'[P_adaptado])",
    displayFolder: "03 Médias Adaptado");

t.AddMeasure("Normas Média Adaptado",
    "AVERAGE('PDA_Base'[N_adaptado])",
    displayFolder: "03 Médias Adaptado");

t.AddMeasure("Autocontrole Médio Adaptado",
    "AVERAGE('PDA_Base'[A_adaptado])",
    displayFolder: "03 Médias Adaptado");

t.AddMeasure("IP Médio Adaptado",
    "AVERAGE('PDA_Base'[IP_adaptado])",
    displayFolder: "03 Médias Adaptado");

t.AddMeasure("TD Médio Adaptado",
    "AVERAGE('PDA_Base'[TD_adaptado])",
    displayFolder: "03 Médias Adaptado");

// ============================================================
// 04 — Radar (usa PDA_Eixos_Longo)
// Sem filtro fixo — segmentação controla
// ============================================================

t.AddMeasure("Valor Médio Eixo",
    "AVERAGE('PDA_Eixos_Longo'[Valor])",
    displayFolder: "04 Radar");

// ============================================================
// 05 — Análise de Gap Natural x Adaptado
// Sem filtro fixo
// ============================================================

t.AddMeasure("Gap Médio Risco",
    "AVERAGE('PDA_Base'[Gap_R])",
    displayFolder: "05 Gap");

t.AddMeasure("Gap Médio Extroversão",
    "AVERAGE('PDA_Base'[Gap_E])",
    displayFolder: "05 Gap");

t.AddMeasure("Gap Médio Paciência",
    "AVERAGE('PDA_Base'[Gap_P])",
    displayFolder: "05 Gap");

t.AddMeasure("Gap Médio Normas",
    "AVERAGE('PDA_Base'[Gap_N])",
    displayFolder: "05 Gap");

t.AddMeasure("Gap Médio Autocontrole",
    "AVERAGE('PDA_Base'[Gap_A])",
    displayFolder: "05 Gap");

t.AddMeasure("Gap Total Médio",
    "AVERAGE('PDA_Base'[Gap_Total_Absoluto])",
    displayFolder: "05 Gap");

t.AddMeasure("Qtd Alto Gap",
    "CALCULATE(COUNTROWS('PDA_Base'), 'PDA_Base'[Gap_Total_Absoluto] > 60)",
    displayFolder: "05 Gap");

t.AddMeasure("% Alto Gap",
    "DIVIDE([Qtd Alto Gap], [Total Avaliações], 0)",
    displayFolder: "05 Gap");

// ============================================================
// 06 — Equilíbrio de Energia
// Sem filtro fixo
// ============================================================

t.AddMeasure("EE Médio",
    "AVERAGE('PDA_Base'[EE])",
    displayFolder: "06 EE");

t.AddMeasure("Qtd Possivelmente Estressados",
    "CALCULATE(COUNTROWS('PDA_Base'), 'PDA_Base'[EE] > 60)",
    displayFolder: "06 EE");

t.AddMeasure("% Estressados",
    "DIVIDE([Qtd Possivelmente Estressados], [Total Avaliações], 0)",
    displayFolder: "06 EE");

t.AddMeasure("Qtd Possivelmente Desmotivados",
    "CALCULATE(COUNTROWS('PDA_Base'), 'PDA_Base'[EE] < 40)",
    displayFolder: "06 EE");

t.AddMeasure("% Desmotivados",
    "DIVIDE([Qtd Possivelmente Desmotivados], [Total Avaliações], 0)",
    displayFolder: "06 EE");

// ============================================================
// 07 — Perfis com Eixos Extremados
// Sem filtro fixo
// ============================================================

t.AddMeasure("Qtd com Autocontrole Extremado",
    "CALCULATE(COUNTROWS('PDA_Base'), 'PDA_Base'[Autocontrole_Extremado] = TRUE())",
    displayFolder: "07 Eixos Extremados");

t.AddMeasure("Qtd com Alerta Perfil Intenso",
    "CALCULATE(COUNTROWS('PDA_Base'), 'PDA_Base'[Alerta_Perfil_Intenso] = TRUE())",
    displayFolder: "07 Eixos Extremados");

t.AddMeasure("% Alerta Perfil Intenso",
    "DIVIDE([Qtd com Alerta Perfil Intenso], [Total Avaliações], 0)",
    displayFolder: "07 Eixos Extremados");

t.AddMeasure("Qtd Eixos Extremados Médio",
    "AVERAGE('PDA_Base'[Qtd_Eixos_Extremados])",
    displayFolder: "07 Eixos Extremados");

// ============================================================
// 08 — Tomada de Decisão
// Sem filtro fixo — usa Classificacao_TD_Natural do PQ
// ============================================================

t.AddMeasure("Qtd por Estilo TD",
    "COUNTROWS('PDA_Base')",
    displayFolder: "08 TD");

t.AddMeasure("% em Tensão TD Natural", @"
DIVIDE(
    CALCULATE(
        COUNTROWS('PDA_Base'),
        'PDA_Base'[Classificacao_TD_Natural] IN {
            ""Tensão confirmada"",
            ""Tensão (arriscado compensado)"",
            ""Tensão (cauteloso compensado)""
        }
    ),
    [Total Avaliações],
    0
)",
    displayFolder: "08 TD");

t.AddMeasure("% em Tensão TD Adaptado", @"
DIVIDE(
    CALCULATE(
        COUNTROWS('PDA_Base'),
        'PDA_Base'[Classificacao_TD_Adaptado] IN {
            ""Tensão confirmada"",
            ""Tensão (arriscado compensado)"",
            ""Tensão (cauteloso compensado)""
        }
    ),
    [Total Avaliações],
    0
)",
    displayFolder: "08 TD");

t.AddMeasure("Qtd Mudou Estilo TD",
    "CALCULATE(COUNTROWS('PDA_Base'), 'PDA_Base'[Mudanca_TD] = \"Mudou estilo de decisão\")",
    displayFolder: "08 TD");

t.AddMeasure("% Mudou Estilo TD",
    "DIVIDE([Qtd Mudou Estilo TD], [Total Avaliações], 0)",
    displayFolder: "08 TD");

// ============================================================
// 09 — Distância ao Perfil Médio do Cargo
// KEEPFILTERS para não comparar Consistentes com Inválidos
// mas ainda respeita filtros externos de area/cargo
// ============================================================

t.AddMeasure("Distância ao Perfil Médio Cargo", @"
VAR MediaR = CALCULATE(
    AVERAGE('PDA_Base'[R_adaptado]),
    KEEPFILTERS('PDA_Base'[Consistencia] = ""Consistente""),
    ALLEXCEPT('PDA_Base', 'PDA_Base'[cargo])
)
VAR MediaE = CALCULATE(
    AVERAGE('PDA_Base'[E_adaptado]),
    KEEPFILTERS('PDA_Base'[Consistencia] = ""Consistente""),
    ALLEXCEPT('PDA_Base', 'PDA_Base'[cargo])
)
VAR MediaP = CALCULATE(
    AVERAGE('PDA_Base'[P_adaptado]),
    KEEPFILTERS('PDA_Base'[Consistencia] = ""Consistente""),
    ALLEXCEPT('PDA_Base', 'PDA_Base'[cargo])
)
VAR MediaN = CALCULATE(
    AVERAGE('PDA_Base'[N_adaptado]),
    KEEPFILTERS('PDA_Base'[Consistencia] = ""Consistente""),
    ALLEXCEPT('PDA_Base', 'PDA_Base'[cargo])
)
RETURN
    SQRT(
        ('PDA_Base'[R_adaptado] - MediaR)^2 +
        ('PDA_Base'[E_adaptado] - MediaE)^2 +
        ('PDA_Base'[P_adaptado] - MediaP)^2 +
        ('PDA_Base'[N_adaptado] - MediaN)^2
    )",
    displayFolder: "09 Cargo");

// ============================================================
// 10 — Score de Risco de Inconsistência
// Sem filtro — faz sentido analisar todos os perfis
// ============================================================

t.AddMeasure("Score Médio Risco Inconsistência",
    "AVERAGE('PDA_Base'[Score_Risco_Inconsistencia])",
    displayFolder: "10 Consistência");

t.AddMeasure("Qtd Score Alto (>=3)",
    "CALCULATE(COUNTROWS('PDA_Base'), 'PDA_Base'[Score_Risco_Inconsistencia] >= 3)",
    displayFolder: "10 Consistência");

t.AddMeasure("% Score Alto",
    "DIVIDE([Qtd Score Alto (>=3)], [Total Avaliações], 0)",
    displayFolder: "10 Consistência");

t.AddMeasure("TF Médio",
    "AVERAGE('PDA_Base'[TF])",
    displayFolder: "10 Consistência");

t.AddMeasure("Qtd Fora do Tempo Normal",
    "CALCULATE(COUNTROWS('PDA_Base'), 'PDA_Base'[TF] < 3 || 'PDA_Base'[TF] > 50)",
    displayFolder: "10 Consistência");

// ============================================================
// 11 — Resumo do Perfil (texto descritivo)
// Medidas textuais para cards de resumo individual
// ============================================================

t.AddMeasure("Resumo Executivo PDA", @"
VAR R = SELECTEDVALUE(PDA_Base[R_natural])
VAR E = SELECTEDVALUE(PDA_Base[E_natural])
VAR P = SELECTEDVALUE(PDA_Base[P_natural])
VAR N = SELECTEDVALUE(PDA_Base[N_natural])
VAR A = SELECTEDVALUE(PDA_Base[A_natural])

VAR TxtR =
    IF(R >= 67,
        ""direta e competitiva"",
        IF(R <= 33,
            ""cautelosa e diplomática"",
            ""equilibrada quanto à tomada de riscos""
        )
    )

VAR TxtE =
    IF(E >= 67,
        ""com forte orientação para pessoas"",
        IF(E <= 33,
            ""mais reservada e analítica"",
            ""com equilíbrio entre interação e análise""
        )
    )

VAR TxtP =
    IF(P >= 67,
        ""valorizando estabilidade e previsibilidade"",
        IF(P <= 33,
            ""buscando dinamismo e mudanças constantes"",
            ""adaptando seu ritmo conforme a situação""
        )
    )

RETURN

""É uma pessoa "" &
TxtR &
"", "" &
TxtE &
"", "" &
TxtP &
"". Seu comportamento demonstra características compatíveis com seu perfil natural PDA."",
    displayFolder: ""11 Resumo"");

t.AddMeasure("Palavras Descritivas", @"
VAR Lista =
{
    IF(MAX(PDA_Base[R_natural]) >= 67, ""Competitivo""),
    IF(MAX(PDA_Base[R_natural]) >= 67, ""Determinado""),
    IF(MAX(PDA_Base[E_natural]) >= 67, ""Comunicativo""),
    IF(MAX(PDA_Base[E_natural]) >= 67, ""Persuasivo""),
    IF(MAX(PDA_Base[P_natural]) <= 33, ""Dinâmico""),
    IF(MAX(PDA_Base[P_natural]) <= 33, ""Ágil""),
    IF(MAX(PDA_Base[N_natural]) >= 67, ""Preciso""),
    IF(MAX(PDA_Base[N_natural]) >= 67, ""Detalhista""),
    IF(MAX(PDA_Base[A_natural]) >= 67, ""Lógico""),
    IF(MAX(PDA_Base[A_natural]) >= 67, ""Racional"")
}

RETURN
CONCATENATEX(
    FILTER(Lista, NOT ISBLANK([Value])),
    [Value],
    "" • ""
)",
    displayFolder: "11 Resumo");

// ============================================================
// 12 — Competência
// Medidas agregadas de scores e classificações por competência
// ============================================================

t.AddMeasure("Total Pares Pessoa-Competência",
    "COUNTROWS('PDA_Competencias_Scores')",
    displayFolder: "12 Competência");

t.AddMeasure("Qtd Excelente",
    "CALCULATE(COUNTROWS('PDA_Competencias_Scores'), 'PDA_Competencias_Scores'[Classificacao] = \"Excelente\")",
    displayFolder: "12 Competência");

t.AddMeasure("Qtd Muito Boa",
    "CALCULATE(COUNTROWS('PDA_Competencias_Scores'), 'PDA_Competencias_Scores'[Classificacao] = \"Muito Boa\")",
    displayFolder: "12 Competência");

t.AddMeasure("Qtd Aceitável",
    "CALCULATE(COUNTROWS('PDA_Competencias_Scores'), 'PDA_Competencias_Scores'[Classificacao] = \"Aceitável\")",
    displayFolder: "12 Competência");

t.AddMeasure("Qtd Baixa",
    "CALCULATE(COUNTROWS('PDA_Competencias_Scores'), 'PDA_Competencias_Scores'[Classificacao] = \"Baixa\")",
    displayFolder: "12 Competência");

t.AddMeasure("% Excelente",
    "DIVIDE([Qtd Excelente], [Total Pares Pessoa-Competência], 0)",
    displayFolder: "12 Competência");

t.AddMeasure("% Muito Boa",
    "DIVIDE([Qtd Muito Boa], [Total Pares Pessoa-Competência], 0)",
    displayFolder: "12 Competência");

t.AddMeasure("% Aceitável",
    "DIVIDE([Qtd Aceitável], [Total Pares Pessoa-Competência], 0)",
    displayFolder: "12 Competência");

t.AddMeasure("% Baixa",
    "DIVIDE([Qtd Baixa], [Total Pares Pessoa-Competência], 0)",
    displayFolder: "12 Competência");

t.AddMeasure("Score Competência", @"
VAR R = SELECTEDVALUE('PDA_Base'[R_natural])
VAR E = SELECTEDVALUE('PDA_Base'[E_natural])
VAR P = SELECTEDVALUE('PDA_Base'[P_natural])
VAR N = SELECTEDVALUE('PDA_Base'[N_natural])
VAR A = SELECTEDVALUE('PDA_Base'[A_natural])
VAR wR = SELECTEDVALUE('PDA_Competencias_Pesos'[Peso_R])
VAR wE = SELECTEDVALUE('PDA_Competencias_Pesos'[Peso_E])
VAR wP = SELECTEDVALUE('PDA_Competencias_Pesos'[Peso_P])
VAR wN = SELECTEDVALUE('PDA_Competencias_Pesos'[Peso_N])
VAR wA = SELECTEDVALUE('PDA_Competencias_Pesos'[Peso_A])
VAR SomaAbsPesos = ABS(wR)+ABS(wE)+ABS(wP)+ABS(wN)+ABS(wA)
VAR Score = 50 + DIVIDE(
    wR*(R-50) + wE*(E-50) + wP*(P-50) + wN*(N-50) + wA*(A-50),
    SomaAbsPesos, 0
)
RETURN
    MAX(0, MIN(100, Score))
",
    displayFolder: "12 Competência");
