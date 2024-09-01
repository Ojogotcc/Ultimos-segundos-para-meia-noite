VAR Avatar0Inicial = "Protagonista"
VAR Avatar0AparenciaInicial = "serio"
VAR Avatar1Inicial = "Militar"
VAR Avatar1AparenciaInicial = "sem_mascara"

-> inicio

=== inicio ===
# AparenciaL1: com_mascara
Militar: Olá Jovem Robô. 
    
# AparenciaL0: sorrindo
Protagonista: Opa, Bom? 

Protagonista: Mascara legal.

Militar: Curtiu? O problema que não dá pra enxergar sem.

Militar: Qual digimon você escolhe?
    * [Agumon]
        -> escolha("Agumon")
    * [Gabumon]
        -> escolha("Gabumon")
    * [Korumon]
        -> escolha("Korumon")
=== escolha(digimon) ===
Militar: Você escolheu o {digimon}!

# AparenciaL1: com_mascara
Militar: Tome muito cuidado nessa terras, há uma lenda que o ET bilu anda por aí

# AparenciaL1: com_mascara
Militar: E também o mario foi visto recentemente aqui

# Aparencia(sorrindo)
Protagonista: Que mario??
-> END