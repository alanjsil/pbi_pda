// Tabular Editor C# Script
// Conecte ao Power BI Desktop com o Start.pbix aberto, depois execute este script
// (CTRL+ENTER ou no menu: Scripts > Execute)
//
// NORMALIZADO conforme medidas_dax.txt
// - Colunas calculadas com underscore (ex: Gap_R)
// - Médias filtram apenas Consistentes
// - Inclui Natural + Adaptado

var t = Model.Tables["PDA_Base"];

// ============================================================
// 1. MEDIDAS — Médias dos Eixos (Perfil Natural)
// ============================================================

t.AddMeasure("Risco Médio Natural",
    "CALCULATE(AVERAGE('PDA_Base'[R_natural]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "Médias Natural");

t.AddMeasure("Extroversão Média Natural",
    "CALCULATE(AVERAGE('PDA_Base'[E_natural]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "Médias Natural");

t.AddMeasure("Paciência Média Natural",
    "CALCULATE(AVERAGE('PDA_Base'[P_natural]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "Médias Natural");

t.AddMeasure("Normas Média Natural",
    "CALCULATE(AVERAGE('PDA_Base'[N_natural]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "Médias Natural");

t.AddMeasure("Autocontrole Médio Natural",
    "CALCULATE(AVERAGE('PDA_Base'[A_natural]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "Médias Natural");

// ============================================================
// 2. MEDIDAS — Médias dos Eixos (Perfil Adaptado)
// ============================================================

t.AddMeasure("Risco Médio Adaptado",
    "CALCULATE(AVERAGE('PDA_Base'[R_adaptado]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "Médias Adaptado");

t.AddMeasure("Extroversão Média Adaptado",
    "CALCULATE(AVERAGE('PDA_Base'[E_adaptado]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "Médias Adaptado");

t.AddMeasure("Paciência Média Adaptado",
    "CALCULATE(AVERAGE('PDA_Base'[P_adaptado]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "Médias Adaptado");

t.AddMeasure("Normas Média Adaptado",
    "CALCULATE(AVERAGE('PDA_Base'[N_adaptado]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "Médias Adaptado");

t.AddMeasure("Autocontrole Médio Adaptado",
    "CALCULATE(AVERAGE('PDA_Base'[A_adaptado]), 'PDA_Base'[Consistencia] = \"Consistente\")",
    displayFolder: "Médias Adaptado");

// ============================================================
// 3. COLUNAS CALCULADAS — Gap Natural vs Adaptado
// ============================================================

t.AddCalculatedColumn("Gap_R", "'PDA_Base'[R_adaptado] - 'PDA_Base'[R_natural]");
t.AddCalculatedColumn("Gap_E", "'PDA_Base'[E_adaptado] - 'PDA_Base'[E_natural]");
t.AddCalculatedColumn("Gap_P", "'PDA_Base'[P_adaptado] - 'PDA_Base'[P_natural]");
t.AddCalculatedColumn("Gap_N", "'PDA_Base'[N_adaptado] - 'PDA_Base'[N_natural]");
t.AddCalculatedColumn("Gap_A", "'PDA_Base'[A_adaptado] - 'PDA_Base'[A_natural]");

t.AddCalculatedColumn("Gap_Total_Absoluto", @"
    ABS('PDA_Base'[Gap_R]) + ABS('PDA_Base'[Gap_E]) +
    ABS('PDA_Base'[Gap_P]) + ABS('PDA_Base'[Gap_N]) + ABS('PDA_Base'[Gap_A])
");

// ============================================================
// 4. CLASSIFICAÇÃO — Tomada de Decisão (TD) — manual pág. 38
// ============================================================
t.AddCalculatedColumn("Classificacao_TD", @"
SWITCH(
    TRUE(),
    'PDA_Base'[TD_natural] > 80, ""Estilo Arriscado"",
    'PDA_Base'[TD_natural] > 50, ""Tensão (arriscado compensado)"",
    'PDA_Base'[TD_natural] = 50, ""Tensão confirmada"",
    'PDA_Base'[TD_natural] >= 20, ""Tensão (cauteloso compensado)"",
    TRUE(), ""Estilo Cauteloso""
)
");

// ============================================================
// 5. ALERTAS — Intensidade do Eixo (IE) extrema — manual pág. 27
// ============================================================
t.AddCalculatedColumn("Risco_Extremado",        "OR('PDA_Base'[IE_R_natural] > 80, 'PDA_Base'[IE_R_natural] < 20)");
t.AddCalculatedColumn("Extroversao_Extremada",  "OR('PDA_Base'[IE_E_natural] > 80, 'PDA_Base'[IE_E_natural] < 20)");
t.AddCalculatedColumn("Paciencia_Extremada",    "OR('PDA_Base'[IE_P_natural] > 80, 'PDA_Base'[IE_P_natural] < 20)");
t.AddCalculatedColumn("Normas_Extremada",       "OR('PDA_Base'[IE_N_natural] > 80, 'PDA_Base'[IE_N_natural] < 20)");
t.AddCalculatedColumn("Autocontrole_Extremado", "OR('PDA_Base'[IE_A_natural] > 80, 'PDA_Base'[IE_A_natural] < 20)");

t.AddCalculatedColumn("Qtd_Eixos_Extremados", @"
    ('PDA_Base'[Risco_Extremado] * 1) + ('PDA_Base'[Extroversao_Extremada] * 1) +
    ('PDA_Base'[Paciencia_Extremada] * 1) + ('PDA_Base'[Normas_Extremada] * 1) +
    ('PDA_Base'[Autocontrole_Extremado] * 1)
");

// ============================================================
// 6. ALERTA — Perfil Intenso (IP > 80 ou Autocontrole extremado)
// ============================================================
t.AddCalculatedColumn("Alerta_Perfil_Intenso", @"
    OR('PDA_Base'[IP_natural] > 80, 'PDA_Base'[Autocontrole_Extremado] = TRUE())
");
