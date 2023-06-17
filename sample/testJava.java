create or replace and compile java source named "testJava" as
public class testJava {
    public static String test(int arg1, java.lang.Date[] arg2, java.lang.String[] arg3, java.lang.String[] arg4) {
        int l_var1 = arg1;
        string l_var2 = "";
        if (null != arg3[0])
           l_var2 = java.lang.String.format("%d\t%s", l_var1, arg3);
        else
           arg3 = l_var2;
        return java.lang.String.format("%d\t%s", l_var1, arg3);
    }
}