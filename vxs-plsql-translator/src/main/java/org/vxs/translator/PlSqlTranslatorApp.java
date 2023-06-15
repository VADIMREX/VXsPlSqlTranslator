package org.vxs.translator;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;
import java.util.Scanner;

import org.vxs.utils.IManInfo;

public class PlSqlTranslatorApp {
    static class Settings {
        public String[] args;
        public String inPath;
        public String outPath;
    }
    public static void main(String[] args )
    {
        final var settings = new Settings();
        settings.args = args;
        
        final HashMap<String,IManInfo> man = new HashMap<String,IManInfo>();
        man.putAll(Map.ofEntries(
            Map.entry("-input", new IManInfo() {
                @Override
                public String getDesctiption() { return "please enter arguments, empty line for continue"; }
                @Override
                public int action(int argNo) {
                    System.out.println("please enter arguments, empty line for continue");
                    var scan = new Scanner(System.in);
                    var lst = new ArrayList<String>();
                    int j = 0;
                    while (j < 2)
                    {
                        var s = scan.nextLine();
                        if ("" == s)
                        {
                            j++;
                            continue;
                        }
                        j = 0;
                        lst.add(s);
                    }
                    scan.close();
                    settings.args = lst.toArray(new String[0]);
                    return -1;
                }
            }),
            Map.entry("-h", new IManInfo() {
                @Override
                public String getDesctiption() { return "this text"; }
                @Override
                public int action(int argNo) {
                    man.forEach((k,v)-> {
                        System.out.println(String.format("%s\t%s", k, v.getDesctiption()));
                    });
                    return settings.args.length;
                }
            }),
            Map.entry("-i", new IManInfo() {
                @Override
                public String getDesctiption() { return "<file path> set input file"; }
                @Override
                public int action(int argNo) {
                    if (argNo + 1 == settings.args.length) return argNo;
                    settings.inPath = settings.args[++argNo];
                    return argNo;
                }
            }),
            Map.entry("-o", new IManInfo() {
                @Override
                public String getDesctiption() { return "<file path> set output file"; }
                @Override
                public int action(int argNo) {
                    if (argNo + 1 == settings.args.length) return argNo;
                    settings.outPath = settings.args[++argNo];
                    return argNo;
                }
            })
        ));
        
        for(var i = 0; i < settings.args.length; i++) {
            if (!man.containsKey(settings.args[i])) {
                System.out.println(String.format("unknown argument: %s", settings.args[i]));
                return;
            }
            i = man.get(settings.args[i]).action(i);
        }


    }
}