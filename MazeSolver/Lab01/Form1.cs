//**********************************************************************
//Lab O1 Review - Mazomatic
//Jared McNamee
//CMPE 2300
//Sept 22 2019
//Description - create a program that will solve a set of mazes using techiniques previously learned in 1600
//**********************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using GDIDrawer;
using System.IO;

namespace Lab01
{
    //delegate that will pass 2 ints and a bool from the thread method
    public delegate void delVoidIntIntBool(int step, int startTime, bool solved);
    public partial class Form1 : Form
    {
        
        //structure that will hold all the information about the maze
        public struct MazeInfo
        {
            public Point _mStart;//the maze entrance
            public Point _mEnd;//the maze exit
            public int _xWidth;//the width of the maze in pixels
            public int _yHeight;//the height of the maze in pixels
            public Color _cSol;//the color of the solution path`
            public Color _cDed;//the color of visted pixels that lead to dead ends
            public int _steps;//the number of steps taken to solve the maze
            public bool _solved;//whether the maze is solved or not
        }

        CDrawer _canvas = null;//intializing a canvas to null

        //enumeration for defining the type of pixel the solver has encountered
        //Open - can move into that space
        //Wall - is blocked from moving that direction
        //Visited - has already visited this space
        public enum state {open, wall, visited }

        //a 2-d of the enumeration, the collision boundaries of the maze
        public state[,] mazeArray;

        //the information on the current maze
        public MazeInfo _maze = new MazeInfo();

        //speed at which the solver moves through the maze higher = slower
        public int speed = 0;
        //string containing the name of the file
        public string _file;
        //boolean that determines if the cancel state is enabled
        public bool cancel = false;
        
        
        public Form1()
        {
            InitializeComponent();

            //itializing all the event functions
            UI_btn_Load.Click += UI_btn_Load_Click;
            UI_btn_Solve.Click += UI_btn_Solve_Click;
            UI_BTN_SColor.Click += UI_BTN_SColor_Click;
            UI_btn_DColor.Click += UI_btn_DColor_Click;
            UI_NUD_Speed.ValueChanged += UI_NUD_Speed_ValueChanged;
        }

        /// <summary>
        /// When the value of the speed Up down is changed setting the current solver speed to that value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_NUD_Speed_ValueChanged(object sender, EventArgs e)
        {
            speed = (int)UI_NUD_Speed.Value;
        }

        /// <summary>
        /// when the ok button is pressed in the color dialog setting _cDed to that color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_btn_DColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if (cd.ShowDialog() == DialogResult.OK) //when ok is pressed in the dialog
            {
                _maze._cDed = cd.Color;//setting the visited color to the select color
                UI_btn_DColor.BackColor = cd.Color;//displaying the color to the user in the main form
            }
        }

