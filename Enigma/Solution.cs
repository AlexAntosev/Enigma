using System.Text;

class Solution
{
    const int RotorCount = 3;
    const int AlphabetLength = 26;
    const string OperationEncode = "ENCODE";

    static string ApplyRotor(string input, string rotor, bool decode = false)
    {
        var sb = new StringBuilder();

        foreach (var t in input)
        {
            int index;
            if (decode)
            {
                index = rotor.IndexOf(t);
                sb.Append((char)(index + 'A'));
            }
            else
            {
                index = t - 'A';
                sb.Append(rotor[index]);
            }
        }

        return sb.ToString();
    }

    static string EncodeMessage(string message, int pseudoRandomNumber, List<string> rotors)
    {
        var shiftedMessage = ShiftMessage(message, pseudoRandomNumber, true);
        var afterFirstRotor = ApplyRotor(shiftedMessage, rotors[0]);
        var afterSecondRotor = ApplyRotor(afterFirstRotor, rotors[1]);
        return ApplyRotor(afterSecondRotor, rotors[2]);
    }

    static string DecodeMessage(string message, int pseudoRandomNumber, List<string> rotors)
    {
        var beforeThirdRotor = ApplyRotor(message, rotors[2], true);
        var beforeSecondRotor = ApplyRotor(beforeThirdRotor, rotors[1], true);
        var beforeFirstRotor = ApplyRotor(beforeSecondRotor, rotors[0], true);
        return ShiftMessage(beforeFirstRotor, pseudoRandomNumber, false);
    }

    static string ShiftMessage(string message, int shiftValue, bool shiftForward)
    {
        var shiftedMessage = new StringBuilder();

        for (var i = 0; i < message.Length; i++)
        {
            var charIndex = message[i] - 'A';
            var currentShiftValue = (shiftForward ? 1 : -1) * (i + shiftValue);
            var shiftedIndex = (charIndex + currentShiftValue + AlphabetLength * (1 + (-currentShiftValue) / AlphabetLength)) % AlphabetLength;
            var shiftedChar = (char)(shiftedIndex + 'A');
            shiftedMessage.Append(shiftedChar);
        }

        return shiftedMessage.ToString();
    }

    static void Main(string[] args)
    {
        var operation = Console.ReadLine();
        var pseudoRandomNumber = int.Parse(Console.ReadLine());
        var rotors = new List<string>();

        for (var i = 0; i < RotorCount; i++)
        {
            string rotor = Console.ReadLine();
            rotors.Add(rotor);
        }

        var message = Console.ReadLine();
        string processedMessage;

        processedMessage = operation.Equals(OperationEncode) 
            ? EncodeMessage(message, pseudoRandomNumber, rotors) 
            : DecodeMessage(message, pseudoRandomNumber, rotors);

        Console.WriteLine(processedMessage);
    }
}