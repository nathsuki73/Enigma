using System;
using System.Collections.Generic;

public class MessageData
{
    public string? Name { get; set; }
    public string? Title { get; set; }
    public DateTime? Time { get; set; }
    public string? EncryptedMessage { get; set; }
    public string? DecryptedMessage { get; set; }
    public string? RotorOrderSettings { get; set; }
    public string? RotorStartingPositionSettings { get; set; }
    public string[]? Plugboard { get; set; }

}

class Enigma
{
    

    private static Dictionary<char, char> plugboardMapping = new Dictionary<char, char>();
    private static List<MessageData> historyList = new List<MessageData>();

    static string tempEncryptionString = "";
    static string tempDecryptionString = "";
    static bool history = true;

    static int SelectedMenuForView = 0;
    static int SelectedSettingForEdit = 0;
    static int SelectedDataForView = 0;
    static int SelectedSaveForEdit = 0;
    static int SelectedSlotForEdit = 0;
    static int SelectedRotorForEdit = 0;

    static int ChosenRotor1 = 0;
    static int ChosenRotor2 = 1;
    static int ChosenRotor3 = 2;

    static string[] rotor0 = { "f`s?Tynk+$K9G[|(XVU)-w2E: gl~Ne@xW%,>*^brLH\\.3O!zdhAZtqRY0\"7_5J;=M&Cv/jñ6iIuaF1#B48m'Do{]pPQ<S}c",
                               " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~ñ", "N" };

    static string[] rotor1 = { "}?#<I@py9:tK)co65/PS\"BWaFq%TCNb'lr 7(dLEnzA&$m2`u{XY\\gVjx8s=|;0]DUfe>Z!+Gk_H~,O*Q.Ri^-h13wJ4v[Mñ",
                               " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~ñ", "G" };

    static string[] rotor2 = { "}*N_)SQV>zI2vCD<`?-7x:md;J]6a3lU\"g0 #+\\{~%if.(cL^[MeR$HPkonTr8uAqBE9=&|XYsWwñpy4,bh5G1KjZF!O/'@t",
                               " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~ñ", "A" };

    static string[] rotor3 = { "/bsPh8X!*zG^V$Qd~(fiq64C>w<7cñuJ`I,OaSEg|Lr_=@WKnBRNy; v0H1}[9.)lFp:\"tA?kY#U2-T{'\\D%x53&moZj+]eM",
                               " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~ñ", "I" };

    static string[] rotor4 = { "W=U[^dt0o-*C?s(X_N1\"$.u]yKSw2A8n:P97zl!ñf;iV%Y&@q|~#a5Gc>hJmeQRg)vTO6<\\b}B4Zk,{FExj/Dr`pI'+L3HM ",
                               " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~ñ", "E" };


    static string[][] allRotors = { rotor0, rotor1, rotor2, rotor3, rotor4 };
    static string alphabet = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~ñ";

    
    static string[] rotorSlot1 = { allRotors[ChosenRotor1][0], allRotors[ChosenRotor1][1], allRotors[ChosenRotor1][2] };
    static string[] rotorSlot2 = { allRotors[ChosenRotor2][0], allRotors[ChosenRotor2][1], allRotors[ChosenRotor2][2] };
    static string[] rotorSlot3 = { allRotors[ChosenRotor3][0], allRotors[ChosenRotor3][1], allRotors[ChosenRotor3][2] };
    static string[] reflector1 = { "}*", "N_", ")S", "QV", ">z", "I2", "vC", "D<", "`?", "-7", "x:", "md", ";J", "]6", "a3", "lñ", "U\"", "g0", " #", "+\\", "{~", "%i", "f.", "(c", "L^", "[M", "eR", "$H", "Pk", "on", "Tr", "8u", "Aq", "BE", "9=", "&|", "XY", "sW", "wp", "y4", ",b", "h5", "G1", "Kj", "ZF", "!O", "/'", "@t" };

    static string NameSave = "";
    static string TitleSave = "";
    static string UsedRotors = $"{ MakeItRomanNum(ChosenRotor1) } : {MakeItRomanNum(ChosenRotor2)} : {MakeItRomanNum(ChosenRotor3)}";
    static string RotorStartingPoint = $"{GetCurrentPosition(rotorSlot1, ChosenRotor1)} : {GetCurrentPosition(rotorSlot2, ChosenRotor2)} : {GetCurrentPosition(rotorSlot3, ChosenRotor3)}";
    static string[] Plugboard = { };

    static int counter = 0;

