CREATE OR REPLACE PACKAGE TestPackage IS
  PROCEDURE Test(arg1 number, arg2 in DATE default sysdate, arg3 in out VARCHAR2, arg4 out varchar2);
  FUNCTION "Test"(arg1 number, arg2 in DATE, arg3 in out VARCHAR2) RETURN VARCHAR2;
END TestPackage;
/

CREATE OR REPLACE PACKAGE BODY TestPackage IS
  t varchar2(1 char) := chr(9);
  PROCEDURE Test(arg1 number, arg2 in DATE default sysdate, arg3 in out VARCHAR2, arg4 out varchar2) is
    l_var1 number;
    l_var2 varchar2(1000 char) := arg1 || t || arg2;
  begin
    l_var1 := arg1 + 1;
    if arg3 is not null then
      l_var2 := l_var2 || t || arg3;
    else
      arg3 := l_var2;
    end if;
    arg4 := "Test"(l_var1, arg2, arg3);
  END;

  FUNCTION "Test"(arg1 number, arg2 in DATE, arg3 in out varchar2) RETURN VARCHAR2 is
    l_var1 number;
    l_var2 varchar2(1000 char) := arg1 || t || arg2;
  begin
    l_var1 := arg1;
    if arg3 is not null then
       l_var2 := l_var2 || t || arg3;
    else
       arg3 := l_var2;
    end if;
    return l_var2 || t || arg3;
  end "Test";

END TestPackage;
/