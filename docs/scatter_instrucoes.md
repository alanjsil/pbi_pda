# Scatter Plot — Mapa de Perfis PDA
## Configuração correta no Power BI

### Campos do visual

| Campo do visual | O que colocar | De onde vem |
|----------------|---------------|-------------|
| Valores | id_colaborador | PDA_Base (coluna) |
| Eixo X | Risco Médio Natural | Medida (grupo 02) |
| Eixo Y | Extroversão Média Natural | Medida (grupo 02) |
| Tamanho | NE_natural | PDA_Base (coluna — Power BI usa SUM, mas como é 1 por pessoa funciona) |
| Legenda | Quadrante_Marston | PDA_Base (coluna — 4 quadrantes = 4 cores automáticas) |
| Dicas de ferramentas | nome, area, cargo, Classificacao_EE | PDA_Base (colunas) |

> **Por que id_colaborador em Valores e não nome?**
> O Power BI usa o campo de Valores para desagregar os pontos.
> id_colaborador é número — garante um ponto por pessoa sem ambiguidade.
> O nome aparece no tooltip via "Dicas de ferramentas".

---

### Configuração dos eixos (Formatar visual)

**Eixo X (Risco):**
- Mínimo: 0
- Máximo: 100
- Título: "Risco (Natural)"

**Eixo Y (Extroversão):**
- Mínimo: 0
- Máximo: 100
- Título: "Extroversão (Natural)"

---

### Linhas de referência

O ponto de corte visual é 50 (centro do gráfico),
que divide os 4 quadrantes de Marston.

**No Eixo X — adicione 1 linha:**
- Valor: 50
- Estilo: Tracejado
- Cor: cinza (#AAAAAA)
- Rótulo: "Centro"

**No Eixo Y — adicione 1 linha:**
- Valor: 50
- Estilo: Tracejado
- Cor: cinza (#AAAAAA)
- Rótulo: "Centro"

> As faixas do manual (33 e 67) podem ser adicionadas como
> linhas secundárias mais finas se quiser mostrar as 3 tendências,
> mas o centro em 50 já é suficiente para leitura dos quadrantes.

---

### Como ficam os 4 quadrantes

```
                  EXTROVERSÃO ALTA
                        |
       Influente        |      Proativo
    (R baixo, E alto)   |  (R alto, E alto)
                        |
RISCO ——————————————— 50,50 ————————————— RISCO
BAIXO                   |                  ALTO
                        |
       Receptivo        |      Dominante
    (R baixo, E baixo)  |  (R alto, E baixo)
                        |
                  EXTROVERSÃO BAIXA
```

A coluna Quadrante_Marston (criada no Power Query) já
classifica cada pessoa em um desses 4 grupos — por isso
ela funciona perfeitamente como Legenda (4 cores distintas).

---

### Segmentações sugeridas ao lado do scatter

- area (segmentação de lista)
- Classificacao_TD_Natural (segmentação de lista)
- Consistencia (segmentação de lista — para excluir Inválidos)