    static void Main()
    {
        Loading();
        while (true)
        {
            ShowUi(0);
            ConsoleKeyInfo keyMenu = Console.ReadKey(true);
            if (keyMenu.Key == ConsoleKey.UpArrow || keyMenu.Key == ConsoleKey.DownArrow)
            {
                SelectedMenuForView = SelectMenu(keyMenu);
                continue;
            }
            // Encryption ================================================================
            if (keyMenu.Key == ConsoleKey.Enter && SelectedMenuForView == 0) {
                while (true) {
                    ShowUi(1);
                    ConsoleKeyInfo Key = Console.ReadKey(true);
                    if (Key.Key == ConsoleKey.Escape) break;
                    if (Key.Key == ConsoleKey.Backspace) Backspace();
                    if (Key.Key == ConsoleKey.Delete) DeleteAll();
                    if (Key.Key == ConsoleKey.S && (Key.Modifiers & ConsoleModifiers.Control) != 0) SaveData();
                    else if (Key.KeyChar >= 32 && Key.KeyChar <= 126)
                    {
                        if (counter == 0)
                        {
                            RotorStartingPoint = new string($"{GetCurrentPosition(rotorSlot1, ChosenRotor1)} : {GetCurrentPosition(rotorSlot2, ChosenRotor2)} : {GetCurrentPosition(rotorSlot3, ChosenRotor3)}");
                            counter++;
                        }
                        EncryptLetter(Key.KeyChar);
                    }
                }
            }

            // Encryption ================================================================
            // History ===================================================================
            if (keyMenu.Key == ConsoleKey.Enter && SelectedMenuForView == 1)
            {
                while (true)
                {
                    if (!history)
                    {
                        PrintInCenter("Viewing history is turned off.");
                        Console.ReadKey(true);
                        break;
                    }
                    ShowUi(2);
                    ConsoleKeyInfo Key = Console.ReadKey(true);
                    if (Key.Key == ConsoleKey.Escape) break;
                    if (Key.Key == ConsoleKey.UpArrow || Key.Key == ConsoleKey.DownArrow)
                    {
                        SelectedDataForView = SelectData(Key);
                        continue;
                    }
                    if (Key.Key == ConsoleKey.Enter && historyList.Count != 0) ViewInformation();
                    if (Key.Key == ConsoleKey.Delete && historyList.Count != 0) ConfirmDeletion();
                }
            }
            // History ===================================================================
            // Enigma Setting =======================================================================
            if (keyMenu.Key == ConsoleKey.Enter && SelectedMenuForView == 2) {
                int CurrentSelectedOpt = 0;
                
                while (true) {
                    ShowUi(3);
                    PrintCurrentSelectedConfig(CurrentSelectedOpt);
                    PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
                    PrintInCenter("║ Commands:                                                             ║");
                    PrintInCenter("║ [Esc] Back                                                            ║");
                    PrintInCenter("╚═══════════════════════════════════════════════════════════════════════╝");
                    ConsoleKeyInfo confirmKey = Console.ReadKey(true);

                    if (confirmKey.Key == ConsoleKey.UpArrow || confirmKey.Key == ConsoleKey.DownArrow)
                    {
                        CurrentSelectedOpt = Choose(confirmKey, CurrentSelectedOpt, 3);
                        continue;
                    }
                    // Rotor Order ============================================================
                    if (confirmKey.Key == ConsoleKey.Enter && CurrentSelectedOpt == 0) {
                        while (true) {
                            ShowUi(3.1);
                            ConsoleKeyInfo Key1 = Console.ReadKey(true);
                            if (Key1.Key == ConsoleKey.Escape) break;
                            if (Key1.Key == ConsoleKey.UpArrow || Key1.Key == ConsoleKey.DownArrow) {
                                SelectedSlotForEdit = SelectRotorSlot(Key1, SelectedSlotForEdit);
                                continue;
                            }
                            else if (Key1.Key == ConsoleKey.LeftArrow || Key1.Key == ConsoleKey.RightArrow) {
                                    ChangeRotorOrder(Key1);
                            }
                        }
                    }
                    // Rotor Position ============================================================
                    if (confirmKey.Key == ConsoleKey.Enter && CurrentSelectedOpt == 1)
                    {
                        while (true)
                        {
                            ShowUi(3.2);
                            ConsoleKeyInfo Key1 = Console.ReadKey(true);
                            if (Key1.Key == ConsoleKey.Escape) break;
                            if (Key1.Key == ConsoleKey.UpArrow || Key1.Key == ConsoleKey.DownArrow)
                            {
                                SelectedRotorForEdit = SelectRotorSlot(Key1, SelectedRotorForEdit);
                                continue;
                            }
                            else if (Key1.Key == ConsoleKey.LeftArrow || Key1.Key == ConsoleKey.RightArrow)
                            {
                                ChangeRotorPosition(Key1);
                            }
                        }
                        counter = 0;
                    }
                    // Plugboard Setting ============================================================
                    if (confirmKey.Key == ConsoleKey.Enter && CurrentSelectedOpt == 2) {
                        Plugboard = new string[0];
                        while (true) {
                            ShowUi(3.3);
                            ConsoleKeyInfo Key1 = Console.ReadKey(true);
                            if (Key1.Key == ConsoleKey.Escape) break;
                            ConsoleKeyInfo Key2 = Console.ReadKey(true);
                            if (Key2.Key == ConsoleKey.Escape) break;
                            else if (char.IsLetter(Key1.KeyChar) && char.IsLetter(Key2.KeyChar)) { 
                                InitializePlugboard(Key1, Key2); 
                            }
                        }
                        AddPlugboardInData();


                    }
                    
                    if (confirmKey.Key == ConsoleKey.Escape) break; // goes back to main menu
                }

            }
            // Enigma Setting =======================================================================
            // Setting ===============================================================================
            if (keyMenu.Key == ConsoleKey.Enter && SelectedMenuForView == 3) {
                while (true)
                {
                    ShowUi(4);
                    ConsoleKeyInfo Key = Console.ReadKey(true);
                    if (Key.Key == ConsoleKey.Escape) break;
                    else if (Key.Key == ConsoleKey.LeftArrow || Key.Key == ConsoleKey.RightArrow)
                    {
                        ChangeSettingStatus();
                    }
                }
            }
            // Setting ===============================================================================
            // About ===============================================================================
            if (keyMenu.Key == ConsoleKey.Enter && SelectedMenuForView == 4)
            {
                ShowUi(5);
            }
            // About ===============================================================================
            // Exit =================================================================================
            if (keyMenu.Key == ConsoleKey.Enter && SelectedMenuForView == 5) {
                int CurrentSelectedOpt = 0;
                while (true) {
                    Console.Clear();
                    PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
                    PrintInCenter("║  Are your sure you want to exit?                                      ║");
                    PrintInCenter("╚═══════════════════════════════════════════════════════════════════════╝");
                    
                    PrintCurrentSelectedOpt(CurrentSelectedOpt);
                    ConsoleKeyInfo confirmKey = Console.ReadKey(true);
                    
                    if (confirmKey.Key == ConsoleKey.UpArrow || confirmKey.Key == ConsoleKey.DownArrow)
                    {
                        CurrentSelectedOpt = Choose(confirmKey, CurrentSelectedOpt, 2);
                        continue;
                    }
                    if (confirmKey.KeyChar == 'Y' || confirmKey.KeyChar == 'y' || (confirmKey.Key == ConsoleKey.Enter && CurrentSelectedOpt == 0)) {
                        Console.Clear();
                        Environment.Exit(0); // Exit the application
                    }
                    else if (confirmKey.KeyChar == 'N' || confirmKey.KeyChar == 'n' || (confirmKey.Key == ConsoleKey.Enter && CurrentSelectedOpt == 1)) {
                        // Continue the outer loop
                        break;
                    }
                    else { continue; }
                }
            }
            // Exit =================================================================================


        }
    }





