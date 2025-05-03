using System.Text;

public class Program
{
    static void Main(string[] args)
    {
        string original = "aaabccccdd";
        string a = "aaabbcccdde";
        Console.WriteLine(Compress(a));
        string compressed = Compress(original);
        Console.WriteLine("Compressed: " + compressed); // a3bc4d2

        string decompressed = Decompress(compressed);
        Console.WriteLine("Decompressed: " + decompressed); // aaabccccdd
    }
    public static string Compress(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "";

        var result = new StringBuilder();
        int count = 1;

        for (int i = 1; i < input.Length; i++)
        {
            if (input[i] == input[i - 1])
            {
                count++;
            }
            else
            {
                result.Append(input[i - 1]);
                if (count > 1)
                    result.Append(count);
                count = 1;
            }
        }

        result.Append(input[input.Length - 1]);
        if (count > 1)
            result.Append(count);

        return result.ToString();
    }
    public static string Decompress(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "";

        var result = new StringBuilder();
        for (int i = 0; i < input.Length;)
        {
            char currentChar = input[i];
            i++;

            string countStr = "";
            while (i < input.Length && char.IsDigit(input[i]))
            {
                countStr += input[i];
                i++;
            }

            int count = string.IsNullOrEmpty(countStr) ? 1 : int.Parse(countStr);
            result.Append(new string(currentChar, count));
        }

        return result.ToString();
    }

}