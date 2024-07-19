# Casos de Uso

## Caso de Uso 1: Registrar Lançamento

**Descrição:** Permite que o comerciante registre um lançamento financeiro (débito ou crédito).

**Atores:**
- Comerciante

**Fluxo Principal:**
1. O comerciante acessa o sistema de controle de lançamentos.
2. O sistema exibe um formulário para inserção de novos lançamentos.
3. O comerciante preenche os detalhes do lançamento (tipo, valor, data, categoria).
4. O comerciante confirma a inserção do lançamento.
5. O sistema valida os dados e salva o lançamento.
6. O sistema exibe uma mensagem de sucesso.

**Fluxos Alternativos:**
- 3a. Se o comerciante tentar registrar um lançamento com dados inválidos (e.g., valor negativo), o sistema exibe uma mensagem de erro e solicita correção.
- 5a. Se o sistema encontrar um erro ao salvar os dados (e.g., falha na conexão com o banco de dados), exibe uma mensagem de erro e permite que o comerciante tente novamente.

## Caso de Uso 2: Consultar Lançamentos

**Descrição:** Permite que o comerciante visualize e consulte lançamentos financeiros por data, categoria e valor.

**Atores:**
- Comerciante

**Fluxo Principal:**
1. O comerciante acessa o sistema de controle de lançamentos.
2. O sistema exibe uma interface para consulta de lançamentos.
3. O comerciante aplica filtros para buscar lançamentos por data, categoria ou valor.
4. O sistema exibe a lista de lançamentos que atendem aos critérios de busca.

**Fluxos Alternativos:**
- 3a. Se o comerciante inserir critérios de busca inválidos (e.g., data no formato incorreto), o sistema exibe uma mensagem de erro e solicita correção.
- 4a. Se não houver lançamentos que atendam aos critérios de busca, o sistema exibe uma mensagem informando que nenhum resultado foi encontrado.

## Caso de Uso 3: Gerar Relatório Diário Consolidado

**Descrição:** Permite que o comerciante gere um relatório consolidado com o saldo diário.

**Atores:**
- Comerciante

**Fluxo Principal:**
1. O comerciante acessa o sistema de consolidação diária.
2. O comerciante solicita a geração do relatório diário.
3. O sistema processa os lançamentos do dia e calcula o saldo consolidado.
4. O sistema gera o relatório em formato PDF ou CSV.
5. O sistema disponibiliza o relatório para download.

**Fluxos Alternativos:**
- 2a. Se o comerciante solicitar um relatório para uma data sem lançamentos, o sistema exibe uma mensagem informando que não há dados para essa data.
- 4a. Se ocorrer um erro durante a geração do relatório (e.g., falha na conexão com o servidor), o sistema exibe uma mensagem de erro e solicita nova tentativa.

## Caso de Uso 4: Visualizar Relatório Diário

**Descrição:** Permite que o comerciante visualize o relatório diário consolidado no sistema.

**Atores:**
- Comerciante

**Fluxo Principal:**
1. O comerciante acessa o sistema de consolidação diária.
2. O comerciante visualiza o relatório consolidado gerado para o dia atual.
3. O sistema exibe os detalhes do saldo diário e os lançamentos que contribuíram para o saldo.

**Fluxos Alternativos:**
- 2a. Se não houver um relatório gerado para o dia atual, o sistema exibe uma mensagem informando que o relatório ainda não está disponível.

## Caso de Uso 5: Exportar Relatório

**Descrição:** Permite que o comerciante exporte o relatório diário consolidado em formato PDF ou CSV.

**Atores:**
- Comerciante

**Fluxo Principal:**
1. O comerciante acessa o relatório diário consolidado no sistema.
2. O comerciante escolhe o formato de exportação (PDF ou CSV).
3. O sistema gera o arquivo no formato escolhido.
4. O sistema disponibiliza o arquivo para download.

**Fluxos Alternativos:**
- 3a. Se ocorrer um erro durante a geração do arquivo (e.g., falha na conversão para PDF), o sistema exibe uma mensagem de erro e solicita nova tentativa.

## Caso de Uso 6: Manter Sistema Disponível

**Descrição:** Garante que o serviço de controle de lançamentos permaneça disponível, mesmo se o serviço de consolidação diária estiver indisponível.

**Atores:**
- Sistema (Automático)

**Fluxo Principal:**
1. O sistema monitora a disponibilidade do serviço de consolidação diária.
2. Em caso de falha no serviço de consolidação, o sistema garante que o serviço de controle de lançamentos continue operacional.

**Fluxos Alternativos:**
- 2a. Se o sistema de controle de lançamentos também encontrar uma falha, o sistema exibe uma mensagem de erro e tenta reconectar automaticamente.

## Caso de Uso 7: Suportar Requisições em Dias de Pico

**Descrição:** Garante que o serviço de consolidação diária suporte até 500 requisições por segundo, com no máximo 5% de perda de requisições.

**Atores:**
- Sistema (Automático)

**Fluxo Principal:**
1. O sistema monitora o volume de requisições recebidas.
2. O sistema aloca recursos adicionais para suportar o aumento de carga durante picos de acesso.
3. O sistema garante que a taxa de perda de requisições não exceda 5%.

**Fluxos Alternativos:**
- 2a. Se o sistema não conseguir alocar recursos adicionais, exibe uma mensagem de alerta e prioriza as requisições críticas.
- 3a. Se a taxa de perda de requisições exceder 5%, o sistema envia um alerta para o administrador.
