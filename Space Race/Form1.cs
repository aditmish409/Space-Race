using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Media;

//Adit Mishra
//May 22,2024
//Cash Register

namespace Space_Race
{
    public partial class Form1 : Form
    {
        //creating player 1 and 2
        Rectangle player2 = new Rectangle(550, 390, 17, 17);
        Rectangle player1 = new Rectangle(225, 390, 17, 17);
        
        //variables for players' speeds and scores
        int playerSpeed = 5;
        int player2Speed = 5;
        int p1Score = 0;
        int p2Score = 0;

        //meteor size
        int ballSize = 10;

        //movement
        bool upPressed = false;
        bool downPressed = false;
        bool wPressed = false;
        bool sPressed = false;

        //lists
        List<Rectangle> ballList = new List<Rectangle>();
        List<int> ballSpeeds = new List<int>();
        List<string> ballColours = new List<string>();

        //brushes for meteors and players
        SolidBrush blueBrush = new SolidBrush(Color.Blue);
        SolidBrush orangeBrush = new SolidBrush(Color.Orange);
        SolidBrush purpleBrush = new SolidBrush(Color.Purple);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush goldBrush = new SolidBrush(Color.Gold);
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        //randome generator
        Random randGen = new Random();
        int randValue = 0;

        public Form1()
        {
            InitializeComponent();
        }

