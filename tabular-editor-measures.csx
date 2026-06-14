// Tabular Editor C# Script
// Conecte ao Power BI Desktop com o Start.pbix aberto, depois execute este script
// (CTRL+ENTER ou no menu: Scripts > Execute)
//
// SOMENTE MEDIDAS — conforme medidas_dax.txt
// Execute este script APÓS o tabular-editor-script.csx (que cria as colunas calculadas)
// para adicionar todas as medidas DAX organizadas em pastas de exibição.

var t = Model.Tables["_Medidas PDA"];

// ============================================================
// 01 — Contagens e qualidade de dados
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

t.AddMeasure("% Inválidos",
    "DIVIDE([Qtd Inválidos], [Total Avaliações], 0)",
    displayFolder: "01 Contagens");

t.AddMeasure("Status Qualidade Dados",
    "IF([% Consistentes] >= 0.9, \"✅ Dentro da meta\", \"⚠️ Abaixo da meta\")",
    displayFolder: "01 Contagens");

// ============================================================
// 02 — Médias dos Eixos (Perfil Natural)
// ============================================================

t.AddMeasure("Risco Médio Natural",
    "CALCULATE(AVERAGE('PDA_Base'[R_natural]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "02 Médias Natural");

t.AddMeasure("Extroversão Média Natural",
    "CALCULATE(AVERAGE('PDA_Base'[E_natural]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "02 Médias Natural");

t.AddMeasure("Paciência Média Natural",
    "CALCULATE(AVERAGE('PDA_Base'[P_natural]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "02 Médias Natural");

t.AddMeasure("Normas Média Natural",
    "CALCULATE(AVERAGE('PDA_Base'[N_natural]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "02 Médias Natural");

t.AddMeasure("Autocontrole Médio Natural",
    "CALCULATE(AVERAGE('PDA_Base'[A_natural]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "02 Médias Natural");

// ============================================================
// 03 — Médias dos Eixos (Perfil Adaptado)
// ============================================================

t.AddMeasure("Risco Médio Adaptado",
    "CALCULATE(AVERAGE('PDA_Base'[R_adaptado]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "03 Médias Adaptado");

t.AddMeasure("Extroversão Média Adaptado",
    "CALCULATE(AVERAGE('PDA_Base'[E_adaptado]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "03 Médias Adaptado");

t.AddMeasure("Paciência Média Adaptado",
    "CALCULATE(AVERAGE('PDA_Base'[P_adaptado]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "03 Médias Adaptado");

t.AddMeasure("Normas Média Adaptado",
    "CALCULATE(AVERAGE('PDA_Base'[N_adaptado]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "03 Médias Adaptado");

t.AddMeasure("Autocontrole Médio Adaptado",
    "CALCULATE(AVERAGE('PDA_Base'[A_adaptado]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "03 Médias Adaptado");

// ============================================================
// 04 — Radar (usa a tabela PDA_Eixos_Longo)
// ============================================================

t.AddMeasure("Valor Médio Eixo", @"
    CALCULATE(
        AVERAGE('PDA_Eixos_Longo'[Valor]),
        FILTER(
            'PDA_Eixos_Longo',
            RELATED('PDA_Base'[Consistencia]) = ""Consistente""
        )
    )",
    displayFolder: "04 Radar");

// ============================================================
// 05 — Análise de Gap Natural x Adaptado
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

t.AddMeasure("Qtd Alto Gap", @"
    CALCULATE(
        COUNTROWS('PDA_Base'),
        'PDA_Base'[Gap_Total_Absoluto] > 60,
        'PDA_Base'[Consistencia] = ""Consistente""
    )",
    displayFolder: "05 Gap");

t.AddMeasure("% Alto Gap",
    "DIVIDE([Qtd Alto Gap], [Total Avaliações], 0)",
    displayFolder: "05 Gap");

// ============================================================
// 06 — Equilíbrio de Energia e Estresse
// ============================================================

t.AddMeasure("Qtd Possivelmente Estressados", @"
    CALCULATE(
        COUNTROWS('PDA_Base'),
        'PDA_Base'[EE] > 60,
        'PDA_Base'[Consistencia] = ""Consistente""
    )",
    displayFolder: "06 EE");

t.AddMeasure("% Estressados",
    "DIVIDE([Qtd Possivelmente Estressados], [Total Avaliações], 0)",
    displayFolder: "06 EE");

t.AddMeasure("Qtd Possivelmente Desmotivados", @"
    CALCULATE(
        COUNTROWS('PDA_Base'),
        'PDA_Base'[EE] < 40,
        'PDA_Base'[Consistencia] = ""Consistente""
    )",
    displayFolder: "06 EE");

t.AddMeasure("% Desmotivados",
    "DIVIDE([Qtd Possivelmente Desmotivados], [Total Avaliações], 0)",
    displayFolder: "06 EE");

t.AddMeasure("EE Médio",
    "CALCULATE(AVERAGE('PDA_Base'[EE]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "06 EE");

// ============================================================
// 07 — Perfis com Eixos Extremados
// ============================================================

t.AddMeasure("Qtd com Autocontrole Extremado", @"
    CALCULATE(
        COUNTROWS('PDA_Base'),
        'PDA_Base'[Autocontrole_Extremado] = TRUE()
    )",
    displayFolder: "07 Eixos Extremados");

t.AddMeasure("Qtd com Alerta Perfil Intenso", @"
    CALCULATE(
        COUNTROWS('PDA_Base'),
        'PDA_Base'[Alerta_Perfil_Intenso] = TRUE()
    )",
    displayFolder: "07 Eixos Extremados");

t.AddMeasure("% Alerta Perfil Intenso",
    "DIVIDE([Qtd com Alerta Perfil Intenso], [Total Avaliações], 0)",
    displayFolder: "07 Eixos Extremados");

t.AddMeasure("Qtd Eixos Extremados Médio",
    "AVERAGE('PDA_Base'[Qtd_Eixos_Extremados])",
    displayFolder: "07 Eixos Extremados");

// ============================================================
// 08 — Tomada de Decisão
// ============================================================

t.AddMeasure("Qtd por Estilo TD",
    "COUNTROWS('PDA_Base')",
    displayFolder: "08 TD");

t.AddMeasure("% em Tensão TD", @"
    DIVIDE(
        CALCULATE(
            COUNTROWS('PDA_Base'),
            'PDA_Base'[Classificacao_TD] IN {
                ""Tensão confirmada"",
                ""Tensão (arriscado compensado)"",
                ""Tensão (cauteloso compensado)""
            }
        ),
        [Total Avaliações],
        0
    )",
    displayFolder: "08 TD");

// ============================================================
// 09 — Distância ao Perfil Médio do Cargo
// ============================================================

t.AddMeasure("Distância ao Perfil Médio Cargo", @"
VAR MediaR = CALCULATE(
    AVERAGE('PDA_Base'[R_adaptado]),
    ALLEXCEPT('PDA_Base', 'PDA_Base'[cargo]),
    'PDA_Base'[Consistencia] = ""Consistente""
)
VAR MediaE = CALCULATE(
    AVERAGE('PDA_Base'[E_adaptado]),
    ALLEXCEPT('PDA_Base', 'PDA_Base'[cargo]),
    'PDA_Base'[Consistencia] = ""Consistente""
)
VAR MediaP = CALCULATE(
    AVERAGE('PDA_Base'[P_adaptado]),
    ALLEXCEPT('PDA_Base', 'PDA_Base'[cargo]),
    'PDA_Base'[Consistencia] = ""Consistente""
)
VAR Media_N = CALCULATE(
    AVERAGE('PDA_Base'[N_adaptado]),
    ALLEXCEPT('PDA_Base', 'PDA_Base'[cargo]),
    'PDA_Base'[Consistencia] = ""Consistente""
)
RETURN
    SQRT(
        ('PDA_Base'[R_adaptado] - MediaR)^2 +
        ('PDA_Base'[E_adaptado] - MediaE)^2 +
        ('PDA_Base'[P_adaptado] - MediaP)^2 +
        ('PDA_Base'[N_adaptado] - Media_N)^2
    )",
    displayFolder: "09 Cargo");
