--usando como banco de dados feito em SQL Server , Modele uma estrutura de dados do seguinte caso:
--• Um cliente possui os seguintes campos: Nome, CPF, UF, Celular;
--• Um cliente possui N financiamentos;
--• Um financiamento possui os seguintes campos: CPF, Tipo Financiamento, Valor Total, Data do Último Vencimento;
--• Um financiamento possui N parcelas;
--• Uma parcela possui os seguintes campos: Id Financiamento, Número da Parcela, Valor
--Parcela, Data Vencimento, Data Pagamento;
--Crie as tabelas que julgar necessárias e insira alguns registros de teste nas mesmas. Na sequência,elabore as seguintes querys:
--• Listar todos os clientes do estado de SP que possuem mais de 60% das parcelas pagas;
--• Listar os primeiros quatro clientes que possuem alguma parcela com mais de cinco dia sem atraso (Data Vencimento maior que data atual E data pagamento nula).

CREATE TABLE Cliente (
    CPF CHAR(11) PRIMARY KEY,
    Nome VARCHAR(300),
    UF CHAR(2),
    Celular VARCHAR(20)
);

CREATE TABLE TipoFinanciamento (
    IdTipoFinanciamento INT PRIMARY KEY,
    TipoFinanciamento VARCHAR(100)
);

CREATE TABLE Financiamento (
    ID INT PRIMARY KEY IDENTITY,
    CPF CHAR(11),
    IdTipoFinanciamento INT,
    ValorTotal DECIMAL(18, 2),
    DataUltimoVencimento DATE,
    FOREIGN KEY (CPF) REFERENCES Cliente(CPF),
    FOREIGN KEY (IdTipoFinanciamento) REFERENCES TipoFinanciamento(IdTipoFinanciamento)
);

CREATE TABLE Parcela (
    ID INT PRIMARY KEY IDENTITY,
    IDFinanciamento INT,
    NumeroParcela INT,
    ValorParcela DECIMAL(18, 2),
    DataVencimento DATE,
    DataPagamento DATE,
    FOREIGN KEY (IDFinanciamento) REFERENCES Financiamento(ID)
);

INSERT INTO Cliente (CPF, Nome, UF, Celular) VALUES
('11111111111', 'João Silva', 'SP', '1199999-9999'),
('22222222222', 'Ana Maria', 'SP', '1198888-8888'),
('33333333333', 'Beto rodrigues', 'RJ', '3197777-7777');

INSERT INTO TipoFinanciamento (IdTipoFinanciamento, TipoFinanciamento) VALUES
(1, 'Financiamento Casa'),
(2, 'Financiamento Carro');

INSERT INTO Financiamento (CPF, IdTipoFinanciamento, ValorTotal, DataUltimoVencimento) VALUES
('11111111111', 1, 100000.00, '2023-12-31'),
('22222222222', 2, 50000.00, '2023-12-31');

INSERT INTO Parcela (IDFinanciamento, NumeroParcela, ValorParcela, DataVencimento, DataPagamento) VALUES
(1, 1, 5000.00, '2022-12-31', '2022-12-28'),
(1, 2, 5000.00, '2023-01-31', NULL),
(2, 1, 2500.00, '2022-12-28', '2022-12-28'),
(2, 2, 2500.00, '2023-01-28', NULL);

---Listar todos os clientes do estado de SP que possuem mais de 60% das parcelas pagas:
SELECT c.*
FROM Cliente c
JOIN Financiamento f ON c.CPF = f.CPF
JOIN (
    SELECT IDFinanciamento, 
           COUNT(*) AS TotalParcelas,
           SUM(CASE WHEN DataPagamento IS NOT NULL THEN 1 ELSE 0 END) AS ParcelasPagas
    FROM Parcela
    GROUP BY IDFinanciamento
) p ON f.ID = p.IDFinanciamento
WHERE c.UF = 'SP'
AND ParcelasPagas / TotalParcelas > 0.6;

--Listar os primeiros quatro clientes que possuem alguma parcela com mais de cinco dias sem atraso
SELECT TOP 4 c.*
FROM Cliente c
JOIN Financiamento f ON c.CPF = f.CPF
JOIN Parcela p ON f.ID = p.IDFinanciamento
WHERE p.DataVencimento > GETDATE() AND p.DataPagamento IS NULL;