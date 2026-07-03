// ============================================================
// Aqui são colunas calculadas adicionadas em PDA_BASE
// ============================================================
Perfil_Referencia_Mais_Proximo =
VAR R = 'PDA_Base'[R_natural]
VAR E = 'PDA_Base'[E_natural]
VAR P = 'PDA_Base'[P_natural]
VAR N = 'PDA_Base'[N_natural]
RETURN
    MINX(
        TOPN(
            1,
            ADDCOLUMNS(
                'PDA_Perfis_Referencia',
                "Dist",
                SQRT(
                    (R - 'PDA_Perfis_Referencia'[R])^2 +
                    (E - 'PDA_Perfis_Referencia'[E])^2 +
                    (P - 'PDA_Perfis_Referencia'[P])^2 +
                    (N - 'PDA_Perfis_Referencia'[N])^2
                )
            ),
            [Dist], ASC
        ),
        'PDA_Perfis_Referencia'[Perfil]
    )


Perfil_Comportamental =
VAR Tarefas   = 'PDA_Base'[R_natural] + 'PDA_Base'[N_natural]
VAR Pessoas   = 'PDA_Base'[E_natural] + 'PDA_Base'[P_natural]
VAR Proativo  = 'PDA_Base'[R_natural] + 'PDA_Base'[E_natural]
VAR Reativo   = 'PDA_Base'[P_natural] + 'PDA_Base'[N_natural]
VAR DifFoco   = ABS(Tarefas - Pessoas)
VAR DifEstilo = ABS(Proativo - Reativo)
VAR Limiar    = 15   -- PONTO A CALIBRAR, ver abaixo
RETURN
    IF(
        DifFoco < Limiar && DifEstilo < Limiar,
        "Friendly",
        SWITCH(
            TRUE(),
            'PDA_Base'[Foco_Atuacao] = "Tarefas" && 'PDA_Base'[Estilo_Reacao] = "Proativo", "Dinâmico",
            'PDA_Base'[Foco_Atuacao] = "Pessoas" && 'PDA_Base'[Estilo_Reacao] = "Proativo", "Cativante",
            'PDA_Base'[Foco_Atuacao] = "Pessoas" && 'PDA_Base'[Estilo_Reacao] = "Reativo", "Colaborativo",
            'PDA_Base'[Foco_Atuacao] = "Tarefas" && 'PDA_Base'[Estilo_Reacao] = "Reativo", "Receptivo"
        )
    )


Estilo_Reacao =
VAR Proativo = ('PDA_Base'[R_natural] + 'PDA_Base'[E_natural])
VAR Reativo = ('PDA_Base'[P_natural] + 'PDA_Base'[N_natural])
RETURN IF(Proativo > Reativo, "Proativo", "Reativo")

Foco_Atuacao =
VAR Tarefas = ('PDA_Base'[R_natural] + 'PDA_Base'[N_natural])
VAR Pessoas = ('PDA_Base'[E_natural] + 'PDA_Base'[P_natural])
RETURN IF(Tarefas > Pessoas, "Tarefas", "Pessoas")
