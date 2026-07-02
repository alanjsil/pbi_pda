# Dados numéricos por pessoa (a base do modelo):

- 5 eixos (R, E, P, N, A) com pontuação 0-100, tanto para Perfil Natural quanto Adaptado (**pág 15 do rélatorio real, e 13 do manual**).
- Indicador de Intensidade do Eixo (IE) — percentual por eixo.
- Indicadores derivados: TD (Tomada de Decisão), IP (Intensidade do Perfil), NE (Nível de Energia), EE (Equilíbrio de Energia), MP (Modificação do Perfil).
- Indicador de Consistência (categórico: Consistente / Pouco consistente / Inválido | **pág 03 do rélatorio real, e 52 do manual.**).
- Tendência do eixo de Autocontrole (pág. 25 — único onde Situacional é ideal)

# Algumas ideias práticas:

## 1. Mapa de perfis da empresa (visão agregada)

Scatter plot com Risco no eixo X e Extroversão no eixo Y (ou os 4 eixos numa matriz 2x2), plotando todos os colaboradores. Isso mostra visualmente a concentração de perfis por área/departamento — útil para RH identificar se um time é homogêneo demais (risco de "ponto cego" coletivo) ou muito heterogêneo.

## 2. Gap Natural vs Adaptado por pessoa/equipe

Como cada eixo tem valor Natural e Adaptado, um gráfico de barras pareadas (ou um radar com duas camadas) mostrando a diferença entre os dois perfis. Combinado com o indicador EE (Equilíbrio de Energia), isso vira um alerta de possível estresse/desmotivação — pessoas com gap grande e EE alto podem estar "forçando" um comportamento que não é natural.

## 3. Dashboard de Tomada de Decisão (TD) por área — manual pág. 38

Já que TD compara Risco vs Normas, você pode classificar toda a base em "Arriscado / Cauteloso / Tensão" e cruzar com performance ou turnover — testar se times com TD em "Tensão" (conflito interno R vs N) têm mais rotatividade.

## 4. Compatibilidade Perfil x Cargo

Se você tiver os perfis Adaptados de quem ocupa cada cargo, pode construir um "perfil médio" por função e usar isso como referência para recrutamento — candidatos novos comparados a esse benchmark via distância euclidiana entre os eixos.

## 5. Qualidade dos dados / Indicador de Consistência

Um painel simples mostrando % de relatórios Consistentes vs Pouco Consistentes vs Inválidos por lote de aplicação — serve como controle de qualidade do processo de RH (se muitos formulários saem inconsistentes, pode indicar problema na condução da aplicação).

## 6. Times de alta intensidade (IE)

Cruzar IE do Autocontrole extremado (acima de 80% ou abaixo de 20%) com os outros eixos extremados, usando a tabela da página 30, para sinalizar perfis que tendem a comportamentos mais extremos sob pressão — útil para times de liderança ou cargos de alta exposição.

# Páginas sugeridas para o relatório

## Página 1 — Visão Geral

| Visual        | Campos                              | Medida           |
| ------------- | ----------------------------------- | ---------------- |
| KPI card      | —                                   | Total Avaliações |
| KPI card      | —                                   | % Consistentes   |
| KPI card      | —                                   | % Estressados    |
| KPI card      | —                                   | % em Tensão TD   |
| Gráfico pizza | Consistencia                        | Total Avaliações |
| Tabela        | nome, area, cargo, Classificacao_EE | EE Médio         |

## Página 2 — Perfil da Empresa (Radar)

| Visual                                                  | Campos                                            |
| ------------------------------------------------------- | ------------------------------------------------- |
| Radar (visual de terceiro: Radar Chart by MAQ Software) | Eixo=Eixo, Valor=Valor Médio Eixo, Legenda=Perfil |
| Segmentação                                             | area                                              |
| Segmentação                                             | cargo                                             |
| Segmentação                                             | Perfil (Natural/Adaptado)                         |

## Página 3 — Mapa de Perfis (Scatter)

| Visual      | Campos                                 |
| ----------- | -------------------------------------- |
| Scatter     | Eixo X = R_natural, Eixo Y = E_natural |
| Detalhes    | nome                                   |
| Tamanho     | NE_natural                             |
| Cor         | area                                   |
| Segmentação | Classificacao_TD                       |

[**Informações no arquivo scatter_instrucoes.md**](./scatter_instrucoes.md)

## Página 4 — Gap Natural x Adaptado

| Visual                      | Campos                                                          |
| --------------------------- | --------------------------------------------------------------- |
| Gráfico barras clusterizado | nome no eixo, Gap_R / Gap_E / Gap_P / Gap_N como valores        |
| Gráfico barras simples      | nome no eixo, Gap_Total_Absoluto                                |
| Tabela                      | nome, Classificacao_EE, Classificacao_MP, Alerta_Perfil_Intenso |

## Página 5 — Alertas

| Visual          | Campos                                                                                                                   |
| --------------- | ------------------------------------------------------------------------------------------------------------------------ |
| Tabela filtrada | Alerta_Perfil_Intenso = TRUE, colunas: nome, area, cargo, Qtd_Eixos_Extremados, Autocontrole_Extremado, Classificacao_EE |
| Tabela filtrada | Consistencia = Inválido ou Pouco_consistente                                                                             |
| KPI card        | Qtd com Alerta Perfil Intenso                                                                                            |
