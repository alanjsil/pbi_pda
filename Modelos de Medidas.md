## 1. Médias gerais dos eixos (Perfil Natural)

```dax
Risco Médio Natural = AVERAGE('PDA'[R_natural])
Extroversao Média Natural = AVERAGE('PDA'[E_natural])
Paciencia Média Natural = AVERAGE('PDA'[P_natural])
Normas Média Natural = AVERAGE('PDA'[N_natural])
Autocontrole Médio Natural = AVERAGE('PDA'[A_natural])
```

Mesma lógica para `_adaptado`. Essas alimentam o "perfil médio da empresa/área" para comparação.

## 2. Gap entre Natural e Adaptado (Indicador de Modificação)

Por eixo, calculado linha a linha:

```dax
Gap Risco = 'PDA'[R_adaptado] - 'PDA'[R_natural]
Gap Extroversao = 'PDA'[E_adaptado] - 'PDA'[E_natural]
Gap Paciencia = 'PDA'[P_adaptado] - 'PDA'[P_natural]
Gap Normas = 'PDA'[N_adaptado] - 'PDA'[N_natural]
Gap Autocontrole = 'PDA'[A_adaptado] - 'PDA'[A_natural]
```

Essas são melhores como colunas calculadas (não medidas), pois você vai querer usá-las em visuais por pessoa.

```dax
Gap Total Absoluto =
ABS('PDA'[Gap Risco]) + ABS('PDA'[Gap Extroversao]) +
ABS('PDA'[Gap Paciencia]) + ABS('PDA'[Gap Normas]) + ABS('PDA'[Gap Autocontrole])
```

## 3. Classificação de Tomada de Decisão (TD) — pág. 38

Coluna calculada, seguindo as faixas do manual:

```dax
Classificacao TD =
SWITCH(
    TRUE(),
    'PDA'[TD_natural] > 80, "Estilo Arriscado",
    'PDA'[TD_natural] > 50, "Tensão possível (arriscado compensado)",
    'PDA'[TD_natural] = 50, "Tensão confirmada",
    'PDA'[TD_natural] >= 20, "Tensão possível (cauteloso compensado)",
    TRUE(), "Estilo Cauteloso"
)
```

Depois, uma medida de contagem:

```dax
Qtd por Classificacao TD = COUNTROWS('PDA')
```

(Arraste junto com Classificacao TD num gráfico de barras)

## 4. Classificação de Intensidade do Perfil (IP) — pág. 40

```dax
Classificacao IP =
SWITCH(
    TRUE(),
    'PDA'[IP_natural] > 80, "Muito rígido (possível inconsistência)",
    'PDA'[IP_natural] > 65, "Perfil muito evidente",
    'PDA'[IP_natural] >= 31, "Flexibilidade normal",
    'PDA'[IP_natural] >= 20, "Perfil pouco evidente",
    TRUE(), "Flexibilidade extrema (possível inconsistência)"
)
```

## 5. Classificação de Equilíbrio de Energia (EE) — pág. 45

```dax
Classificacao EE =
SWITCH(
    TRUE(),
    'PDA'[EE] > 80, "Sobrecarga extrema (possível inconsistência)",
    'PDA'[EE] > 60, "Possivelmente estressado",
    'PDA'[EE] >= 40, "Motivado / equilibrado",
    'PDA'[EE] >= 20, "Possivelmente desmotivado",
    TRUE(), "Energia muito acima do necessário (possível inconsistência)"
)
```

Esse cruzado com área/cargo é ótimo pra identificar times sob risco de burnout (EE alto) ou subaproveitados (EE baixo).

## 6. Alertas de Intensidade do Eixo (IE) extrema — pág. 27

Para cada eixo natural, uma coluna booleana:

```dax
Risco Extremado = OR('PDA'[IE_R_natural] > 80, 'PDA'[IE_R_natural] < 20)
Extroversao Extremada = OR('PDA'[IE_E_natural] > 80, 'PDA'[IE_E_natural] < 20)
Paciencia Extremada = OR('PDA'[IE_P_natural] > 80, 'PDA'[IE_P_natural] < 20)
Normas Extremada = OR('PDA'[IE_N_natural] > 80, 'PDA'[IE_N_natural] < 20)
Autocontrole Extremado = OR('PDA'[IE_A_natural] > 80, 'PDA'[IE_A_natural] < 20)
```

```dax
Qtd Eixos Extremados =
('PDA'[Risco Extremado] * 1) + ('PDA'[Extroversao Extremada] * 1) +
('PDA'[Paciencia Extremada] * 1) + ('PDA'[Normas Extremada] * 1) +
('PDA'[Autocontrole Extremado] * 1)
```

Pessoas com Qtd Eixos Extremados >= 2 e Autocontrole extremado merecem leitura adicional conforme a tabela da pág. 30 — bom gatilho para o RH revisar com devolutiva.

## 7. Indicador de Consistência — painel de qualidade

```dax
Total Avaliacoes = COUNTROWS('PDA')

Qtd Consistentes = CALCULATE(COUNTROWS('PDA'), 'PDA'[Consistencia] = "Consistente")

% Consistentes = DIVIDE([Qtd Consistentes], [Total Avaliacoes])

Qtd Invalidos = CALCULATE(COUNTROWS('PDA'), 'PDA'[Consistencia] = "Inválido")

% Invalidos = DIVIDE([Qtd Invalidos], [Total Avaliacoes])
```

## 8. Distância de um colaborador ao perfil médio do cargo (compatibilidade)

Aqui entra a parte mais "modelo" — uma medida que calcula a distância euclidiana entre o perfil Adaptado de uma pessoa e a média do cargo:

```dax
Distancia ao Perfil Medio Cargo =
VAR MediaR = CALCULATE(AVERAGE('PDA'[R_adaptado]), ALLEXCEPT('PDA', 'PDA'[cargo]))
VAR MediaE = CALCULATE(AVERAGE('PDA'[E_adaptado]), ALLEXCEPT('PDA', 'PDA'[cargo]))
VAR MediaP = CALCULATE(AVERAGE('PDA'[P_adaptado]), ALLEXCEPT('PDA', 'PDA'[cargo]))
VAR MediaN = CALCULATE(AVERAGE('PDA'[N_adaptado]), ALLEXCEPT('PDA', 'PDA'[cargo]))
RETURN
SQRT(
    ('PDA'[R_adaptado] - MediaR)^2 +
    ('PDA'[E_adaptado] - MediaE)^2 +
    ('PDA'[P_adaptado] - MediaP)^2 +
    ('PDA'[N_adaptado] - MediaN)^2
)
```

Útil quando você tiver mais de uma pessoa por cargo — quanto menor a distância, mais "alinhado" ao perfil típico daquela função.
