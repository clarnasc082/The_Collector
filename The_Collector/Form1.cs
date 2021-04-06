using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace The_Collector
{
    public partial class form1 : Form
    {
        string gameState = "waiting";

        //ball
        Image diamond = Properties.Resources.diamond_icon;
        int ballX;
        int ballY;
        //int ballSpeed;
        //int ballColour;
        int ballSize = 30;

        //character
        Image rocket = Properties.Resources.rocket_icon;
        int characterX = 350;
        int characterY = 400;
        int characterWidth = 35;
        int characterHeight = 35;
        int characterXSpeed = 7;
        int characterYSpeed = 7;

        bool leftArrowDown = false;
        bool rightArrowDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;

        //invaders
        List<int> invaderXList = new List<int>();
        List<int> invaderYList = new List<int>();

        List<int> invaderSpeedXList = new List<int>();
        List<int> invaderSpeedYList = new List<int>();
        List<string> invaderColourList = new List<string>();
        int invaderWidth = 3;
        int invaderHeight = 2;

        List<int> astroidXList = new List<int>();
        List<int> astroidYList = new List<int>();

        List<int> astroidSpeedXList = new List<int>();
        List<int> astroidSpeedYList = new List<int>();
        //List<string> astroidColourList = new List<string>(); /////////////////////////////////////////
        int astroidWidth = 10;
        int astroidHeight = 10;

        string difficulty;  
         
        Random randGen = new Random();
        int randValue = 0;

        int time = 600;
        int score = 0;

        SolidBrush blueBrush = new SolidBrush(Color.Blue);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush greenBrush = new SolidBrush(Color.Green);

        public form1()
        {
            InitializeComponent();
        }

        public void GameInitialize()
        {
            gameState = "level";
            gameTimer.Enabled = true;

            invaderXList.Clear();
            invaderYList.Clear();

            invaderSpeedXList.Clear();
            invaderSpeedYList.Clear();
            astroidSpeedXList.Clear();
            astroidSpeedYList.Clear();
            astroidXList.Clear();
            astroidYList.Clear();

            time = 600;
            score = 0;
            ballX = 50;
            ballY = 200;
            easyButton.Visible = true;
            mediumButton.Visible = true;
            hardButton.Visible = true;
            winnerLabel.Text = " ";
        }
        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "level")
            {
                titleLabel.Text = "+-+- THE COLLECTOR -+-+";
                subTitleLabel.Text = "Choose your level...";
                easyButton.Visible = true;
                mediumButton.Visible = true;
                hardButton.Visible = true;
            }
            else if (gameState == "waiting")
            {
                titleLabel.Text = "+-+- THE COLLECTOR -+-+";
                subTitleLabel.Text = "Press Space Bar to Start or Escape to Exit";
            }
            else if (gameState == "running")
            {
                // draw text at top 
                titleLabel.Text = $" ";
                subTitleLabel.Text = $" ";
                winnerLabel.Text = " ";
                scoreLabel.Text = $"Score: {score}";
                timeLabel.Text = $"Time left: {time}";

                //e.Graphics.FillRectangle(blueBrush, characterX, characterY, characterWidth, characterHeight);
                e.Graphics.DrawImage(diamond, ballX, ballY, ballSize, ballSize);
                e.Graphics.DrawImage(rocket, characterX, characterY, characterWidth, characterHeight);
                for (int i = 0; i < invaderYList.Count(); i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, invaderXList[i], invaderYList[i], invaderWidth, invaderHeight);
                }

                for (int i = 0; i < astroidYList.Count(); i++)
                { 
                    e.Graphics.FillRectangle(whiteBrush, astroidXList[i], astroidYList[i], astroidWidth, astroidHeight);
                }
            }
            else if (gameState == "over")
            {
                titleLabel.Text = "GAME OVER";
                subTitleLabel.Text = "Press Space Bar to Start or Escape to Exit";
                scoreLabel.Text = " ";
            }
            else if (gameState == "winner")
            {
                titleLabel.Text = "WINNER";
                subTitleLabel.Text = "Press Space Bar to Start or Escape to Exit";
                scoreLabel.Text = " ";
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Space:
                    if (gameState == "waiting" || gameState == "over" || gameState == "winner")
                    {
                        GameInitialize();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "over" || gameState == "winner")
                    {
                        Application.Exit();
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //Generate colored dot
            Rectangle characterRec = new Rectangle(characterX, characterY, characterWidth, characterHeight);

            for (int i = 0; i < invaderYList.Count; i++)
            {
                Rectangle ballRec = new Rectangle(ballX, ballY, ballSize, ballSize);
                if (characterRec.IntersectsWith(ballRec))
                {
                    ballX = randGen.Next(20, this.Width - 50);
                    ballY = randGen.Next(20, this.Height - 400);
                    score = score + 1;
                    SoundPlayer Player = new SoundPlayer(Properties.Resources._220173__gameaudio__spacey_1up_power_up);
                    Player.Play();
                }
            }

            //check to see if a new invader should be created            
            randValue = randGen.Next(0, 101);
            if (randValue < 12)
            {
                invaderYList.Add(randGen.Next(0, this.Height - 50));
                invaderXList.Add(10);
                invaderSpeedXList.Add(randGen.Next(2, 10));
                invaderSpeedYList.Add(0);

                if (randValue < 1)
                {
                    astroidYList.Add(randGen.Next(0, this.Height - 50));
                    astroidXList.Add(10);
                    astroidSpeedXList.Add(randGen.Next(10, 20));
                    astroidSpeedYList.Add(0);
                }

                
                if (difficulty == "medium" || difficulty == "hard")
                {
                    invaderYList.Add(randGen.Next(0, this.Height - 50));
                    invaderXList.Add(660);
                    invaderSpeedXList.Add(randGen.Next(-10, -2));
                    invaderSpeedYList.Add(0);
                }

                if (difficulty == "hard")
                {
                    invaderXList.Add(randGen.Next(0, this.Width - 50));
                    invaderYList.Add(10);
                    invaderSpeedXList.Add(0);
                    invaderSpeedYList.Add(randGen.Next(2, 10));                
                }

                invaderColourList.Add("white");
            }

            for (int i = 0; i < invaderXList.Count(); i++)
            {
                invaderXList[i] += invaderSpeedXList[i];
                invaderYList[i] += invaderSpeedYList[i];
            }

            for (int i = 0; i < astroidXList.Count(); i++)
            {
                astroidXList[i] += astroidSpeedXList[i];
                astroidYList[i] += astroidSpeedYList[i];
            }

            //move character
            if (upArrowDown == true && characterY > 0)
            {
                characterY -= characterYSpeed;
            }

            if (downArrowDown == true && characterY < this.Height - characterHeight)
            {
                characterY += characterYSpeed;
            }

            if (leftArrowDown == true && characterX > 0)
            {
                characterX -= characterXSpeed;
            }

            if (rightArrowDown == true && characterX < this.Width - characterWidth)
            {
                characterX += characterXSpeed;
            }


            //if character and invader collide..
            for (int i = 0; i < invaderYList.Count; i++)
            {
                Rectangle invaderRec = new Rectangle(invaderXList[i], invaderYList[i], invaderWidth, invaderHeight);

                if (characterRec.IntersectsWith(invaderRec))
                {
                    characterY = 400;
                }
            }

            //if character and astroid collide..
            for (int i = 0; i < astroidYList.Count; i++)
            {
                Rectangle astroidRec = new Rectangle(astroidXList[i], astroidYList[i], astroidWidth, astroidHeight);

                if (characterRec.IntersectsWith(astroidRec))
                {
                    gameState = "over";
                    timeLabel.Text = " ";
                    SoundPlayer Player = new SoundPlayer(Properties.Resources._253886__themusicalnomad__negative_beeps__1_);
                    Player.Play();
                }
            }

            //decrease time counter and check to see if time is up 
            time--;

            if (time == 0)
            {
                gameTimer.Enabled = false;
                gameState = "over";
                characterY = 400;
                characterX = 350;

            }

            //if player reaches a score of 3
            if (score == 3)
            {
                gameState = "winner";
                winnerLabel.Text = "Congratulations";
                scoreLabel.Text = " ";
                characterY = 400;
                characterX = 350;
            }

            Refresh();
        }

        private void easyButton_Click(object sender, EventArgs e)
        {
            GameInitialize();
            gameState = "running";
            difficulty = "easy";
            easyButton.Visible = false;
            mediumButton.Visible = false;
            hardButton.Visible = false;
            time = 600;
            SoundPlayer Player = new SoundPlayer(Properties.Resources._478337__joao_janz__bouncing_power_up_1_2);
            Player.Play();
            this.Focus();
        }

        private void mediumButton_Click(object sender, EventArgs e)
        {
            GameInitialize();
            gameState = "running";
            easyButton.Visible = false;
            mediumButton.Visible = false;
            hardButton.Visible = false;
            difficulty = "medium";
            time = 600;
            SoundPlayer Player = new SoundPlayer(Properties.Resources._544886__mrthenoronha__twinkly_power_up);
            Player.Play();
            this.Focus();
        }

        private void hardButton_Click(object sender, EventArgs e)
        {
            GameInitialize();
            gameState = "running";
            easyButton.Visible = false;
            mediumButton.Visible = false;
            hardButton.Visible = false;
            difficulty = "hard";
            time = 600;
            SoundPlayer Player = new SoundPlayer(Properties.Resources._511385__mrthenoronha__power_up_8_bit);
            Player.Play();
            this.Focus();
        }
    }
}
