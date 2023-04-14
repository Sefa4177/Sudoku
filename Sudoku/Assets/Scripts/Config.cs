using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class Config : MonoBehaviour
{
    #if UNITY_ANDROID && !UNITY_EDITOR
    private static string dir = Application.persistentDataPath;
    #else
    private static string dir = Directory.GetCurrentDirectory();
    #endif

    private static string file = @"\board_data.ini";
    private static string path = dir + file;

    public static void DeleteDataFile()
    {
        File.Delete(path);
    }

    public static void SaveBoardData(SudokuData.SudokuBoardData board_data, string level, int board_index, int error_number, Dictionary<string, List<string>> grid_notes)
    {
        File.WriteAllText(path, string.Empty);
        StreamWriter writer = new StreamWriter(path, false);
        string current_time = "#time:" + Clock.GetCurrentTime();
        string level_string = "#level:" + level;
        string error_number_string = "#errors:" + error_number;
        string board_index_string = "#board_index:" + board_index.ToString();
        string unsolved_string = "#unsolved:";
        string solved_string = "#solved:";

        foreach (var unsolved_data in board_data.unsolvedData)
        {
            unsolved_string += unsolved_data.ToString() + ",";
        }

        foreach (var solved_data in board_data.solvedData)
        {
            solved_string += solved_data.ToString() + ",";
        }

        writer.WriteLine(current_time);
        writer.WriteLine(level_string);
        writer.WriteLine(error_number_string);
        writer.WriteLine(board_index_string);
        writer.WriteLine(unsolved_string);
        writer.WriteLine(solved_string);

        foreach(var square in grid_notes)
        {
            string square_string = '#' + square.Key + ":";
            bool save = false;

            foreach(var note in square.Value)
            {
                if (note != " ")
                {
                    square_string += note + ",";
                    save = false;
                }
            }

            if(save)
            {
                writer.WriteLine(square_string);
            }
        }

        writer.Close();
    }

    public static Dictionary<int, List<int>> GetGridNotes()
    {
        Dictionary<int, List<int>> grid_notes = new Dictionary<int, List<int>>();
        string line;
        StreamReader file = new StreamReader(path);

        while((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if(word[0] == "#square_note")
            {
                int square_index = -1;
                List<int> notes = new List<int>();
                int.TryParse(word[1], out square_index);
                string[] substring = Regex.Split(word[2], ",");

                foreach(var note in substring)
                {
                    int note_number = -1;
                    int.TryParse(note, out note_number);
                    if(note_number > 0)
                    {
                        notes.Add(note_number);
                    }
                }

                grid_notes.Add(square_index,notes);
            }
        }

        file.Close();

        return grid_notes;
    }

    public static string ReadBoardLevel()
    {
        string line;
        string level = "";
        StreamReader file = new StreamReader(path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#level")
            {
                level = word[1];
            }
        }

        file.Close();
        return level;
    }

    public static SudokuData.SudokuBoardData ReadGridData()
    {
        string line;
        StreamReader file = new StreamReader(path);

        int[] unsolved_data = new int[81];
        int[] solved_data = new int[81];

        int unsolved_index = 0;
        int solved_index = 0;

        while((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if(word[0] == "#unsolved")
            {
                string[] substrings = Regex.Split(word[1], ",");

                foreach(var value in substrings)
                {
                    int square_number = -1;
                    if(int.TryParse(value, out square_number))
                    {
                        unsolved_data[unsolved_index] = square_number;
                        unsolved_index++;
                    }
                }
            }

            if(word[0] == "#solved")
            {
                string[] substrings = Regex.Split(word[1], ",");

                foreach(var value in substrings)
                {
                    int square_number = -1;
                    if(int.TryParse(value, out square_number))
                    {
                        solved_data[solved_index] = square_number;
                        solved_index++;
                    }
                }
            }
        }

        file.Close();
        return new SudokuData.SudokuBoardData(unsolved_data, solved_data);
    }

    public static int ReadGameBoardLevel()
    {
        int level = -1;
        string line ;
        StreamReader file = new StreamReader(path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if(word[0] == "#board_index")
            {
                int.TryParse(word[1], out level);
            }
        }

        file.Close();
        return level;
    }


    public static float ReadGameTime()
    {
        float time = -1.0f;
        string line;

        StreamReader file = new StreamReader(path);

        while((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if(word[0] == "#time")
            {
                float.TryParse(word[1], out time);
            }
            
        }

        file.Close();
        return time;
    }

    public static int ErrorNumber()
    {
        int errors = 0;
        string line;

        StreamReader file = new StreamReader(path);

        while((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if(word[0] == "#errors")
            {
                int.TryParse(word[1], out errors);
            }
            
        }

        file.Close();
        return errors;
    }

    public static bool GameDataFileExist()
    {
        return File.Exists(path);
    }

}
