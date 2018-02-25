using System;

public class StringUtility {
    
    public static string FirstCharToUpper(string input) {
        if (String.IsNullOrEmpty(input))
            throw new ArgumentException("ARGH!");
        return input[0].ToString().ToUpper() + input.Substring(1);
    }
}
