using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CompGraphics_Laba7
{
    public partial class Form1 : Form // Size Form: 1 000, 600, Size PictureBox: 975, 490
    {
        // Битовая картинка pictureBox
        Bitmap pictureBoxBitMap;
        // Битовая картинка динамического изображения
        Bitmap spriteBitMap;

        Bitmap shipBitMap;
        // Битовая картинка для временного хранения области экрана
        Bitmap cloneBitMap;
        // Графический контекст picturebox
        Graphics g_pictureBox;
        //Drawing Ship
        Graphics g_ship;
        // Графический контекст спрайта
        Graphics g_sprite;
        int x, y; // Координаты ракеты
        int width = 300, height = 175; // Ширина и высота 
        Timer timer;

        public Form1()
        {
            InitializeComponent();
        }
        void DrawShip()
        {
            Pen myWindows = new Pen(Color.BurlyWood, 2);
            // Определение кистей
            SolidBrush myBody = new SolidBrush(Color.Black);
            SolidBrush myHold = new SolidBrush(Color.DarkRed);
            SolidBrush myTube = new SolidBrush(Color.Gray);
            // Рисование и закраска труб, трюма и корпуса корабля
            g_ship.FillRectangle(myTube, 100, 0, 25, 25);
            g_ship.FillRectangle(myTube, 150, 0, 25, 25);

            g_ship.FillPolygon(myBody, new Point[] {
                new Point(0, 75),new Point(300, 75),
                new Point(235, 175), new Point(50, 175),
                new Point(0, 75)
            });
            g_ship.FillRectangle(myHold, 50, 25, 175, 50);

            g_ship.DrawEllipse(myWindows, 90, 40, 20, 20);
            g_ship.DrawEllipse(myWindows, 130, 40, 20, 20);
            g_ship.DrawEllipse(myWindows, 170, 40, 20, 20);
        }
        // Функция рисования спрайта (ракета)
        void DrawSprite()
        {
            // Задаем красный цвет для носа ракеты
            SolidBrush myNose = new SolidBrush(Color.Red);
            // Задаем белый и серый цвет для корпуса ракеты
            SolidBrush myRBody = new SolidBrush(Color.Silver);
            SolidBrush myLine = new SolidBrush(Color.Gray);


            {
                //Рисуем нос ракеты
                g_sprite.FillPolygon(myNose, new Point[]
            {

            new Point(275+20,143-1),new Point(275+20,260-1),
            new Point(380+150,235+150), new Point(300,145),
            //new Point(335,145),new Point(335,300),
            //new Point(300,300),new Point(250,145),
            //new Point(250,145),new Point(300,300)

                });
                //Рисуем нижнюю часть ракеты
                g_sprite.FillPolygon(myLine, new Point[] {

            new Point(204+10,130),new Point(204+10,130),
            new Point(204+10,150),new Point(235+10,150),
            //new Point(120,145),new Point(120,200)

            });
                //Рисуем корпус ракеты белым цветом
                g_sprite.FillRectangle(myRBody, 204+10, 145, 72+10, 155+10);
            }


        }

        // Функция сохранения части изображения шириной
        void SavePart(int xt, int yt)
        {
            Rectangle cloneRect = new Rectangle(xt, yt, width, height);
            System.Drawing.Imaging.PixelFormat format =
            pictureBoxBitMap.PixelFormat;
            // Клонируем изображение, заданное прямоугольной областью
            cloneBitMap = pictureBoxBitMap.Clone(cloneRect, format);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Создаём Bitmap для pictureBox1 и графический контекст
            pictureBox1.Image = Image.FromFile("F:\\Labs\\C-gr7\\ocean.jpg");
            pictureBoxBitMap = new Bitmap(pictureBox1.Image);
            g_pictureBox = Graphics.FromImage(pictureBox1.Image);
            // Создаём Bitmap для спрайта и графический контекст
            spriteBitMap = new Bitmap(width, height);
            g_sprite = Graphics.FromImage(spriteBitMap);
            // Создаём Bitmap для корбля и графический контекст
            shipBitMap = new Bitmap(width, height);
            g_ship = Graphics.FromImage(shipBitMap);
            // Рисование и закраска моря
            int r = 50, iks = 50;
            while (iks <= pictureBox1.Width + r)
            {
                g_pictureBox.FillPie(Brushes.Blue, -50 + iks, 375, 50, 50, 0, -180);
                iks += 50;
            }
            // Создаём Bitmap для временного хранения части изображения
            cloneBitMap = new Bitmap(width, height);
            // Задаем начальные координаты вывода движущегося объекта
            x = 0; y = 200;
            // Сохраняем область экрана перед первым выводом объекта
            SavePart(x, y);
            // Выводим корабль на графический контекст g_pictureBox
            DrawShip();
            g_pictureBox.DrawImage(shipBitMap, x-60, y);
            g_pictureBox.DrawImage(spriteBitMap, x+120, y);
            // Перерисовываем pictureBox1
            pictureBox1.Invalidate();
            // Создаём таймер с интервалом 100 миллисекунд
            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(timer1_Tick);

        }


        // Обрабатываем событие от таймера
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Восстанавливаем затёртую область статического изображения
            g_pictureBox.DrawImage(cloneBitMap, x+200, y);
            // Изменяем координаты для следующего вывода
            x += 20;
            // Проверяем на выход изображения автобуса за правую границу
            if (x > pictureBox1.Width - 1) x = pictureBox1.Location.X;
            // Сохраняем область экрана перед первым выводом автобуса
            SavePart(x, y);
            // Выводим
            g_pictureBox.DrawImage(spriteBitMap, x, y);
            // Перерисовываем pictureBox1
            pictureBox1.Invalidate();
        }
        // Включаем таймер по нажатию на кнопку
        private void button1_Click(object sender, EventArgs e)
        {
            DrawSprite();
            timer.Enabled = true;
        }
    }
}

/*            new Point(275,143),new Point(275,260),
            new Point(380,235)
*/

/*
  new Point(204,130),new Point(204,130),
        new Point(204,150),new Point(235,150),
 */
