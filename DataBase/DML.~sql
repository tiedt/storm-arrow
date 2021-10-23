-- Inserindo jogador para William Mello na base de dados
INSERT INTO PERFIL (ID, ENDERECO_MAC, NOME, PONTUACAO_TOTAL)
   VALUES (SEQ_PERFILID.NEXTVAL, '74-D0-2B-9E-1F-63', 'xXxcollazzoxXx', 0);
COMMIT;

-- Inserindo pontuação para o perfil do jogador xXxcollazzoxX, estilo Dance, música I Dont know - Erika na base de dados
INSERT INTO PONTUACAO_MUSICA (ID, IDPERFIL, ESTILO, MUSICA, PONTUACAO)
    VALUES (SEQ_PONTUACAO_MUSICAID.NEXTVAL, (SELECT ID FROM PERFIL WHERE NOME = 'xXxcollazzoxXx'), 'Dance', 'I Dont know - Erika', 0);
COMMIT;
