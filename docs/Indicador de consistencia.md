# 1. Como o Indicador de Consistência é calculado

O manual (pág. 51-54) explica que o sistema PDA calcula internamente cruzando 5 variáveis. **Ele não dá uma fórmula matemática explícita** — isso é proprietário do sistema. O que ele descreve são os **gatilhos que degradam a consistência**:

| Variável                   | Quando vira sinal de inconsistência |
| -------------------------- | ----------------------------------- |
| NE (Nível de Energia)      | < 20% ou > 80%                      |
| EE (Equilíbrio de Energia) | < 20% ou > 80%                      |
| IP (Intensidade do Perfil) | < 20% ou > 80%                      |
| MP (Modificação do Perfil) | < 20% ou > 80%                      |
| TF (Tempo do Formulário)   | < 3 min ou > 50 min                 |

O sistema conta quantos desses sinais aparecem combinados. Um sinal isolado não invalida — quando aparecem **vários juntos**, o relatório vai para Pouco Consistente ou Inválido.

**Consequência prática para o CSV:** o campo `Consistencia` no CSV não é algo que calculamos — ele vem **exportado diretamente do sistema PDA**. O não precisa calcular isso, só consumir o valor. No nosso exemplo de dados fictícios eu coloquei manualmente, mas na vida real você recebe esse campo pronto na exportação.

O que podemos fazer no Power BI é replicar um score de alerta aproximado para fins de monitoramento, combinando os gatilhos que temos:

```text
Score Risco Inconsistência =
VAR flagNE = IF('PDA_Base'[NE_natural] > 80 || 'PDA_Base'[NE_natural] < 20, 1, 0)
VAR flagEE = IF('PDA_Base'[EE] > 80 || 'PDA_Base'[EE] < 20, 1, 0)
VAR flagIP = IF('PDA_Base'[IP_natural] > 80 || 'PDA_Base'[IP_natural] < 20, 1, 0)
VAR flagMP = IF('PDA_Base'[MP] > 80 || 'PDA_Base'[MP] < 20, 1, 0)
RETURN flagNE + flagEE + flagIP + flagMP
-- 0 = sem risco | 1-2 = atenção | 3-4 = alto risco de inconsistência
```
