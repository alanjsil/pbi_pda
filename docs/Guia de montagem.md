# Guia de Montagem — PDA no Power BI

## 1. Ordem de importação das Queries (Power Query)

### 1.1 PDA_Base

1. Nova Query > CSV > selecione `pda_exemplo.csv`
2. Nome da base `PDA_Base`
3. Cole o código do **PASSO 1** do arquivo `powerquery_pda.m`
4. Fechar e Aplicar

### 1.2 PDA_Eixos_Longo

1. Nova Query em Branco
2. Renomeie para `PDA_Eixos_Longo`
3. Cole o código do **PASSO 2** (referencia PDA_Base automaticamente)
4. Aplicar

### 1.3 PDA_Faixas

1. Nova Query em Branco
2. Renomeie para `PDA_Faixas`
3. Cole o código do **PASSO 3**
4. Aplicar

---

## 2. Relacionamento entre tabelas

No Model View, crie:

| De                       | Para                            | Tipo  | Cardinalidade |
| ------------------------ | ------------------------------- | ----- | ------------- |
| PDA_Base[id_colaborador] | PDA_Eixos_Longo[id_colaborador] | Ativo | 1:N           |

PDA_Faixas não precisa de relacionamento — é usada como tabela de referência visual.

---

## 3. Colunas calculadas vs Medidas

As colunas abaixo já vêm prontas do Power Query:

- Gap_R, Gap_E, Gap_P, Gap_N, Gap_A, Gap_Total_Absoluto
- Classificacao_TD, Classificacao_IP, Classificacao_NE, Classificacao_EE, Classificacao_MP
- Risco_Extremado, Extroversao_Extremada, Paciencia_Extremada, Normas_Extremada, Autocontrole_Extremado
- Qtd_Eixos_Extremados, Alerta_Perfil_Intenso
- Tendencia_Autocontrole, Eixo_Dominante

Cole as medidas do arquivo `medidas_dax.txt` no modelo após fechar o Power Query.
Crie uma tabela vazia chamada `_Medidas PDA` para organizar todas elas.

---

## 4. Páginas sugeridas para o relatório

### Página 1 — Visão Geral

| Visual        | Campos                              | Medida           |
| ------------- | ----------------------------------- | ---------------- |
| KPI card      | —                                   | Total Avaliações |
| KPI card      | —                                   | % Consistentes   |
| KPI card      | —                                   | % Estressados    |
| KPI card      | —                                   | % em Tensão TD   |
| Gráfico pizza | Consistencia                        | Total Avaliações |
| Tabela        | nome, area, cargo, Classificacao_EE | EE Médio         |

### Página 2 — Perfil da Empresa (Radar)

| Visual                                                  | Campos                                            |
| ------------------------------------------------------- | ------------------------------------------------- |
| Radar (visual de terceiro: Radar Chart by MAQ Software) | Eixo=Eixo, Valor=Valor Médio Eixo, Legenda=Perfil |
| Segmentação                                             | area                                              |
| Segmentação                                             | cargo                                             |
| Segmentação                                             | Perfil (Natural/Adaptado)                         |

### Página 3 — Mapa de Perfis (Scatter)

| Visual      | Campos                                 |
| ----------- | -------------------------------------- |
| Scatter     | Eixo X = R_natural, Eixo Y = E_natural |
| Detalhes    | nome                                   |
| Tamanho     | NE_natural                             |
| Cor         | area                                   |
| Segmentação | Classificacao_TD                       |

> Linhas de referência no scatter: X=33, X=67, Y=33, Y=67
> dividem o gráfico nos 4 quadrantes de Marston (D/I/S/C)

### Página 4 — Gap Natural x Adaptado

| Visual                      | Campos                                                          |
| --------------------------- | --------------------------------------------------------------- |
| Gráfico barras clusterizado | nome no eixo, Gap_R / Gap_E / Gap_P / Gap_N como valores        |
| Gráfico barras simples      | nome no eixo, Gap_Total_Absoluto                                |
| Tabela                      | nome, Classificacao_EE, Classificacao_MP, Alerta_Perfil_Intenso |

### Página 5 — Alertas

| Visual          | Campos                                                                                                                   |
| --------------- | ------------------------------------------------------------------------------------------------------------------------ |
| Tabela filtrada | Alerta_Perfil_Intenso = TRUE, colunas: nome, area, cargo, Qtd_Eixos_Extremados, Autocontrole_Extremado, Classificacao_EE |
| Tabela filtrada | Consistencia = Inválido ou Pouco_consistente                                                                             |
| KPI card        | Qtd com Alerta Perfil Intenso                                                                                            |

---

## 5. Dica de formatação condicional

Na tabela de alertas, aplique formatação condicional por regras:

**Coluna Classificacao_EE:**

- "Possivelmente estressado" → fundo laranja
- "Sobrecarga extrema" → fundo vermelho
- "Motivado / equilibrado" → fundo verde

**Coluna Consistencia:**

- "Inválido" → fundo vermelho
- "Pouco_consistente" → fundo amarelo
