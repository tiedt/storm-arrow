DROP SEQUENCE SEQ_PONTUACAO_MUSICAID;
DROP SEQUENCE SEQ_PERFILID;

DROP TABLE PONTUACAO_MUSICA CASCADE CONSTRAINT;
DROP TABLE PERFIL CASCADE CONSTRAINT;

CREATE TABLE PERFIL(
    ID NUMBER(10) NOT NULL,
    ENDERECO_MAC VARCHAR2(17) NOT NULL,
    NOME VARCHAR2(15) NOT NULL,
    PONTUACAO_TOTAL NUMBER(10) NOT NULL,
    CONSTRAINT PK_PERFILID PRIMARY KEY (ID),
    CONSTRAINT UK_PERFILENDMACNOME UNIQUE (ENDERECO_MAC)
);
CREATE SEQUENCE SEQ_PERFILID;

CREATE TABLE PONTUACAO_MUSICA(
    ID NUMBER(10) NOT NULL,
    IDPERFIL NUMBER(10) NOT NULL,
    ESTILO VARCHAR2(20) NOT NULL,
    MUSICA VARCHAR2(60) NOT NULL,
    PONTUACAO NUMBER(10) NOT NULL,
    CONSTRAINT PK_PONTUACAO_MUSICAID PRIMARY KEY (ID),
    CONSTRAINT UK_PONTUACAO_MUSICAPERESTMUS UNIQUE (IDPERFIL, ESTILO, MUSICA),
    CONSTRAINT FK_PONTUACAO_MUSICAIDPERFIL FOREIGN KEY (IDPERFIL) REFERENCES PERFIL (ID)
);
CREATE SEQUENCE SEQ_PONTUACAO_MUSICAID;