    // Menu ================================================================
    static int SelectMenu(ConsoleKeyInfo key) {
        int numberOfMenu = 6;
        int step = (key.Key == ConsoleKey.UpArrow) ? -1 : 1;
        int n = (SelectedMenuForView + step + numberOfMenu) % numberOfMenu;
        return n;
    }

    static void PrintCurrentSelectedMenu() {
        string[] slots = { "║                         Encryption/Decryption                         ║",
                           "║                                History                                ║",
                           "║                             Configuration                             ║",
                           "║                                Setting                                ║",
                           "║                                 About                                 ║",
                           "║                                 Exit                                  ║" };
        int windowWidth = Console.WindowWidth;


        for (int i = 0; i < slots.Length; i++)
        {
            if (SelectedMenuForView == i) Console.ForegroundColor = ConsoleColor.Green;
            int leftPadding = (windowWidth - slots[i].Length) / 2;
            Console.WriteLine("".PadLeft(leftPadding) + "╔═══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("".PadLeft(leftPadding) + slots[i]);
            Console.WriteLine("".PadLeft(leftPadding) + "╚═══════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }
    }
    // Menu ================================================================









    // Loading ==================================================================
    static void Loading()
    {
        
        string[] rotor = { "███╗   ███╗  █████╗ ███████╗ ███╗   ██╗ ██╗  ██████╗  ",
                           "██║ ██╔════╝  ████╗ ████║ ██╔══██╗██╔════╝ ████╗  ██║ ",
                           "██╔████╔██║ ███████║█████╗   ██╔██╗ ██║ ██║ ██║  ███╗ ",
                           "██║ ██║   ██║ ██║╚██╔╝██║ ██╔══██║██╔══╝   ██║╚██╗██║ ",
                           "██║ ╚═╝ ██║ ██║  ██║███████╗ ██║ ╚████║ ██║ ╚██████╔╝ ",
                           "╚═╝  ╚═════╝  ╚═╝     ╚═╝ ╚═╝  ╚═╝╚══════╝ ╚═╝  ╚═══╝ "};
        string[] blocks = { "║ ", "   ", "   ", "   ", "   ", "   ", "   ", "   ", "   ", "   ", "   ", "   ", "   ", "   ", "   ", "   ", "   ", "   ", "   ", "   ", "   ", "║" };

        for (int i = 1; i <= 20; i++)
        {
            Console.Clear();
            PrintInCenter("═════════════════════════════════════════════════════════════");
            rotor[0] = rotor[0][1..] + rotor[0][0];
            rotor[1] = rotor[1][^1] + rotor[1][0..^1];
            rotor[2] = rotor[2][1..] + rotor[2][0];
            rotor[3] = rotor[3][^1] + rotor[3][0..^1];
            rotor[4] = rotor[4][1..] + rotor[4][0];
            rotor[5] = rotor[5][^1] + rotor[5][0..^1];
            
            PrintInCenter(rotor[0]);
            PrintInCenter(rotor[1]);
            PrintInCenter(rotor[2]);
            PrintInCenter(rotor[3]);
            PrintInCenter(rotor[4]);
            PrintInCenter(rotor[5]);
            Console.ForegroundColor = ConsoleColor.Green;
            PrintInCenter("╔═════════════════════════════════════════════════════════════╗");
            for (int j = i; j != 0; j--)
            {
                blocks[j] = "██ ";
            }
            PrintInCenter(string.Join("", blocks));

            PrintInCenter("╚═════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            PrintInCenter("═════════════════════════════════════════════════════════════");
            Thread.Sleep(250);
        }

        PrintInCenter("Press any key to start...");



        Console.ResetColor();


        Console.ReadKey(true);
        int c = 0;
        string pass = "123456";
        string temp = "";
        
        while (true)
        {

            Console.Clear();
            bool CorrectPass = false;
            CorrectPass = (temp == pass) ? true : false;
            if (c == 6) {
                
                if (CorrectPass)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else Console.ForegroundColor = ConsoleColor.Red;
            }
            
            string[] loginBlocks = { "║ ", "   ", "   ", "   ", "   ", "   ", "   ", "║" };
            PrintInCenter("═════════════════════════════════════════════════════════════");
            PrintInCenter(rotor[0]);
            PrintInCenter(rotor[1]);
            PrintInCenter(rotor[2]);
            PrintInCenter(rotor[3]);
            PrintInCenter(rotor[4]);
            PrintInCenter(rotor[5]);
            PrintInCenter("═════════════════════════════════════════════════════════════");
            PrintInCenter("╔═══════════════════╗");
            if (c != 0) {
                for (int i = c; i != 0; i--)
                {

                    loginBlocks[i] = "██ ";
                }
            }
            PrintInCenter(string.Join("", loginBlocks));
            PrintInCenter("╚═══════════════════╝");
            
            if (c == 6)
            {
                if (CorrectPass)
                {
                    PrintInCenter("Correct passkey. ");
                    Thread.Sleep(2000);
                    Console.ResetColor();
                    break;
                }
                else
                {
                    PrintInCenter("Incorrect passkey. Please try again");
                    Thread.Sleep(1000);
                    Console.ResetColor();
                    c = 0;
                    temp = "";
                    continue;
                }
            }
            else PrintInCenter("Enter a passkey.");
            ConsoleKeyInfo Key = Console.ReadKey(true);
            if (Key.Key == ConsoleKey.Backspace)
            {
                if (temp.Length == 0) continue;
                temp = temp.Substring(0, temp.Length - 1);
                c--;
            }
            if (Key.KeyChar >= 32 && Key.KeyChar <= 126) {
                temp += Key.KeyChar;
                c++;
            }
            
        }
       

    }
    static void PrintRotorRotation() {
        PrintInCenter($"< {GetCurrentPosition(rotorSlot1, ChosenRotor1)} > < {GetCurrentPosition(rotorSlot2, ChosenRotor2)} > < {GetCurrentPosition(rotorSlot3, ChosenRotor3)} >");
    }
    static void SaveData() {
        while (true)
        {
            if (!history)
            {
                PrintInCenter("Saving history is turned off.");
                Console.ReadKey(true);
                break;
            }
            ShowUi(1.1);
            ConsoleKeyInfo Key = Console.ReadKey(true);
            if (Key.Key == ConsoleKey.Backspace) {
                if (!string.IsNullOrEmpty(NameSave) && SelectedSaveForEdit == 0){
                    NameSave = NameSave[..^1];
                }
                if (!string.IsNullOrEmpty(TitleSave) && SelectedSaveForEdit == 1)
                {
                    TitleSave = TitleSave[..^1];
                }
            }
            if (IsWithinAsciiRange(Key.KeyChar)) {
                EditNameOrTitle(Key.KeyChar);
            }
            else if (Key.Key == ConsoleKey.Escape) break;
            else if (Key.Key == ConsoleKey.UpArrow || Key.Key == ConsoleKey.DownArrow) {
                SelectedSaveForEdit = (SelectedSaveForEdit == 0) ? 1 : 0;
            }
            else if (Key.Key == ConsoleKey.Enter) {
                ContinueSave();
                ShowUi(1.2);
                Console.ReadKey();
                NameSave = "";
                TitleSave = "";
                break;
                
            }
        }
    }

    static void EditNameOrTitle(char c) {
        if (SelectedSaveForEdit == 0) {
            NameSave += c;
        }
        else {
            TitleSave += c;
        }
    }
    static bool IsWithinAsciiRange(char c)
    {
        return c >= 32 && c <= 126;
    }

    static void ContinueSave() {
        MessageData data = new MessageData();
        data.Name = NameSave;
        data.Title = TitleSave;
        data.Time = DateTime.Now;
        data.EncryptedMessage = tempEncryptionString;
        data.DecryptedMessage = tempDecryptionString;
        data.RotorOrderSettings = UsedRotors;
        data.RotorStartingPositionSettings = RotorStartingPoint;
        data.Plugboard = Plugboard;
        historyList.Add(data);

    }
    static void PrintCurrentSelectedTitleOrName() {
        string[] slots = { $"{NameSave}", $"{TitleSave}" };

        int windowWidth = Console.WindowWidth;

        for (int i = 0; i < slots.Length; i++)
        {
            string s = (i == 0) ? "Name:                                                               " : "Title:                                                              ";
            if (SelectedSaveForEdit == i) Console.ForegroundColor = ConsoleColor.Green;
            PrintInCenter(s);
            int leftPadding = (windowWidth - 5) / 2;
            Console.WriteLine("".PadLeft(leftPadding) + slots[i]);
            Console.ResetColor();
        }
       


    }



    static void Backspace() {
        if (!string.IsNullOrEmpty(tempEncryptionString))
        {
            tempEncryptionString = tempEncryptionString[..^1];
            tempDecryptionString = tempDecryptionString[..^1];
            ReverseRotate();
        }

    }

    static void DeleteAll() {
        ResetRotorSlots();
    }




    static void EncryptLetter(char Key) {
        tempEncryptionString += Key;
        char NewChar = RunInPlugboard(Key);
        Rotate();
        NewChar = RunInRotorsAndReflector(NewChar);
        NewChar = RunInPlugboard(NewChar);
        tempDecryptionString += NewChar;
    }

  

    static char RunInRotorsAndReflector(char letter) {

        int index = alphabet.IndexOf(letter);
        index = rotor(index, rotorSlot1);
        index = rotor(index, rotorSlot2);
        index = rotor(index, rotorSlot3);
        
        index = reflector(index);
        index = rotorReversed(index, rotorSlot3);
        index = rotorReversed(index, rotorSlot2);
        index = rotorReversed(index, rotorSlot1);

        char NewLetter = alphabet[index];
        return NewLetter;
    }

    static int rotorReversed(int index, string[] rotor) {
        char letter = rotor[1][index];
        int Newindex = rotor[0].IndexOf(letter);
        return Newindex;
    
    }

    static int reflector(int index) {
        string tempReflector = string.Join("", reflector1);
        char letter = tempReflector[index];
        int Newindex = 0;
        foreach (string twoChar in reflector1) {
            foreach (char Char in twoChar) {
                if (Char == letter) {
                    char newLetter = (Char == twoChar[0]) ? twoChar[1]:twoChar[0];
                    Newindex = tempReflector.IndexOf(newLetter);
                }
            }
        }
        return Newindex;
        
    }


    static int rotor(int index, string[] rotor) {
        char letter = rotor[0][index];
        int Newindex = rotor[1].IndexOf(letter);
        return Newindex;
    }
    

    static void Rotate() {
        RotateRotor(rotorSlot1);
        if (rotorSlot1[0][0] == rotorSlot1[2][0])
        {
            RotateRotor(rotorSlot2);
            if (rotorSlot2[0][0] == rotorSlot2[2][0])
            {
                RotateRotor(rotorSlot3);
            }
        }
    }

    static void ReverseRotate()
    {
        ReverseRotateRotor(rotorSlot1);
        if (rotorSlot1[0][95] == rotorSlot1[2][0])
        {
            ReverseRotateRotor(rotorSlot2);
            if (rotorSlot2[0][95] == rotorSlot2[2][0])
            {
                ReverseRotateRotor(rotorSlot3);
            }
        }
    }

    static void RotateRotor(string[] rotor) {
        rotor[0] = rotor[0][^1] + rotor[0][0..^1];
        rotor[1] = rotor[1][^1] + rotor[1][0..^1];
    }
    static char RunInPlugboard(char Key) {
        if (plugboardMapping.TryGetValue(Key, out char mappedChar))
        {
            return mappedChar;
        }
        else
        {
            return Key;
        }
    }
    // Encrypt ================================================================

    // History -===============================================================

    static void ConfirmDeletion() {
        int selectedOpt = 0;
        while (true)
        {

            Console.Clear();

            PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
            PrintInCenter("║ Are you sure you want to delete this information?                     ║");
            PrintInCenter("╚═══════════════════════════════════════════════════════════════════════╝");
            PrintCurrentSelectedOpt(selectedOpt);
            ConsoleKeyInfo confirmKey = Console.ReadKey(true);

            if (confirmKey.Key == ConsoleKey.UpArrow || confirmKey.Key == ConsoleKey.DownArrow)
            {
                selectedOpt = Choose(confirmKey, selectedOpt, 2);
                continue;
            }
            if (confirmKey.Key == ConsoleKey.Enter && selectedOpt == 0)
            {
                historyList.RemoveAt(SelectedDataForView);
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
                PrintInCenter("║                          Information deleted.                         ║");
                PrintInCenter("╚═══════════════════════════════════════════════════════════════════════╝");
                Console.ResetColor();
                Console.ReadKey(true);
                break;
            }
            else if(confirmKey.Key == ConsoleKey.Enter && selectedOpt == 1) break;
        }
    }
    static void ViewInformation() {
        Console.Clear();
        MessageData data = historyList[SelectedDataForView];
        PrintInCenter($"Name: {data.Name}");
        PrintInCenter($"Title: {data.Title}");
        PrintInCenter($"Time: {data.Time}");
        PrintInCenter($"Encrypted Message: {data.EncryptedMessage}", 64);
        PrintInCenter($"Decrypted Message: {data.DecryptedMessage}", 64);
        PrintInCenter($"Rotor Order Settings: {data.RotorOrderSettings}");
        PrintInCenter($"Rotor Starting Position Settings: {data.RotorStartingPositionSettings}");
        if (data.Plugboard != null)
        {
            PrintInCenter("Plugboard:");
            foreach (var plug in data.Plugboard)
            {
                PrintInCenter(plug);
            }
        }
        PrintInCenter("Press any key to continue...");
        Console.ReadKey(true);
    }
    static void PrintCurrentSelectedData() 
    {
        int counter = 0;
        int maxWidth = 70; 
        int i = 0;
        int minSpaces = 5; 
        int maxNameLength = 13; 
        int maxTitleLength = 20; 

        foreach (var item in historyList)
        {
            counter++;
            if (SelectedDataForView == i) Console.ForegroundColor = ConsoleColor.Green;

            string truncatedName = !string.IsNullOrEmpty(item?.Name) ? (item.Name.Length > maxNameLength ? item.Name.Substring(0, maxNameLength - 3) + "..." : item.Name) : string.Empty; // Return an empty string if item.Name is null or empty


            string truncatedTitle = !string.IsNullOrEmpty(item?.Title) ? (item.Title.Length > maxTitleLength ? item.Title.Substring(0, maxTitleLength - 3) + "..." : item.Title) : string.Empty; // Return an empty string if item.Title is null or empty


            int timeLength = (item?.Time?.ToString()?.Length) ?? 0;
            int availableSpaceForTitle = maxWidth - 14 - maxNameLength - minSpaces - timeLength; 

            string line = $"{counter,-2}. {truncatedName.PadRight(maxNameLength + minSpaces)}" +
                          $"{truncatedTitle.PadRight(maxTitleLength + minSpaces)}{item?.Time?.ToString() ?? "N/A",-20}"; 

            if (line.Length > maxWidth)
            {
                int diff = line.Length - maxWidth;
                int trimIndex = truncatedName.Length - diff;
                if (trimIndex > 0)
                {
                    truncatedName = truncatedName.Substring(0, trimIndex - 3) + "...";
                    line = $"{counter,-2}. {truncatedName.PadRight(maxNameLength + minSpaces)}" +
                           $"{truncatedTitle.PadRight(maxTitleLength + minSpaces)}{item?.Time?.ToString() ?? "N/A",-20}";
                }
                else
                {
                    line = line.Substring(0, maxWidth);
                }
            }

            int leftPadding = (Console.WindowWidth - maxWidth) / 2;

            Console.WriteLine("".PadLeft(leftPadding) + line);
            Console.ResetColor();
            i++;
        }
    }




    static int SelectData(ConsoleKeyInfo key) {
        int numberOfElements = historyList.Count;
        int step = (key.Key == ConsoleKey.UpArrow) ? -1 : 1;
        int n = (SelectedDataForView + step + numberOfElements) % numberOfElements;
        return n;
    }
    // History -===============================================================

    // Enigma Config ==================================================================
    static void PrintCurrentSelectedConfig(int selectedOpt)
    {
        string[] slots = { "║                              Rotor Order                              ║",
                           "║                            Rotor Position                             ║",
                           "║                              Plugboard                                ║",};
        int windowWidth = Console.WindowWidth;


        for (int i = 0; i < slots.Length; i++)
        {
            if (selectedOpt == i) Console.ForegroundColor = ConsoleColor.Green;
            int leftPadding = (windowWidth - slots[i].Length) / 2;
            Console.WriteLine("".PadLeft(leftPadding) + "╔═══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("".PadLeft(leftPadding) + slots[i]);
            Console.WriteLine("".PadLeft(leftPadding) + "╚═══════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

    }

    // Enigma Config ==================================================================


    //Rotor Slot Order Setting ================================================================
    static void ChangeRotorOrder(ConsoleKeyInfo key) {
        int[] chosenRotors = { ChosenRotor1, ChosenRotor2, ChosenRotor3 };

        if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.RightArrow)
        {
            int step = (key.Key == ConsoleKey.LeftArrow) ? -1 : 1;

            chosenRotors[SelectedSlotForEdit] = (chosenRotors[SelectedSlotForEdit] + step + 5) % 5;

            ChosenRotor1 = chosenRotors[0];
            ChosenRotor2 = chosenRotors[1];
            ChosenRotor3 = chosenRotors[2];
        }
        ResetRotorSlots();
    }

   

    static int SelectRotorSlot(ConsoleKeyInfo key, int CurrentSlot) {
        int slot = CurrentSlot;
        if (key.Key == ConsoleKey.UpArrow){
            slot = (slot == 0) ? 2 : slot - 1;
        }
        else if (key.Key == ConsoleKey.DownArrow){
            slot = (slot == 2) ? 0 : slot + 1;
        }
        return slot;
    }

    

    static void PrintCurrentSelectedRotor() {
        string[] slots = { $"< { MakeItRomanNum(ChosenRotor1) } >", $"< {MakeItRomanNum(ChosenRotor2)} >", $"< {MakeItRomanNum(ChosenRotor3)} >" };

        int windowWidth = Console.WindowWidth;


        for (int i = 0; i < slots.Length; i++){
            if (SelectedSlotForEdit == i) Console.ForegroundColor = ConsoleColor.Green;
            int leftPadding = (windowWidth - 5) / 2;
            Console.WriteLine("".PadLeft(leftPadding) + slots[i]);
            Console.ResetColor();
        }


    }

    static string MakeItRomanNum(int i) {
        string[] romans = { "I  ", "II ", "III", "IV ", "V  ", };
        return romans[i];
    }



    //Rotor Slot Order Setting ================================================================

    //Rotor Starting Position Setting ================================================================
    static void ChangeRotorPosition(ConsoleKeyInfo key) {
        string[][] rotorSlot = { rotorSlot1, rotorSlot2, rotorSlot3 };

        if (key.Key == ConsoleKey.LeftArrow)
        {
            RotateRotor(rotorSlot[SelectedRotorForEdit]);
        }
        else {
            ReverseRotateRotor(rotorSlot[SelectedRotorForEdit]);
        }
        
    }

    static void ReverseRotateRotor(string[] rotor)
    {
        rotor[0] = rotor[0][1..] + rotor[0][0];
        rotor[1] = rotor[1][1..] + rotor[1][0];
    }
    static void PrintCurrentSelectedRotorPosition() {
        string[] slots = { $"< {GetCurrentPosition(rotorSlot1, ChosenRotor1)} >", $"< {GetCurrentPosition(rotorSlot2, ChosenRotor2)} >", $"< {GetCurrentPosition(rotorSlot3, ChosenRotor3)} >" };

        int windowWidth = Console.WindowWidth;


        for (int i = 0; i < slots.Length; i++)
        {
            if (SelectedRotorForEdit == i) Console.ForegroundColor = ConsoleColor.Green;
            int leftPadding = (windowWidth - 5) / 2;
            Console.WriteLine("".PadLeft(leftPadding) + slots[i]);
            Console.ResetColor();
        }
    }

    static string GetCurrentPosition(string[] rotorSlot, int chosenRotor)
    {
        char firstLetter = rotorSlot[1][0];
        string num = "";
        foreach (char item in allRotors[chosenRotor][0])
        {
            if (item == firstLetter)
            {
                num += $"{item}";
                num += (item < 10) ? " " : "";
                break;
            }

        }
        return num;
    }

    

    //Rotor Starting Position Setting ================================================================

    //Plugboard Setting ================================================================
    static void InitializePlugboard(ConsoleKeyInfo keyInfo, ConsoleKeyInfo keyInfo2)
    {               
        char letter1 = char.ToUpper(keyInfo.KeyChar);
        char letter2 = char.ToUpper(keyInfo2.KeyChar);

        AddPlug(letter1, letter2);
        
    }

    static void AddPlug(char letter1, char letter2)
    {
        bool containsLetter1 = plugboardMapping.ContainsKey(letter1);
        bool containsLetter2 = plugboardMapping.ContainsKey(letter2);

        if (containsLetter1 && containsLetter2)
        {
            plugboardMapping.Remove(letter1);
            plugboardMapping.Remove(letter2);
            PrintInCenter($"Both letters '{letter1}' and '{letter2}' are already connected. Connection removed.");
            Console.ReadKey();
        }
        else if (containsLetter1 || containsLetter2)
        {
            PrintInCenter($"Only one of the letters '{letter1}' and '{letter2}' is connected. Please connect both or remove the existing connection.");
            Console.ReadKey();
        }
        else if (letter1 == letter2) {
            PrintInCenter("If you want to connect the letter to itself, you don't have to input anything.");
            Console.ReadKey();
        }
        else
        {
            plugboardMapping[letter1] = letter2;
            plugboardMapping[letter2] = letter1;
            PrintInCenter($"Connection added: '{letter1}' <-> '{letter2}'");
            Console.ReadKey();
        }
    }

    static void PrintPlugboardSettings()
    {
        PrintInCenter("Current Plugboard Settings:                                         ");
        HashSet<string> printedConnections = new HashSet<string>();

        foreach (var pair in plugboardMapping)
        {
            string connection = $"{pair.Key} = {pair.Value}";
            string connectionReversed = $"{pair.Value} = {pair.Key}";

            if (!printedConnections.Contains(connectionReversed))
            {
                printedConnections.Add(connection);
                PrintInCenter(connection);
            }
        }

    }

    static void AddPlugboardInData()
    {
        HashSet<string> printedConnections = new HashSet<string>();

        foreach (var pair in plugboardMapping)
        {
            string connection = $"{pair.Key} = {pair.Value}";
            string connectionReversed = $"{pair.Value} = {pair.Key}";

            if (!printedConnections.Contains(connectionReversed))
            {
                printedConnections.Add(connection);
                Plugboard = Plugboard.Concat(new string[] { connection }).ToArray();
            }
        }

    }

    static void PrintInCenterAllPlugboards() {
        foreach (string connection in Plugboard)
        {
            PrintInCenter(connection);
        }
    }
    // Plugboard Setting ====================================================================

    // Setting ===============================================================================

    static int SelectSetting(ConsoleKeyInfo key)
    {
        int numberOfSetting = 1;
        int step = (key.Key == ConsoleKey.UpArrow) ? -1 : 1;
        int n = (SelectedSettingForEdit + step + numberOfSetting) % numberOfSetting;
        return n;
    }

    static void ChangeSettingStatus() {
        switch (SelectedSettingForEdit)
        {
            case 0:
                history = !history;
                break;
            default:
                break;
        }


    }

    static void PrintCurrentSelectedSetting() {
        string[] status = { (history) ? "on" : "off"};
        string[] slots = {$"History                                                 < {status[0]} >",};

        int windowWidth = Console.WindowWidth;


        for (int i = 0; i < slots.Length; i++)
        {                                                                                                   
            if (history) Console.ForegroundColor = ConsoleColor.Green;
            if (!history) Console.ForegroundColor = ConsoleColor.Red;
            int leftPadding = (status[i] == "on") ? (windowWidth - (slots[i].Length + 1)) / 2:(windowWidth - slots[i].Length) / 2;
            Console.WriteLine("".PadLeft(leftPadding) + slots[i]);
            Console.ResetColor();
        }
    }


    // Setting ===============================================================================



    static int Choose(ConsoleKeyInfo key, int current, int total) {
        int step = (key.Key == ConsoleKey.UpArrow) ? -1 : 1;
        int n = (current + step + total) % total;
        return n;
    }

    static void PrintCurrentSelectedOpt(int selectedOpt) {
        string[] slots = { "║        Yes        ║",
                           "║        No         ║",};
        int windowWidth = Console.WindowWidth;


        for (int i = 0; i < slots.Length; i++)
        {
            if (selectedOpt == i) Console.ForegroundColor = ConsoleColor.Green;
            int leftPadding = (windowWidth - slots[i].Length) / 2;
            Console.WriteLine("".PadLeft(leftPadding) + "╔═══════════════════╗");
            Console.WriteLine("".PadLeft(leftPadding) + slots[i]);
            Console.WriteLine("".PadLeft(leftPadding) + "╚═══════════════════╝");
            Console.ResetColor();
        }
    }





    static void PrintInCenter(string message) {
        int windowWidth = Console.WindowWidth;

        // Calculate the number of spaces needed to center the message
        int leftPadding = (windowWidth - message.Length) / 2;

        // Print spaces and then the message to center it
        Console.WriteLine("".PadLeft(leftPadding) + message);
    }

    static void PrintInCenter(string message, int newLine)
    {
        int windowWidth = Console.WindowWidth;

        if (message.Length > newLine)
        {
            string chunk = message.Substring(0, newLine);
            int length = chunk.Length;
            int leftPadding = (windowWidth - length) / 2;
            Console.WriteLine("".PadLeft(leftPadding) + chunk);
            PrintInCenter(message.Substring(newLine), newLine);
        }
        else
        {
            int length = message.Length;
            int leftPadding = (windowWidth - length) / 2;
            Console.WriteLine("".PadLeft(leftPadding) + message);
        }
    }

    static void ResetRotorSlots()
    {
        rotorSlot1 = new string[] { allRotors[ChosenRotor1][0], allRotors[ChosenRotor1][1], allRotors[ChosenRotor1][2] };
        rotorSlot2 = new string[] { allRotors[ChosenRotor2][0], allRotors[ChosenRotor2][1], allRotors[ChosenRotor2][2] };
        rotorSlot3 = new string[] { allRotors[ChosenRotor3][0], allRotors[ChosenRotor3][1], allRotors[ChosenRotor3][2] };
        tempEncryptionString = new string ("");
        tempDecryptionString = new string ("");
        NameSave = new string ("");
        TitleSave = new string ("");
        UsedRotors = new string ($"{MakeItRomanNum(ChosenRotor1)} : {MakeItRomanNum(ChosenRotor2)} : {MakeItRomanNum(ChosenRotor3)}");
        counter = 0;

    }

    static void ShowUi(double window) {
        if (window == 0) {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
            PrintInCenter("║         ███████╗ ███╗   ██╗ ██╗  ██████╗  ███╗   ███╗  █████╗         ║");
            PrintInCenter("║         ██╔════╝ ████╗  ██║ ██║ ██╔════╝  ████╗ ████║ ██╔══██╗        ║");
            PrintInCenter("║         █████╗   ██╔██╗ ██║ ██║ ██║  ███╗ ██╔████╔██║ ███████║        ║");
            PrintInCenter("║         ██╔══╝   ██║╚██╗██║ ██║ ██║   ██║ ██║╚██╔╝██║ ██╔══██║        ║");
            PrintInCenter("║         ███████╗ ██║ ╚████║ ██║ ╚██████╔╝ ██║ ╚═╝ ██║ ██║  ██║        ║");
            PrintInCenter("║         ╚══════╝ ╚═╝  ╚═══╝ ╚═╝  ╚═════╝  ╚═╝     ╚═╝ ╚═╝  ╚═╝        ║");
            PrintInCenter("╚═══════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            PrintInCenter("This console app is inspired by the Enigma machine,");
            PrintInCenter("a historic device used for encoding and decoding secret");
            PrintInCenter("messages during World War II.");
            PrintCurrentSelectedMenu();
        }
        if (window == 1)
        {
            Console.Clear();
            PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
            PrintInCenter("║                         Encryption & Decryption                       ║");
            PrintInCenter("╠═══════════════════════════════════════════════════════════════════════╣");
            PrintInCenter("║ Info: Enter a message to encrypt/decrypt.                             ║");
            PrintInCenter("╚═══════════════════════════════════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.Green;
            PrintRotorRotation();
            Console.ResetColor();
            PrintInCenter("Message:                                                            ");
            Console.ForegroundColor = ConsoleColor.Green;
            PrintInCenter(tempEncryptionString, 64);
            Console.ResetColor();
            PrintInCenter("Output:                                                             ");
            Console.ForegroundColor = ConsoleColor.Green;
            PrintInCenter(tempDecryptionString, 64);
            Console.ResetColor();

            PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
            PrintInCenter("║ Commands:                                                             ║");
            PrintInCenter("║ [Esc] Continue                                                        ║");
            PrintInCenter("║ [Ctrl + S] Save                                                       ║");
            PrintInCenter("║ [Del] Reset                                                           ║");
            PrintInCenter("╚═══════════════════════════════════════════════════════════════════════╝");
        }
        if (window == 1.1)
        {
            Console.Clear();
            PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
            PrintInCenter("║                            Save Information                           ║");
            PrintInCenter("╚═══════════════════════════════════════════════════════════════════════╝");
            PrintCurrentSelectedTitleOrName();
            PrintInCenter("Message:                                                            ");
            Console.ForegroundColor = ConsoleColor.Green;
            PrintInCenter(tempEncryptionString, 64);
            Console.ResetColor();
            PrintInCenter("Encrypted Message:                                                  ");
            Console.ForegroundColor = ConsoleColor.Green;
            PrintInCenter(tempDecryptionString, 64);
            Console.ResetColor();
            PrintInCenter("Rotors Used:                                                        ");
            Console.ForegroundColor = ConsoleColor.Green;
            PrintInCenter(UsedRotors);
            Console.ResetColor();
            PrintInCenter("Rotors Starting Point:                                              ");
            Console.ForegroundColor = ConsoleColor.Green;
            PrintInCenter(RotorStartingPoint);
            Console.ResetColor();
            PrintInCenter("Plugboards:                                                         ");
            Console.ForegroundColor = ConsoleColor.Green;
            PrintInCenterAllPlugboards();
            Console.ResetColor();
            PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
            PrintInCenter("║ Commands:                                                             ║");
            PrintInCenter("║ [Esc] Continue                                                        ║");
            PrintInCenter("║ [Enter] Save                                                          ║");
            PrintInCenter("╚═══════════════════════════════════════════════════════════════════════╝");
        }
        if (window == 1.2)
        {
            Console.Clear();

            PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
            PrintInCenter("║                           Information saved.                          ║");
            PrintInCenter("╚═══════════════════════════════════════════════════════════════════════╝");
            PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
            PrintInCenter("║ Commands:                                                             ║");
            PrintInCenter("║ [Enter] Continue                                                      ║");
            PrintInCenter("╚═══════════════════════════════════════════════════════════════════════╝");
        }

        if (window == 2)
        {
            Console.Clear();

            PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
            PrintInCenter("║                                History                                ║");
            PrintInCenter("╚═══════════════════════════════════════════════════════════════════════╝");
            PrintCurrentSelectedData();
            PrintInCenter("");
            PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
            PrintInCenter("║ Commands:                                                             ║");
            PrintInCenter("║ [Enter] View                                                          ║");
            PrintInCenter("║ [Del] Remove                                                          ║");
            PrintInCenter("║ [Esc] Back                                                            ║");
            PrintInCenter("╚═══════════════════════════════════════════════════════════════════════╝");
        }

        if (window == 3) {
            Console.Clear();
            PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
            PrintInCenter("║                          Enigma Configuration                         ║");
            PrintInCenter("╠═══════════════════════════════════════════════════════════════════════╣");
            PrintInCenter("║ Info: This is the setting for enigma where you can modify the         ║");
            PrintInCenter("║       arrangement of rotors, initial position, and plugboard          ║");
            PrintInCenter("║       connections.                                                    ║");
            PrintInCenter("╚═══════════════════════════════════════════════════════════════════════╝");

        }

        if (window == 3.1)
        {
            Console.Clear();
            PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
            PrintInCenter("║                              Rotor Order                              ║");
            PrintInCenter("╠═══════════════════════════════════════════════════════════════════════╣");
            PrintInCenter("║ Info: Use arrow buttons to modify the Current Rotors.                 ║");
            PrintInCenter("╠══════════════════╦═════════════════════════╦══════════════════════════╝");
            PrintInCenter("║ Available Rotors:║ [I] [II] [III] [IV] [V] ║                           ");
            PrintInCenter("╠══════════════════╬═════════════════════════╝                           ");
            PrintInCenter("║ Current Position:║                                                      ");
            PrintInCenter("╚══════════════════╝                                                      ");
            PrintCurrentSelectedRotor();
            PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
            PrintInCenter("║ Commands:                                                             ║");
            PrintInCenter("║ [Esc] Back                                                            ║");
            PrintInCenter("╚═══════════════════════════════════════════════════════════════════════╝");
        }

        if (window == 3.2)
        {
            Console.Clear();
            
            PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
            PrintInCenter("║                             Rotor Position                            ║");
            PrintInCenter("╠═══════════════════════════════════════════════════════════════════════╣");
            PrintInCenter("║ Info: Use arrow buttons to modify the starting position of rotors.    ║");
            PrintInCenter("╠═════════════════╦═════════════════════════════════════════════════════╝");
            PrintInCenter("║Current Position:║                                                      ");
            PrintInCenter("╚═════════════════╝                                                      ");
            PrintCurrentSelectedRotorPosition();
            PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
            PrintInCenter("║ Commands:                                                             ║");
            PrintInCenter("║ [Esc] Back                                                            ║");
            PrintInCenter("╚═══════════════════════════════════════════════════════════════════════╝");
        }
        if (window == 3.3)
        {
            Console.Clear();
            PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
            PrintInCenter("║                           Plugboard Settings                          ║");
            PrintInCenter("╠═══════════════════════════════════════════════════════════════════════╣");
            PrintInCenter("║ Info: Input two letters to connect them.                              ║");
            PrintInCenter("╚═══════════════════════════════════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.Green;
            PrintPlugboardSettings();
            Console.ResetColor();
            PrintInCenter("                                                                    ");
            PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
            PrintInCenter("║ Commands:                                                             ║");
            PrintInCenter("║ [Esc] Back                                                            ║");
            PrintInCenter("╚═══════════════════════════════════════════════════════════════════════╝");
        }
        if (window == 4)
        {
            Console.Clear();
            PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
            PrintInCenter("║                                Settings                               ║");
            PrintInCenter("╚═══════════════════════════════════════════════════════════════════════╝");
            PrintInCenter("General Settings:                                                    ");
            PrintCurrentSelectedSetting();
            Console.ResetColor();
            PrintInCenter("                                                                    ");
            PrintInCenter("╔═══════════════════════════════════════════════════════════════════════╗");
            PrintInCenter("║ Commands:                                                             ║");
            PrintInCenter("║ [Esc] Back                                                            ║");
            PrintInCenter("╚═══════════════════════════════════════════════════════════════════════╝");
        }
        if (window == 5)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("-------------------------");
            Console.WriteLine("|   About Enigma        |");
            Console.WriteLine("-------------------------");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nEnigma Console Program v1.0");
            Console.ResetColor();

            Console.WriteLine("\nDescription:");
            Console.WriteLine("This Enigma console program is a simulation of the famous World War II encryption machine used for secure communication. It allows users to encrypt and decrypt messages using various cipher settings, rotor configurations, and plugboard settings.");

            Console.WriteLine("\nFeatures:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("- Encryption and decryption of messages");
            Console.WriteLine("- Customizable cipher settings");
            Console.WriteLine("- It also features an encryption history");
            Console.WriteLine("- Simulates rotor and plugboard configurations");
            Console.ResetColor();

            // Credits or Developer Information
            Console.WriteLine("\nCredits:");

            Console.WriteLine("\nDeveloper:");
            Console.WriteLine("- Segovia, Nathaniel C.");
            Console.ReadKey(true);
        }
    }

    
}
