CREATE OR REPLACE PACKAGE TestPackage IS
  PROCEDURE Test(arg1 number, arg2 in DATE default sysdate, arg3 in out VARCHAR2, arg4 out varchar2);
  FUNCTION "Test"(arg1 number, arg2 in DATE, arg3 in out VARCHAR2) RETURN VARCHAR2;
END TestPackage;
/

CREATE OR REPLACE PACKAGE BODY TestPackage IS
    /*
        Package for testing
     */

    t varchar2(1 char) := chr(9);
    PROCEDURE Test(arg1 number, arg2 in DATE default sysdate, arg3 in out VARCHAR2, arg4 out varchar2) is
        l_var1 number;
        l_var2 varchar2(1000 char) := arg1 || t || arg2;
    begin
        l_var1 := -arg1 * .5f;
        if arg2 is not null then
            l_var2 := l_var2 || t || arg3;
        else
            arg3 := l_var2 || ''' string' || q'!, q' string 1!' || q'[, q' string 2]' || q'(, q' string 3)' || q'<, q' string 4>' || q'{, q' string 5}';
        end if;
        arg4 := "Test"(l_var1**2d/3., arg2, arg3);
    END Test;

    FUNCTION "Test"(arg1 number, arg2 in DATE, arg3 in out varchar2) RETURN VARCHAR2 is
        l_var1 number;
        l_var2 varchar2(1000 char) := arg1 || t || arg2;
    begin
        for i in 1..2 LOOP
          for cur in (select level lvl from dual connect by level < 2) LOOP
            l_var1 := i / 0.5 - cur.lvl * 5e-1;
          end LOOP;
        end loop;
        LOOP
          exit when true;
        end loop;
        if arg3 is not null then
            l_var2 := l_var2 || t || arg3;
        elsif false THEN 
          null;
        else
            arg3 := l_var2;
        end if;
        <<block_name>>
        declare
            l_var1 number := 2;
        begin
            return "Test".l_var1 || t || block_name.l_var1 || t || l_var2 || t || arg3;
        EXCEPTION
            when others THEN
                null;
        end;
    end "Test";
BEGIN
    null;
END TestPackage;
/