        /// <summary>
        /// when the ok button is pressed in the color dialog setting _cSol to that color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_BTN_SColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if(cd.ShowDialog() == DialogResult.OK)//when ok is pressed in the dialog
            {
                _maze._cSol = cd.Color;//setting the solution paths color to the selected color
                UI_BTN_SColor.BackColor = cd.Color;//displaying the color to the user in the main form
            }

        }

        /// <summary>
        /// When the solve button is pressed beginning either a normal or threaded solve,
        /// changing the solve button to a cancel button and if its clicked again cancelling the solve
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_btn_Solve_Click(object sender, EventArgs e)
        {
            if (UI_btn_Solve.Text == "Solve")//if the button is in solve mode
            {
                UI_btn_Solve.Text = "Cancel";//changing the text to cancel
                if ((_maze._xWidth * _maze._yHeight > 4000) || speed > 4)//if the maze is larger than 4000 sqr^2 or the speed is larger than 4 starting a threaded solve
                {
                    //telling the user a threaded solve is being used
                    UI_LB_MessageViewer.Items.Insert(0, $"Threaded Maze Starting for file: {_file}");
                    //starting a thread and setting the stack memory to 30mb
                    Thread mazeThread = new Thread(new ParameterizedThreadStart(threadStart), 30000000);
                    //sending the maze information to the thread as an object
                    mazeThread.Start(_maze);
                }
                else
                {
                    //teling the user that a non threaded solve has been started
                    UI_LB_MessageViewer.Items.Insert(0, $"Non-threaded Maze Starting for file: {_file}");
                    //calculating the start time of the solve
                    int startTime = Environment.TickCount;
                    //calling the recursive solve function
                    _maze = recSolve(_maze, _maze._mStart);
                    //sending the results to a function that will display everything to the user
                    solveResults(_maze._steps, startTime, _maze._solved);
                }
            }
            else
            {
                //if cancel was clicked setting the text to solve
                UI_btn_Solve.Text = "Solve";
                //enabling the cancel stated
                cancel = true;
            }

        }

        /// <summary>
        /// the function that will control the thread while the maze is solving
        /// </summary>
        /// <param name="maze"> the maze information must be cast back to MazeInfo</param>
        private void threadStart(object maze)
        {
            //importing the maze information and casting it to the correct struct
            MazeInfo tMaze = (MazeInfo)maze;
            //calculating the start time
            int startTime = Environment.TickCount;
            //calling the recursive solve function with the mazes information
            tMaze = recSolve(tMaze, _maze._mStart);
            //invoking the delegate to pass the results to a function that will display the results to the user
            Invoke(new delVoidIntIntBool(solveResults), tMaze._steps, startTime, tMaze._solved);
        }
        /// <summary>
        /// A function displays the results of a maze solve to the user
        /// </summary>
        /// <param name="steps">the number of steps it took to complete the maze</param>
        /// <param name="startTime">the time the maze was started</param>
        /// <param name="solved">whether the maze was actually solved or not</param>
        private void solveResults(int steps, int startTime, bool solved)
        {
            int endTime = Environment.TickCount - startTime; //subtracting the end time from the start time to get how long it took to solve or cancel the maze
            //if the maze wasn't solve and wasn't cancelled - failed to solve
            if (!solved && !cancel)
            {
                //telling the user the maze is unsolvable 
                UI_LB_MessageViewer.Items.Insert(0, $"Unable to solve Maze");
            }
            //if the maze was cancelled
            if (cancel)
            {
                //displaying to the user that the maze was cancelled and the time it was cancelled
                UI_LB_MessageViewer.Items.Insert(0, $"Solve canceled at {endTime} ms");
                //setting the cancel boolean back to false
                cancel = false;
                
            }
            else
                //the maze was solved displaying the number of steps and the time it took to complete to the user
                UI_LB_MessageViewer.Items.Insert(0, $"Solved in {steps} steps in {endTime} ms");

            //setting the solve button back to solve mode
            UI_btn_Solve.Text = "Solve";
            //disabling the solve switch until a new maze is loaded
            UI_btn_Solve.Enabled = false;
        }

        /// <summary>
        /// moving through the maze recursively to find the correct path through the maze
        /// </summary>
        /// <param name="maze">the information about the maze</param>
        /// <param name="curPo">the starting position of the recursive function</param>
        /// <returns></returns>
        private MazeInfo recSolve(MazeInfo maze, Point curPo)
        {
            //if the cancel button is pressed escaping the function
            //setting the steps to zero and that the maze was not solved
            if(cancel == true)
            {
                maze._steps = 0;
                maze._solved = false;
                return maze;
            }
            //if the maze exit is reached marking the maze as solve and escaping the function
            if (curPo == maze._mEnd)
            {
                maze._solved = true;
                return maze;
            }
            //if the current position of the function is out of bounds backing up 
            if ((curPo.X >= maze._xWidth) || (curPo.X < 0) || (curPo.Y >= maze._yHeight) || (curPo.Y < 0))
                return maze;
            //if the current position of the function is in a wall backing up
            if (mazeArray[curPo.X, curPo.Y] == state.wall) return maze;
            //if the current position of the function is a pixel that has been visited already backing up
            if (mazeArray[curPo.X, curPo.Y] == state.visited) return maze;

            //if the current position is not the start of the maze
            if(curPo != maze._mStart)
            {
                mazeArray[curPo.X, curPo.Y] = state.visited;//setting that pixel to visited
                _canvas.SetBBScaledPixel(curPo.X, curPo.Y, maze._cSol);//setting the color to the solution color
                _canvas.Render();//rendering the canvas
            }

            //incrementing the step counter
            maze._steps += 1;

            //if the speed is not zero sleeping the solver 
            if (speed != 0)
                Thread.Sleep(speed);

            //attempting to move the solver in each direction in the order (up, right, down, left) moving down 1 level of recursion
            if ((maze = recSolve(maze, new Point(curPo.X, curPo.Y + 1)))._solved) return maze;
            if ((maze = recSolve(maze, new Point(curPo.X + 1, curPo.Y)))._solved) return maze;
            if ((maze = recSolve(maze, new Point(curPo.X, curPo.Y - 1)))._solved) return maze;
            if ((maze = recSolve(maze, new Point(curPo.X - 1, curPo.Y)))._solved) return maze;

            //if the maze wasn't solve moving down the chain of recursion 
            if(curPo != maze._mStart)
            {
                _canvas.SetBBScaledPixel(curPo.X, curPo.Y, maze._cDed);//setting the current path to the visited color
                _canvas.Render();//rendering the canvas
            }

            //decrementing the steps
            maze._steps -= 1;

            //moving up 1 level of recursion
            return maze;
        }
        /// <summary>
        /// When the load button is pressed loading the bitmap into the cdrawer and parcing the information that we need to solve the map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UI_btn_Load_Click(object sender, EventArgs e)
        {
            //variable that represents the current scale factor on the maze
            int mapScaling = 10;

            //initalizing a file dialog to hand
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //customizing the filters in the dialog to only show .bmp or all files
            openFileDialog1.Filter = "All files(*.*)|*.*|bmp files (*.bmp)|*.bmp";
            //setting the inital directory
            openFileDialog1.InitialDirectory = Path.GetFullPath(Environment.CurrentDirectory + @"\..\..");

            //opening the file dialog and if the ok button is pressed
            if (openFileDialog1.ShowDialog()== DialogResult.OK)
            {
                try
                {
                    if (_canvas != null)//trying to close a canvas if the canvas is not null
                    {
                        _canvas.Close();
                    }

                    //importing the new bitmap from the file dialog
                    Bitmap bm = new Bitmap(openFileDialog1.FileName);
                    //if the bit map is a medium sized  bit map setting the scaling appropriately 
                    if (bm.Width > 150 || bm.Height > 100)
                    {
                        //throw new Exception("Bitmap Exceeds Size Requirements");
                        mapScaling = 5;
                    }
                    //if the bit map is larger reducing the scaling to make it fit the screen
                    if (bm.Width > 200 || bm.Height > 150)
                    {
                        mapScaling = 2;
                    }
                    //creating a new enum array set to the width and height of the bitmap
                    mazeArray = new state[bm.Width, bm.Height];
                    //creating a new canvas set to the height and width of the bitmap scaled with continuous update off, pixel scale set to the scaling and a background of white
                    _canvas = new CDrawer(bm.Width * mapScaling, bm.Height * mapScaling, false, false) { Scale = mapScaling, BBColour = Color.White };
                    //moving the canvas to be right next to the form
                    _canvas.Position = new Point(Location.X + Width, Location.Y);
                    
                    //moving through the bit maps x and y coords
                    for (int y = 0; y < bm.Height; ++y)
                    {
                        for (int x = 0; x < bm.Width; ++x)
                        {
                            //getting the color of the current pixel
                            _canvas.SetBBScaledPixel(x, y, bm.GetPixel(x, y));
                            if (bm.GetPixel(x, y) == Color.FromArgb(0, 0, 0))//if its black setting it to a wall state in the enum array
                                mazeArray[x, y] = state.wall;
                            if (bm.GetPixel(x, y) == Color.FromArgb(255, 255, 255))//if its white setting it to open state in the array
                                mazeArray[x, y] = state.open;
                            if (bm.GetPixel(x, y) == Color.FromArgb(255, 0, 0))//if its red setting that pixel as the maze exit
                                _maze._mEnd = new Point(x, y);
                            if (bm.GetPixel(x, y) == Color.FromArgb(0, 255, 0))//if its green setting that pixel as the maze entrance
                                _maze._mStart = new Point(x, y);
                        }
                    }
                    //rendering the canvas
                    _canvas.Render();
                    //setting the name of the form to the file name
                    Text = Path.GetFileName(openFileDialog1.FileName);
                    //displaying that the file has been loaded to the user with the dimensions
                    UI_LB_MessageViewer.Items.Insert(0,$"Loaded: {Path.GetFileName(openFileDialog1.FileName)} file Dimensions {bm.Width} X {bm.Height}");
                    //saving the file name for later use
                    _file = Path.GetFileName(openFileDialog1.FileName).ToString();

                    //initialing the maze info struct
                    _maze._xWidth = bm.Width;//setting the maze width
                    _maze._yHeight = bm.Height;//setting the maze height
                    _maze._cSol = UI_BTN_SColor.BackColor;//setting the color of the solution path
                    _maze._cDed = UI_btn_DColor.BackColor;//setting the color of the visited path
                    _maze._steps = 0;//initializing the step counter
                    _maze._solved = false;//setting solved to false

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);//catching exceptions during the loading process
                }
                UI_btn_Solve.Enabled = true;//enabling the solve button
            }

        }
    }
}