        //reset game
        public void InitializeGame()
        {
            titleLabel.Text = "";
            subtitleLabel.Text = "";

            gameTimer.Enabled = true;

            p1Score = 0;
            p2Score = 0;

            ballColours.Clear();
            ballList.Clear();
            ballSpeeds.Clear();

            player2 = new Rectangle(550, 390, 17, 17);
            player1 = new Rectangle(225, 390, 17, 17);
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wPressed = true;
                    break;
                case Keys.S:
                    sPressed = true;
                    break;
                case Keys.Up:
                    upPressed = true;
                    break;
                case Keys.Down:
                    downPressed = true;
                    break;
                case Keys.Escape:
                    if (gameTimer.Enabled == false)
                    {
                        Application.Exit();
                    }
                    break;
                case Keys.Space:
                    if (gameTimer.Enabled == false)
                    {
                        InitializeGame();
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wPressed = false;
                    break;
                case Keys.S:
                    sPressed = false;
                    break;
                case Keys.Up:
                    upPressed = false;
                    break;
                case Keys.Down:
                    downPressed = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move player 1
            if (wPressed == true && player1.Y > 0)
            {
                player1.Y = player1.Y - playerSpeed;
            }

            if (sPressed == true && player1.Y < 410 - player1.Height)
            {
                player1.Y = player1.Y + playerSpeed;
            }

            //move player 2
            if (upPressed == true && player2.Y > 0)
            {
                player2.Y = player2.Y - player2Speed;
            }

            if (downPressed == true && player2.Y < 410 - player2.Height)
            {
                player2.Y = player2.Y + player2Speed;
            }

            //move balls from left to right of the screen
            for (int i = 0; i < ballList.Count(); i++)
            {
                //get new position of x
                int x = ballList[i].X + ballSpeeds[i];

                //update the ball object
                ballList[i] = new Rectangle(x, ballList[i].Y, ballSize, ballSize);
            }

            //spawn balls
            randValue = randGen.Next(0, 10000);

            if (randValue < 40)
            {
                if (randValue < 10)
                {
                    randValue = randGen.Next(10, 385 - ballSize * 2);

                    Rectangle ball = new Rectangle(0, randValue, ballSize, ballSize);
                    ballList.Add(ball);
                    ballColours.Add("purple");
                    ballSpeeds.Add(randGen.Next(5, 15));

                }
                else if (randValue < 20)
                {
                    randValue = randGen.Next(10, 385 - ballSize * 2);

                    Rectangle ball = new Rectangle(0, randValue, ballSize, ballSize);
                    ballList.Add(ball);
                    ballColours.Add("red");
                    ballSpeeds.Add(randGen.Next(5, 15));
                }
                else if (randValue < 30)
                {
                    randValue = randGen.Next(10, 385 - ballSize * 2);

                    Rectangle ball = new Rectangle(800, randValue, ballSize, ballSize);
                    ballList.Add(ball);
                    ballColours.Add("gold");
                    ballSpeeds.Add(randGen.Next(-15, -5));
                }
                else if (randValue < 40)
                {
                    randValue = randGen.Next(10, 385 - ballSize * 2);

                    Rectangle ball = new Rectangle(800, randValue, ballSize, ballSize);
                    ballList.Add(ball);
                    ballColours.Add("white");
                    ballSpeeds.Add(randGen.Next(-15, -5));
                }
            }

            //check for collision between ball and player 1
            for (int i = 0; i < ballList.Count(); i++)
            {
                if (ballList[i].IntersectsWith(player1))
                {
                    player1 = new Rectangle(225, 390, 17, 17);
                    SoundPlayer player = new SoundPlayer(Properties.Resources.bump);
                    player.Play();
                }
            }

            //check for collision between ball and player 2
            for (int i = 0; i < ballList.Count(); i++)
            {
                if (ballList[i].IntersectsWith(player2))
                {
                    player2 = new Rectangle(550, 390, 17, 17);
                    SoundPlayer player = new SoundPlayer(Properties.Resources.bump);
                    player.Play();
                }
            }

            //if player 1 scores
            if (player1.Y == 0)
            {
                p1Score++;
                player1 = new Rectangle(225, 390, 17, 17);
                p1ScoreLabel.Text = $"Score: {p1Score}";
                SoundPlayer player = new SoundPlayer(Properties.Resources.addpoint);
                player.Play();
            }
            //if player 2 scores
            if (player2.Y == 0)
            {
                p2Score++;
                player2 = new Rectangle(550, 390, 17, 17);
                p2ScoreLabel.Text = $"Score: {p2Score}";
                SoundPlayer player = new SoundPlayer(Properties.Resources.addpoint);
                player.Play();
            }

            //if player1 wins
            if (p1Score == 3)
            {
                gameTimer.Stop();
            }
            //if player 2 wins
            if (p2Score == 3)
            {
                gameTimer.Stop();
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //start screen
            if (gameTimer.Enabled == false && p1Score == 0 && p2Score == 0)
            {
                titleLabel.Text = "SPACE RACE";
                subtitleLabel.Text = "Press SPACE to start or ESC to exit";
                SoundPlayer player = new SoundPlayer(Properties.Resources.startgame);
                player.Play();
            }
            else if (gameTimer.Enabled == true)
            {
                //update labels
                p1ScoreLabel.Text = $"Score: {p1Score}";
                p2ScoreLabel.Text = $"Score: {p2Score}";

                e.Graphics.FillRectangle(blueBrush, player1);
                e.Graphics.FillRectangle(orangeBrush, player2);

                //move balls
                for (int i = 0; i < ballList.Count(); i++)
                {
                    if (ballColours[i] == "purple")
                    {
                        e.Graphics.FillEllipse(purpleBrush, ballList[i]);
                    }
                    else if (ballColours[i] == "red")
                    {
                        e.Graphics.FillEllipse(redBrush, ballList[i]);
                    }
                    else if (ballColours[i] == "gold")
                    {
                        e.Graphics.FillEllipse(goldBrush, ballList[i]);
                    }
                    else
                    {
                        e.Graphics.FillEllipse(whiteBrush, ballList[i]);
                    }
                }
            }           
            //end screen for p1 win
            else if (gameTimer.Enabled == false && p1Score == 3)
            {
                titleLabel.Text = "PLAYER 1 WINS";
                titleLabel.Text += $"\nP1 Score was {p1Score}";
                titleLabel.Text += $"\nP2 Score was {p2Score}";
                subtitleLabel.Text = "Press SPACE to Start or ESC to Exit";
                p1ScoreLabel.Text = "0";
                p2ScoreLabel.Text = "0";
                SoundPlayer player = new SoundPlayer(Properties.Resources.endgame);
                player.Play();
            }

            //end screen for p2 win
            else if (gameTimer.Enabled == false && p2Score == 3)
            {
                titleLabel.Text = "PLAYER 2 WINS";
                titleLabel.Text += $"\nP1 Score was {p1Score}";
                titleLabel.Text += $"\nP2 Score was {p2Score}";
                subtitleLabel.Text = "Press SPACE to Start or ESC to Exit";
                p1ScoreLabel.Text = "0";
                p2ScoreLabel.Text = "0";
                SoundPlayer player = new SoundPlayer(Properties.Resources.endgame);
                player.Play();
            }
        }

    }
}
