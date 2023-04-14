using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuGrid : MonoBehaviour
{
    public int columns = 0;
    public int rows = 0;
    public float squareOffSet = 0.0f;
    public GameObject gridSquare;
    public Vector2 startPosition = new Vector2(0.0f , 0.0f);
    public float squareScale = 1.0f;
    public float square_gap = 0.1f;
    public Color line_highlight_color = Color.blue;

    private List<GameObject> gridSqaures_ = new List<GameObject>();
    private int selectedGridData = -1;

    void Start()
    {
        if(gridSquare.GetComponent<GridSquare>() == null)
        {
            Debug.LogError("need gridsquare script");
        }    
        CreateGrid();
        if(GameSettings.Instance.GetContinuePreviusGame())
        {
            SetGridFromFile();
        }
        else
        {
            SetGridNumber(GameSettings.Instance.GetGameMode());
        }
    }

    void  SetGridFromFile()
    {
        string level = GameSettings.Instance.GetGameMode();
        selectedGridData = Config.ReadGameBoardLevel();
        var data = Config.ReadGridData();

        setGridSquareData(data);
        SetGridNotes(Config.GetGridNotes());
    }

    private void SetGridNotes(Dictionary<int, List<int>> notes)
    {
        foreach(var note in notes)
        {
            gridSqaures_[note.Key].GetComponent<GridSquare>().SetGridNotes(note.Value);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetSquarePosition();
    }

    private void SpawnGridSquares()
    {
        int square_index = 0;
        for(int row = 0; row < rows; row++ )
        {
            for(int column = 0; column < columns; column++)
            {
                gridSqaures_.Add(Instantiate(gridSquare) as GameObject);
                gridSqaures_[gridSqaures_.Count -1].GetComponent<GridSquare>().SetSquareIndex(square_index);
                gridSqaures_[gridSqaures_.Count -1].transform.parent = this.transform;
                gridSqaures_[gridSqaures_.Count -1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);

                square_index++;
            }
        }
    }

    private void SetSquarePosition()
    {
        var sqaureRect = gridSqaures_[0].GetComponent<RectTransform>();
        Vector2 offset = new Vector2();
        Vector2 square_gap_number = new Vector2(0.0f, 0.0f);
        bool row_moved = false;

        offset.x = sqaureRect.rect.width * sqaureRect.transform.localScale.x + squareOffSet;
        offset.y = sqaureRect.rect.height * sqaureRect.transform.localScale.y + squareOffSet;

        int columnNumber = 0;
        int rowNumber = 0;

        foreach(GameObject sqaure in gridSqaures_)
        {
            if(columnNumber +1 > columns)
            {  
                rowNumber++;
                columnNumber = 0;
                square_gap_number.x = 0;
                row_moved = false;
            }
            var posXoffSet = offset.x * columnNumber + (square_gap_number.x * square_gap);
            var posyoffSet = offset.y * rowNumber + ( square_gap_number.y * square_gap);

            if(columnNumber > 0 && columnNumber % 3 ==0)
            {
                square_gap_number.x++;
                posXoffSet += square_gap;
            }
            if(rowNumber > 0 && rowNumber % 3 == 0 && row_moved == false)
            {
                row_moved = true;
                square_gap_number.y++;
                posyoffSet += square_gap;
            }

            sqaure.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x + posXoffSet, startPosition.y - posyoffSet);
            columnNumber++;
        }
    }

    private void SetGridNumber(string level)
    {
        selectedGridData = Random.Range(0, SudokuData.Instance.sudokuGame[level].Count);
        var data = SudokuData.Instance.sudokuGame[level][selectedGridData];

        setGridSquareData(data);


    /*  foreach(var square in gridSqaures_)
        {
            square.GetComponent<GridSquare>().SetNumber(Random.Range(0,10));
        }   */
    }

    private void setGridSquareData(SudokuData.SudokuBoardData data)
    {
        for(int index = 0; index < gridSqaures_.Count; index++)
        {
            gridSqaures_[index].GetComponent<GridSquare>().SetHasDefaultValue(data.unsolvedData[index] !=0 && data.unsolvedData[index] == data.solvedData[index]);
            gridSqaures_[index].GetComponent<GridSquare>().SetNumber(data.unsolvedData[index]);
            gridSqaures_[index].GetComponent<GridSquare>().SetCorrectNumber(data.solvedData[index]);
        }
    }

    private void OnEnable() 
    {
       GameEvents.OnSquareSelected += OnSquareSelected; 
       GameEvents.OnCheckBoardCompleted += CheckBoardCompleted;
    }

    private void OnDisable() 
    {
      GameEvents.OnSquareSelected -= OnSquareSelected;
      GameEvents.OnCheckBoardCompleted -= CheckBoardCompleted;

      //************************************************
      var solved_data = SudokuData.Instance.sudokuGame[GameSettings.Instance.GetGameMode()][selectedGridData].solvedData;
      int[] unsolved_data = new int[81];
      Dictionary<string, List<string>> grid_notes = new Dictionary<string, List<string>>();

      for(int i = 0 ; i < gridSqaures_.Count; i++)
      {
        var comp = gridSqaures_[i].GetComponent<GridSquare>();
        unsolved_data[i] = comp.GetSquareNumber();
        string key =  "square_note" + i.ToString();
        grid_notes.Add(key, comp.GetSquareNotes());
      }

      SudokuData.SudokuBoardData current_game_data = new SudokuData.SudokuBoardData(unsolved_data, solved_data);
      
      if(GameSettings.Instance.GetExitAfterWon() == false)
      {
        Config.SaveBoardData(current_game_data, GameSettings.Instance.GetGameMode(), selectedGridData, Lives.instance.GetErrorNumbers(),grid_notes);
      }
      else
      {
        Config.DeleteDataFile();
      }
      GameSettings.Instance.SetExitAfterWon(false);
    }

    private void SetSqauresColour(int[] data, Color col)
    {
        foreach (var index in data)
        {
            var comp = gridSqaures_[index].GetComponent<GridSquare>();
            if(comp.HasWrongValue() == false && comp.isSelected() == false)
            {
                comp.SetSqaureColour(col);
            }
        }
    }

    public void OnSquareSelected(int sqaure_index)
    {
        var horizontal_line = LineIndicator.instance.GetHorizantalLine(sqaure_index);
        var vertical_line = LineIndicator.instance.GetVerticalLine(sqaure_index);
        var square = LineIndicator.instance.GetSquare(sqaure_index);

        if(gridSqaures_[sqaure_index].GetComponent<GridSquare>().GetHasDefaultValue() == false)
        {
            SetSqauresColour(LineIndicator.instance.GetAllSqauresIndexes(), Color.white);

            SetSqauresColour(horizontal_line, line_highlight_color);
            SetSqauresColour(vertical_line, line_highlight_color);
            SetSqauresColour(square, line_highlight_color);
        }
        else
        {
            foreach(var gridSquare in gridSqaures_)
            {
                var comp = gridSquare.GetComponent<GridSquare>();
                if(comp.HasWrongValue() == false && comp.isSelected() == false)
                {
                    comp.SetSqaureColour(Color.white);
                }
            }
        }

        
    }

    private void CheckBoardCompleted()
    {
        foreach(var square in gridSqaures_)
        {
            var comp = square.GetComponent<GridSquare>();
            if(comp.IsCorrectNumberSet() == false)
            {
                return;
            }
        }
        GameEvents.OnBoardCompletedMethod();
    }

    public void SolveSudoku()
    {
        foreach(var square in gridSqaures_)
        {
            var comp = square.GetComponent<GridSquare>();
            comp.SetCorrectNumber();
        }

        CheckBoardCompleted(); 
    }
}
